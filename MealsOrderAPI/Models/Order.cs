using MealsOrderAPI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MealsOrderAPI.Models
{
    /// <summary>
    /// 個人訂餐資訊
    /// </summary>
    public partial class MealOrder
    {
        public int Id { get; set; }
        [Required]
        public int MealId { get; set; }
        [Required]
        public int UserId { get; set; }
        public IEnumerable<MealOrderUnit> items { get; set; }
        public int Amount { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
    /// <summary>
    /// 餐點單項
    /// </summary>
    public partial class MealOrderUnit
    {
        [Required]
        public string Name { get; set; }
        public int Qty { get; set; }
        public float UnitPrice { get; set; }
        [Required]
        public int Amount { get; set; }
        public string? Description { get; set; }
    }
    /// <summary>
    /// 廠商訂單
    /// </summary>
    public partial class Order
    {
        public int Id { get; set; }
        public int MealId { get; set; }
        public string? VendorName { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public int Amount { get; set; }
        public IEnumerable<MealOrderUnit>? Items { get; set; }
        public Status Status { get; set; } = Common.Status.Normal;
    }
    /// <summary>
    /// 帳務用
    /// </summary>
    public partial class Account
    {
        public int OrderId { get; set; }
        public int Amount { get; set; }
        public MealOrder? mealOrder { get; set; }
    }
}
