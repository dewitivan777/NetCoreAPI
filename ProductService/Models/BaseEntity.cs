using MongoDB.Bson.Serialization.Attributes;

namespace ProductService.Models
{
    public abstract class BaseEntity
    {
        [BsonId]
        public string Id { get; set; }
    }
}
