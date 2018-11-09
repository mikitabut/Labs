using Microsoft.Win32;
using System.IO;
using System.Windows;

namespace lab3
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

        private void CalculateSHA1_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                byte[] bytes = File.ReadAllBytes(openFileDialog.FileName);


            }
        }
    }
}
