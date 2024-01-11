using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZeroHunger.DTOs.FoodSource
{
    public class FoodSourceDTO
    {
        public int Id { get; set; }
        public string FoodSourceId { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}