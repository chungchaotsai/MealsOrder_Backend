using MealsOrderAPI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MealsOrderAPI.Models
{
    public partial class Menu
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string VendorName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public int LowConsumption { get; set; }
        public string OffDay { get; set; }
        public string Image { get; set; }
        public string Status { get; set; }
        public string Annotation { get; set; }
        public float Rank { get; set; }
        public DateTime UpdateTime { get; set; }
    }

    public partial class MenuPic
    {
        public int MenuId { get; set; }
        public string FilePath { get; set; }
        public int FileNo { get; set; }
    }
}
