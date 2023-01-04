using SH.Bookstore.Books.Application.Books.DTOs;
using SH.Bookstore.Books.Application.Contracts.Queries;
using SH.Bookstore.Books.Domain.Mongo;

namespace SH.Bookstore.Books.Application.Books.Queries;
public record SearchBooksQuery(string? Title, string? Author, string? Tag, Order OrderBy) : IPagedQuery<MgBook>;

public enum Order
{
    CreateDateAscending,
    CreateDateDescending,
    TitleAscending,
    TitleDescending,
    PriceDescending,
    PriceAscending
}
