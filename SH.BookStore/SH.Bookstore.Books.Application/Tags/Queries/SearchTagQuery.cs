using SH.Bookstore.Books.Application.Contracts.Queries;
using SH.Bookstore.Books.Application.Tags.DTOs;

namespace SH.Bookstore.Books.Application.Tags.Queries;
public record SearchTagQuery(string? Title) : IPagedQuery<TagDto>;
