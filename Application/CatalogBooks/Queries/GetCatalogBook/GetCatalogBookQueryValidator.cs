using Application.AllBooks.Queries;
using FluentValidation;

namespace Application.CatalogBooks.Queries.GetCatalogBook
{
    public class GetCatalogBookQueryValidator: AbstractValidator<GetBookQuery>
    {

        public GetCatalogBookQueryValidator()
        {
            RuleFor(v => v.Id)
                .NotEmpty().WithMessage("Id is required.");
        }
    }
}
