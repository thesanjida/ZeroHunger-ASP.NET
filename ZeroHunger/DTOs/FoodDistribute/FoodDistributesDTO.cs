using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ZeroHunger.EF;

namespace ZeroHunger.DTOs.FoodDistribute
{
    public class FoodDistributesDTO
    {
        public int Id { get; set; }
        public string FoodDistributeId { get; set; }
        public int? FoodRequestId { get; set; }
        public int? UserId { get; set; }
        public bool? Status { get; set; }
        public DateTime? DeliveryDoneDate { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual EF.FoodRequest FoodRequest { get; set; }
        public virtual EF.User User { get; set; }
    }
}