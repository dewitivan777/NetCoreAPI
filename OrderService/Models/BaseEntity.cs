using MongoDB.Bson.Serialization.Attributes;

namespace OrderService.Models
{
    public abstract class BaseEntity
    {
        [BsonId]
        public string Id { get; set; }
    }
}
