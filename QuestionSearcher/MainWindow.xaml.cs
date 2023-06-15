using QuestionSearcher.AL;
using System.Windows;

namespace QuestionSearcher
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();
      AppLogic appLogic = new AppLogic();
      DataContext = appLogic.MainViewModel;
    }
  }
}