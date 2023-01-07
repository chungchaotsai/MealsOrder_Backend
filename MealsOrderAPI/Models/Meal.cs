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
        public DateTime DealDate { get; set; }
        public int Amount { get; set; }
        public int PeopleCount { get; set; }
        [Required]
        public DateTime ExpireTime { get; set; }
        public Status Status { get; set; } = Common.Status.Normal;
        public int Issuer { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
