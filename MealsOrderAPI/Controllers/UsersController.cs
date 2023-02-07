﻿using AutoMapper;
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
using Serilog;
using System;
using AutoMapper.QueryableExtensions;
using MealsOrderAPI.Common;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Data;
using AutoMapper.Execution;
using Microsoft.EntityFrameworkCore;
using MealsOrderAPI.Repository;

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
        private readonly ILogger<UsersController> _logger;
        private readonly IMapper _mapper;
        private readonly JwtHelper _jwtHelper;
        private ClaimsPrincipal _user;
        public UsersController(
            ILogger<UsersController> logger,
            IUsersRepository usersRepository,
            IMapper mapper,
            JwtHelper jwtHelper,
            ClaimsPrincipal user
            )
        {
            _usersRepository = usersRepository;
            _logger = logger;
            _mapper = mapper;
            _jwtHelper = jwtHelper;
            _user = user;
        }
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] Login login)
        {
            var user = ValidateUser(login);
            if (user != null)
            {
                var urs = _usersRepository.GetRolesByUserId(user.Id).ToList();

                var roles = new List<string>();
                foreach (var ur in urs)
                {
                    roles.Add(ur.Name);
                }
                var roles1 = roles.ToArray();
                var roles2 = new string[] { "Admin", "User" };
                var token = _jwtHelper.GenerateToken(login.Username, roles1);

                return Ok(new { token });

            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("Claims")]
        public async Task<IActionResult> GetClaims()
        {
            return Ok(_user.Claims.Select(p => new { p.Type, p.Value }));
        }

        [HttpGet("UserRoles/{userId}"), Authorize(Roles = "Admin,Client,God")]
        public IQueryable<Role> GetUserRoles(int userId)
        {
            var rt = _usersRepository.GetRolesByUserId(userId);
            return rt;
        }

        [HttpGet("Username")]
        public async Task<IActionResult> GetUsername()

        {
            return Ok(_user.Identity?.Name);
        }

        [HttpGet("IsInRole/{name}")]
        public async Task<IActionResult> GetIsInRole(string name)

        {
            return Ok(_user.IsInRole(name));
        }

        private User ValidateUser(Login login)
        {
            var q = _usersRepository.GetByAccountIdNPassword(login.Username, login.Password);
            return q.Queryable.SingleOrDefault<User>();
        }


        //Get all users
        [HttpGet,Authorize(Roles = "Admin")]
        [EnableQuery(PageSize = 10)]
        public IQueryable<UserDto> Get()
        {
            // 資料庫要對應的欄位 @_@, 就算是null , 也是需要欄位
            var originalUsers = _usersRepository.List();
            var result = originalUsers.ProjectTo<UserDto>(_mapper.ConfigurationProvider);
            //var originalUsers = _usersRepository.List().ToList();

            //var result = _mapper.Map<IEnumerable<User>, IEnumerable<UserDto>>(originalUsers);
            //var result = _mapper.Map<IQueryable<User>, IQueryable<UserDto>>(originalUsers); 這個不行, 所以改用擴充方法

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
                    ShowName = u.Name,
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
