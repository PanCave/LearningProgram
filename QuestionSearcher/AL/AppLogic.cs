using LearningProgram.BL;
using LearningProgram.BO;
using QuestionSearcher.Converter;
using QuestionSearcher.ViewModels;

namespace QuestionSearcher.AL
{
  internal class AppLogic
  {
    public AppLogic()
    {
      QuestionaireLoader questionaireLoader = new QuestionaireLoader();
      Questionaire questionaire = questionaireLoader.LoadQuestionare(@"C:\Users\j.freiny\Desktop\LernProgramm\psm_II_questions_big.json");
      QuestionaireMatcher questionaireMatcher = new QuestionaireMatcher();
      QuestionToQuestionViewModelConverter questionToQuestionViewModelConverter = new QuestionToQuestionViewModelConverter();

      MainViewModel = new MainViewModel(questionaire, questionaireMatcher, questionToQuestionViewModelConverter);
    }

    public MainViewModel MainViewModel { get; }
  }
}