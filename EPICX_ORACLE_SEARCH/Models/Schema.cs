using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPICX_ORACLE_SEARCH.Models
{
    public class Schema
    {
        public String schema { get; set; }
        public List<Table> tables { get; set; }
    }
}