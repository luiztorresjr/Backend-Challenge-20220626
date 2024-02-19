using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScraping.Infra.Models
{
    public class MongoDBSetttings : IMongoDBSettings
    {
        public string ConnectionURI { get; set; }
        public string DatabaseName { get; set; }
        public string CollectionName { get; set; }

        

    }

    public interface IMongoDBSettings
    {
        string ConnectionURI { get; set; }
        string DatabaseName { get; set; }
        string CollectionName { get; set; }
    }
}
