using LearningProgram.BO;
using Newtonsoft.Json;
using System.Text;

namespace LearningProgram.Persistence.Os
{
  public class JsonSaver
  {
    public void SaveQuestionsAsJson(Question[] questions, string filepath)
    {
      StringBuilder sb = new StringBuilder();
      StringWriter stringWriter = new StringWriter(sb);
      using (JsonWriter w = new JsonTextWriter(stringWriter))
      {
        w.Formatting = Formatting.Indented;

        w.WriteStartArray();

        foreach (var q in questions)
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
      }

      File.WriteAllText(filepath, sb.ToString());
    }
  }
}