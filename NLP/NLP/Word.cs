using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NLP
{
    public class Word : INotifyPropertyChanged
    {
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
            return $"{Name}<Noun>";
        }
    }
}
