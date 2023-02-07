using MealsOrderAPI.Models;
using Microsoft.AspNetCore.OData.Results;
using System;

namespace MealsOrderAPI.Repository.Interface
{
    public interface IUsersRepository : IRepository<User>
    {
        public IQueryable<Role> GetRolesByUserId(int userId);
        public SingleResult<User> GetByAccountIdNPassword(string name, string password);
    }
}