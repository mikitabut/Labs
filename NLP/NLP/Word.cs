using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NLP
{
    public class Word : INotifyPropertyChanged
    {
        #region Properties

        public int Id { get; set; }

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged();
            }
        }

        private int amount;
        public int Amount
        {
            get { return amount; }
            set
            {
                amount = value;
                OnPropertyChanged();
            }
        }

        public static List<string> Articles = new List<string>() { "a", "an", "the" };

        public static List<string> Prepositions = new List<string>() { "about", "above", "across", "after", "against", "along",
            "amid", "amidst", "among", "amongst", "around", "as", "aside", "at", "before", "behind", "below", "beside",
            "besides", "between", "beyond", "by", "down", "during", "except", "for", "from", "in", "inside", "into", "of", "off",
            "on", "outside", "over", "past", "round", "since", "through", "till", "until", "to", "towards", "under", "up",
            "upon", "with", "within", "without" };

        public static List<string> Particles = new List<string>() { "all", "alone", "but", "else", "even", "exactly", "just",
            "merely", "not", "only", "precisely", "right", "simply", "solely", "still", "yet" };

        public static List<string> Conjunctions = new List<string>() { "against", "also", "and", "as", "because", "beyond",
            "but", "however", "for", "if", "like", "meanwhile", "moreover", "nevertheless", "nor", "once", "or", "otherwise",
            "still", "than", "that", "therefore", "although", "thus", "unless", "unlike", "what", "whether", "with", "within",
            "without", "whereas", "while", "yet" };

        public static List<string> Pronouns = new List<string>() { "i", "me", "mine", "myself", "you", "your", "yours",
            "youself", "he", "him", "his", "himself", "she", "her", "hers", "herself", "it", "its", "itself", "we",
            "us", "our", "ours", "ourselves", "you", "your", "yours", "yourselves", "they", "them", "theirs",
            "themselves", "this", "these", "that", "those", "such", "same", "who", "whom", "what", "which", "whose",
            "that" };

        public static List<string> Numerals = new List<string>() { "zero", "one", "two", "three", "four", "five",
            "six", "seven", "eight", "nine", "ten" };

        #endregion

        public Word() { }

        public Word(string name, int amount)
        {
            Name = name;
            Amount = amount;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public string GetAnalyzedWord()
        {
            string name = Name.ToLower();
            string partOfSpeech = String.Empty;
            if (Articles.Contains(name))
            {
                partOfSpeech = "article";
            }
            else if (Prepositions.Contains(name))
            {
                partOfSpeech = "preposition";
            }
            else if (Particles.Contains(name))
            {
                partOfSpeech = "particle";
            }
            else if (Conjunctions.Contains(name))
            {
                partOfSpeech = "conjunction";
            }
            else if (Pronouns.Contains(name))
            {
                partOfSpeech = "pronoun";
            }
            else if (Numerals.Contains(name))
            {
                partOfSpeech = "numeral";
            }
            else if (name.EndsWith("ly"))
            {
                partOfSpeech = "adverb";
            }
            else if (name.EndsWith("ful") || name.EndsWith("less") || name.EndsWith("able") || name.EndsWith("ible")
                || name.EndsWith("ic") || name.EndsWith("ical") || name.EndsWith("ous") || name.EndsWith("ate") || 
                name.EndsWith("ish") || name.EndsWith("ive") || name.EndsWith("y"))
            {
                partOfSpeech = "adjective";
            }
            else if (name.EndsWith("ed") || name.EndsWith("ing"))
            {
                partOfSpeech = "verb";
            }
            else
            {
                partOfSpeech = "noun";
            }

            return $"{Name}<{partOfSpeech}>";
        }
    }
}
