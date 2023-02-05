using MealsOrderAPI.Models;
using Microsoft.AspNetCore.OData.Results;
using System;

namespace MealsOrderAPI.Repository.Interface
{
    public interface IUsersRepository : IRepository<User>
    {

        public SingleResult<User> GetByUsernameNPassword(string name, string password);
    }
}