using MealsOrderAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;

namespace MealsOrderAPI.Context
{
    public class MealsOrderContext : DbContext
    {
        public MealsOrderContext(DbContextOptions<MealsOrderContext> option) : base(option) { }
        public virtual DbSet<User>? Users { get; set; }
        public virtual DbSet<Meal>? Meals { get; set; }
        public virtual DbSet<MealOrder>? MealOrders { get; set; }
        public virtual DbSet<Menu>? Menus { get; set; }
    }
}
