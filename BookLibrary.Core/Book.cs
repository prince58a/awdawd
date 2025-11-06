
namespace BookLibrary.Core
{
    public interface IDomainObject
    {
        int Id { get; set; }
    }

    public class Book : IDomainObject
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }

        public int GenreId { get; set; }
        public Genre Genre { get; set; }

        public Book() { }

        public Book(int id, string title, string author, int year, int genreId)
        {
            Id = id;
            Title = title;
            Author = author;
            Year = year;
            GenreId = genreId;
        }

        public override string ToString()
        {
            var genreName = Genre?.Name ?? $"(Id {GenreId})";
            return $"Id-{Id}: \"{Title}\" - {Author} ({Year}), {genreName}";
        }
    }
    public class Genre : IDomainObject
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Genre() { }
        public Genre(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public override string ToString() => $"{Id}: {Name}";
    }

}