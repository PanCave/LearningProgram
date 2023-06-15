using LearningProgram.BO;

namespace LearningProgram.BL
{
  public class QuestionaireMatcher
  {
    public bool Matches(string query, Question question, SearchMode searchMode)
    {
      if ((searchMode == SearchMode.QuestionsOnly || searchMode == SearchMode.Combined) &&
        question.QuestionText.ToLower().Contains(query.ToLower()))
        return true;
      else if ((searchMode == SearchMode.AnswersOnly || searchMode == SearchMode.Combined) &&
        question.Answers.Any(answer => answer.ToLower().Contains(query.ToLower())))
        return true;
      return false;
    }
  }
}