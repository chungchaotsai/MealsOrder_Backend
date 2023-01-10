using MealsOrderAPI.Context;
using MealsOrderAPI.Models;
using MealsOrderAPI.Repository.Interface;
using Microsoft.AspNetCore.OData.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection.Metadata;

namespace MealsOrderAPI.Repository
{
    public class UserRepository : IUsersRepository
    {
        private readonly MealsOrderContext _context;
        public UserRepository(MealsOrderContext context)
        {
            _context = context;
        }
        public async Task<IQueryable<User>> List()
        {
            return _context.Users.AsQueryable();
        }
        // work around but route path incorrect
        public async Task<SingleResult<User>> Get(int key)
        {
            var u = _context.Users.Where(p => p.Id == key).AsQueryable();
            return SingleResult.Create(u);
        }

        public async Task Add(User user)
        {
            _ = _context.Users.AddAsync(user);
            _ = _context.SaveChangesAsync();
            return;
        }

        public async Task Delete(User user)
        {
            var u = _context.Users.First(p => p.Id == user.Id);
            _context.Users.Remove(u);
            _context.SaveChanges();
        }

        public async Task Update(User user)
        {
            var u = _context.Users.First(p => p.Id == user.Id);
            u.Name = user.Name;
            u.Email = user.Email;
            _context.SaveChanges();
        }

    }
}
