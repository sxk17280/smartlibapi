using MongoDB.Driver;
using SmartLibraryManager.Common.IntitialData;
using SmartLibraryManager.Models;

namespace SmartLibraryManager.Migration
{
    public class MigrateDB
    {

        private readonly IMongoCollection<User> _userCollection;
        private readonly IMongoCollection<Book> _bookCollection;
        private readonly IMongoCollection<BookCategory> _bookCategoryCollection;
        private readonly IMongoCollection<Author> _bookAuthorCollection;
        private readonly IMongoClient _mongoClient;
        private readonly IMongoDatabase _mongoDatabase;
        public MigrateDB(AppSettings appSettings)
        {
            _mongoClient = new MongoClient(
               appSettings.DatabaseSettings.ConnectionString);
            _mongoDatabase = _mongoClient.GetDatabase(
                appSettings.DatabaseSettings.DatabaseName);

            _userCollection = _mongoDatabase.GetCollection<User>("Users");
            _bookCollection = _mongoDatabase.GetCollection<Book>("Books");
            _bookCategoryCollection = _mongoDatabase.GetCollection<BookCategory>("BookCategory");
            _bookAuthorCollection = _mongoDatabase.GetCollection<Author>("Authors");

        }

        public void MigrateDatabase()
        {

            BookStoreData bookStoreData = new BookStoreData();

            if (_userCollection.Find(_ => true).CountDocuments() == 0)
            {
                _userCollection.InsertMany(bookStoreData.GetUsers());
            }

            if (_bookCollection.Find(_ => true).CountDocuments() == 0)
            {
                _bookCollection.InsertMany(bookStoreData.GetBooks());
            }

            if (_bookCategoryCollection.Find(_ => true).CountDocuments() == 0)
            {
                _bookCategoryCollection.InsertMany(bookStoreData.GetBookCategories());
            }

            if (_bookAuthorCollection.Find(_ => true).CountDocuments() == 0)
            {
                _bookAuthorCollection.InsertMany(bookStoreData.GetAuthors());
            }

        }


    }
}
