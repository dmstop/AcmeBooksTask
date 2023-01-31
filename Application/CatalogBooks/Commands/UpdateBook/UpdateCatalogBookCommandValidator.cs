using Application.CatalogBooks.Commands.CreateBook;
using Application.CatalogBooks.Commands.Dto;
using Application.Common.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.CatalogBooks.Commands.UpdateBook;

public class UpdateCatalogBookCommandValidator : AbstractValidator<UpdateCatalogBookCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateCatalogBookCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Id)
            .NotEmpty();

        RuleFor(v => v.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title must not exceed 200 characters.")
            .MustAsync(BeUniqueTitle).WithMessage("The specified title already exists.");

        RuleFor(v => v.ISBN)
            .NotEmpty().WithMessage("ISBN is required.")
            .Length(13).WithMessage("ISBN must be 13 characters long.")
            .MustAsync(BeUniqueISBN).WithMessage("The specified ISBN already exists.");
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