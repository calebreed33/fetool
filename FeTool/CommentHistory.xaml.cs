using FeTool.ViewModels;
using System.Windows;
using System.Windows.Controls;
namespace FeTool
{
    /// <summary>
    /// Interaction logic for CommentHistory.xaml
    /// </summary>

    public partial class CommentHistory : Window
    {
        private ListView listview1 = new ListView();

        public CommentHistory()
        {
            InitializeComponent();
            this.DataContext = new CommentHistoryVM();
        }

        private void user_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            (this.DataContext as CommentHistoryVM).FilteredComments.Refresh();
        }

    }
}

