using Benriya.Share.Models.Clients;
using Benriya.Utils;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Benriya.Core.Services.LoggingDB
{
    public class LoggingDBServices
    {
        private readonly IMongoCollection<RequestLogs> _logs;

        public LoggingDBServices(IMongoSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _logs = database.GetCollection<RequestLogs>("Request_Logs");
        }

        public RequestLogs Create(RequestLogs idata)
        {
            _logs.InsertOne(idata);
            return idata;
        }

        public IList<RequestLogs> Read() =>
            _logs.Find(sub => true).ToList();

        public RequestLogs Find(string id) =>
            _logs.Find(sub => sub.id == id).SingleOrDefault();

        public void Update(RequestLogs RequestLogs) =>
            _logs.ReplaceOne(sub => sub.id == RequestLogs.id, RequestLogs);

        public void Delete(string id) =>
            _logs.DeleteOne(sub => sub.id == id);
    }
}
