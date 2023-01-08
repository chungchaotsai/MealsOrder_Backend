using MealsOrderAPI.Context;
using MealsOrderAPI.Models;
using MealsOrderAPI.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace MealsOrderAPI.Controllers
{
    [Route("api/v1/[controller]")]
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

        //Get all students
        [HttpGet]
        [EnableQuery(PageSize = 3)]
        public IEnumerable<User> Get()
        {
            return _usersRepository.GetAll();
        }
    }
}
