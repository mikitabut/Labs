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

namespace NLP
{
    /// <summary>
    /// Interaction logic for ClassesWindow.xaml
    /// </summary>
    public partial class ClassesWindow : Window
    {
        public ClassesWindow()
        {
            InitializeComponent();
        }

        private int _picNumber = 0;
        private const int PicCount = 7;

        private void Window_Initialized(object sender, EventArgs e)
        {
            RefreshImage();
        }

        private void RefreshImage()
        {
            MainImage.Source = new BitmapImage(new Uri($"/resources/classes{_picNumber}.png", UriKind.Relative));
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            _picNumber--;

            if (_picNumber == 0)
            {
                _picNumber = PicCount - 1;
            }

            RefreshImage();
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            _picNumber++;

            if (_picNumber == PicCount)
            {
                _picNumber = 0;
            }

            RefreshImage();
        }
    }
}
