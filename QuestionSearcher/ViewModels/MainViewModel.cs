using LearningProgram.BL;
using LearningProgram.BO;
using QuestionSearcher.Converter;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Threading;

namespace QuestionSearcher.ViewModels
{
  internal class MainViewModel : INotifyPropertyChanged
  {
    private readonly Questionaire questionaire;
    private readonly QuestionaireMatcher questionaireMatcher;
    private readonly QuestionToQuestionViewModelConverter questionToQuestionViewModelConverter;
    private readonly DispatcherTimer searchDelayTimer;
    private QuestionViewModel[] questionViewModels;
    private string searchText;
    private SearchMode searchMode;

    public MainViewModel(Questionaire questionaire, QuestionaireMatcher questionaireMatcher, QuestionToQuestionViewModelConverter questionToQuestionViewModelConverter)
    {
      searchText = string.Empty;
      questionViewModels = questionToQuestionViewModelConverter.ConvertQuestions(questionaire.Questions);
      Matches = new QuestionViewModel[0];
      this.questionaire = questionaire;
      this.questionaireMatcher = questionaireMatcher;
      this.questionToQuestionViewModelConverter = questionToQuestionViewModelConverter;
      searchMode = SearchMode.Combined;
      searchDelayTimer = new DispatcherTimer();
      searchDelayTimer.Interval = TimeSpan.FromMilliseconds(500);
      searchDelayTimer.Tick += SearchDelayTimer_Tick;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public string SearchText
    {
      get => searchText;
      set
      {
        if (value != searchText)
        {
          searchText = value;
          ResetSeachDelayTimer();
        }
      }
    }

    public bool OnlyQuestionsSelected
    {
      get => searchMode == SearchMode.QuestionsOnly;
      set
      {
        if (value)
        {
          searchMode = SearchMode.QuestionsOnly;
          PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OnlyQuestionsSelected)));
          PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OnlyAnswersSelected)));
          PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CombinedSelected)));
          UpdateMatches();
        }
      }
    }

    public bool OnlyAnswersSelected
    {
      get => searchMode == SearchMode.AnswersOnly;
      set
      {
        if (value)
        {
          searchMode = SearchMode.AnswersOnly;
          PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OnlyQuestionsSelected)));
          PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OnlyAnswersSelected)));
          PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CombinedSelected)));
          UpdateMatches();
        }
      }
    }

    public bool CombinedSelected
    {
      get => searchMode == SearchMode.Combined;
      set
      {
        if (value)
        {
          searchMode = SearchMode.Combined;
          PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OnlyQuestionsSelected)));
          PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(OnlyAnswersSelected)));
          PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CombinedSelected)));
          UpdateMatches();
        }
      }
    }

    public QuestionViewModel[] Matches { get; set; }

    private void SearchDelayTimer_Tick(object? sender, EventArgs e)
    {
      searchDelayTimer.Stop();
      UpdateMatches();
    }

    private void ResetSeachDelayTimer()
    {
      searchDelayTimer.Stop();
      searchDelayTimer.Start();
    }

    private void UpdateMatches()
    {
      if (searchText.Length < 4)
        return;

      Matches = questionViewModels.Where(questionViewModel => questionaireMatcher.Matches(searchText, questionViewModel.Question, searchMode)).ToArray();
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Matches)));
    }
  }
}