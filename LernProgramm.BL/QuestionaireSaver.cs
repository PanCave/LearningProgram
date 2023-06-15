using LearningProgram.BO;
using LearningProgram.Persistence.Os;

namespace LearningProgram
{
  public class QuestionaireSaver
  {
    private readonly JsonSaver jsonSaver;
    private readonly PdfSaver pdfSaver;

    public QuestionaireSaver()
    {
      jsonSaver = new();
      pdfSaver = new();
    }

    public void SaveQuestions(Question[] questions, string filepath)
    {
      jsonSaver.SaveQuestionsAsJson(questions, filepath);
      pdfSaver.SaveQuestionsAsPdf(questions, filepath);
    }
  }
}