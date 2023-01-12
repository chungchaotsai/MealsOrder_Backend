using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Security.Cryptography;
using MealsOrderAPI.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace MealsOrderAPI.Models
{
    /// <summary>
    /// 使用者資訊
    /// </summary>
    [Table("users")]
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string Email { get; set; }
        public string? Password { get; set; }
        public string? Phones { get; set; }
        public int? AccoutId { get; set; }


    }

    public class UserDto
    {
        public int Id { get; set; }
        public string ?Name { get; set; }
        public string ?Email { get; set; }
        public string ?Phones { get; set; }

    }
}
