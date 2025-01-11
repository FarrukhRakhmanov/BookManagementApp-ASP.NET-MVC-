using Final_Project_Farrukh_Rakhmanov.Data;
using Final_Project_Farrukh_Rakhmanov.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Reflection.Metadata.BlobBuilder;


namespace Final_Project_Farrukh_Rakhmanov.Controllers
{
    /// <summary>
    /// Controller responsible for managing library books, including inventory management
    /// and book lending operations
    /// </summary>
    public class BookController : Controller
    {
        // Database context for accessing the book management system
        private UserAccountContext context { get; set; }

        // Constructor injection of the database context
        public BookController(UserAccountContext ctx) =>
            context = ctx;

        public IActionResult MemberIndex(string searchTerm, string genre, string sortBy, string filterBy)
        {
            // Retrieve the currently logged-in user's ID from the session
            int? userId = HttpContext.Session.GetInt32("UserId");

            // Fetch the member associated with the user ID, including their issued books
            var member = context.Users
                .OfType<Member>()
                .Include(m => m.MemberBooks)
                    .ThenInclude(mb => mb.Book)
                .FirstOrDefault(m => m.Id == userId);

            // Ensure MemberBooks is initialized to avoid null reference issues
            member.MemberBooks = member.MemberBooks ?? new List<MemberBook>();

            List<Book> books;

            // Fetch books that are not already issued to the member
            if (member.MemberBooks.Any())
            {
                // Get the IDs of books currently issued to the member
                var issuedBookIds = member.MemberBooks.Select(mb => mb.BookId).ToList();

                // Retrieve books that are available and not issued to the member
                books = context.Books
                    .Where(b => b.AvailableQuantity > 0 && !issuedBookIds.Contains(b.BookId))
                    .Include(b => b.Genre) // Load genre information eagerly
                    .OrderBy(b => b.Title) // Sort books alphabetically by title
                    .ToList();
            }
            else
            {
                // Retrieve all available books if the member has no issued books
                books = context.Books
                    .Where(b => b.AvailableQuantity > 0)
                    .Include(b => b.Genre)
                    .OrderBy(b => b.Title)
                    .ToList();
            }

            // Apply search, filter, and sort functionalities to the list of books
            var bookList = Book.SearchBooks(books, searchTerm); // Filter books by the search term
            bookList = Book.FilterByGenre(bookList, genre);     // Filter books by the selected genre
            bookList = Book.FilterBy(bookList, filterBy);       // Apply additional filtering criteria
            bookList = Book.SortBooks(bookList, sortBy);        // Sort books based on the selected sorting option

            // Create a view model to pass data to the view
            var viewModel = new SearchViewModel
            {
                SortBy = sortBy,                  // Sorting criteria
                Books = bookList,                 // List of processed books
                Genres = Book.GetGenres(bookList),// Extract available genres from the book list
                SearchTerm = searchTerm,          // Current search term
                SelectedGenre = genre,            // Selected genre for filtering
                FilterBy = filterBy               // Additional filtering criteria
            };

            return View("MemberIndex", viewModel); // Return the view with the prepared view model
        }

        public IActionResult AdminIndex(string searchTerm, string genre, string sortBy, string filterBy)
        {
            // Fetch all available books from the database
            var books = context.Books
                    .Where(b => b.AvailableQuantity > 0) // Include only books with available copies
                    .Include(b => b.Genre)              // Load genre information eagerly
                    .OrderBy(b => b.Title)              // Sort books alphabetically by title
                    .ToList();

            // Apply search, filter, and sort functionalities to the list of books
            var bookList = Book.SearchBooks(books, searchTerm); // Filter books by the search term
            bookList = Book.FilterByGenre(bookList, genre);     // Filter books by the selected genre
            bookList = Book.FilterBy(bookList, filterBy);       // Apply additional filtering criteria
            bookList = Book.SortBooks(bookList, sortBy);        // Sort books based on the selected sorting option

            // Create a view model to pass data to the view
            var viewModel = new SearchViewModel
            {
                SortBy = sortBy,                  // Sorting criteria
                Books = bookList,                 // List of processed books
                Genres = Book.GetGenres(bookList),// Extract available genres from the book list
                SearchTerm = searchTerm,          // Current search term
                SelectedGenre = genre,            // Selected genre for filtering
                FilterBy = filterBy               // Additional filtering criteria
            };

            return View("AdminIndex", viewModel); // Return the same view ("Index") with the prepared view model
        }


        #region CRUD operations over books

        /// <summary>
        /// Displays the form for adding a new book
        /// </summary>
        [HttpGet]
        public IActionResult AddBook()
        {
            // Populate ViewBag with genres for dropdown
            ViewBag.Genres = context.Genres
                .OrderBy(g => g.Name)
                .ToList();

            return View("EditBook", new Book());
        }

        /// <summary>
        /// Displays the form for editing an existing book
        /// </summary>
        /// <param name="id">Book ID to edit</param>
        [HttpGet]
        public IActionResult EditBook(int id)
        {
            // Populate ViewBag with genres for dropdown
            ViewBag.Genres = context.Genres
                .OrderBy(g => g.Name)
                .ToList();

            var book = context.Books.Find(id);

            return View(book);
        }

