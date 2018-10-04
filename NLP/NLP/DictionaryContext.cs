using System.Data.Entity;

namespace NLP
{
    class DictionaryContext : DbContext
    {
        public DictionaryContext() : base("DefaultConnection"){ }

        public DbSet<Word> Words { get; set; }
    }
}
