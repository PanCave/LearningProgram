using System.Text;

namespace LearningProgram.BO
{
  public class Question : IQuestion
  {
    private readonly StringBuilder stringBuilder;

    public Question(string questionText, string[] answers, string[] realAnswers, int[] realAnswerIndexes, string explanation)
    {
      QuestionText = questionText;
      Answers = answers;
      RealAnswers = realAnswers;
      RealAnswerIndexes = realAnswerIndexes;
      Explanation = explanation;
      stringBuilder = new StringBuilder();
    }

    public string QuestionText { get; }
    public string[] Answers { get; }
    public string[] RealAnswers { get; }
    public int[] RealAnswerIndexes { get; }
    public string Explanation { get; }

    public string GetFormattedQuestionText()
    {
      stringBuilder.Clear();
      stringBuilder.Append(QuestionText);
      if (RealAnswers.Length > 1)
      {
        stringBuilder.Append(Environment.NewLine);
        stringBuilder.Append(Environment.NewLine);
        stringBuilder.Append($"(Choose {RealAnswers.Length} answers)");
      }

      return stringBuilder.ToString();
    }

    public void ShuffleAnswers(Random random)
    {
      int n = Answers.Length;
      while (n > 1)
      {
        int k = random.Next(n--);
        string temp = Answers[n];
        Answers[n] = Answers[k];
        Answers[k] = temp;
      }
    }
  }
}