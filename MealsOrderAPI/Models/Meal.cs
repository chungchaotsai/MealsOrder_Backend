using MealsOrderAPI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MealsOrderAPI.Models
{
    /// <summary>
    /// 新開餐點資訊
    /// </summary>
    public partial class Meal
    {
        public int Id { get; set; }
        [Required]
        public int MunuId { get; set; }
        public DateTime MealTime { get; set; }
        public int IssuerUserId { get; set; }
        [Required]
        public DateTime ExpireTime { get; set; }
        public int MinimumOrder { get; set; }
        public int MinimumPeople { get; set; }
        [Required]
        public string Status { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
