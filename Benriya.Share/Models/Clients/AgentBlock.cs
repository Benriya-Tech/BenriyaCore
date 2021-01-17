using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace Benriya.Share.Models.Clients
{
    public class AgentBlock : Common_Model
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public string ua { get; set; }
        public string name { get; set; }
    }
}
