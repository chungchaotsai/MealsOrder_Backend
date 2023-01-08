using MealsOrderAPI.Context;
using MealsOrderAPI.Models;
using MealsOrderAPI.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace MealsOrderAPI.Repository
{
    public class UserRepository : IUsersRepository
    {
        private readonly MealsOrderContext _context;
        public UserRepository(MealsOrderContext context)
        {
            _context = context;
        }
        public IQueryable<User> GetAll()
        {
            return _context.Users.AsQueryable();
        }
    }
}
