using LearningProgram.BO;

namespace QuestionSearcher.ViewModels
{
  internal class QuestionViewModel
  {
    public QuestionViewModel(Question question, AnswerViewModel[] answers)
    {
      QuestionText = question.QuestionText;
      Question = question;
      Answers = answers;
      Explanation = question.Explanation;
    }

    public string QuestionText { get; }
    public Question Question { get; }
    public AnswerViewModel[] Answers { get; }
    public string Explanation { get; }
  }
}