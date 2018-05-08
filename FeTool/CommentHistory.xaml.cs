using FeTool.ViewModels;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SQLite;
using System.IO;
using System.Collections;
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

