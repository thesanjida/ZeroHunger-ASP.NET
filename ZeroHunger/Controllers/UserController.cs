using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ZeroHunger.DTOs;
using ZeroHunger.DTOs.NGO;
using ZeroHunger.DTOs.Role;
using ZeroHunger.DTOs.User;
using ZeroHunger.EF;
using ZeroHunger.Helpers;
using ZeroHunger.Mapping;

namespace ZeroHunger.Controllers
{
    public class UserController : Controller
    {
        AutoMapperConfig _mapper;
        ZeroHungerEntities _db;
        public UserController()
        {
            _mapper = new AutoMapperConfig();
            _db = new ZeroHungerEntities();
        }
        [HttpGet]
        public ActionResult Index()
        {
            var users = _db.Users.ToList();
            var userDTOs = _mapper.MakeList<User, UserDTO>(users);

            return View(userDTOs);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var roles = _db.Roles.ToList();
            var rolesDTO = _mapper.MakeList<Role, RoleDTO>(roles);
            ViewBag.Roles = rolesDTO;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SignupDTO signupDTO)
        {
            var roles = _db.Roles.ToList();
            var rolesDTO = _mapper.MakeList<Role, RoleDTO>(roles);
            ViewBag.Roles = rolesDTO;

            if (!ModelState.IsValid)
            {
                ViewBag.Msg = "Please, input all the field.";
            }

            var alreadyExistEmail = _db.Users.FirstOrDefault(u => u.Email == signupDTO.Email);
            if (alreadyExistEmail != null)
            {
                ViewBag.Msg = "Email already exists.";
                return View();
            }

            var alreadyExistMobile = _db.Users.FirstOrDefault(u => u.Mobile == signupDTO.Mobile);
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

        [HttpGet]
        public ActionResult Detail(string userId)
        {
            var user = _db.Users.FirstOrDefault(u => u.UserId == userId);
            var userDTO = _mapper.MakeSingleInstance<User, UserDTO>(user);
            return View(userDTO);
        }

        [HttpGet]
        public ActionResult Delete(string userId)
        {
            var deleteUser = _db.Users.FirstOrDefault(u => u.UserId == userId);
            _db.Users.Remove(deleteUser);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(string userId)
        {
            var roles = _db.Roles.ToList();
            var rolesDTO = _mapper.MakeList<Role, RoleDTO>(roles);
            ViewBag.Roles = rolesDTO;

            var user = _db.Users.FirstOrDefault(u => u.UserId == userId);
            var userDTO = _mapper.MakeSingleInstance<User, UserDTO>(user);
            return View(userDTO);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserDTO userDTO)
        {
            var user = _db.Users.FirstOrDefault(u => u.UserId == userDTO.UserId);
            var updateUser = _mapper.MakeSingleInstance<UserDTO, User>(userDTO);
            updateUser.UpdatedAt = DateTime.Now;
            _db.Entry(user).CurrentValues.SetValues(updateUser);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}