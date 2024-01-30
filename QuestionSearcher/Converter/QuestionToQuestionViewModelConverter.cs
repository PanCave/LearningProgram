using LearningProgram.BO;
using QuestionSearcher.ViewModels;
using System.Collections.Generic;

namespace QuestionSearcher.Converter
{
  internal class QuestionToQuestionViewModelConverter
  {
    public QuestionViewModel[] ConvertQuestionaireCatalog(QuestionaireCatalog questionaireCatalog)
    {
      List<QuestionViewModel> questionViewModels = new List<QuestionViewModel>();

      foreach (Questionaire questionaire in questionaireCatalog.Questionaires)
      {
        foreach (Question question in questionaire.Questions)
        {
          questionViewModels.Add(Convert(question));
        }
      }

      return questionViewModels.ToArray();
    }

    private QuestionViewModel Convert(Question question)
    {
      AnswerViewModel[] answerViewModels = new AnswerViewModel[question.Answers.Length];
      HashSet<int> indexSet = new HashSet<int>(question.RealAnswerIndexes);
      for (int i = 0; i < question.Answers.Length; i++)
      {
        answerViewModels[i] = new AnswerViewModel(question.Answers[i], indexSet.Contains(i));
      }
      return new QuestionViewModel(question, answerViewModels);
    }
  }
}