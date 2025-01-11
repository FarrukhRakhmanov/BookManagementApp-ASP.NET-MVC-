using Final_Project_Farrukh_Rakhmanov.Data;
using Final_Project_Farrukh_Rakhmanov.Models;
using Microsoft.AspNetCore.Mvc;

namespace Final_Project_Farrukh_Rakhmanov.Controllers
{
    public class AccountController : Controller
    {
        public readonly UserAccountContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AccountController(UserAccountContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("Username") != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user == null)
            {
                TempData["InvalidCredentials"] = "Invalid Username or Password";
            }
            else
            {
                HttpContext.Session.SetString("Role", user.Role);
                HttpContext.Session.SetString("Username", user.Username);
                HttpContext.Session.SetString("UserId", user.Id.ToString());
                HttpContext.Session.SetInt32("UserId", user.Id);

                TempData["Title"] = user.Role == "Admin" ? "Admin Panel" : "Member Page";
                return RedirectToAction("Index", "Home", new { id = user.Id });
            }
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        public IActionResult Register()
        {
            User user = new User();
            return View(user);
        }

        [HttpPost]
        public IActionResult Register(User user, IFormFile imageFile)
        {

            if (ModelState.IsValid)
            {
                if (_context.Users.Any(u => u.Username == user.Username))
                {
                    ModelState.AddModelError("Username", "Username already exists");
                    return View(user);
                }

                string uplodaFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
                string filePath = Path.Combine(uplodaFolder, imageFile.FileName);

                Directory.CreateDirectory(uplodaFolder);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    imageFile.CopyTo(fileStream);
                }

                user.ProfilePicture = new ProfilePicture
                {
                    Name = user.Name,
                    ContentType = imageFile.ContentType,
                    ImagePath = filePath
                };

                user.Role = "Member";
                _context.Users.Add(user);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Successfull registration, please login to access your profile!";
                return RedirectToAction("Login", TempData);
            }
            return View(user);
        }

        [HttpGet]
        public IActionResult UpdatePassword(int id)
        {
            var user = _context.Users.Find(id);
            return View(user);
        }

        [HttpPost]
        public IActionResult UpdatePassword(string oldPassword, User user)
        {
            var existingUser = _context.Users.Find(user.Id);
            if (ModelState.ContainsKey("Name")) ModelState.Remove("Name");
            if (ModelState.ContainsKey("DateOfBirth")) ModelState.Remove("DateOfBirth");
            if (ModelState.ContainsKey("Age")) ModelState.Remove("Age");
            if (ModelState.ContainsKey("Role")) ModelState.Remove("Role");
            if (ModelState.ContainsKey("Username")) ModelState.Remove("Username");

            if (ModelState.IsValid)
            {
                if (existingUser != null)
                {
                     if(oldPassword == user.Password || oldPassword == user.ConfirmPassword)
                    {
                        TempData["WrongPassword"] = "You are trying to assign the old password";
                        return View("UpdatePassword", user);
                    }


                    if (existingUser.Password == oldPassword)
                    {
                        existingUser.Password = user.Password;
                        existingUser.Password = user.ConfirmPassword;
                        _context.Update(existingUser);
                    }
                    else
                    {
                        TempData["WrongPassword"] = "Provided password is not correct";
                        return View("UpdatePassword", user);
                    }

                    TempData["SuccessMessage"] = "Password updated successfully!";
                    _context.SaveChanges();
                    return RedirectToAction("Profile", "Home", new { id = existingUser.Id });
                }
            }
            return View(existingUser);
        }
    }
}
