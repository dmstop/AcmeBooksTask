namespace Domain.Entities
{
    public class Author
    {
        public Author(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public Guid Id { get; set; } = Guid.NewGuid();

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public List<Book> Books { get; set; } = new();
    }
}
