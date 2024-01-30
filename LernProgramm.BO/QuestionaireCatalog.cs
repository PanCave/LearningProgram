using System.Collections.ObjectModel;

namespace LearningProgram.BO
{
  public class QuestionaireCatalog
  {
    public QuestionaireCatalog()
    {
      Questionaires = new();
    }

    public ObservableCollection<Questionaire> Questionaires { get; }
  }
}