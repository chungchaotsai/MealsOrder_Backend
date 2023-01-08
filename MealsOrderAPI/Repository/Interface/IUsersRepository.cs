using MealsOrderAPI.Models;
using System;

namespace MealsOrderAPI.Repository.Interface
{
    public interface IUsersRepository
    {
        public IQueryable<User> GetAll();
    }
}