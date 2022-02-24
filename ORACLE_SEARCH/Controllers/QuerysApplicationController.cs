using EPICX_ORACLE_SEARCH.Repositories;
using Newtonsoft.Json.Linq;
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
    public class QuerysApplicationController : ApiController
    {
        public JObject Get(string application)
        {
            return Repository_EPICX.GetQuerysApplication(application);
        }
    }
}
