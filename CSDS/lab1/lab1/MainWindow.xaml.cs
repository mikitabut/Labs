using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;

namespace lab1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public const int LettersInAlphabet = 26;
        public const int A_IndexInASCII = 65;
        public const int Z_IndexInASCII = 90;
        public const int a_IndexInASCII = 97;
        public const int z_IndexInASCII = 122;
        public const int LongestWordLength = 20;

        public string Text { get; set; }

        public string CurrentFileName { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                CurrentFileName = openFileDialog.FileName;
                Text = File.ReadAllText(CurrentFileName);
            }
        }

        private void EncodeCeasar_Click(object sender, RoutedEventArgs e)
        {
            File.WriteAllText(AddStringToFileName(CurrentFileName, $"_Ceasar({Shift.Text})"), 
                GetCeasarEncodedText(Text, Convert.ToInt32(Shift.Text) % LettersInAlphabet));
        }

        private void EncodeVigenere_Click(object sender, RoutedEventArgs e)
        {
            File.WriteAllText(AddStringToFileName(CurrentFileName, $"_Vigenere({SecretWord.Text})"),
                GetVigenereEncodedText(Text, SecretWord.Text.ToLower()));
        }

        private void DecodeVigenere_Click(object sender, RoutedEventArgs e)
        {
            string trimmedText = Regex.Replace(Text, "[^A-Za-z]", String.Empty).ToLower();
            var words = new List<Word>();
            for (int wordLength = 2; wordLength < 5; wordLength++)
            {
                for (int i = 0; i <= trimmedText.Length - i; i++)
                {
                    string word = trimmedText.Substring(i, wordLength);
                    var currentWord = words.FirstOrDefault(x => x.Name == word);
                    if (currentWord == null)
                    {
                        words.Add(new Word(word, i));
                    }
                    else
                    {
                        currentWord.AddEntryIndex(i);
                    }
                }
            }

            var frequentWords = words.Where(x => x.EntryIndexes.Count > 5).OrderBy(x => x.GetMostFrequentDistance());
            var mostFrequentDistance = frequentWords.Max(x => x.GetMostFrequentDistance());
            MessageBox.Show($"Intended secret word length is {mostFrequentDistance}", "Info", MessageBoxButton.OK);
        }

        private string AddStringToFileName(string fileName, string line) =>
            Path.Combine(Path.GetDirectoryName(fileName), 
                Path.GetFileNameWithoutExtension(fileName) + line + Path.GetExtension(fileName));

        private string GetCeasarEncodedText(string text, int shift)
        {
            var builder = new StringBuilder();
            foreach (var c in Encoding.ASCII.GetBytes(text))
            {
                builder.Append(GetCeasarEncodedChar(c, shift));
            }

            return builder.ToString();
        }

        private string GetVigenereEncodedText(string text, string secretWord)
        {
            var builder = new StringBuilder();
            var secretWordBytes = Encoding.ASCII.GetBytes(secretWord);
            int secretWordIndex = 0;
            foreach (var c in Encoding.ASCII.GetBytes(text))
            {
                if (Char.IsLetter((char)c))
                {
                    builder.Append(GetCeasarEncodedChar(c, secretWordBytes[secretWordIndex] - a_IndexInASCII));
                    secretWordIndex = (secretWordIndex == secretWord.Length - 1) ? 0 : secretWordIndex + 1;
                }
                else
                {
                    builder.Append((char)c);
                }
            }

            return builder.ToString();
        }

        private char GetCeasarEncodedChar(byte c, int shift)
        {
            if (c >= A_IndexInASCII && c <= Z_IndexInASCII)
            {
                return (char)(A_IndexInASCII + ((c - A_IndexInASCII + shift) % LettersInAlphabet));
            }
            else if (c >= a_IndexInASCII && c <= z_IndexInASCII)
            {
                return (char)(a_IndexInASCII + ((c - a_IndexInASCII + shift) % LettersInAlphabet));
            }
            else
            {
                return (char)c;
            }
        }

        private class Word
        {
            public string Name { get; set; }

            public List<int> EntryIndexes { get; set; } = new List<int>();

            public Word(string name, int firstEntryIndex)
            {
                Name = name;
                EntryIndexes.Add(firstEntryIndex);
            }

            public void AddEntryIndex(int newEntryIndex) => EntryIndexes.Add(newEntryIndex);

            public int GetMostFrequentDistance()
            {
                var distances = GetDistances();
                int mostFrequentDistance = 1;
                int mostFrequentDistanceAmount = 0;
                for (int wordLength = 2; wordLength < LongestWordLength; wordLength++)
                {
                    int amount = distances.Count(x => x % wordLength == 0);
                    if (amount > mostFrequentDistanceAmount)
                    {
                        mostFrequentDistanceAmount = amount;
                        mostFrequentDistance = wordLength;
                    }
                }

                return mostFrequentDistance;
            }

            private IEnumerable<int> GetDistances()
            {
                for (int i = 1; i < EntryIndexes.Count; i++)
                {
                    yield return EntryIndexes[i] - EntryIndexes[i - 1];
                }
            }
        }
    }
}
