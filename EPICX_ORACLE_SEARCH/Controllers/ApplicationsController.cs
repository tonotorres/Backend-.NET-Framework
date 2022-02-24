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
    public class ApplicationsController : ApiController
    {
        // GET: api/Applications
        public IEnumerable<string> Get()
        {
            return Repository_EPICX.GetApplications();
        }
    }
}
