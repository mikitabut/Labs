using Microsoft.Win32;
using System;
using System.Collections;
using System.IO;
using System.Linq;
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
                bytes = FillFinishBytes(bytes);
                //BitArray qwe =

            }
        }

        private byte[] FillFinishBytes(byte[] bytes)
        {
            long length = bytes.LongLength;
            var lengthInBytes = BitConverter.GetBytes(bytes.LongLength);
            var additionalBits = GetAdditionalBits(bytes.Length % 64);
        }

        private byte[] GetAdditionalBits(int lastFragmentBytesAmount)
        {
            byte[] firstByte = BitConverter.GetBytes(128); //1000 0000
            byte[] zeroByte = BitConverter.GetBytes(0);    //0000 0000
            byte[] result;
            if (lastFragmentBytesAmount < 56)
            {
                result = firstByte;
                result.Concat(Enumerable.Repeat(zeroByte, 2).ToArray());
            }
        }
    }
}
