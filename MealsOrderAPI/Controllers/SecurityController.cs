using MealsOrderAPI.Common;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace MealsOrderAPI.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly JwtHelper _jwtHelper;
        private ClaimsPrincipal _user;

        public SecurityController(
            ILogger<UsersController> logger,
            JwtHelper jwtHelper,
            ClaimsPrincipal user
            )
        {
            _logger = logger;
            _jwtHelper = jwtHelper;
            _user = user;
        }

        [HttpGet, Authorize(Roles ="Admin")]
        public async Task<IActionResult> Get()
        {
            return Ok();
        }
    }
}
