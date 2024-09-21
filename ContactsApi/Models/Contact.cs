using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ContactsApi.Models    
{
      [BsonIgnoreExtraElements]
    public class Contact
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
          public string? Id { get; set; } // Make Id optional

        [BsonElement("name")]
        public required string Name { get; set; }

        [BsonElement("number")]
        public required string Number { get; set; }
    }
}