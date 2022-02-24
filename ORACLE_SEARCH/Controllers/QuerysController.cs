using EPICX_ORACLE_SEARCH.Repositories;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Mvc;

namespace EPICX_ORACLE_SEARCH.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class QuerysController : ApiController
    {
        // GET: Querys
        public JObject Get(string schema, string table, [FromUri] string[] field)
        {
            return Repository_EPICX.GetQuerys(schema, table, field);
        }

    }
}
