using MealsOrderAPI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MealsOrderAPI.Models
{
    public partial class Menu
    {
        public int Id { get; set; }
        [Required]
        public string VendorName { get; set; }
        [Required]
        public string Phone { get; set; }
        public string Address { get; set; }
        public int LowConsumption { get; set; }
        public DayOfWeek[] OffDay { get; set; }
        public string Image { get; set; }
        public Status Status { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
