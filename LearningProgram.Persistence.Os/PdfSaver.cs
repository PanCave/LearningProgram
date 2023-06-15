using iTextSharp.text;
using iTextSharp.text.pdf;
using LearningProgram.BO;
using System.Text;

namespace LearningProgram.Persistence.Os
{
  public class PdfSaver
  {
    public void SaveQuestionsAsPdf(Question[] questions, string filepath)
    {
      StringBuilder stringBuilder = new();

      // Create a new document
      Document document = new Document();

      // Create a PdfWriter to write the document to a file
      PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filepath, FileMode.Create));

      // Open the document
      document.Open();

      // Add a suitable header
      Font headerFont = new Font(Font.FontFamily.HELVETICA, 14);
      Paragraph header = new Paragraph("Wrong Questions", headerFont);
      header.Alignment = Element.ALIGN_CENTER;
      document.Add(header);
      document.Add(new Paragraph(" "));

      // Create a table with two columns
      PdfPTable table = new PdfPTable(2);
      table.WidthPercentage = 110;

      // Set the width ratio of the table columns
      table.SetWidths(new float[] { 2f, 1f });

      Font smallFont = new Font(Font.FontFamily.HELVETICA, 8);
      Font smallBoldFont = new Font(Font.FontFamily.HELVETICA, 8, 3);
      BaseColor lightLightGray = new BaseColor(230, 230, 230);

      // Add the questions to the table
      for (int i = 0; i < questions.Length; i++)
      {
        Question question = questions[i];
        stringBuilder.Clear();
        int j = 97;
        foreach (string answer in question.Answers)
        {
          stringBuilder.AppendLine($"{(char)j}) {answer}");
          j++;
        }
        string answersText = stringBuilder.ToString();
        stringBuilder.Clear();
        j = 97;
        foreach (string realAnswer in question.RealAnswers)
        {
          stringBuilder.AppendLine($"{(char)j}) {realAnswer}");
          j++;
        }
        string realAnswersText = stringBuilder.ToString();
        // Add the question text and possible answers to the left column
        PdfPCell leftCell = new PdfPCell();
        leftCell.BorderWidthLeft = 0;
        leftCell.BorderWidthTop = 0;
        leftCell.BorderWidthRight = 1;
        leftCell.BorderWidthBottom = i == questions.Length - 1 ? 0 : 0.5f;
        leftCell.BackgroundColor = i % 2 == 0 ? BaseColor.WHITE : lightLightGray;
        leftCell.AddElement(new Paragraph(question.QuestionText, smallFont));
        leftCell.AddElement(new Paragraph(" "));
        leftCell.AddElement(new Paragraph(answersText, smallFont));
        table.AddCell(leftCell);

        // Add the real answers and explanation to the right column
        PdfPCell rightCell = new PdfPCell();
        rightCell.BorderWidthLeft = 0;
        rightCell.BorderWidthTop = 0;
        rightCell.BorderWidthRight = 0;
        rightCell.BorderWidthBottom = i == questions.Length - 1 ? 0 : 0.5f;
        rightCell.BackgroundColor = i % 2 == 0 ? BaseColor.WHITE : lightLightGray;
        rightCell.AddElement(new Paragraph(realAnswersText, smallFont));
        rightCell.AddElement(new Paragraph(" "));
        if (!string.IsNullOrEmpty(question.Explanation))
        {
          rightCell.AddElement(new Paragraph("Explanation:", smallBoldFont));
          rightCell.AddElement(new Paragraph(question.Explanation, smallFont));
        }
        table.AddCell(rightCell);
      }

      // Add the table to the document
      document.Add(table);

      // Close the document
      document.Close();
    }
  }
}