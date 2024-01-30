using LearningProgram.BO;
using LearningProgram.Persistence.Os;
using Newtonsoft.Json.Linq;

namespace LearningProgram.BL
{
  public class QuestionaireLoader
  {
    private readonly JsonLoader jsonLoader;

    public QuestionaireLoader()
    {
      jsonLoader = new JsonLoader();
    }

    public Questionaire LoadQuestionare(string filepath)
    {
      if (!File.Exists(filepath))
        throw new FileNotFoundException(filepath);

      string json = File.ReadAllText(filepath);
      if (string.IsNullOrEmpty(json))
        throw new ArgumentException($"Invalid or empty content");

      JArray questions = JArray.Parse(json);

      return jsonLoader.LoadQuestionaireFromJson(questions, Path.GetFileName(filepath));
    }
  }
}