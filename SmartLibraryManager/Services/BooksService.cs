using Microsoft.Extensions.Options;
using SmartLibraryManager.Models;
using MongoDB.Driver;
using SmartLibraryManager.Common.Models;
using SmartLibraryManager.Common.IntitialData;
using MongoDB.Bson;
using SmartLibraryManager.ViewModels;
using static MongoDB.Bson.Serialization.Serializers.SerializerHelper;
using System.Transactions;

namespace SmartLibraryManager.Services
{
    public class BooksService
    {
        private readonly IMongoCollection<Book> _booksCollection;
        private readonly UserService _userService;
        private readonly EmailService _emailService;

        public BooksService(IOptions<DatabaseSettings> bookStoreDatabaseSettings, UserService userService, EmailService emailService)
        {
            var mongoClient = new MongoClient(
            bookStoreDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                bookStoreDatabaseSettings.Value.DatabaseName);

            _booksCollection = mongoDatabase.GetCollection<Book>("Books");

            _userService = userService;
            _emailService = emailService;
        }
        public async Task<List<Book>> GetBooksAsync()
        {
            return await _booksCollection.Find(_ => _.isAvailable).ToListAsync();
        }
        public async Task<Book> GetBookAsync(string id)
        {
            return await _booksCollection.Find(x => x.BookId == id).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(Book book)
        {

            var users = await _userService.GetAsync();

            var emails = users.Select(x => x.Email).ToArray();

            var subject = $"New Arrival status of book - {book.Title}";
            var body = $"<p>The book with Title <b>{book.Title}</b> by the author <b>{book.Author}</b> recently added in the Library. Kindly visit the library to know more about the book.</p><p>Thank you</p>";

            if (emails != null)
                _emailService.SendEmail(new Message(emails, subject, body, new string[] { }, new string[] { }, true));


            await _booksCollection.InsertOneAsync(book);
        }

        public async Task UpdateAsync(string id, Book updatedBook)
        {
            await _booksCollection.ReplaceOneAsync(x => x.Id == id, updatedBook);
        }

        public async Task RemoveAsync(string id)
        {
            await _booksCollection.DeleteOneAsync(x => x.Id == id);
        }

        public async Task<BookRecord> GetTransaction(string transactionId)
        {
            return await _booksCollection.Aggregate().Unwind<Book, BookRecord>(x => x.bookTransactions).Match(x => x.bookTransactions.TransactionId == transactionId).FirstOrDefaultAsync();
        }

        public async Task<List<BookRecord>> AdminIssuedBooks()
        {
            var transactions = await _booksCollection.Aggregate().Unwind<Book, BookRecord>(x => x.bookTransactions).Match(x => true).ToListAsync();
            foreach (var item in transactions)
            {
                // if (item.bookTransactions.Status != "Returned")
                //   item.bookTransactions.Fine = await UpdateFine(item.bookTransactions.TransactionId, item.BookId, item.bookTransactions.CheckInDateTime);
            }

            return transactions;
        }

        public async Task<List<BookRecord>> UserIssuedBooks(string userId)
        {
            var transactions = await _booksCollection.Aggregate().Unwind<Book, BookRecord>(x => x.bookTransactions).Match(x => x.bookTransactions.UserId == userId).ToListAsync();
            foreach (var item in transactions)
            {
                // if (item.bookTransactions.Status != "Returned")
                //   item.bookTransactions.Fine = await UpdateFine(item.bookTransactions.TransactionId, item.BookId, item.bookTransactions.CheckInDateTime);
            }
            return transactions;
        }

        public async Task<int> UpdateFine(string transactionId, string bookId, DateTime CheckInDateTime)
        {

            var fine = 0;
            var days = (int)(DateTime.Now - CheckInDateTime).TotalDays;
            if (days > 15)
            {
                fine = days - 15;
            }
            var filter = Builders<Book>.Filter.Eq(x => x.BookId, bookId)
                        & Builders<Book>.Filter.ElemMatch(c => c.bookTransactions, Builders<BookTransaction>.Filter.Eq(x => x.TransactionId, transactionId));
            var update = Builders<Book>.Update.Set(x => x.bookTransactions[-1].Fine, fine);

            await _booksCollection.UpdateOneAsync(filter, update);

            return fine;

        }

        public async Task UpdateTransaction(TransactionVM transaction, bool isRenew, int prevFine)
        {
            var fine = prevFine;
            var days = (int)(DateTime.Now - transaction.DueDate).TotalDays;
            if (days < 0)
            {
                days = 0;
            }
            fine = prevFine + days;
            if (isRenew)
            {
                transaction.CheckInDateTime = DateTime.Now;
                transaction.DueDate = DateTime.Now.AddDays(15);
                transaction.IsActive = true;
            }
            else
            {
                transaction.CheckOutDateTime = DateTime.Now;
                transaction.IsActive = false;
            }
            transaction.Status = isRenew ? BookStatus.Renewed.ToString() : BookStatus.Returned.ToString();
            transaction.RenewalCount = isRenew ? transaction.RenewalCount + 1 : transaction.RenewalCount;

            var filter = Builders<Book>.Filter.Eq(x => x.BookId, transaction.BookId)
                       & Builders<Book>.Filter.ElemMatch(c => c.bookTransactions, Builders<BookTransaction>.Filter.Eq(x => x.TransactionId, transaction.TransactionId));
            var update = Builders<Book>.Update
                                        .Set(x => x.bookTransactions[-1].Status, transaction.Status)
                                        .Set(x => x.bookTransactions[-1].DueDate, transaction.DueDate)
                                        .Set(x => x.bookTransactions[-1].IsActive, transaction.IsActive)
                                        .Set(x => x.bookTransactions[-1].CheckOutDateTime, transaction.CheckOutDateTime)
                                        .Set(x => x.bookTransactions[-1].CheckInDateTime, transaction.CheckInDateTime)
                                        .Set(x => x.bookTransactions[-1].Fine, fine)
                                        .Set(x => x.bookTransactions[-1].RenewalCount, transaction.RenewalCount);


            var user = await _userService.GetAsync(transaction.UserId);
            user.Fine = +fine;
            await _userService.UpdateAsyncUser(user.Id, user);

            await _booksCollection.UpdateOneAsync(filter, update);
        }

        public async Task<long> GetTotalBookCount()
        {
            return await _booksCollection.CountDocumentsAsync(new BsonDocument());
        }
    }

}
