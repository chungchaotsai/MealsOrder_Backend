using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Security.Cryptography;
using MealsOrderAPI.Common;

namespace MealsOrderAPI.Models
{
    /// <summary>
    /// 使用者資訊
    /// </summary>
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password256 { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime UpdateTime { get; set; }
        public Status Status { get; set; } = Common.Status.Normal;
        public static string MakeSHA256(string s)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(s));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
