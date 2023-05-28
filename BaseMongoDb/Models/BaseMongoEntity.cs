namespace BaseMongoDb.Models
{
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    public abstract class BaseMongoEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null;

        public DateTime Created { get; set; }

        public DateTime? Updated { get; set; }
    }
}
