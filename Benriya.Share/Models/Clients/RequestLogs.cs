using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Benriya.Share.Models.Clients
{
    public class RequestLogs : Common_Model
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string  id { get; set; }
        [StringLength(40)]
        public string ip { get; set; }
        public string ua { get; set; }
        public string name { get; set; }
    
    }
}
