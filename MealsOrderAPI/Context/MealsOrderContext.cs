using MealsOrderAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace MealsOrderAPI.Context
{
    public class MealsOrderContext: DbContext
    {
        public MealsOrderContext(DbContextOptions<MealsOrderContext> option): base(option) { }
        public DbSet<User> Users { get; set; }
        public virtual DbSet<Meal> Meal { get; set; }
        public virtual DbSet<MealOrder> MealOrder { get; set; }
        public virtual DbSet<Menu> Menu { get; set; }
    }
}
