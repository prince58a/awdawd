
namespace BookLibrary.Core
{
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

        public override string ToString() => $"{Name}";
    }

}