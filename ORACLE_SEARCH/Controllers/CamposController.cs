using EPICX_ORACLE_SEARCH.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace EPICX_ORACLE_SEARCH.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CamposController : ApiController
    {
        public List<string> Get(string schema, string table)
        {
            return Repository_EPICX.GetCamposNamesRegistro(schema, table);
        }
        //public string[] Get(string schema, [FromUri] string[] tabla)
        //{
        //    return tabla;
        //}


    }
}
