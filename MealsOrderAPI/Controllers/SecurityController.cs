using AutoMapper;
using MealsOrderAPI.Common;
using MealsOrderAPI.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using MealsOrderAPI.Models;
using MealsOrderAPI.Repository;
using Microsoft.AspNetCore.OData.Query;
using System.Drawing.Printing;

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
