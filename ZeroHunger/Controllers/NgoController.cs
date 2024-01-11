using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZeroHunger.DTOs.NGO;
using ZeroHunger.DTOs.Role;
using ZeroHunger.DTOs.User;
using ZeroHunger.EF;
using ZeroHunger.Helpers;
using ZeroHunger.Mapping;

namespace ZeroHunger.Controllers
{
    public class NgoController : Controller
    {
        AutoMapperConfig _mapper;
        ZeroHungerEntities _db;

        public NgoController()
        {
            _mapper = new AutoMapperConfig();
            _db = new ZeroHungerEntities();
        }

        [HttpGet]
        public ActionResult Index()
        {
            var ngos = _db.NGOs.ToList();
            var ngoDTOs = _mapper.MakeList<NGO, NgoDTO>(ngos);
            return View(ngoDTOs);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NgoDTO ngoDTO)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Msg = "Please, input all the field.";
            }

            var alreadyExistNgo = _db.NGOs.FirstOrDefault(n => n.Name.ToLower() == ngoDTO.Name.Trim().ToLower());
            if (alreadyExistNgo != null)
            {
                ViewBag.Msg = "Ngo already exists.";
                return View();
            }

            var ngo = _mapper.MakeSingleInstance<NgoDTO, NGO>(ngoDTO);
            ngo.NgoId = GenerateId.MakeId();
            ngo.CreatedAt = DateTime.Now;
            _db.NGOs.Add(ngo);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Detail(string ngoId)
        {
            var ngo = _db.NGOs.FirstOrDefault(n => n.NgoId == ngoId);
            var ngoDTO = _mapper.MakeSingleInstance<NGO, NgoDTO>(ngo);
            return View(ngoDTO);
        }

        [HttpGet]
        public ActionResult Delete(string ngoId)
        {
            var delete = _db.NGOs.FirstOrDefault(n => n.NgoId == ngoId);
            _db.NGOs.Remove(delete);
            _db.SaveChanges();
            return RedirectToAction("Index");

        }

        [HttpGet]
        public ActionResult Edit(string ngoId)
        {
            var ngo = _db.NGOs.FirstOrDefault(n => n.NgoId == ngoId);
            var ngoDTO = _mapper.MakeSingleInstance<NGO, NgoDTO>(ngo);
            return View(ngoDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(NgoDTO ngoDTO)
        {
            var alreadyExistNgo = _db.NGOs.FirstOrDefault(n => n.Name.ToLower() == ngoDTO.Name.Trim().ToLower());
            if (alreadyExistNgo != null)
            {
                ViewBag.Msg = "Ngo already exists.";
                return View();
            }

            var ngo = _db.NGOs.FirstOrDefault(n => n.NgoId == ngoDTO.NgoId);

            var updateNgo = _mapper.MakeSingleInstance<NgoDTO, NGO>(ngoDTO);
            updateNgo.UpdatedAt = DateTime.Now;
            _db.Entry(ngo).CurrentValues.SetValues(updateNgo);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}