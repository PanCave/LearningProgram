using LearningProgram.BL;
using LearningProgram.BO;
using Microsoft.Win32;
using System;
using System.Windows;
using System.Windows.Input;

namespace QuestionSearcher.Commands
{
  public class AddQuestionaireCommand : ICommand
  {
    private readonly QuestionaireCatalog questionaireCatalog;
    private readonly QuestionaireLoader questionaireLoader;

    public AddQuestionaireCommand(QuestionaireCatalog questionaireCatalog, QuestionaireLoader questionaireLoader)
    {
      this.questionaireCatalog = questionaireCatalog;
      this.questionaireLoader = questionaireLoader;
    }

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter) => true;

    public void Execute(object? parameter)
    {
      OpenFileDialog openFileDialog = new OpenFileDialog();

      // Set the filter to display only JSON files.
      openFileDialog.Filter = "JSON Files (*.json)|*.json|All Files (*.*)|*.*";

      // Set initial directory (optional).
      openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

      // Set the title of the dialog box.
      openFileDialog.Title = "Open JSON File";

      // Allow multiple file selections (optional).
      openFileDialog.Multiselect = false;

      // Show the dialog and check if the user clicked the "OK" button.
      if (openFileDialog.ShowDialog() == true)
      {
        try
        {
          // Get the selected file name and read its contents as JSON.
          string selectedFilePath = openFileDialog.FileName;
          questionaireCatalog.Questionaires.Add(questionaireLoader.LoadQuestionare(selectedFilePath));
        }
        catch (Exception ex)
        {
          MessageBox.Show($"Fehler beim Öffnen der Datei: {ex.Message}");
        }
      }
    }
  }
}