using Application.CatalogBooks.Commands.CreateBook;
using Application.CatalogBooks.Commands.DeleteBook;
using Application.CatalogBooks.Commands.UpdateBook;
using Application.CatalogBooks.Queries.GetCatalogBook;
using Application.Dto;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AcmeCatalogController : MediatrControllerBase
{
    [HttpGet(Name = "GetAllCatalogBooks")]
    public async Task<IEnumerable<BookDto>> Get()
    {
        return await Mediator.Send(new GetAllCatalogBooksQuery());
    }

    [HttpPost("Create")]
    public async Task<ActionResult<Guid>> Create(CreateNewCatalogBookCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("Update/{id:Guid}")]
    public async Task<ActionResult> Update(Guid id, UpdateCatalogBookCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return Ok();
    }

    [HttpDelete("Delete/{id:Guid}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        await Mediator.Send(new DeleteCatalogBookCommand(id));

        return Ok();
    }
}