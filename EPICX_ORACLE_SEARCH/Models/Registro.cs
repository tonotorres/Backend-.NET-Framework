using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EPICX_ORACLE_SEARCH.Models
{
    public class Registro
    {
        public String Schema { get; set; }
        public String Tabla { get; set; }
        public String Campo { get; set; }
        public String Aplicacion { get; set; }
        public String Query { get; set; }
    }
}