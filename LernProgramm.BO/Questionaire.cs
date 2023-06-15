namespace LearningProgram.BO
{
  public class Questionaire
  {
    public Questionaire(Question[] questions)
    {
      Questions = questions;
    }

    public Question[] Questions { get; }
    public Question this[int index] => Questions[index];

    public void ShuffleQuestions(Random random)
    {
      int n = Questions.Length;
      while (n > 1)
      {
        int k = random.Next(n--);
        Question temp = Questions[n];
        Questions[n] = Questions[k];
        Questions[k] = temp;
      }
    }
  }
}