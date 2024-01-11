using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZeroHunger.DTOs.FoodSource;
using ZeroHunger.DTOs.NGO;
using ZeroHunger.DTOs.User;
using ZeroHunger.EF;
using ZeroHunger.Helpers;
using ZeroHunger.Mapping;

namespace ZeroHunger.Controllers
{
    public class FoodSourceController : Controller
    {
        AutoMapperConfig _mapper;
        ZeroHungerEntities _db;

        public FoodSourceController()
        {
            _mapper = new AutoMapperConfig();
            _db = new ZeroHungerEntities();
        }

        [HttpGet]
        public ActionResult Index()
        {
            var foodSources = _db.FoodSources.ToList();
            var foodSourceDTOs = _mapper.MakeList<FoodSource, FoodSourceDTO>(foodSources);
            return View(foodSourceDTOs);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FoodSourceDTO foodSourceDTO)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Msg = "Please, input all the field.";
            }

            var alreadyExistFoodSource = _db.FoodSources.FirstOrDefault(fs => fs.Name.ToLower() == foodSourceDTO.Name.Trim().ToLower());
            if (alreadyExistFoodSource != null)
            {
                ViewBag.Msg = "Food Source already exists.";
                return View();
            }

            var foodSource = _mapper.MakeSingleInstance<FoodSourceDTO, FoodSource>(foodSourceDTO);
            foodSource.FoodSourceId = GenerateId.MakeId();
            foodSource.CreatedAt = DateTime.Now;
            _db.FoodSources.Add(foodSource);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(string foodSourceId)
        {
            var foodSource = _db.FoodSources.FirstOrDefault(fs => fs.FoodSourceId == foodSourceId);
            var foodSourceDTO = _mapper.MakeSingleInstance<FoodSource, FoodSourceDTO>(foodSource);
            return View(foodSourceDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FoodSourceDTO foodSourceDTO)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Msg = "Please, input all the field.";
            }

            var alreadyExistFoodSource = _db.FoodSources.FirstOrDefault(fs => fs.Name.ToLower() == foodSourceDTO.Name.Trim().ToLower());
            if (alreadyExistFoodSource != null)
            {
                ViewBag.Msg = "Food Source already exists.";
                return View();
            }

            var foodSource = _db.FoodSources.FirstOrDefault(fs => fs.FoodSourceId == foodSourceDTO.FoodSourceId);

            var updateFoodSource = _mapper.MakeSingleInstance<FoodSourceDTO, FoodSource>(foodSourceDTO);
            updateFoodSource.UpdatedAt = DateTime.Now;
            _db.Entry(foodSource).CurrentValues.SetValues(updateFoodSource);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Detail(string foodSourceId)
        {
            var foodSource = _db.FoodSources.FirstOrDefault(fs => fs.FoodSourceId == foodSourceId);
            var foodSourceDTO = _mapper.MakeSingleInstance<FoodSource, FoodSourceDTO>(foodSource);
            return View(foodSourceDTO);
        }

        [HttpGet]
        public ActionResult Delete(string foodSourceId)
        {
            var delete = _db.FoodSources.FirstOrDefault(fs => fs.FoodSourceId == foodSourceId);
            _db.FoodSources.Remove(delete);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}