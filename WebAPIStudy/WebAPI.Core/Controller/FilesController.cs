using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebAPI.Core.Controller
{
    [RoutePrefix("api/file")]
    public class FilesController : ApiController
    {
        private const string DOWNLOAD_PATH = @"C:\Temp\";

        [Route("download")]
        [HttpGet]
        public HttpResponseMessage DownloadFile(string fileName)
        {
            HttpResponseMessage response = Request.CreateResponse();
            try
            {
                string filePath = Path.Combine(DOWNLOAD_PATH, fileName);
                FileInfo fileInfo = new FileInfo(filePath);

                if (!fileInfo.Exists)
                {
                    response = Request.CreateResponse(HttpStatusCode.NotFound, "File Not Found!", new MediaTypeHeaderValue("text/json"));
                }
                else
                {
                    response.Headers.AcceptRanges.Add("bytes");
                    response.StatusCode = HttpStatusCode.OK;
                    int bufferSize = 1048575; // 1MB
                    response.Content = new StreamContent(new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize));
                    response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                    response.Content.Headers.ContentDisposition.FileName = fileName;
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    response.Content.Headers.ContentLength = fileInfo.Length;
                }
            }
            catch (Exception exception)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, exception.Message, new MediaTypeHeaderValue("text/json"));
            }
            return response;
        }

        [Route("downloadasync")]
        [HttpGet]
        public async Task<HttpResponseMessage> DownloadFileAsync(string fileName)
        {
            return await new TaskFactory().StartNew(
                () =>
                {
                    return DownloadFile(fileName);
                });
        }
        
    }
}
