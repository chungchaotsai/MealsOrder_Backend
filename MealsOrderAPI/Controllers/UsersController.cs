using MealsOrderAPI.Context;
using MealsOrderAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace MealsOrderAPI.Controllers
{
    public class UsersController : ODataController
    {
        private readonly MealsOrderContext _db;
        private readonly ILogger<UsersController> _logger;
        public UsersController(MealsOrderContext dbContext, ILogger<UsersController> logger)
        {
            _logger = logger;
            _db = dbContext;
        }

        //Get all students        
        [EnableQuery]
        public IQueryable<User> Get()
        {
            var a = _db.Users;
            return a;
        }

        //Get by Id
        [EnableQuery]
        public SingleResult<User> Get([FromODataUri] int key)
        {
            var result = _db.Users.Where(c => c.Id == key);
            return SingleResult.Create(result);
        }

        [EnableQuery]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return Created(user);
        }
    }
}
