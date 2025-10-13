using System;
using System.ComponentModel.DataAnnotations;

namespace SmsAPI.Models
{
    public class Users
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string UserName { get; set; }
        
        [Required]
        public string PasswordHash { get; set; }
        
        public string? RefreshToken { get; set; }
        
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
