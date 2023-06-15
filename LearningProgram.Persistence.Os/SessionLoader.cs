using LearningProgram.BO;
using Newtonsoft.Json.Linq;

namespace LearningProgram.Persistence.Os
{
  public class SessionLoader
  {
    private readonly JsonLoader jsonLoader;

    public SessionLoader()
    {
      jsonLoader = new JsonLoader();
    }

    public Session LoadSession(string filepath)
    {
      if (!File.Exists(filepath))
        throw new FileNotFoundException(filepath);

      string json = File.ReadAllText(filepath);
      if (string.IsNullOrEmpty(json))
        throw new ArgumentException($"Invalid or empty content");

      JObject sess = JObject.Parse(json);

      Guid guid = (Guid)sess["Guid"];
      int questionIndex = (int)sess["QuestionIndex"];
      List<int> wrongQuestionIndexes = sess["WrongQuestionIndexes"].Select(x => (int)x).ToList();
      Questionaire questionaire = jsonLoader.LoadQuestionaireFromJson((JArray)sess["Questionaire"]);

      return new Session(guid, questionIndex, questionaire, wrongQuestionIndexes);
    }
  }
}