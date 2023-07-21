using Microsoft.Extensions.Options;
using SmartLibraryManager.Models;
using MongoDB.Driver;
using SmartLibraryManager.Common.Models;
using SmartLibraryManager.Common.IntitialData;

namespace SmartLibraryManager.Services
{
    public class TransactionService {

        private readonly IMongoCollection<BookTransaction> _bookTransactionCollection;
        public TransactionService(IOptions<DatabaseSettings> bookStoreDatabaseSettings)
        {
            var mongoClient = new MongoClient(
            bookStoreDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                bookStoreDatabaseSettings.Value.DatabaseName);

            _bookTransactionCollection = mongoDatabase.GetCollection<BookTransaction>("BookTransaction");

        }
        public async Task<List<BookTransaction>> GetAsync()
        {
            return await _bookTransactionCollection.Find(_ => true).ToListAsync();
        }
        public async Task<BookTransaction?> GetAsync(string id)
        {
            return await _bookTransactionCollection.Find(x => x.TransactionId == id).FirstOrDefaultAsync();
        }
    }

}
