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
using MealsOrderAPI.Extensions;
namespace MealsOrderAPI.Controllers
{
    /// <summary>
    /// User operation
    /// </summary>
    public class UsersController : ODataController
    {
        private readonly IUsersRepository _usersRepository;
        public UsersController(
            IUsersRepository usersRepository
            )
        {
            _usersRepository = usersRepository;
        }

        //Get all users
        [EnableQuery(PageSize = 3)]
        [HttpGet("api/Users")]
        [HttpGet("api/Users/$count")]
        public async Task<IQueryable<User>> Get()
        {
            var result = await _usersRepository.List();
            return result;
        }

        //[HttpGet("Users({id})")]
        [HttpGet("{key}")]
        [EnableQuery]
        public Task<SingleResult<User>> Get([FromODataUri] int key)
        {
            return _usersRepository.Get(key);
        }

        [HttpGet("api/Users/{key}/Test(name={name})")]
        public string Sample1(int key, string name)
        {
            return $"Send Order({key}) to location at ({name})";
        }

        [HttpGet]
        [EnableQuery]
        public SingleResult<User> sample2([FromODataUri] int key)
        {
            var c = new List<User>();
            c.Add(new User { Id = key });
            c.Add(new User { Id = key + 1 });
            var b = c.Where(x => x.Id == key).AsQueryable();

            return new SingleResult<User>(b);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            try
            {
                var u = await _usersRepository.Get(user.Id);

                if (u == null)
                {
                    _ = _usersRepository.Add(user);
                }
                else
                {
                    string title = $"User '{user.Id}' not found in DB ToolInfo";
                    return HttpContext.ProblemDetailsError(StatusCodes.Status404NotFound, title);
                }
            }
            catch (Exception ex)
            {
                return HttpContext.ProblemDetailsException(ex);
            }

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] User user)
        {
            try
            {
                var u = await _usersRepository.Get(user.Id);

                if (u == null)
                {
                    string title = $"User '{user.Id}' not found in DB ToolInfo";
                    return HttpContext.ProblemDetailsError(StatusCodes.Status404NotFound, title);

                }
                else
                {
                    _ = _usersRepository.Update(user);
                }
            }
            catch (Exception ex)
            {
                return HttpContext.ProblemDetailsException(ex);
            }

            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] User user)
        {
            try
            {
                var u = await _usersRepository.Get(user.Id);

                if (u == null)
                {
                    string title = $"User '{user.Id}' not found in DB ToolInfo";
                    return HttpContext.ProblemDetailsError(StatusCodes.Status404NotFound, title);

                }
                else
                {
                    _ = _usersRepository.Delete(user);
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
