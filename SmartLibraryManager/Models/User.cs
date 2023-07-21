﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SmartLibraryManager.Models
{
    public class User {
        
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string UserId { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool IsAdmin { get; set; }
        public int? Fine { get; set; }
    }
    public class UserVM
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool IsAdmin { get; set; }
        public int? Fine { get; set; }
    }
    public class BookBasicDetail
    {
        public string BookId { get; set; } = null!;
        public string Title { get; set; } = null!;
    }
}
