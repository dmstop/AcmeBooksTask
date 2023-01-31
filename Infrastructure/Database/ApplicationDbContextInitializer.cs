using Domain.Entities;

namespace Infrastructure.Database;

public class ApplicationDbContextInitializer
{
    private readonly ApplicationDbContext _context;

    public ApplicationDbContextInitializer(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddDefaultData()
    {
        var catalogBook1 = new Book
        {
            ISBN = "1111111111111",
            Title = "CatalogBook1",
            Genre = "Gen1",
            Authors =
            {
                new Author("F1", "S1")
            }
        };
        var catalogBook2 = new Book
        {
            ISBN = "0000000000001",
            Title = "CatalogBook2",
            Genre = "Gen1",
            Authors =
            {
                new Author("F2", "S2"),
                new Author("F3", "S3"),
            }
        };

        _context.Catalog.AddRange(new AcmeCatalogBook
            {
                Book = catalogBook1
            },
            new AcmeCatalogBook
            {
                Book = catalogBook2
            },
            new AcmeCatalogBook
            {
                Book = new Book
                {
                    ISBN = "0000000000041",
                    Title = "CatalogBook3",
                    Genre = "Gen2",
                    Authors =
                    {
                        new Author("F8", "S8"),
                    }
                }
            },
            new AcmeCatalogBook
            {
                Book = new Book
                {
                    ISBN = "0000000000341",
                    Title = "CatalogBook4",
                    Genre = "Gen1",
                    Authors =
                    {
                        new Author("F9", "S9")
                    }
                }
            });

        var authors = new[] { new Author("F5", "S5"), new Author("F6", "S6") };

        _context.Books.AddRange(new Book
            {
                ISBN = "1111100111111",
                Title = "NonCatalogBook1",
                Genre = "Gen2",
                Authors =
                {
                    authors[0]
                }
            },
            new Book
            {
                ISBN = "3479838476345",
                Title = "NonCatalogBook2",
                Genre = "Gen3",
                Authors = authors.ToList()
            });

        _context.Users.Add(new User("Dmitry")
        {
            LikedBooks = new List<Book>
            {
                catalogBook1, catalogBook2
            }
        });

        await _context.SaveChangesAsync();
    }
}