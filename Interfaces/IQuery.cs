using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.StorageDomain.Interfaces
{
    public interface IQuery
    {
        string Name { get; set; }
        Dictionary<string, dynamic> Parameters { get; set; }
    }
}
