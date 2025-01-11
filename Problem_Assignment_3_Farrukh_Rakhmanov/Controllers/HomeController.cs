using Final_Project_Farrukh_Rakhmanov.Data;
using Final_Project_Farrukh_Rakhmanov.Models;
using Microsoft.AspNetCore.Mvc;

namespace Final_Project_Farrukh_Rakhmanov.Controllers
{
    public class HomeController : Controller
    {
        public readonly UserAccountContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        /// <summary>
        /// Initializes the HomeController with the specified database context and hosting environment.
        /// </summary>
        /// <param name="context">The user account database context.</param>
        /// <param name="webHostEnvironment">The hosting environment for file uploads.</param>
        public HomeController(UserAccountContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        /// Checks if a user session is active by verifying the presence of a username in the session.
        /// </summary>
        /// <returns>True if the session is active; otherwise, false.</returns>
        public bool IsSessionActive()
        {
            return !string.IsNullOrEmpty(HttpContext.Session.GetString("Username"));
        }

        /// <summary>
        /// Displays the index page for a user by ID. Redirects to the login page if the session is not active.
        /// </summary>
        /// <param name="id">The ID of the user to display.</param>
        /// <returns>The index view if the session is active; otherwise, redirects to login.</returns>
        public IActionResult Index(int id)
        {
            if (!IsSessionActive())
            {
                TempData["SessionExpired"] = "Please log in or register to continue.";
                return RedirectToAction("Login", "Account");
            }
            var user = _context.Users.Find(id);
            return View(user);
        }

        /// <summary>
        /// Displays the profile page for a user. If the ID is 0, retrieves the currently logged-in user.
        /// </summary>
        /// <param name="id">The ID of the user whose profile is to be displayed.</param>
        /// <returns>The profile view for the specified user.</returns>
        [HttpGet]
        public IActionResult Profile(int id)
        {
            int userId = id == 0 ? HttpContext.Session.GetInt32("UserId") ?? 0 : id;

            var user = _context.Users.FirstOrDefault(u => u.Id == userId);

            var images = _context.ProfilePictures.ToList();
            ViewBag.Images = images;

            return View(user);
        }

        /// <summary>
        /// Updates a user's profile with new data. Ensures username uniqueness and validates the model.
        /// </summary>
        /// <param name="user">The user object containing the updated data.</param>
        /// <returns>The updated profile view if successful; otherwise, the view with validation errors.</returns>
        [HttpPost]
        public IActionResult Profile(User user)
        {
            var images = _context.ProfilePictures.ToList();
            ViewBag.Images = images;

            // Remove validation for password fields as they are not updated in this method
            if (ModelState.ContainsKey("Password")) ModelState.Remove("Password");
            if (ModelState.ContainsKey("ConfirmPassword")) ModelState.Remove("ConfirmPassword");

            if (ModelState.IsValid)
            {
                // Check for duplicate username
                var existingUserWithSameUsername = _context.Users
                    .FirstOrDefault(u => u.Username == user.Username && u.Id != user.Id);

                if (existingUserWithSameUsername != null)
                {
                    ModelState.AddModelError("Username", "Username already exists");
                    return View(user);
                }

                var existingUser = _context.Users.FirstOrDefault(u => u.Id == user.Id);

                if (existingUser != null)
                {
                    // Update existing user
                    _context.Entry(existingUser).CurrentValues.SetValues(user);
                    _context.Update(existingUser);
                    TempData["SuccessMessage"] = "Profile updated successfully!";

                    // Update session data
                    HttpContext.Session.SetString("Username", existingUser.Username);
                    HttpContext.Session.SetString("UserId", existingUser.Id.ToString());
                }
                else
                {
                    // Add new user if none exists
                    _context.Users.Add(user);
                    TempData["SuccessMessage"] = "Profile created successfully!";
                }

                _context.SaveChanges();
                return View(user);
            }

            return View(user);
        }

        /// <summary>
        /// Redirects to the profile page for a specified user.
        /// </summary>
        /// <param name="id">The ID of the user to redirect to.</param>
        /// <returns>A redirection to the Profile action.</returns>
        public IActionResult Upload(int id)
        {
            var user = _context.Users.Find(id);
            return RedirectToAction("Profile", id);
        }

        /// <summary>
        /// Handles the upload of a profile picture for a user.
        /// </summary>
        /// <param name="name">The name of the uploaded image.</param>
        /// <param name="imageFile">The image file to upload.</param>
        /// <param name="id">The ID of the user uploading the image.</param>
        /// <returns>A redirection to the Profile action if successful; otherwise, the current view.</returns>
        [HttpPost]
        public IActionResult Upload(string name, IFormFile imageFile, int id)
        {
            if (ModelState.IsValid && imageFile != null)
            {
                var user = _context.Users.Find(id);
                if (user == null)
                {
                    throw new Exception("User not found");
                }

                string uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
                string filePath = Path.Combine(uploadFolder, imageFile.FileName);

                Directory.CreateDirectory(uploadFolder);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    imageFile.CopyTo(fileStream);
                }

                user.ProfilePicture = new ProfilePicture
                {
                    Name = name,
                    ContentType = imageFile.ContentType,
                    ImagePath = filePath
                };

                _context.Users.Update(user);
                _context.SaveChanges();
                return RedirectToAction("Profile", "Home", user);
            }
            return View();
        }

        /// <summary>
        /// Deletes a user's profile picture and updates the database.
        /// </summary>
        /// <param name="id">The ID of the user whose profile picture is to be deleted.</param>
        /// <returns>The profile view with the profile picture removed.</returns>
        [HttpPost]
        public IActionResult DeleteImage(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                user.ProfilePicture = null;
                _context.Update(user);
            }
            _context.SaveChanges();
            return View("Profile", user);
        }

        /// <summary>
        /// Displays a profile picture by serving the image file from the server.
        /// </summary>
        /// <param name="id">The ID of the profile picture to display.</param>
        /// <returns>The image file if found; otherwise, a 404 Not Found response.</returns>
        public IActionResult Display(int id)
        {
            var image = _context.ProfilePictures.Find(id);

            if (image != null && !string.IsNullOrEmpty(image.ImagePath))
            {
                var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, image.ImagePath);
                if (System.IO.File.Exists(imagePath))
                {
                    var imageFileStream = System.IO.File.OpenRead(imagePath);
                    return File(imageFileStream, image.ContentType);
                }
            }
            return NotFound();
        }
    }
}