        /// <summary>
        /// Handles both creation and updating of books
        /// </summary>
        /// <param name="book">Book data from form submission</param>
        [HttpPost]
        public IActionResult EditBook(Book book)
        {
            if (ModelState.IsValid)
            {
                // Update availability status based on quantity
                book.SetDefaults();

                // Handle new book creation
                if (book.BookId == 0)
                {
                    // Ensure available quantity matches total quantity for new books
                    if (book.AvailableQuantity != book.Quantity)
                    {
                        book.AvailableQuantity = book.Quantity;
                        book.IsAvailable = true;
                    }
                    context.Books.Add(book);
                }
                // Handle existing book update
                else
                {
                    // Check if any copies are currently checked out
                    var memberList = context.Users.OfType<Member>()
                        .Where(m => m.MemberBooks.Any(mb => mb.BookId == book.BookId)).ToList();

                    if (memberList.Count > 0)
                    {
                        // Adjust available quantity based on checked out copies
                        book.AvailableQuantity = book.Quantity - memberList.Count;
                    }
                    else
                    {
                        book.AvailableQuantity = book.Quantity;
                    }

                    // Update availability status
                    book.IsAvailable = book.AvailableQuantity > 0;

                    context.Update(book);
                }
                context.SaveChanges();
                return RedirectToAction("AdminIndex");
            }
            else
            {
                // If validation fails, repopulate genres dropdown
                ViewBag.Genres = context.Genres
                    .OrderBy(g => g.Name)
                    .ToList();

                return View(book);
            }
        }

        /// <summary>
        /// Displays confirmation page for book deletion
        /// </summary>
        /// <param name="id">Book ID to delete</param>

        [HttpGet]
        public IActionResult DeleteBook(int id)
        {
            var book = context.Books.Find(id);

            // Check if book is currently checked out
            var memberList = context.Users
                .OfType<Member>()
                .Where(m => m.MemberBooks.Any(mb => mb.BookId == book.BookId))
                .ToList();

            ViewBag.Members = memberList;

            return View(book);
        }

        /// <summary>
        /// Handles the actual deletion of a book
        /// </summary>
        /// <param name="book">Book to delete</param>
        [HttpPost]
        public IActionResult DeleteBook(Book book)
        {
            // Prevent deletion if book is checked out
            var memberList = context.Users
                .OfType<Member>()
                .Where(m => m.MemberBooks.Any(mb => mb.BookId == book.BookId))
                .ToList();

            context.Books.Remove(book);
            context.SaveChanges();
            return RedirectToAction("AdminIndex");
        }
        #endregion

        #region Return & Issue books
        /// <summary>
        /// Displays list of members with checked out books
        /// </summary>
        public IActionResult ReturnBook()
        {
            var memberList = context.Users
                .OfType<Member>()
                .Include(m => m.MemberBooks) 
                    .ThenInclude(mb => mb.Book) 
                .Where(m => m.MemberBooks != null && m.MemberBooks.Any())
                .OrderBy(m => m.Name)
                .ToList();

            return View(memberList);
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
                }
            }

            // If return fails, reload member list with books
            var memberList = context.Users
                        .OfType<Member>()
                        .Include(m => m.MemberBooks)
                        .ThenInclude(mb => mb.Book)
                        .Where(m => m.MemberBooks != null && m.MemberBooks.Any())
                        .OrderBy(m => m.Name)
                        .ToList();

            return View("ReturnBook", memberList); // Use the correct view name for displaying the member list
        }


        /// <summary>
        /// Displays form for issuing a book to a member
        /// </summary>
        public IActionResult IssueBook()
        {
            
            ViewBag.Members = context.Users
                .OfType<Member>()
                .Include(m => m.MemberBooks)
                .OrderBy(m => m.Name).ToList();

            ViewBag.Books = context.Books
                .Where(b => b.AvailableQuantity > 0)
                .OrderBy(b => b.Title).ToList();

            return View(new MemberBook());
        }

        /// <summary>
        /// Processes book issuance to a member
        /// </summary>
        /// <param name="member">Member data including book to issue</param>
        [HttpPost]
        public IActionResult IssueBook(MemberBook memberBook)
        {
            ViewBag.Members = context.Users
                .OfType<Member>()
                .Include(m => m.MemberBooks)
                .OrderBy(m => m.Name).ToList();

            ViewBag.Books = context.Books
                .Where(b => b.AvailableQuantity > 0)
                .OrderBy(b => b.Title).ToList(); 

            var issueToMember = context.Users
                .OfType<Member>()
                .Include(m => m.MemberBooks)
                    .ThenInclude(mb => mb.Book)
                .FirstOrDefault(m => m.Id == memberBook.Id);

            if (issueToMember != null)
            {
                var bookToIssue = context.Books.Find(memberBook.BookId);

                if (bookToIssue != null)
                {
                    // Check if the book is already issued to the member
                    bool alreadyIssued = issueToMember.MemberBooks
                        .Any(mb => mb.BookId == bookToIssue.BookId);

                    if (alreadyIssued)
                    {
                        TempData["ErrorMessage"] = "This book is already issued to the member.";
                        return View(new MemberBook());
                    }

                    // Create a new MemberBook instance
                    var newMemberBook = new MemberBook
                    {
                        Member = issueToMember,
                        Book = bookToIssue,
                        BookId = bookToIssue.BookId
                    };

                    issueToMember.MemberBooks.Add(newMemberBook);

                    // Update book availability
                    bookToIssue.IssueBook();

                    // Save changes
                    context.SaveChanges();

                    return RedirectToAction("ReturnBook");
                }
            }

            TempData["ErrorMessage"] = "An error occurred. Please try again.";
            return View(new MemberBook());
        }

        #endregion
    }
}