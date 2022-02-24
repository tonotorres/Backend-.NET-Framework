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
    public class TablesController : ApiController
    {

        public List<string> Get(string schema)
        {
            return Repository_EPICX.GetTablesNamesRegistro(schema);
        }
    }
}
