using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.CatalogBooks.Commands.DeleteBook;

public record DeleteCatalogBookCommand(Guid Id) : IRequest;

public class DeleteCatalogBookCommandHandler : IRequestHandler<DeleteCatalogBookCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteCatalogBookCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Unit> Handle(DeleteCatalogBookCommand request, CancellationToken cancellationToken)
    {
        var catalogBook = await _context.Catalog
            .Include(c => c.Book)
            .ThenInclude(b => b.Authors)
            .Where(l => l.Book.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        if (catalogBook == null)
        {
            throw new NotFoundException(nameof(AcmeCatalogBook), request.Id);
        }

        foreach (var author in catalogBook.Book.Authors.Where(author => author.Books.Count > 1))
        {
            _context.Authors.Remove(author);
        }

        _context.Catalog.Remove(catalogBook);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}