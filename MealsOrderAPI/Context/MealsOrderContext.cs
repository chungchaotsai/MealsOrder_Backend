using MealsOrderAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MealsOrderAPI.Context
{
    public class MealsOrderContext: DbContext
    {
        public MealsOrderContext(DbContextOptions<MealsOrderContext> option): base(option) { }
        public DbSet<User> Users { get; set; }
    }
}
