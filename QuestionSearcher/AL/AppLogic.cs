using LearningProgram.BL;
using LearningProgram.BO;
using QuestionSearcher.Commands;
using QuestionSearcher.Converter;
using QuestionSearcher.ViewModels;
using System;
using System.IO;
using System.Windows.Input;

namespace QuestionSearcher.AL
{
  internal class AppLogic
  {
    public AppLogic()
    {
      QuestionaireCatalog questionaireCatalog = new QuestionaireCatalog();
      QuestionaireLoader questionaireLoader = new QuestionaireLoader();
      questionaireCatalog.Questionaires.Add(questionaireLoader.LoadQuestionare(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "psm_II_questions_big.json")));
      QuestionaireMatcher questionaireMatcher = new QuestionaireMatcher();
      QuestionToQuestionViewModelConverter questionToQuestionViewModelConverter = new QuestionToQuestionViewModelConverter();

      ICommand addQuestionaireCommand = new AddQuestionaireCommand(questionaireCatalog, questionaireLoader);

      MainViewModel = new MainViewModel(questionaireCatalog, questionaireMatcher, questionToQuestionViewModelConverter, addQuestionaireCommand);
    }

    public MainViewModel MainViewModel { get; }
  }
}