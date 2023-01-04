using SH.Bookstore.Books.Application.Authors.DTOs;
using SH.Bookstore.Books.Application.Contracts.Queries;

namespace SH.Bookstore.Books.Application.Authors.Queries;
public record SearchAuthorsQuery(string? Name) : IPagedQuery<AuthorDto>;
