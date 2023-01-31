namespace Domain.Entities
{
    public class User
    {
        public User(string name)
        {
            Name = name;
        }

        public Guid Id { get; set; } = Guid.NewGuid();

        public string? Name { get; set; }

        public List<Book> LikedBooks { get; set; } = new();
    }
}
