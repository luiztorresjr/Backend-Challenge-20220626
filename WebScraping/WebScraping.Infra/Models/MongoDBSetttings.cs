﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebScraping.Infra.Models
{
    public class MongoDBSetttings
    {
        public string? ConnectionURI { get; set; } = null;
        public string? DatabaseName { get; set; } = null;
        public string? CollectionName { get; set; } = null;

    }
}
