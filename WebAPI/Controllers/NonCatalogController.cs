using Application.Dto;
using Application.NotCatalogBooks.Queries;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class NonCatalogController : MediatrControllerBase
    {
        [HttpGet(Name = "GetAllNonCatalogBooks")]
        public async Task<IEnumerable<BookDto>> Get()
        {
            return await Mediator.Send(new GetNotCatalogBooksQuery());
        }
    }
}
