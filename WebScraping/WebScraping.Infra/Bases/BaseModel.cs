using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Reflection.Metadata;

namespace WebScraping.Infra.Bases
{
    public abstract partial class BaseModel 
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        [BsonElement("code")]
        public long Code { get; set; }
    }
}
