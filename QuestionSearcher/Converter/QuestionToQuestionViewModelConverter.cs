using LearningProgram.BO;
using QuestionSearcher.ViewModels;
using System.Collections.Generic;

namespace QuestionSearcher.Converter
{
  internal class QuestionToQuestionViewModelConverter
  {
    public QuestionViewModel[] ConvertQuestions(Question[] questions)
    {
      QuestionViewModel[] questionViewModels = new QuestionViewModel[questions.Length];

      for (int i = 0; i < questions.Length; i++)
      {
        questionViewModels[i] = Convert(questions[i]);
      }

      return questionViewModels;
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