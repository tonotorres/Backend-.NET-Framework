using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPICX_ORACLE_SEARCH.Models
{
    public class Table
    {
        public String table { get; set; }
        public List<Query> values { get; set; }
    }
}