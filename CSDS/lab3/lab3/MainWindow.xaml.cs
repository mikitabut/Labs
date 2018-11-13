using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
        public MainWindow() => InitializeComponent();

        private void CalculateSHA1_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                byte[] bytes = File.ReadAllBytes(openFileDialog.FileName);
                bytes = FillFinishBytes(bytes);

                string path = Path.Combine(Path.GetDirectoryName(openFileDialog.FileName), "SHA-1 results.txt");
                File.AppendAllText(path, openFileDialog.SafeFileName + " - " + DateTime.Now + " - " + 
                    BitConverter.ToString(bytes).Replace("-", String.Empty) + Environment.NewLine);
            }
        }

        private byte[] FillFinishBytes(byte[] bytes)
        {
            var lengthInBytes = BitConverter.GetBytes(bytes.LongLength).Reverse();
            var additionalBits = GetAdditionalBits(bytes.Length % 64);
            var result = bytes.ToList();
            result.AddRange(additionalBits);
            result.AddRange(lengthInBytes);
            return result.ToArray();
        }

        private byte[] GetAdditionalBits(int lastFragmentBytesAmount)
        {
            byte firstByte = BitConverter.GetBytes(128).First(); //1000 0000
            byte zeroByte = BitConverter.GetBytes(0).First();    //0000 0000
            List<byte> result = new List<byte>();
            if (lastFragmentBytesAmount < 56)
            {
                result.Add(firstByte);
                result.AddRange(Enumerable.Repeat(zeroByte, 55 - lastFragmentBytesAmount));
            }
            else if (lastFragmentBytesAmount > 56)
            {
                result.Add(firstByte);
                result.AddRange(Enumerable.Repeat(zeroByte, 119 - lastFragmentBytesAmount));
            }

            return result.ToArray();
        }
    }
}
