using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZeroHunger.DTOs.FoodDistribute;
using ZeroHunger.DTOs.FoodRequest;
using ZeroHunger.DTOs.User;
using ZeroHunger.EF;
using ZeroHunger.Helpers;
using ZeroHunger.Mapping;

namespace ZeroHunger.Controllers
{
    public class FoodDistributeController : Controller
    {
        AutoMapperConfig _mapper;
        ZeroHungerEntities _db;
        
        public FoodDistributeController()
        {
            _mapper = new AutoMapperConfig();
            _db = new ZeroHungerEntities();
        }

        [HttpGet]
        public ActionResult Index()
        {
            var foodDistributes = _db.FoodDistributes.ToList();
            var foodDistributeDTOs = _mapper.MakeList<FoodDistribute, FoodDistributesDTO>(foodDistributes);
            return View(foodDistributeDTOs);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var foodRequests = _db.FoodRequests.ToList();
            var foodRequestsDTO = _mapper.MakeList<FoodRequest, FoodRequestDTO>(foodRequests);
            ViewBag.FoodRequests = foodRequestsDTO;

            var user = _db.Users.ToList();
            var userDTO = _mapper.MakeList<User, UserDTO>(user);
            var foodDistributors = userDTO.Where(u => u.Role?.Name == "Food Distributer").ToList();
            ViewBag.FoodDistributer = foodDistributors;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FoodDistributesDTO foodDistributesDTO)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Msg = "Please, input all the field.";
            }

            var foodDistribute = _mapper.MakeSingleInstance<FoodDistributesDTO, FoodDistribute>(foodDistributesDTO);
            foodDistribute.FoodDistributeId = GenerateId.MakeId();
            foodDistribute.Status = false;
            foodDistribute.CreatedAt = DateTime.Now;

            _db.FoodDistributes.Add(foodDistribute);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(string foodDistributeId)
        {
            var foodDistribute = _db.FoodDistributes.FirstOrDefault(f => f.FoodDistributeId == foodDistributeId);
            var foodDistributeDTO = _mapper.MakeSingleInstance<FoodDistribute, FoodDistributesDTO>(foodDistribute);

            var foodRequests = _db.FoodRequests.ToList();
            var foodRequestsDTO = _mapper.MakeList<FoodRequest, FoodRequestDTO>(foodRequests);
            ViewBag.FoodRequests = foodRequestsDTO;

            var user = _db.Users.ToList();
            var userDTO = _mapper.MakeList<User, UserDTO>(user);
            var foodDistributors = userDTO.Where(u => u.Role?.Name == "Food Distributer").ToList();
            ViewBag.FoodDistributer = foodDistributors;

            return View(foodDistributeDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FoodDistributesDTO foodDistributesDTO)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Msg = "Please, input all the field.";
            }

            var foodDistribute = _mapper.MakeSingleInstance<FoodDistributesDTO, FoodDistribute>(foodDistributesDTO);
            foodDistribute.UpdatedAt = DateTime.Now;

            _db.Entry(foodDistribute).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult DeliveryDone(string foodDistributeId)
        {
            var foodDistribute = _db.FoodDistributes.FirstOrDefault(fd => fd.FoodDistributeId == foodDistributeId);
            var foodDistributeDTO = _mapper.MakeSingleInstance<FoodDistribute, FoodDistributesDTO>(foodDistribute);

            foodDistributeDTO.Status = true;
            foodDistributeDTO.DeliveryDoneDate = DateTime.Now;

            var updateFoodDistribute = _mapper.MakeSingleInstance<FoodDistributesDTO, FoodDistribute>(foodDistributeDTO);
            _db.Entry(foodDistribute).CurrentValues.SetValues(updateFoodDistribute);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(string foodDistributeId)
        {
            var delete = _db.FoodDistributes.FirstOrDefault(fd => fd.FoodDistributeId == foodDistributeId);
            _db.FoodDistributes.Remove(delete);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}