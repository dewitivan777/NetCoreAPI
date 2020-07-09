using MongoDB.Bson.Serialization.Attributes;

namespace ClassificationService.Models
{
    public abstract class BaseEntity
    {
        [BsonId]
        public string Id { get; set; }
    }
}
