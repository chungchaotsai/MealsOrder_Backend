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
        public IQueryable<User> List()
        {
            try
            {
                return _context.Users.AsQueryable();
            } catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return _context.Users.AsQueryable();
        }
        // work around but route path incorrect
        public SingleResult<User> Get(int key)
        {
            var u = _context.Users.Where(p => p.Id == key).AsQueryable();
            return SingleResult.Create(u);
        }

        public async Task Add(User user)
        {
             _context.Users.AddAsync(user);
             await _context.SaveChangesAsync();
        }

        public async Task Delete(User user)
        {
            var u = _context.Users.First(p => p.Id == user.Id);
            _context.Users.Remove(u);
            await _context.SaveChangesAsync();
        }

        public async Task Update(User user)
        {
            var u = _context.Users.First(p => p.Id == user.Id);
            u.Name = user.Name;
            u.Email = user.Email;
            await _context.SaveChangesAsync();
        }

    }
}
