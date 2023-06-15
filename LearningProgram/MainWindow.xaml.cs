using LearningProgram.BL;
using LearningProgram.BO;
using LearningProgram.Persistence.Os;
using Microsoft.Win32;
using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace LearningProgram
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    private const string notKnown = "notKnown";
    private readonly Session session;
    private readonly SessionSaver sessionSaver;
    private readonly SessionLoader sessionLoader;
    private readonly QuestionaireSaver questionaireSaver;
    private readonly SolidColorBrush grayBrush;
    private readonly SolidColorBrush redBrush;
    private readonly SolidColorBrush greenBrush;
    private readonly SolidColorBrush yellowBrush;
    private readonly Random random;
    private readonly PdfSaver pdfSaver;
    private QuestionState questionState;
    private string directory;
    private string filename;
    private string extension;
    private StringBuilder stringBuilder;

    public MainWindow()
    {
      InitializeComponent();

      random = new Random();
      stringBuilder = new StringBuilder();
      sessionSaver = new SessionSaver();
      sessionLoader = new SessionLoader();
      pdfSaver = new PdfSaver();

      grayBrush = new SolidColorBrush(Color.FromRgb(224, 224, 224));
      redBrush = new SolidColorBrush(Color.FromRgb(255, 204, 188));
      greenBrush = new SolidColorBrush(Color.FromRgb(142, 255, 193));
      yellowBrush = new SolidColorBrush(Color.FromRgb(253, 227, 167));

      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.Filter = "Json-Dateien|*.json|Session-Dateien|*.session";
      openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

      if (openFileDialog == null)
        return;

      if (openFileDialog.ShowDialog() == true)
      {
        directory = Path.GetDirectoryName(openFileDialog.FileName);
        filename = Path.GetFileNameWithoutExtension(openFileDialog.SafeFileName);
        extension = Path.GetExtension(openFileDialog.SafeFileName);
      }
      else return;

      if (extension.Equals(".json"))
      {
        QuestionaireLoader questionaireLoader = new QuestionaireLoader();
        Questionaire questionaire = questionaireLoader.LoadQuestionare($@"{directory}\{filename}{extension}");

        session = new Session(Guid.NewGuid(), questionaire);
        ShuffleQuestionaire();
      }
      else if (extension.Equals(".session"))
      {
        session = sessionLoader.LoadSession($@"{directory}\{filename}{extension}");
        UpdateBackground();
      }

      questionState = QuestionState.Question;
      questionaireSaver = new QuestionaireSaver();

      btn_not_known.IsEnabled = false;
      btn_not_known.Background = grayBrush;
      btn_next.Background = grayBrush;

      UpdateWindowTitle();

      txt_question.Text = session.Questionaire[session.QuestionIndex].GetFormattedQuestionText();
    }

    private void UpdateWindowTitle()
    {
      Title = $"Lernhelfer - Frage {session.QuestionIndex + 1}/{session.Questionaire.Questions.Length} - {session.WrongQuestionIndexes.Count} Fragen falsch beantwortet ({Math.Round((double)session.WrongQuestionIndexes.Count / (session.QuestionIndex == 0 ? 1 : session.QuestionIndex) * 100)}%) - Geladener Fragenkatalog: {filename} - ";
    }

    private void ShuffleQuestionaire()
    {
      session.Questionaire.ShuffleQuestions(random);
      foreach (Question question in session.Questionaire.Questions)
      {
        question.ShuffleAnswers(random);
      }
    }

    private void NextState(bool next = true)
    {
      if (questionState == QuestionState.Question)
      {
        stringBuilder.Clear();
        int i = 97;
        foreach (string answer in session.Questionaire[session.QuestionIndex].Answers)
        {
          stringBuilder.AppendLine($"{(char)i}) {answer}");
          i++;
        }
        string answersText = stringBuilder.ToString();
        txt_answers.Text = answersText;
        questionState = QuestionState.Answers;
      }
      else if (questionState == QuestionState.Answers)
      {
        stringBuilder.Clear();
        foreach (int realAnswerIndex in session.Questionaire[session.QuestionIndex].RealAnswerIndexes)
        {
          stringBuilder.AppendLine($"{(char)(realAnswerIndex + 97)}) {session.Questionaire[session.QuestionIndex].Answers[realAnswerIndex]}");
        }
        string realAnswersText = stringBuilder.ToString();
        txt_real_answers.Text = realAnswersText;
        if (string.IsNullOrEmpty(session.Questionaire[session.QuestionIndex].Explanation))
        {
          questionState = QuestionState.Explanation;
          btn_not_known.IsEnabled = true;
          btn_not_known.Background = redBrush;
          btn_next.Background = greenBrush;
          txt_explanation.Text = "Keine Erklärung vorhanden";
        }
        else
        {
          questionState = QuestionState.RealAnswers;
        }
      }
      else if (questionState == QuestionState.RealAnswers)
      {
        txt_explanation.Text = session.Questionaire[session.QuestionIndex].Explanation;
        questionState = QuestionState.Explanation;
        btn_not_known.IsEnabled = true;
        btn_not_known.Background = redBrush;
        btn_next.Background = greenBrush;
      }
      else if (questionState == QuestionState.Explanation)
      {
        txt_answers.Text = string.Empty;
        txt_real_answers.Text = string.Empty;
        if (session.QuestionIndex == session.Questionaire.Questions.Length - 1)
        {
          string message = string.Empty;
          if (session.WrongQuestionIndexes.Count > 0)
          {
            if ((double)session.WrongQuestionIndexes.Count / session.Questionaire.Questions.Length > 0.15d)
            {
              message = "Alle Fragen durchgegangen. Wenn dies eine Prüfung wäre, hättest du sie leider nicht bestanden. Falsch beantwortete Fragen werden gespeichert!";
              SaveWrongQuestions();
            }
            else
            {
              message = "Alle Fragen durchgegangen. Herzlichen Glückwunsch, du hättest die Prüfung bestanden! Falsch beantwortete Fragen werden gespeicht!";
              SaveWrongQuestions();
            }
          }
          else
          {
            MessageBox.Show("Herzlichen Glückwunsch, du hast alle Fragen richtig beantwortet!");
          }
          MessageBox.Show(message);
        }

        if (!next)
        {
          session.WrongQuestionIndexes.Add(session.QuestionIndex);
        }

        session.QuestionIndex = (session.QuestionIndex + 1) % session.Questionaire.Questions.Length;
        txt_question.Text = session.Questionaire[session.QuestionIndex].GetFormattedQuestionText();
        txt_explanation.Text = string.Empty;
        questionState = QuestionState.Question;
        btn_not_known.IsEnabled = false;
        btn_next.Background = grayBrush;
        btn_not_known.Background = grayBrush;

        UpdateWindowTitle();
        UpdateBackground();
      }
    }

    private void UpdateBackground()
    {
      if (session.WrongQuestionIndexes.Count > 0)
      {
        if ((double)session.WrongQuestionIndexes.Count / session.QuestionIndex > 0.15)
        {
          Background = redBrush;
        }
        else
        {
          Background = yellowBrush;
        }
      }
      else
      {
        Background = greenBrush;
      }
    }

    private void btn_next_Click(object sender, RoutedEventArgs e)
    {
      NextState();
    }

    private void Window_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Enter)
      {
        NextState();
      }
      else if (questionState == QuestionState.Explanation && e.Key == Key.RightCtrl)
      {
        NextState(false);
      }
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      SaveWrongQuestions();
    }

    private void SaveWrongQuestions()
    {
      Question[] wrongQuestions = new Question[session.WrongQuestionIndexes.Count];
      int i = 0;
      foreach (int index in session.WrongQuestionIndexes)
      {
        wrongQuestions[i] = session.Questionaire[index];
        i++;
      }
      pdfSaver.SaveQuestionsAsPdf(wrongQuestions, $@"{directory}\{filename}_{DateTime.Now:yyyy_dd_M_HH_mm_ss}.pdf");
      sessionSaver.SaveSession(session, $@"{directory}\{filename}_{DateTime.Now:yyyy_dd_M_HH_mm_ss}.session");
      questionaireSaver.SaveQuestions(wrongQuestions, $@"{directory}\{filename}_{notKnown}{extension}");
    }

    private void btn_not_known_Click(object sender, RoutedEventArgs e)
    {
      NextState(false);
    }
  }
}