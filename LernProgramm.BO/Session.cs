namespace LearningProgram.BO
{
  public class Session
  {
    public Session(Guid guid, Questionaire questionaire)
    {
      Guid = guid;
      Questionaire = questionaire;
      QuestionIndex = 0;
      WrongQuestionIndexes = new();
    }

    public Session(Guid guid, int questionIndex, Questionaire questionaire, List<int> wrongQuestionIndexes)
    {
      Guid = guid;
      QuestionIndex = questionIndex;
      Questionaire = questionaire;
      WrongQuestionIndexes = wrongQuestionIndexes;
    }

    public Guid Guid { get; }
    public int QuestionIndex { get; set; }
    public Questionaire Questionaire { get; }
    public List<int> WrongQuestionIndexes { get; }
  }
}