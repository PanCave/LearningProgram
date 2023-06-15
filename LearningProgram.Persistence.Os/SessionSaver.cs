using LearningProgram.BO;
using Newtonsoft.Json;
using System.Text;

namespace LearningProgram.Persistence.Os
{
  public class SessionSaver
  {
    public void SaveSession(Session session, string filepath)
    {
      StringBuilder sb = new StringBuilder();
      StringWriter stringWriter = new StringWriter(sb);
      using (JsonWriter w = new JsonTextWriter(stringWriter))
      {
        w.Formatting = Formatting.Indented;

        w.WriteStartObject();
        w.WritePropertyName("Guid");
        w.WriteValue(session.Guid);
        w.WritePropertyName("QuestionIndex");
        w.WriteValue(session.QuestionIndex);
        w.WritePropertyName("WrongQuestionIndexes");
        w.WriteStartArray();

        foreach (int index in session.WrongQuestionIndexes)
        {
          w.WriteValue(index);
        }

        w.WriteEndArray();

        w.WritePropertyName("Questionaire");
        w.WriteStartArray();

        foreach (Question q in session.Questionaire.Questions)
        {
          w.WriteStartObject();

          w.WritePropertyName("QuestionType");
          w.WriteValue("MultipleChoice");

          w.WritePropertyName("Question");
          w.WriteValue(q.QuestionText);

          w.WritePropertyName("Answers");
          w.WriteStartArray();

          foreach (string answer in q.Answers)
          {
            w.WriteValue(answer);
          }

          w.WriteEndArray();

          w.WritePropertyName("CorrectAnswerIndexes");
          w.WriteStartArray();

          foreach (string realAnswer in q.RealAnswers)
          {
            w.WriteValue(q.Answers.Select((v, i) => new { v, i }).First(x => x.v.Equals(realAnswer)).i);
          }

          w.WriteEndArray();

          w.WritePropertyName("Explanation");
          w.WriteValue(q.Explanation);

          w.WriteEndObject();
        }
        w.WriteEndArray();
        w.WriteEndObject();
      }

      File.WriteAllText(filepath, sb.ToString());
    }
  }
}