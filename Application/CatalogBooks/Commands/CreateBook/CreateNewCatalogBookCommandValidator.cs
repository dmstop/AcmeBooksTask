using Application.CatalogBooks.Commands.Dto;
using Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.CatalogBooks.Commands.CreateBook;

public class CreateNewCatalogBookCommandValidator : AbstractValidator<CreateNewCatalogBookCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateNewCatalogBookCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters.")
            .MustAsync(BeUniqueTitle).WithMessage("The specified title already exists.");

        RuleFor(v => v.ISBN)
            .NotEmpty().WithMessage("ISBN is required.")
            .Length(13).WithMessage("ISBN must be 13 characters long.")
            .MustAsync(BeUniqueISBN).WithMessage("The specified ISBN already exists.");

        RuleFor(v => v.Authors)
            .NotEmpty()
            .Must(BeMaximumLength).WithMessage("Authors First Name and Last Name Length must be less than 200");
    }

    public bool BeMaximumLength(IEnumerable<AuthorReqDto> authors)
    {
        return authors.All(author => author.LastName.Length <= 200 && author.FirstName.Length <= 200);
    }

    public async Task<bool> BeUniqueTitle(string title, CancellationToken cancellationToken)
    {
        return await _context.Books
            .AllAsync(l => l.Title != title, cancellationToken);
    }

    public async Task<bool> BeUniqueISBN(string isbn, CancellationToken cancellationToken)
    {
        return await _context.Books
            .AllAsync(l => l.ISBN != isbn, cancellationToken);
    }
}