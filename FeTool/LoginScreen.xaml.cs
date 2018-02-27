using System;
using System.Collections.Generic;
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
    /// Interaction logic for LoginScreen.xaml
    /// </summary>
    public partial class LoginScreen : Window
    {
        ComboBox comboBox1 = new ComboBox();
        TextBox PasswordBox = new TextBox();
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

    }
}