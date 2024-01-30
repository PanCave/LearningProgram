using LearningProgram.BO;
using LearningProgram.Persistence.Os;
using Microsoft.Win32;
using System.Windows;
using System.Windows.Controls;

namespace QuestionMaker
{
  internal enum QuestionMode
  {
    Add,
    Edit
  }

  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    private readonly JsonSaver jsonSaver;
    private readonly List<Question> questions;
    private QuestionMode questionMode;

    public MainWindow()
    {
      InitializeComponent();
      AddAnswerField(2);
      jsonSaver = new JsonSaver();
      questions = new List<Question>();
      questionMode = QuestionMode.Add;
    }

    private void AddAnswerField(int answers = 1, string answerText = "", bool isCorrectAnswer = false)
    {
      for (int i = 0; i < answers; i++)
      {
        Grid answerGrid = new Grid();
        ColumnDefinition checkBoxColumnDefinition = new ColumnDefinition();
        ColumnDefinition answerTexctColumnDefinition = new ColumnDefinition();
        checkBoxColumnDefinition.Width = new GridLength(25);
        answerTexctColumnDefinition.Width = new GridLength(1, GridUnitType.Star);
        answerGrid.ColumnDefinitions.Add(checkBoxColumnDefinition);
        answerGrid.ColumnDefinitions.Add(answerTexctColumnDefinition);
        CheckBox checkBox = new CheckBox();
        checkBox.IsChecked = isCorrectAnswer;
        // Add checkBox to first Column
        Grid.SetColumn(checkBox, 0);
        answerGrid.Children.Add(checkBox);
        TextBox answerTextBox = new TextBox();
        answerTextBox.Text = answerText;
        answerTextBox.Width = 200;
        // Add answerTextBox to second Column
        Grid.SetColumn(answerTextBox, 1);
        answerGrid.Children.Add(answerTextBox);

        lst_anwers.Items.Add(answerGrid);
      }
    }

    private void btn_addQuestion_Click(object sender, RoutedEventArgs e)
    {
      if (questionMode == QuestionMode.Add)
      {
        AddQuestionToQuestionaire();
      }
      else if (questionMode == QuestionMode.Edit)
      {
        UpdateQuestion();
      }
    }

    private void UpdateQuestion()
    {
      Question? currentQuestion = lst_questions.SelectedItem as Question;
      if (currentQuestion == null)
        return;

      int questionIndex = questions.IndexOf(currentQuestion);
      questions.Remove(currentQuestion);

      Question? newQuestion = CreateQuestion();
      if (newQuestion == null)
        return;
      questions.Insert(questionIndex, newQuestion);

      lst_questions.Items.Remove(currentQuestion);
      lst_questions.Items.Insert(questionIndex, newQuestion);
    }

    private void AddQuestionToQuestionaire()
    {
      Question? question = CreateQuestion();
      if (question == null)
        return;
      questions.Add(question);
      lst_questions.Items.Add(question);
      Title = $"{questions.Count} Frage(n) im aktuellen Fragekatalog";
      ClearQuestion();
    }

    private Question? CreateQuestion()
    {
      List<string> answers = new List<string>();
      List<string> realAnswers = new List<string>();
      List<int> realAnswersIndexes = new List<int>();
      int answerIndex = 0;
      bool foundAtLeastOneCorrectAnswer = false;
      foreach (Grid grid in lst_anwers.Items)
      {
        CheckBox checkBox = null;
        TextBox textBox = null;

        // Loop through each control in the Grid
        foreach (var control in grid.Children)
        {
          if (control is CheckBox)
            checkBox = (CheckBox)control;
          else if (control is TextBox)
            textBox = (TextBox)control;
        }

        if (checkBox != null && textBox != null)
        {
          // Now you have the CheckBox and TextBox, you can access their properties
          bool isChecked = checkBox.IsChecked ?? false;
          string text = textBox.Text;

          // Do something with isChecked and text
          // For example, just printing them
          answers.Add(text);
          if (isChecked)
          {
            foundAtLeastOneCorrectAnswer = true;
            realAnswers.Add(text);
            realAnswersIndexes.Add(answerIndex);
          }
        }
        answerIndex++;
      }
      if (!foundAtLeastOneCorrectAnswer)
      {
        MessageBox.Show("Jede Frage muss mindestens eine richtige Antwort haben!");
        return null;
      }
      Question question = new Question(txt_question.Text, answers.ToArray(), realAnswers.ToArray(), realAnswersIndexes.ToArray(), txt_explanation.Text);
      return question;
    }

    private void ClearQuestion()
    {
      txt_question.Text = string.Empty;
      lst_anwers.Items.Clear();
      AddAnswerField(2);
      txt_explanation.Text = string.Empty;
    }

    private void btn_addAnswer_Click(object sender, RoutedEventArgs e)
    {
      AddAnswerField();
    }

    private void btn_saveQuestionaire_Click(object sender, RoutedEventArgs e)
    {
      SaveFileDialog saveFileDialog = new SaveFileDialog();
      saveFileDialog.Filter = "Json-Dateien|*.json|Alle Dateien|*.*";
      saveFileDialog.RestoreDirectory = true;
      saveFileDialog.Title = "Speicherort auswählen";
      saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
      if (saveFileDialog.ShowDialog() == true)
      {
        jsonSaver.SaveQuestionsAsJson(questions.ToArray(), saveFileDialog.FileName);
      }
    }

    private void lst_questions_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      if (lst_questions.SelectedItem == null)
      {
        questionMode = QuestionMode.Add;
        btn_addQuestion.Content = "Frage hinzufügen";
      }

      Question? question = lst_questions.SelectedItem as Question;
      if (question == null)
        return;

      ClearQuestion();
      txt_question.Text = question.QuestionText;
      lst_anwers.Items.Clear();

      int answerIndex = 0;
      foreach (string answer in question.Answers)
      {
        AddAnswerField(1, answer, question.RealAnswerIndexes.Contains(answerIndex));
        answerIndex++;
      }

      txt_explanation.Text = question.Explanation;
      btn_addQuestion.Content = "Frage aktualisieren";
      questionMode = QuestionMode.Edit;
    }

    private void btn_clearQuestion_Click(object sender, RoutedEventArgs e)
    {
      txt_question.Text = string.Empty;
      lst_anwers.Items.Clear();
      AddAnswerField(2);
      txt_explanation.Text = string.Empty;
      lst_questions.UnselectAll();
      btn_addQuestion.Content = "Frage hinzufügen";
      questionMode = QuestionMode.Add;
    }
  }
}