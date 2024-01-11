using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZeroHunger.DTOs.User
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Password { get; set; }
        public int? RoleId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual EF.Role Role { get; set; }
    }
}