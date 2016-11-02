using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebAPI.Core.Controller
{
    [RoutePrefix("api/accesstest")]
    public class AccessTestController : ApiController
    {
        [Authorize]
        [Route("autorized")]
        [HttpGet]
        public HttpResponseMessage TestAccess()
        {
            HttpResponseMessage response = Request.CreateResponse();
            try
            {
                response = Request.CreateResponse(HttpStatusCode.OK, "Access OK!", new MediaTypeHeaderValue("text/json"));
            }
            catch (Exception exception)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, exception.Message, new MediaTypeHeaderValue("text/json"));
            }
            return response;
        }
    }
}
