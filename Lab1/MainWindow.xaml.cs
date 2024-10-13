using System;
using System.Collections.Generic;
using System.IO;
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

namespace Lab1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        

        private string currentFilePath; 

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(currentFilePath))
            {
                
                SaveAs_Click(sender, e);
            }
            else
            {
                
                TextRange textRange = new TextRange(textEditor.Document.ContentStart, textEditor.Document.ContentEnd);

                
                using (FileStream fileStream = new FileStream(currentFilePath, FileMode.Create))
                {
                    textRange.Save(fileStream, DataFormats.Rtf); 
                }

                MessageBox.Show("Файл збережено.");
            }
        }

        private void SaveAs_Click(object sender, RoutedEventArgs e)
        {
            
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = "Rich Text Format (*.rtf)|*.rtf|Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                Title = "Зберегти файл як"
            };

            
            if (saveFileDialog.ShowDialog() == true)
            {
                
                currentFilePath = saveFileDialog.FileName; 

                
                TextRange textRange = new TextRange(textEditor.Document.ContentStart, textEditor.Document.ContentEnd);

                
                using (FileStream fileStream = new FileStream(currentFilePath, FileMode.Create))
                {
                    textRange.Save(fileStream, DataFormats.Rtf); 
                }

                MessageBox.Show("Файл збережено як: " + currentFilePath);
            }
        }

        private void Print_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();

            
            if (printDialog.ShowDialog() == true)
            {
                
                printDialog.PrintVisual(textEditor as Visual, "Друк вмісту редактора");
               
            }
        }


        private void InsertImage_Click(object sender, RoutedEventArgs e)
        {
            
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";

            bool? result = dlg.ShowDialog();

            if (result == true)
            {
                string filename = dlg.FileName;

                
                Image img = new Image();
                img.Source = new BitmapImage(new Uri(filename));

                
                textEditor.Document.Blocks.Add(new BlockUIContainer(img));
            }
        }



        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            
            if (textEditor.Selection.Text.Length > 0)
            {
               
                Clipboard.SetText(textEditor.Selection.Text);
            }
        }

        private void Cut_Click(object sender, RoutedEventArgs e)
        {
           
            if (textEditor.Selection.Text.Length > 0)
            {
                
                Clipboard.SetText(textEditor.Selection.Text);

                
                TextRange textRange = new TextRange(textEditor.Selection.Start, textEditor.Selection.End);
                textRange.Text = string.Empty; 
            }
        }

        private void Paste_Click(object sender, RoutedEventArgs e)
        {
            
            if (Clipboard.ContainsText())
            {
                
                string text = Clipboard.GetText();

                
                textEditor.CaretPosition.InsertTextInRun(text); 
            }
        }

        private void ColorButton_Click(object sender, RoutedEventArgs e)
        {
            
            Button clickedButton = sender as Button;

            
            string selectedColor = clickedButton.Tag.ToString();

            
            TextRange selectedText = new TextRange(textEditor.Selection.Start, textEditor.Selection.End);

            
            SolidColorBrush brush = new SolidColorBrush((Color)ColorConverter.ConvertFromString(selectedColor));

            
            selectedText.ApplyPropertyValue(TextElement.ForegroundProperty, brush);

            
            ColorPopup.IsOpen = false;
        }

        private void OpenColorPicker_Click(object sender, RoutedEventArgs e)
        {
            ColorPopup.IsOpen = true; 
        }


        private void InsertTable_Click(object sender, RoutedEventArgs e)
        {
            
            TableInputDialog inputDialog = new TableInputDialog();

            if (inputDialog.ShowDialog() == true)
            {
                int rows = inputDialog.Rows;
                int columns = inputDialog.Columns;

                
                Table table = new Table();

                
                for (int i = 0; i < columns; i++)
                {
                    table.Columns.Add(new TableColumn());
                }

               
                for (int i = 0; i < rows; i++)
                {
                    TableRow row = new TableRow();
                    for (int j = 0; j < columns; j++)
                    {
                        TableCell cell = new TableCell(new Paragraph(new Run(" ")));
                        cell.BorderBrush = Brushes.Black; 
                        cell.BorderThickness = new Thickness(1);  
                        row.Cells.Add(cell);
                    }
                    TableRowGroup trg = new TableRowGroup();
                    trg.Rows.Add(row);
                    table.RowGroups.Add(trg);
                }

                
                textEditor.Document.Blocks.Add(table);
            }
        }
       




        private void InsertShape_Click(object sender, RoutedEventArgs e)
        {
            ShapeSelectionWindow shapeSelectionWindow = new ShapeSelectionWindow();

            if (shapeSelectionWindow.ShowDialog() == true)
            {
                Shape shape = null;

                if (shapeSelectionWindow.SelectedShape == "Rectangle")
                {
                   
                    shape = new Rectangle
                    {
                        Width = 100,
                        Height = 50,
                        Fill = Brushes.Blue
                    };
                }
                else if (shapeSelectionWindow.SelectedShape == "Circle")
                {
                    
                    shape = new Ellipse
                    {
                        Width = 100,
                        Height = 100,
                        Fill = Brushes.Green
                    };
                }

                
                if (shape != null)
                {
                    textEditor.Document.Blocks.Add(new BlockUIContainer(shape));
                }
            }

        }


        private void Open_Click(object sender, RoutedEventArgs e)
        {
            
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Rich Text Format (*.rtf)|*.rtf|Text Files (*.txt)|*.txt|All Files (*.*)|*.*",
                Title = "Відкрити файл"
            };

            
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;

                
                using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
                {
                    textEditor.Document.Blocks.Clear(); 
                    TextRange textRange = new TextRange(textEditor.Document.ContentStart, textEditor.Document.ContentEnd);
                    textRange.Load(fileStream, DataFormats.Rtf); 
                }

               
                currentFilePath = filePath; 
            }
        }

        private void FontComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string selectedFont = selectedItem.Content.ToString();
                textEditor.FontFamily = new FontFamily(selectedFont);
            }
        }

        private void FontSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox comboBox && comboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                double selectedSize = Convert.ToDouble(selectedItem.Content);
                textEditor.FontSize = selectedSize;
            }
        }

        private void Bold_Click(object sender, RoutedEventArgs e)
        {
            if (textEditor.Selection.GetPropertyValue(Inline.FontWeightProperty) is FontWeight currentWeight && currentWeight != FontWeights.Bold)
            {
                textEditor.Selection.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Bold);
            }
            else
            {
                textEditor.Selection.ApplyPropertyValue(Inline.FontWeightProperty, FontWeights.Normal);
            }
        }

        private void Italic_Click(object sender, RoutedEventArgs e)
        {
           
            var currentStyle = textEditor.Selection.GetPropertyValue(Inline.FontStyleProperty);

            
            if (currentStyle is FontStyle style && style != FontStyles.Italic)
            {
                textEditor.Selection.ApplyPropertyValue(Inline.FontStyleProperty, FontStyles.Italic);
            }
            else 
            {
                textEditor.Selection.ApplyPropertyValue(Inline.FontStyleProperty, FontStyles.Normal);
            }
        }


        private void Underline_Click(object sender, RoutedEventArgs e)
        {
            
            var currentDecoration = textEditor.Selection.GetPropertyValue(Inline.TextDecorationsProperty);

            
            if (currentDecoration != TextDecorations.Underline)
            {
                textEditor.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
            }
            else 
            {
                textEditor.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, null); 
            }
        }


        private void AlignLeft_Click(object sender, RoutedEventArgs e)
        {
            textEditor.Selection.ApplyPropertyValue(Paragraph.TextAlignmentProperty, TextAlignment.Left);
        }

        private void AlignCenter_Click(object sender, RoutedEventArgs e)
        {
            textEditor.Selection.ApplyPropertyValue(Paragraph.TextAlignmentProperty, TextAlignment.Center);
        }

        private void AlignRight_Click(object sender, RoutedEventArgs e)
        {
            textEditor.Selection.ApplyPropertyValue(Paragraph.TextAlignmentProperty, TextAlignment.Right);
        }

        private void AlignJustify_Click(object sender, RoutedEventArgs e)
        {
            textEditor.Selection.ApplyPropertyValue(Paragraph.TextAlignmentProperty, TextAlignment.Justify);
        }

        private void BulletList_Click(object sender, MouseButtonEventArgs e)
        {
           
            if (textEditor.Selection.Text.Length > 0)
            {
                
                AddBulletList(textEditor.Selection.Text);
            }
            else
            {
                
                textEditor.AppendText("Новий пункт списку...\n");
                textEditor.CaretPosition = textEditor.Document.ContentEnd;
            }
        }

        private void AddBulletList(string selectedText)
        {
            
            Paragraph paragraph = new Paragraph();
            List list = new List();

            ListItem listItem = new ListItem(new Paragraph(new Run(selectedText)));
            list.ListItems.Add(listItem);

          
            textEditor.Document.Blocks.Add(list);
        }
    }
}
