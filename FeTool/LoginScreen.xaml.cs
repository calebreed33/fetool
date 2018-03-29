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
    public static class globalvariables{
            public static List<string> DatabaseLocations = new List<string>();
    }

    public partial class LoginScreen : Window
    {
        private ComboBox comboBox1 = new ComboBox();
        private TextBox PasswordBox = new TextBox();
        public char PasswordChar { get; set; }

        public LoginScreen()
        {
            InitializeComponent();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            comboBox1.Name = "Combobox1";
            comboBox1.Background = SystemColors.MenuBrush;
            comboBox1.Items.Add("test1");
        }

        private void ImportData(object sender, RoutedEventArgs e)
        {
            // Create OpenFileDialog
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Set filter for file extension and default file extension
            dlg.DefaultExt = ".db";
            dlg.Filter = "SQLite DB (*.db)|*.db";

            // Display OpenFileDialog by calling ShowDialog method
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                // Save to global variable
                globalvariables.DatabaseLocations.Add(dlg.FileName);

                //TODO: Populate username/password fields with DB data

                // Verify the database connection/location
                string messageBoxText = "Connected to:";
                foreach (string databaseitem in globalvariables.DatabaseLocations) {
                    messageBoxText = messageBoxText + Environment.NewLine + databaseitem.Split('\\').Last();
                }

                string caption = "New DB Connection";
                MessageBoxButton button = MessageBoxButton.OK;
                MessageBoxImage icon = MessageBoxImage.Information;
                MessageBox.Show(messageBoxText, caption, button, icon);
            }
        }

        private void LoginClick(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            this.Close();
            window.ShowDialog();
        }
    }
}