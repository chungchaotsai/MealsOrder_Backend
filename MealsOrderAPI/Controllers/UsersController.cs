using MealsOrderAPI.Context;
using MealsOrderAPI.Extensions;
using MealsOrderAPI.Models;
using MealsOrderAPI.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.OData.Edm;
namespace MealsOrderAPI.Controllers
{
    /// <summary>
    /// User operation
    /// </summary>
    [Route("api/[Controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersRepository _usersRepository;
        public UsersController(
            IUsersRepository usersRepository
            )
        {
            _usersRepository = usersRepository;
        }

        //Get all users
        [HttpGet]
        [EnableQuery(PageSize = 10)]
        public IQueryable<UserDto> Get()
        {
            var result = _usersRepository.List().Select(u =>
                new UserDto()
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                });

            return result;
        }

        [HttpGet("{Id}")]
        [EnableQuery]
        public SingleResult<UserDto> Get([FromODataUri] int Id)
        {
            var result = _usersRepository.Get(Id).Queryable.Select(u =>
                new UserDto()
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                });

            return new SingleResult<UserDto>(result);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            try
            {
                var u = _usersRepository.Get(user.Id);
                if (u.Queryable.SingleOrDefault() == null)
                {
                    await _usersRepository.Add(user);
                }
                else
                {
                    string title = $"User '{user.Id}' not found in DB";
                    return HttpContext.ProblemDetailsError(StatusCodes.Status404NotFound, title);
                }
            }
            catch (Exception ex)
            {
                return HttpContext.ProblemDetailsException(ex);
            }

            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] User user)
        {
            try
            {
                var u = _usersRepository.Get(user.Id);

                if (u.Queryable.SingleOrDefault() == null)
                {
                    string title = $"User '{user.Id}' not found in DB";
                    return HttpContext.ProblemDetailsError(StatusCodes.Status404NotFound, title);

                }
                else
                {
                    await _usersRepository.Update(user);
                }
            }
            catch (Exception ex)
            {
                return HttpContext.ProblemDetailsException(ex);
            }

            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int Id)
        {
            try
            {
                var u = _usersRepository.Get(Id);

                if (u == null)
                {
                    string title = $"User '{Id}' not found in DB";
                    return HttpContext.ProblemDetailsError(StatusCodes.Status404NotFound, title);

                }
                else
                {
                    var fuser = new User { Id = Id };
                    await _usersRepository.Delete(fuser);
                }
            }
            catch (Exception ex)
            {
                return HttpContext.ProblemDetailsException(ex);
            }

            return Ok();
        }
    }
}
