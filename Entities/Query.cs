using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.StorageDomain.Interfaces;

namespace Company.StorageDomain.Entities
{
    public class Query : IQuery
    {
        public string Name { get; set; }
        public Dictionary<string, dynamic> Parameters { get; set; }
    }
}
