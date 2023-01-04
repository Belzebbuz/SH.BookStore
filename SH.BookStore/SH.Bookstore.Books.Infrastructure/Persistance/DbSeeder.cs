using SH.Bookstore.Books.Application.Contracts.Repository;
using SH.Bookstore.Books.Domain.Aggregates.AuthorAggregate;
using SH.Bookstore.Books.Domain.Aggregates.BookAggrgate;
using SH.Bookstore.Books.Domain.Aggregates.TagAggregate;

namespace SH.Bookstore.Books.Infrastructure.Persistance;
internal class DbSeeder
{
    private readonly IRepository<Author> _authorRepository;
    private readonly IRepository<Tag> _tagRepository;
    private readonly IRepository<Book> _bookRepository;

    public DbSeeder(
        IRepository<Author> authorRepository, 
        IRepository<Tag> tagRepository,
        IRepository<Book> bookRepository)
    {
        _authorRepository = authorRepository;
        _tagRepository = tagRepository;
        _bookRepository = bookRepository;
    }

    public async Task SeedAsync()
    {
        if (await _authorRepository.AnyAsync())
            return;

        List<Author> authors = new(){
            Author.Create("Smitt", "Lorem ipsum dolor sit amet, consectetur adipiscing elit," +
            " sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim"),
            Author.Create("Pushkin", "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod " +
            "tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim")
        };

        List<Tag> tags = new()
        {
            Tag.Create("History"),
            Tag.Create("Detective")
        };
        await _authorRepository.AddRangeAsync(authors);
        await _tagRepository.AddRangeAsync(tags);

        List<Book> books = new()
        {
            Book.Create("Red hat", "Lorem ipsum", null, tags.Where((x, index) => index == 0), authors, 200, DateTime.Now.AddDays(-1)),
            Book.Create("Sherlock Holmes", "Lorem ipsum", null, tags.Where((x, index) => index == 1), authors.Where((x, index) => index == 0), 300, DateTime.Now.AddDays(-1)),
        };

        await _bookRepository.AddRangeAsync(books);
    }
}
