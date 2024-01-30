using LearningProgram.BO;
using Newtonsoft.Json.Linq;

namespace LearningProgram.Persistence.Os
{
  public class JsonLoader
  {
    public Questionaire LoadQuestionaireFromJson(JArray questions, string name)
    {
      List<Question> questionList = new List<Question>();
      foreach (var question in questions)
      {
        string questionText = (string)question["Question"];
        string[] answers = question["Answers"].ToObject<string[]>();
        int[] correctAnswersIndexes = question["CorrectAnswerIndexes"].ToObject<int[]>();
        string[] correctAnswers = new string[correctAnswersIndexes.Length];
        for (int i = 0; i < correctAnswersIndexes.Length; i++)
        {
          correctAnswers[i] = answers[correctAnswersIndexes[i]];
        }
        string explanation = string.Empty;
        try
        {
          explanation = (string)question["Explanation"];
        }
        catch (Exception)
        {
        }
        questionList.Add(new Question(questionText, answers, correctAnswers, correctAnswersIndexes, explanation));
      }

      return new Questionaire(name, questionList.ToArray());
    }
  }
}