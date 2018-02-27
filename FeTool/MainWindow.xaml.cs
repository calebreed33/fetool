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

namespace FeTool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            StackPanel1.DataContext = new ExpanderListViewModel();
        }

        private void LogoutClick(object sender, RoutedEventArgs e)
        {
            
            LoginScreen window = new LoginScreen();
            this.Close();
            window.ShowDialog();
        }
        private void HistoryClick(object sender, RoutedEventArgs e)
        {

            CommentHistory window = new CommentHistory();
            window.ShowDialog();
        }
        private void ImportBaselineClick(object sender, RoutedEventArgs e)
        {
            Process process = new Process();
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.FileName = @"C:\";
            process.Start();
        }
        private void ImportTestClick(object sender, RoutedEventArgs e)
        {
            Process process = new Process();
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.FileName = @"C:\";
            process.Start();
        }
    }
}
