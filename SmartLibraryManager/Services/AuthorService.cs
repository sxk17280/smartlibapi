﻿using Microsoft.Extensions.Options;
using SmartLibraryManager.Models;
using MongoDB.Driver;
using SmartLibraryManager.Common.Models;
using SmartLibraryManager.Common.IntitialData;
using MongoDB.Bson;

namespace SmartLibraryManager.Services
{
    public class AuthorService {

        private readonly IMongoCollection<Author> _authorCollection;
        public AuthorService(IOptions<DatabaseSettings> bookStoreDatabaseSettings)
        {
            var mongoClient = new MongoClient(
            bookStoreDatabaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                bookStoreDatabaseSettings.Value.DatabaseName);

            _authorCollection = mongoDatabase.GetCollection<Author>("Authors");

        }
        public async Task<List<Author>> GetAsync()
        {
            return await _authorCollection.Find(_ => true).ToListAsync();
        }
        public async Task<Author?> GetAsync(int id)
        {
            return await _authorCollection.Find(x => x.AuthorId == id).FirstOrDefaultAsync();
        }
        public async Task CreateAsync(Author author)
        {
            author.Id = ObjectId.GenerateNewId().ToString();
            await _authorCollection.InsertOneAsync(author);
        }
        public async Task<long> GetTotalAuthorCount()
        {
            return await _authorCollection.CountDocumentsAsync(new BsonDocument());
        }
    }

}
