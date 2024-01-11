using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using ZeroHunger.EF;

namespace ZeroHunger.DTOs
{
    public class SignupDTO
    {
        [Required]
        public string Name { get; set; }
        public string UserId { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }
        [Required]
        public string Mobile { get; set; }
        [Required]
        [MinLength(6)]
        public string Password { get; set; }
        public int? RoleId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}