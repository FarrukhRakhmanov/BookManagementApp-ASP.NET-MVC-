using Final_Project_Farrukh_Rakhmanov.Data;
using Final_Project_Farrukh_Rakhmanov.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Final_Project_Farrukh_Rakhmanov.Controllers
{

    /// <summary>
    /// Controller responsible for managing library members and their book checkouts
    /// </summary>
    public class MemberController : Controller
    {
        // Database context for accessing the book management system
        private UserAccountContext context { get; set; }
        private readonly IWebHostEnvironment _webHostEnvironment;

        // Constructor injection of the database context
        public MemberController(UserAccountContext ctx, IWebHostEnvironment webHostEnvironment)
        {
            context = ctx;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index(string searchTerm, string sortBy, string filterBy)
        {
            var memberList = context.Users
                .OfType<Member>()
                .Include(b => b.MemberBooks)
                    .ThenInclude(mb => mb.Book)
                .ToList();

            var viewModel = new SearchViewModel();
            viewModel.SortBy = sortBy;

            memberList = Member.SearchMembers(memberList, searchTerm);
            memberList = Member.FilterByHasIssuedBook(memberList, filterBy);
            memberList = Member.SortMembers(memberList, sortBy);


            viewModel.Members = memberList;
            viewModel.SearchTerm = searchTerm;
            viewModel.FilterBy = filterBy;

            return View("MemberList", viewModel);
        }

        #region CRUD operations over members

        /// <summary>
        /// Displays the form for adding a new member
        /// </summary>
        [HttpGet]
        public IActionResult AddMember()
        {
            return View("EditMember", new Member());
        }

        /// <summary>
        /// Retrieves and displays the form for editing an existing member.
        /// </summary>
        /// <param name="id">The ID of the member to be edited.</param>
        /// <returns>The view for editing the specified member.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the member with the given ID is not found.</exception>
        [HttpGet]
        public IActionResult EditMember(int id)
        {
            var member = context.Users
                .OfType<Member>()
                .Include(m => m.MemberBooks)
                    .ThenInclude(mb => mb.Book)
                .FirstOrDefault(m => m.Id == id);

            return View(member);
        }


        /// <summary>
        /// Handles both creation and updating of members
        /// </summary>
        /// <param name="member">Member data from form submission</param>
        [HttpPost]
        public IActionResult EditMember(Member member)
        {
            if (ModelState.IsValid)
            {
                if (context.Users.Any(u => u.Username == member.Username))
                {
                    ModelState.AddModelError("Username", "Username already exists");
                    return View(member);
                }

                // Handle new member creation
                if (member.Id == 0)
                {
                    context.Users.Add(member);
                }

                // Handle existing member update
                else
                {
                    context.Users.Update(member);
                }
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(member);
        }

        /// <summary>
        /// Displays confirmation page for member deletion
        /// </summary>
        /// <param name="id">Member ID to delete</param>
        [HttpGet]
        public IActionResult DeleteMember(int id)
        { 
            var member = context.Users
                .OfType<Member>()
                .Include(m => m.MemberBooks)
                    .ThenInclude(mb => mb.Book)
                .FirstOrDefault(m => m.Id == id);

            return View(member);
        }

        /// <summary>
        /// Handles the actual deletion of a member
        /// </summary>
        /// <param name="member">Member to delete</param>
        [HttpPost]
        public IActionResult DeleteMember(Member member)
        {
            var existingMember = context.Users
               .OfType<Member>()
               .Include(m => m.MemberBooks)
                   .ThenInclude(mb => mb.Book)
               .FirstOrDefault(m => m.Id == member.Id);

            context.Users.Remove(existingMember);
            context.SaveChanges();
            return RedirectToAction("Index");
        }

        #endregion

        /// <summary>
        /// Displays the profile view of a specified user. If no ID is provided, retrieves the currently logged-in user.
        /// </summary>
        /// <param name="id">The ID of the user to display the profile for. If 0, the logged-in user's ID is used.</param>
        /// <returns>The profile view of the specified user.</returns>
        [HttpGet]
        public IActionResult Profile(int id)
        {
            int userId = id == 0 ? HttpContext.Session.GetInt32("UserId") ?? 0 : id;

            var member = context.Users
                .OfType<Member>()
                .Include(m => m.MemberBooks)
                    .ThenInclude(mb => mb.Book)
                .FirstOrDefault(m => m.Id == userId);

            var images = context.ProfilePictures.ToList();
            ViewBag.Images = images;

            return View(member);
        }

        /// <summary>
        /// Redirects to the profile page of the specified user.
        /// </summary>
        /// <param name="id">The ID of the user.</param>
        /// <returns>A redirection to the profile action.</returns>
        public IActionResult Upload(int id)
        {
            var user = context.Users.Find(id);
            return RedirectToAction("Profile", id);
        }

        /// <summary>
        /// Handles the upload of a profile picture for a user.
        /// </summary>
        /// <param name="name">The name of the image to associate with the profile picture.</param>
        /// <param name="imageFile">The uploaded image file.</param>
        /// <param name="id">The ID of the user uploading the profile picture.</param>
        /// <returns>A redirection to the user's profile page if successful, otherwise re-displays the upload form.</returns>
        /// <exception cref="Exception">Thrown if the user is not found.</exception>
        [HttpPost]
        public IActionResult Upload(string name, IFormFile imageFile, int id)
        {
            if (ModelState.IsValid && imageFile != null)
            {
                var user = context.Users.Find(id);
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

                context.Users.Update(user);
                context.SaveChanges();
                return RedirectToAction("Profile", "Member", user);
            }
            return View();
        }

        /// <summary>
        /// Deletes the profile picture of the specified user.
        /// </summary>
        /// <param name="id">The ID of the user whose profile picture is to be deleted.</param>
        /// <returns>The profile view with the profile picture removed.</returns>
        [HttpPost]
        public IActionResult DeleteImage(int id)
        {
            var user = context.Users.Find(id);
            if (user != null)
            {
                user.ProfilePicture = null;
                context.Update(user);
            }
            context.SaveChanges();
            return View("Profile", user);
        }

        /// <summary>
        /// Displays a profile picture by serving the image file from the server.
        /// </summary>
        /// <param name="id">The ID of the profile picture to display.</param>
        /// <returns>The image file if found, otherwise a 404 Not Found response.</returns>
        public IActionResult Display(int id)
        {
            var image = context.ProfilePictures.Find(id);

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


        #region Return & Issue books
        /// <summary>
        /// Displays list of members with checked out books
        /// </summary>
        public IActionResult ReturnBook()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            var member = context.Users
                .OfType<Member>()
                .Include(m => m.MemberBooks)
                    .ThenInclude(mb => mb.Book)
                .FirstOrDefault(m => m.Id == userId);

            return View(member);
        }

        /// <summary>
        /// Processes book return from a member
        /// </summary>
        /// <param name="MemberId">ID of member returning the book</param>
        [HttpPost]
        public IActionResult ReturnBook(int MemberId, int BookId)
        {
            var member = context.Users
                .OfType<Member>()
                .Include(m => m.MemberBooks)
                    .ThenInclude(mb => mb.Book)
                .SingleOrDefault(m => m.Id == MemberId);

            if (member != null)
            {
                // Find the MemberBook relationship for removal
                var memberBook = member.MemberBooks.FirstOrDefault(mb => mb.BookId == BookId);
                if (memberBook != null)
                {
                    // Remove the relationship
                    member.MemberBooks.Remove(memberBook);

                    // Update book availability if needed
                    var issuedBook = context.Books.Find(BookId);
                    if (issuedBook != null)
                    {
                        issuedBook.ReturnBook(); // Ensure this method updates book availability
                    }

                    // Save changes to the database
                    context.SaveChanges();

                    // Redirect to the EditMember view for the updated member
                    return RedirectToAction("ReturnBook", "Member", new { id = MemberId });
                }
            }

            // If return fails, reload member list with books
            var memberList = context.Users
                .OfType<Member>()
                .Include(m => m.MemberBooks)
                    .ThenInclude(mb => mb.Book)
                .OrderBy(m => m.Name)
                .ToList();

            return View("MemberList", memberList); // Use the correct view name for displaying the member list
        }


        /// <summary>
        /// Displays form for issuing a book to a member
        /// </summary>
        public IActionResult IssueBook(int id)
        {
            List<Book> books = new List<Book>();

            int userId = id == 0 ? HttpContext.Session.GetInt32("UserId") ?? 0 : id;
            var member = context.Users
                .OfType<Member>()
                .Include(m => m.MemberBooks)
                    .ThenInclude(mb => mb.Book)
                .FirstOrDefault(m => m.Id == userId);

            // Ensure MemberBooks is not null
            member.MemberBooks = member.MemberBooks ?? new List<MemberBook>();

            var memberBook = new MemberBook
            {
                Id = member.Id,
                Member = member
            };

            // Get books not already issued to the member
            if (member.MemberBooks.Any())
            {
                var issuedBookIds = member.MemberBooks.Select(mb => mb.BookId).ToList();
                books = context.Books
                    .Where(b => b.AvailableQuantity > 0 && !issuedBookIds.Contains(b.BookId))
                    .OrderBy(b => b.Title)
                    .ToList();
            }
            else
            {
                books = context.Books
                    .Where(b => b.AvailableQuantity > 0)
                    .OrderBy(b => b.Title)
                    .ToList();
            }

            ViewBag.Books = books;

            return View(memberBook);
        }


        /// <summary>
        /// Processes book issuance to a member
        /// </summary>
        /// <param name="member">Member data including book to issue</param>
        [HttpPost]
        public IActionResult IssueBook(MemberBook member)
        {
            var issueToMember = context.Users
                .OfType<Member>()
                .Include(m => m.MemberBooks)
                    .ThenInclude(mb => mb.Book)
                .FirstOrDefault(m => m.Id == member.Id);

            if (issueToMember != null)
            {
                var bookToIssue = context.Books.Find(member.BookId);

                if (bookToIssue != null)
                {
                    // Assign book to member                   
                    member.Book = bookToIssue;
                    if(issueToMember.MemberBooks == null)
                    {
                        issueToMember.MemberBooks = new List<MemberBook>();
                    }
                    issueToMember.MemberBooks.Add(member);

                    // Update book availability
                    bookToIssue.IssueBook();

                    context.SaveChanges();

                    return RedirectToAction("ReturnBook", "Member");
                }
            }

            // If issuance fails, repopulate dropdowns
            if (issueToMember.MemberBooks != null)
            {
                if(issueToMember.MemberBooks.Count > 0)
                {
                    var issuedBookIds = issueToMember.MemberBooks.Select(mb => mb.BookId).ToList();
                    ViewBag.Books = context.Books
                    .Where(b => b.AvailableQuantity > 0 && !issuedBookIds.Contains(b.BookId))
                    .OrderBy(b => b.Title)
                    .ToList();
                }
            }
            else
            {
                ViewBag.Books = context.Books
                .Where(b => b.AvailableQuantity > 0)
                .OrderBy(b => b.Title)
                .ToList();
            }
            return View(member);
        }
        #endregion
    }
}