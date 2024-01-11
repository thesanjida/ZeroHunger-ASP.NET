using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ZeroHunger.DTOs.FoodRequest
{
    public class FoodRequestDTO
    {
        public int Id { get; set; }
        public string FoodRequestId { get; set; }
        public int FoodSourceId { get; set; }
        public string FoodNames { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? ExpDate { get; set; }
        public bool? Status { get; set; }

        public DateTime? ApproveDate { get; set; }
        public int NgoId { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public string CombinedFoodName
        {
            get { return $"{FoodNames} - {FoodSource?.Name} - {ExpDate}" ; }
        }
        
        public virtual EF.FoodSource FoodSource { get; set; }
        public virtual EF.NGO NGO { get; set; }
    }
}