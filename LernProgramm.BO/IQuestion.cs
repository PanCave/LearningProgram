namespace LearningProgram.BO
{
  public interface IQuestion
  {
    string[] Answers { get; }
    string Explanation { get; }
    string QuestionText { get; }
    string[] RealAnswers { get; }

    string GetFormattedQuestionText();
    void ShuffleAnswers(Random random);
  }
}