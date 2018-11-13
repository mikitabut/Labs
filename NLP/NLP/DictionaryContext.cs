using System.Data.Entity;
using System.Linq;

namespace NLP
{
    public class DictionaryContext : DbContext
    {
        public DictionaryContext() : base("DefaultConnection"){ }

        public DbSet<Word> Words { get; set; }

        public DbSet<Text> Texts { get; set; }

        public void TruncateWords()
        {
            //Database.ExecuteSqlCommand("DELETE FROM [WordsInTexts]");
            Database.ExecuteSqlCommand("DELETE FROM [Words]");
        }

        public void TruncateTexts() => Database.ExecuteSqlCommand("DELETE FROM [Texts]");

        public void AddText(Text text)
        {
            if (!Texts.Any(x => x.Path == text.Path))
            {
                Texts.Add(text);
                SaveChanges();
            }
        }
    }
}
