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
    public class SchemasController : ApiController
    {
        public List<string> Get()
        {
            List<string> schemas = Repository_EPICX.GetSchemas();
            return schemas;
        }
    }
}
