using Application.AllBooks.Queries;
using Application.Dto;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AllBooksController : MediatrControllerBase
    {
        [HttpGet("{id:Guid}")]
        public async Task<BookDto> Get(Guid id)
        {
            return await Mediator.Send(new GetBookQuery(id));
        }

        [HttpGet]
        [Route("SearchBooks")]
        public async Task<IEnumerable<BookDto>> SearchBooks(string startsWith)
        {
            return await Mediator.Send(new SearchBookQuery(startsWith));
        }
    }
}
