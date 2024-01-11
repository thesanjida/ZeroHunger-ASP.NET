using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ZeroHunger.DTOs;
using ZeroHunger.DTOs.Role;
using ZeroHunger.EF;
using ZeroHunger.Helpers;
using ZeroHunger.Mapping;


namespace ZeroHunger.Controllers
{
    public class AuthController : Controller
    {
        AutoMapperConfig _mapper;
        ZeroHungerEntities _db;
        public AuthController()
        {
            _mapper = new AutoMapperConfig();
            _db = new ZeroHungerEntities();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginDTO loginDTO)
        {
            if (loginDTO.Email == null || loginDTO.Password == null)
            {
                ViewBag.Msg = "Email & password can't be empty.";
                return View();
            }
           
            var user = _db.Users.FirstOrDefault(u => u.Email == loginDTO.Email);

            var checkHashedPassword = PasswordHelper.VerifyPassword(loginDTO.Password, user.Password);

            if (checkHashedPassword == false)
            {
                ViewBag.Msg = "Email or password is incorrect.";
                return View();
            }

            return RedirectToAction("Index", "User");
        }

        [HttpGet]
        public ActionResult Signup()
        {
            var roles = _db.Roles.ToList();
            var rolesDTO = _mapper.MakeList<Role, RoleDTO>(roles);
            ViewBag.Roles = rolesDTO;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Signup(SignupDTO signupDTO)
        {
            var roles = _db.Roles.ToList();
            var rolesDTO = _mapper.MakeList<Role, RoleDTO>(roles);
            ViewBag.Roles = rolesDTO;

            if (!ModelState.IsValid)
            {
                ViewBag.Msg = "Please, input all the field.";
            }

            var alreadyExistEmail = _db.Users.Where(u => u.Email == signupDTO.Email).FirstOrDefault();
            if (alreadyExistEmail != null)
            {
                ViewBag.Msg = "Email already exists.";
                return View();
            }

            var alreadyExistMobile = _db.Users.Where(u => u.Mobile == signupDTO.Mobile).FirstOrDefault();
            if (alreadyExistMobile != null)
            {
                ViewBag.Msg = "Mobile already exists.";
                return View();
            }

            var newUser = new User()
            {
                UserId = GenerateId.MakeId(),
                Password = PasswordHelper.HashPassword(signupDTO.Password),
            };

            signupDTO.Password = newUser.Password;
            signupDTO.UserId = newUser.UserId;
            signupDTO.CreatedAt = DateTime.Now;

            var user = _mapper.MakeSingleInstance<SignupDTO, User>(signupDTO);
            _db.Users.Add(user);
            _db.SaveChanges();

            var getRole = _db.Roles.FirstOrDefault(r => r.Id == signupDTO.RoleId);

            if (getRole.Name == "NGO")
            {
                var addNGOs = new NGO()
                {
                    NgoId = GenerateId.MakeId(),
                    Name = signupDTO.Name + " " + getRole.Name,
                    CreatedAt = DateTime.Now,
                };
                _db.NGOs.Add(addNGOs);
                _db.SaveChanges();
            }

            if (getRole.Name == "Restaurant")
            {
                var addFoodSource = new FoodSource()
                {
                    FoodSourceId = GenerateId.MakeId(),
                    Name = signupDTO.Name + " " + getRole.Name,
                    CreatedAt = DateTime.Now,
                };
                _db.FoodSources.Add(addFoodSource);
                _db.SaveChanges();
            }

            return RedirectToAction("Index", "User");
        }
    }
}