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
using System.Windows.Shapes;

namespace Lab1
{
    /// <summary>
    /// Логика взаимодействия для TableInputDialog.xaml
    /// </summary>
    public partial class TableInputDialog : Window
    {
        public int Rows { get; private set; }
        public int Columns { get; private set; }

        public TableInputDialog()
        {
            InitializeComponent();
        }

        private void Button_OK_Click(object sender, RoutedEventArgs e)
        {
            Rows = int.Parse(TextBoxRows.Text);
            Columns = int.Parse(TextBoxColumns.Text);
            DialogResult = true;
            Close();
        }

        private void Button_Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
