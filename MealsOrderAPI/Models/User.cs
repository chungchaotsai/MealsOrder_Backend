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
    [Table("user")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string Email { get; set; }
        public string? Password { get; set; }
        public string? Phones { get; set; }
        public string? AccountId { get; set; }
    }

    [Table("role")]
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    [Table("userrolemap")]
    public class UserRoleMap
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
    }

    public class UserDto
    {
        public int Id { get; set; }
        public string? ShowName { get; set; }
        public string? Email { get; set; }
        public string? Phones { get; set; }
    }

    public class Login
    {
        [Required(ErrorMessage = "Name cannot be empty")] public string? Username { get; set; }

        [Required(ErrorMessage = "Name cannot be empty")] public string? Password { get; set; }
    }
}
