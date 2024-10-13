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
    /// Логика взаимодействия для ShapeSelectionWindow.xaml
    /// </summary>
    public partial class ShapeSelectionWindow : Window
    {
        public string SelectedShape { get; private set; }
        public ShapeSelectionWindow()
        {
            InitializeComponent();
        }

        private void CircleButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedShape = "Circle";
            DialogResult = true; 
            Close();
        }
        private void RectangleButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedShape = "Rectangle";
            DialogResult = true; 
            Close();
        }
    }
}
