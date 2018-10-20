using System.ComponentModel;
using System.Linq;
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

        private string tags;
        public string Tags
        {
            get { return tags; }
            set
            {
                tags = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName]string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #endregion

        public Word() { }

        public Word(string name, int amount, string tag)
        {
            Name = name;
            Amount = amount;
            tags = tag;
        }

        public void IncrementAmountAndAddNewTag(string newTag)
        {
            Amount++;
            if (!tags.Split(',').Contains(newTag))
            {
                tags += "," + newTag;
            }
        }
    }
}
