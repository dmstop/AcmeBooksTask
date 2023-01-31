using Application.Dto;
using Application.Users;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class UsersController : MediatrControllerBase
    {
        [HttpGet(Name = "GetAllUsers")]
        public async Task<IEnumerable<UserDto>> Get()
        {
            return await Mediator.Send(new GetAllUsersQuery());
        }
    }
}
