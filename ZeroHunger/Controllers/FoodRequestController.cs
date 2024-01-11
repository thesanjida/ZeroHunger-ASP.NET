using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZeroHunger.DTOs.FoodRequest;
using ZeroHunger.DTOs.FoodSource;
using ZeroHunger.DTOs.NGO;
using ZeroHunger.DTOs.Role;
using ZeroHunger.EF;
using ZeroHunger.Helpers;
using ZeroHunger.Mapping;

namespace ZeroHunger.Controllers
{
    public class FoodRequestController : Controller
    {
        AutoMapperConfig _mapper;
        ZeroHungerEntities _db;

        public FoodRequestController()
        {
            _mapper = new AutoMapperConfig();
            _db = new ZeroHungerEntities();
        }

        [HttpGet]
        public ActionResult Index()
        {
            var foodRequests = _db.FoodRequests.ToList();
            var foodRequestDTOs = _mapper.MakeList<FoodRequest, FoodRequestDTO>(foodRequests);
            return View(foodRequestDTOs);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var foodSources = _db.FoodSources.ToList();
            var foodSourcesDTO = _mapper.MakeList<FoodSource, FoodSourceDTO>(foodSources);
            ViewBag.FoodSources = foodSourcesDTO;

            var ngos = _db.NGOs.ToList();
            var ngosDTO = _mapper.MakeList<NGO, NgoDTO>(ngos);
            ViewBag.NGOs = ngosDTO;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FoodRequestDTO foodRequestDTO)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Msg = "Please, input all the field.";
            }

            var foodRequest = _mapper.MakeSingleInstance<FoodRequestDTO, FoodRequest>(foodRequestDTO);
            foodRequest.FoodRequestId = GenerateId.MakeId();
            foodRequest.Status = false;
            foodRequest.CreatedAt = DateTime.Now;
            _db.FoodRequests.Add(foodRequest);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(string foodRequestId)
        {
            var foodSources = _db.FoodSources.ToList();
            var foodSourcesDTO = _mapper.MakeList<FoodSource, FoodSourceDTO>(foodSources);
            ViewBag.FoodSources = foodSourcesDTO;

            var ngos = _db.NGOs.ToList();
            var ngosDTO = _mapper.MakeList<NGO, NgoDTO>(ngos);
            ViewBag.NGOs = ngosDTO;

            var foodRequest = _db.FoodRequests.FirstOrDefault(fr => fr.FoodRequestId == foodRequestId);
            var foodRequestDTO = _mapper.MakeSingleInstance<FoodRequest, FoodRequestDTO>(foodRequest);

            return View(foodRequestDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FoodRequestDTO foodRequestDTO)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Msg = "Please, input all the field.";
            }

            var foodRequest = _db.FoodRequests.FirstOrDefault(fr => fr.FoodRequestId == foodRequestDTO.FoodRequestId);

            var updateFoodRequest = _mapper.MakeSingleInstance<FoodRequestDTO, FoodRequest>(foodRequestDTO);
            updateFoodRequest.UpdatedAt = DateTime.Now;
            _db.Entry(foodRequest).CurrentValues.SetValues(updateFoodRequest);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult MakeApprove(string foodRequestId)
        {
            var foodRequest = _db.FoodRequests.FirstOrDefault(fr => fr.FoodRequestId == foodRequestId);
            var foodRequestDTO = _mapper.MakeSingleInstance<FoodRequest, FoodRequestDTO>(foodRequest);

            foodRequestDTO.Status = true;
            foodRequestDTO.ApproveDate = DateTime.Now;

            var updateFoodRequest = _mapper.MakeSingleInstance<FoodRequestDTO, FoodRequest>(foodRequestDTO);
            _db.Entry(foodRequest).CurrentValues.SetValues(updateFoodRequest);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(string foodRequestId)
        {
            var delete = _db.FoodRequests.FirstOrDefault(fr => fr.FoodRequestId == foodRequestId);
            _db.FoodRequests.Remove(delete);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}