using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace NLP
{
    public partial class MainWindow : Window
    {
        DictionaryContext db = new DictionaryContext();
        public ObservableCollection<Word> WordDictionary { get; set; } = new ObservableCollection<Word>();

        public List<string> Files { get; set; } = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
            WordDictionary = new ObservableCollection<Word>(db.Words);
            DataContext = this;
        }

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog { Multiselect = true };
            if (openFileDialog.ShowDialog() == true)
            {
                Files.Add(openFileDialog.FileName);
                using (FileStream fstream = File.OpenRead(openFileDialog.FileName))
                {
                    byte[] array = new byte[fstream.Length];
                    fstream.Read(array, 0, array.Length);
                    string text = Encoding.UTF8.GetString(array);
                    string[] split = text.ToLower().Split(new char[] { ' ', '.', ',', ':', ';', '\t', '\n', '\r', '?', '!',
                        '"', '\'', '—', '[', ']', '(', ')', '%', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9'});
                    int counter = 0;
                    foreach (var name in split)
                    {
                        if (!String.IsNullOrWhiteSpace(name))
                        {
                            var currentWord = WordDictionary.FirstOrDefault(x => x.Name == name);
                            if (currentWord == null)
                            {
                                WordDictionary.Add(new Word(name, 1));
                            }
                            else
                            {
                                currentWord.Amount++;
                            }
                        }

                        counter++;
                        ProgressBar.Dispatcher.Invoke(() => ProgressBar.Value = counter * 100 / split.Length, DispatcherPriority.Background);
                    }

                    ClearWordsDatabase();
                    db.Words.AddRange(WordDictionary.ToList());
                    db.SaveChanges();
                    StatusLine.Text = $"{openFileDialog.FileName} has been parsed.";
                }
            }
        }

        private void AmountAsc_Click(object sender, RoutedEventArgs e) =>
            UpdateWordDictionary(WordDictionary.OrderBy(x => x.Amount).ToList());

        private void AmountDesc_Click(object sender, RoutedEventArgs e) =>
            UpdateWordDictionary(WordDictionary.OrderByDescending(x => x.Amount).ToList());

        private void NameAsc_Click(object sender, RoutedEventArgs e) =>
            UpdateWordDictionary(WordDictionary.OrderBy(x => x.Name).ToList());

        private void NameDesc_Click(object sender, RoutedEventArgs e) => 
            UpdateWordDictionary(WordDictionary.OrderByDescending(x => x.Name).ToList());

        private void UpdateWordDictionary(List<Word> list)
        {
            WordDictionary.Clear();
            foreach (var word in list)
            {
                WordDictionary.Add(word);
            }
        }

        private void ClearDatabase_Click(object sender, RoutedEventArgs e)
        {
            WordDictionary.Clear();
            ClearWordsDatabase();
            StatusLine.Text = "Table has been cleared.";
        }

        private void ClearWordsDatabase() => db.Database.ExecuteSqlCommand("DELETE FROM [Words]");

        private void WordDictionaryListView_PreviewMouseLeftButtonUp(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var word = (sender as ListView).SelectedItem as Word;
            var modalWindow = new WordEditingModalWindow(word.Name);
            if (modalWindow.ShowDialog() == true)
            {
                if (modalWindow.NewWordTextBox.Text != word.Name)
                {
                    Files.ForEach(x => ReplaceWord(x, word.Name, modalWindow.NewWordTextBox.Text));
                }
            }
        }

        private void ReplaceWord(string filename, string oldWord, string newWord)
        {
            string text;
            using (FileStream fstream = File.OpenRead(filename))
            {
                byte[] array = new byte[fstream.Length];
                fstream.Read(array, 0, array.Length);
                text = Encoding.UTF8.GetString(array);
            }

            text = text.Replace(oldWord, newWord);
            File.WriteAllText(filename, text);
        }
    }
}
