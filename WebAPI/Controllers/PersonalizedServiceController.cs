using Application.Dto;
using Application.PersonalizedBooks.Queries;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonalizedServiceController : MediatrControllerBase
    {
        [HttpGet("{userId:Guid}")]
        public async Task<List<BookDto>> Get(Guid userId)
        {
            return await Mediator.Send(new GetPersonalizedBooksQuery(userId));
        }
    }
}
