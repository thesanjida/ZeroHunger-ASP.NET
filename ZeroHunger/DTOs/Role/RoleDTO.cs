using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZeroHunger.EF;

namespace ZeroHunger.DTOs.Role
{
    public class RoleDTO
    {
        public int Id { get; set; }
        public string RoleId { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}