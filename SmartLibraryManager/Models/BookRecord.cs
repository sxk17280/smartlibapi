﻿using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace SmartLibraryManager.Models
{
    public class BookRecord
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string BookId { set; get; } = null!;
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Image { get; set; } = null!;
        public string ISBN { get; set; } = null!;
        public string Category { set; get; } = null!;
        public int PublishedYear { get; set; }
        public string Author { get; set; } = null!;
        public string Status { get; set; } = null!;
        public bool isAvailable { get; set; }
        public BookTransaction bookTransactions { set; get; } = null!;
    }
}
