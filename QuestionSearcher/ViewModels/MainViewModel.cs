using LearningProgram.BL;
using LearningProgram.BO;
using QuestionSearcher.Converter;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using System.Windows.Threading;

namespace QuestionSearcher.ViewModels
{
  internal class MainViewModel : INotifyPropertyChanged
  {
    private readonly QuestionaireCatalog questionaireCatalog;
    private readonly Questionaire questionaire;
    private readonly QuestionaireMatcher questionaireMatcher;
    private readonly QuestionToQuestionViewModelConverter questionToQuestionViewModelConverter;
    private readonly DispatcherTimer searchDelayTimer;
    private QuestionViewModel[] questionViewModels;
    private string searchText;
    private SearchMode searchMode;

    public MainViewModel(
      QuestionaireCatalog questionaireCatalog,
      QuestionaireMatcher questionaireMatcher,
      QuestionToQuestionViewModelConverter questionToQuestionViewModelConverter,
      ICommand addQuestionareCommand)
    {
      searchText = string.Empty;
      questionViewModels = questionToQuestionViewModelConverter.ConvertQuestionaireCatalog(questionaireCatalog);
      Matches = new QuestionViewModel[0];
      this.questionaireCatalog = questionaireCatalog;
      questionaireCatalog.Questionaires.CollectionChanged += Questionaires_CollectionChanged;
      this.questionaire = questionaire;
      this.questionaireMatcher = questionaireMatcher;
      this.questionToQuestionViewModelConverter = questionToQuestionViewModelConverter;
      AddQuestionareCommand = addQuestionareCommand;
      searchMode = SearchMode.Combined;
      searchDelayTimer = new DispatcherTimer();
      searchDelayTimer.Interval = TimeSpan.FromMilliseconds(500);
      searchDelayTimer.Tick += SearchDelayTimer_Tick;
      CatalogNames = new ObservableCollection<string>();
      SetCatalogNames();
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

    public ObservableCollection<string> CatalogNames { get; set; }

    public QuestionViewModel[] Matches { get; set; }

    public ICommand AddQuestionareCommand { get; }

    private void SetCatalogNames()
    {
      CatalogNames.Clear();
      foreach (Questionaire questionaire in questionaireCatalog.Questionaires)
      {
        CatalogNames.Add(questionaire.Name);
      }
    }

    private void Questionaires_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
      questionViewModels = questionToQuestionViewModelConverter.ConvertQuestionaireCatalog(questionaireCatalog);
      SetCatalogNames();
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CatalogNames)));
    }

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