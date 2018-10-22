using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using NLog;
using System.Threading.Tasks;

namespace NLP
{
    public partial class MainWindow : Window
    {
        DictionaryContext db = new DictionaryContext();
        public ObservableCollection<Word> WordDictionary { get; set; } = new ObservableCollection<Word>();

        public SortingType CurrentSortingType { get; set; } = SortingType.None;

        public string CurrentText { get; set; }

        public static Logger Logger = LogManager.GetCurrentClassLogger();

        public MainWindow()
        {
            InitializeComponent();
            //WordDictionaryListView.ItemsSource = db.Words.ToList();
            WordDictionary = new ObservableCollection<Word>(db.Words);
            DataContext = this;
        }

        #region Events

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                string filePath = openFileDialog.FileName;
                db.AddText(new Text(filePath));
                string text = TextIsTagged(filePath) ? File.ReadAllText(filePath) : Tagger.TagText(filePath);
                int counter = 0;
                IEnumerable<Match> matches = Regex.Matches(text, @"(?<word>[a-zA-Z][a-zA-Z-—']*)\/(?<tag>[a-zA-Z?$]*)").Cast<Match>();
                foreach (var match in matches)
                {
                    string word = match.Groups["word"].Value;
                    string tag = match.Groups["tag"].Value;
                    var currentWord = WordDictionary.FirstOrDefault(x => x.Name == word);
                    if (currentWord == null)
                    {
                        WordDictionary.Add(new Word(word, 1, tag));
                    }
                    else
                    {
                        currentWord.IncrementAmountAndAddNewTag(tag);
                    }

                    counter++;
                    ProgressBar.Dispatcher.Invoke(() => ProgressBar.Value = counter * 100 / matches.Count(), DispatcherPriority.Background);
                }

                Task.Factory.StartNew(() => SaveWordDictionary());
                StatusLine.Text = $"{openFileDialog.FileName} has been parsed.";
            }
        }

        private bool TextIsTagged(string filePath)
        {
            string firstLine = new StreamReader(filePath).ReadLine();
            string firstWord = firstLine.Split(' ').FirstOrDefault();
            return Regex.IsMatch(firstWord, @"[a-zA-Z-—']*\/[a-zA-Z?$]*");
        }

        private void SaveWordDictionary()
        {
            db.TruncateWords();
            db.Words.AddRange(WordDictionary.ToList());
            db.SaveChanges();
            Application.Current.Dispatcher.Invoke(() => { StatusLine.Text = $"Changes has been saved."; });
        }

        private void ClearDatabase_Click(object sender, RoutedEventArgs e)
        {
            WordDictionary.Clear();
            db.TruncateWords();
            db.TruncateTexts();
            StatusLine.Text = "Table has been cleared.";
        }

        private void NameTableHeader_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentSortingType == SortingType.NameAscending)
            {
                CurrentSortingType = SortingType.NameDescending;
                UpdateWordDictionary(WordDictionary.OrderByDescending(x => x.Name).ToList());
            }
            else
            {
                CurrentSortingType = SortingType.NameAscending;
                UpdateWordDictionary(WordDictionary.OrderBy(x => x.Name).ToList());
            }
        }

        private void AmountTableHeader_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentSortingType == SortingType.AmountDescending)
            {
                CurrentSortingType = SortingType.AmountAscending;
                UpdateWordDictionary(WordDictionary.OrderBy(x => x.Amount).ToList());
            }
            else
            {
                CurrentSortingType = SortingType.AmountDescending;
                UpdateWordDictionary(WordDictionary.OrderByDescending(x => x.Amount).ToList());
            }
        }

        private void TagsTableHeader_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentSortingType == SortingType.TagsAscending)
            {
                CurrentSortingType = SortingType.TagsDescending;
                UpdateWordDictionary(WordDictionary.OrderByDescending(x => x.Tags).ToList());
            }
            else
            {
                CurrentSortingType = SortingType.TagsAscending;
                UpdateWordDictionary(WordDictionary.OrderBy(x => x.Tags).ToList());
            }
        }

        private void UpdateWord_Click(object sender, RoutedEventArgs e)
        {
            var modalWindow = new WordEditingModalWindow();
            if (modalWindow.ShowDialog() == true)
            {
                if (modalWindow.NewWordTextBox.Text != String.Empty && modalWindow.OldWordTextBox.Text != String.Empty)
                {
                    db.Texts.ToList().ForEach(x => ReplaceWord(x.Path, modalWindow.OldWordTextBox.Text, modalWindow.NewWordTextBox.Text));
                }
            }
        }

        #endregion

        #region Supporting functions

        private void UpdateWordDictionary(List<Word> list)
        {
            WordDictionary.Clear();
            foreach (var word in list)
            {
                WordDictionary.Add(word);
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

        #endregion
    }

    public enum SortingType
    {
        NameAscending,
        NameDescending,
        AmountAscending,
        AmountDescending,
        TagsAscending,
        TagsDescending,
        None
    }
}
