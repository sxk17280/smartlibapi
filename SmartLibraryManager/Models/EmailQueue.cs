using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SmartLibraryManager.Models
{
    public class EmailQueue 
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        public string EmailProperties { set; get; } = null!;
        public bool IsSent { get; set; } = false;
    }
}
