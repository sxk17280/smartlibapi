using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using SmartLibraryManager.Models;
using SmartLibraryManager.Services;
using SmartLibraryManager.Utilties;
using SmartLibraryManager.ViewModels;

namespace SmartLibraryManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BooksService _booksService;
        private readonly EmailService _emailService;
        private readonly BookCategoryService _bookCategoryService;
        private readonly AuthorService _authorService;
        private readonly UserService _userService;

        public BooksController(BooksService booksService, EmailService emailService, BookCategoryService bookCategoryService, AuthorService authorService, UserService userService)
        {
            _booksService = booksService;
            _emailService = emailService;
            _bookCategoryService = bookCategoryService;
            _authorService = authorService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<List<Book>> Get()
        {
            return await _booksService.GetBooksAsync();
        }

        [HttpGet("{bookId}")]
        public async Task<Book> Get(string bookId)
        {
            return await _booksService.GetBookAsync(bookId);
        }


        [HttpGet("adminIssuedBooks")]
        public async Task<List<BookRecord>> AdminIssuedBooks()
        {
            return await _booksService.AdminIssuedBooks();
        }

        [HttpGet("userIssuedBooks/{userId}")]
        public async Task<List<BookRecord>> UserIssuedBooks(string userId)
        {
            return await _booksService.UserIssuedBooks(userId);
        }


        [HttpPost]
        public async Task<IActionResult> Post(BookVM model)
        {
            var book = new Book()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Title = model.Title,
                ISBN = string.IsNullOrEmpty(model.ISBN) ? Guid.NewGuid().ToString() : model.ISBN,
                Description = model.Description,
                Author = model.Author,
                PublishedYear = model.PublishedYear,
                Category = model.Category,
                Image = model.Image,
                Status = BookStatus.Available.ToString(),
                isAvailable = true,
                bookTransactions = new List<BookTransaction>(),
                BookId = string.IsNullOrEmpty(model.BookId) ? Guid.NewGuid().ToString() : model.BookId
            };

            await _booksService.CreateAsync(book);

            return CreatedAtAction(nameof(Get), new { id = book.Title }, book);
        }

        [HttpPost("Checkin")]
        public async Task<IActionResult> Checkin(TransactionVM transaction)
        {
            var bookRecord = await _booksService.UserIssuedBooks(transaction.UserId);
            bookRecord = bookRecord.Where(x => x.bookTransactions.IsActive).ToList();
            if (bookRecord.Count >= 3)
            {
                return BadRequest("LIMIT_EXCEEDED");
            }

            var book = await _booksService.GetBookAsync(transaction.BookId);
            var user = await _userService.GetAsync(transaction.UserId);

            if (book == null || user == null)
            {
                return BadRequest();
            }
            else
            {
                var bookTransaction = new BookTransaction()
                {
                    Id = ObjectId.GenerateNewId().ToString(),
                    UserId = user.UserId,
                    BookId = transaction.BookId,
                    TransactionDate = DateTime.Now,
                    CheckInDateTime = DateTime.Now,
                    DueDate = DateTime.Now.AddDays(15),
                    Status = BookStatus.Issued.ToString(),
                    IsActive = true,
                    TransactionId = Guid.NewGuid().ToString()
                };
                book.bookTransactions.Add(bookTransaction);
                book.Status = BookStatus.NotAvailable.ToString();
                book.isAvailable = false;
                await _booksService.UpdateAsync(book.Id, book);

                var emailBody = PrepareEmailBody(new MailBodyProperties()
                {
                    BookId = transaction.BookId,
                    Status = BookStatus.Issued.ToString(),
                    UserId = user.UserId,
                    DueDate = bookTransaction.DueDate,
                    DateTaken = bookTransaction.CheckInDateTime,
                    ISBN = book.ISBN,
                    Title = book.Title,
                    Description = book.Description,
                    Fine = 0
                });
                _emailService.SendEmail(new Message(new string[] { user.Email }, "Book Issued", emailBody, new string[] { }, new string[] { }, true));

                return Ok(true);
            }
        }

        [HttpPost("Checkout")]
        public async Task<IActionResult> Checkout(TransactionVM transaction)
        {
            var bookRecord = await _booksService.GetTransaction(transaction.TransactionId);
            var book = await _booksService.GetBookAsync(transaction.BookId);
            var user = await _userService.GetAsync(transaction.UserId);

            if (book == null || user == null || bookRecord == null)
            {
                return BadRequest();
            }
            else
            {
                book.isAvailable = true;
                book.Status = BookStatus.Available.ToString();
                transaction.CheckInDateTime = bookRecord.bookTransactions.CheckInDateTime;
                transaction.DueDate = bookRecord.bookTransactions.DueDate;
                await _booksService.UpdateAsync(book.Id, book);
                await _booksService.UpdateTransaction(transaction, isRenew: false, prevFine: bookRecord.bookTransactions.Fine);


                bookRecord = await _booksService.GetTransaction(transaction.TransactionId);

                var emailBody = PrepareEmailBody(new MailBodyProperties()
                {
                    BookId = transaction.BookId,
                    Status = BookStatus.Returned.ToString(),
                    UserId = user.UserId,
                    DueDate = bookRecord.bookTransactions.DueDate,
                    DateTaken = bookRecord.bookTransactions.CheckInDateTime,
                    ISBN = book.ISBN,
                    Title = book.Title,
                    Description = book.Description,
                    Fine = bookRecord.bookTransactions.Fine
                });
                _emailService.SendEmail(new Message(new string[] { user.Email }, "Book Returned", emailBody, new string[] { }, new string[] { }, true));

                return Ok(true);
            }
        }

        [HttpPost("Renew")]
        public async Task<IActionResult> Renew(TransactionVM transaction)
        {
            var bookRecord = await _booksService.GetTransaction(transaction.TransactionId);
            var book = await _booksService.GetBookAsync(transaction.BookId);
            var user = await _userService.GetAsync(transaction.UserId);

            if (book == null || user == null || bookRecord == null)
            {
                return BadRequest();
            }
            else if (bookRecord.bookTransactions.RenewalCount >= 3)
            {
                return BadRequest("LIMIT_EXCEEDED");
            }
            else
            {
                transaction.CheckInDateTime = bookRecord.bookTransactions.CheckInDateTime;
                transaction.DueDate = bookRecord.bookTransactions.DueDate;
                transaction.RenewalCount = bookRecord.bookTransactions.RenewalCount;
                await _booksService.UpdateTransaction(transaction, isRenew: true, prevFine: bookRecord.bookTransactions.Fine);

                bookRecord = await _booksService.GetTransaction(transaction.TransactionId);

                var emailBody = PrepareEmailBody(new MailBodyProperties()
                {
                    BookId = transaction.BookId,
                    Status = BookStatus.Returned.ToString(),
                    UserId = user.UserId,
                    DueDate = bookRecord.bookTransactions.DueDate,
                    DateTaken = bookRecord.bookTransactions.CheckInDateTime,
                    ISBN = book.ISBN,
                    Title = book.Title,
                    Description = book.Description,
                    Fine = bookRecord.bookTransactions.Fine
                });
                _emailService.SendEmail(new Message(new string[] { user.Email }, "Book Renewed", emailBody, new string[] { }, new string[] { }, true));

                return Ok(true);
            }
        }


        private string PrepareEmailBody(MailBodyProperties bodyProperties)
        {
            var templateManager = new EmailTemplateManager();
            var body = templateManager.GetTemplate(EmailTemplate.BookNotification);
            body = body.Replace("##USERID", bodyProperties.UserId);
            body = body.Replace("##BOOKID", bodyProperties.BookId);
            body = body.Replace("##ISBN", bodyProperties.ISBN);
            body = body.Replace("##STATUS", bodyProperties.Status);
            body = body.Replace("##TITLE", bodyProperties.Title);
            body = body.Replace("##DUEDATE", bodyProperties.DueDate.ToString());
            body = body.Replace("##DATETAKEN", bodyProperties.DateTaken.ToString());
            body = body.Replace("##DESCRIPTION", bodyProperties.Description);
            body = body.Replace("##FINE", bodyProperties.Fine.ToString());

            return body;
        }


        [HttpGet("dashboard")]
        public async Task<DashboardModel> DashboardData()
        {
            return new DashboardModel()
            {
                TotalAuthors = await _authorService.GetTotalAuthorCount(),
                TotalBooks = await _booksService.GetTotalBookCount(),
                TotalCategories = await _bookCategoryService.GetTotalCategoryCount(),
                TotalFine =await _userService.GetTotalFinesCount(),
                TotalUsers= await _userService.GetTotalUserCount()
            };
        }

    }
}
