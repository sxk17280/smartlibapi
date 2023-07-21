using Microsoft.Extensions.Options;
using SmartLibraryManager.Models;
using MongoDB.Driver;
using SmartLibraryManager.Common.Models;
using SmartLibraryManager.Common.IntitialData;
using MongoDB.Bson;

namespace SmartLibraryManager.Services
{
    public class BookCategoryService
    {

        private readonly IMongoCollection<BookCategory> _bookCategoryCollection;
        public BookCategoryService(IOptions<DatabaseSettings> bookStoreDatabaseSettings)
        {

            var mongoClient = new MongoClient(
            bookStoreDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                bookStoreDatabaseSettings.Value.DatabaseName);

            _bookCategoryCollection = mongoDatabase.GetCollection<BookCategory>("BookCategory");
        }
        public async Task<List<BookCategory>> GetAsync()
        {
            return await _bookCategoryCollection.Find(_ => true).ToListAsync();
        }
        public async Task<BookCategory?> GetAsync(int id)
        {
            return await _bookCategoryCollection.Find(x => x.CategoryId == id).FirstOrDefaultAsync();
        }
        public async Task CreateAsync(BookCategory bookCategory)
        {
            bookCategory.Id = ObjectId.GenerateNewId().ToString();
            await _bookCategoryCollection.InsertOneAsync(bookCategory);
        }
        public async Task<long> GetTotalCategoryCount()
        {
            return await _bookCategoryCollection.CountDocumentsAsync(new BsonDocument());
        }
    }

}
