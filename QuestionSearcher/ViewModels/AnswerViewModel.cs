namespace QuestionSearcher.ViewModels
{
  internal class AnswerViewModel
  {
    public AnswerViewModel(string answerText, bool isCorrect)
    {
      AnswerText = answerText;
      IsCorrect = isCorrect;
    }

    public bool IsCorrect { get; }
    public string AnswerText { get; }
  }
}