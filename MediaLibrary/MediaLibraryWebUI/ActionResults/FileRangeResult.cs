using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace MediaLibraryWebUI.ActionResults
{
    public class RangeFileContentResult : ActionResult
    {
        private byte[] fileData;
        private string mediaType;
        private long? from,
                      to;
        private bool hasValidRange = false;

        public RangeFileContentResult(byte[] fileData, string range, string mediaType)
        {
            this.fileData = fileData;
            this.mediaType = mediaType;
            hasValidRange = RangeHeaderValue.TryParse(range, out RangeHeaderValue header);

            if (hasValidRange)
            {
                RangeItemHeaderValue headerValue = header.Ranges.ElementAt(0);
                
                from = headerValue.From;
                to = headerValue.To;
            }
        }

        public override void ExecuteResult(ControllerContext context)
        {
            HttpResponseBase response = context.HttpContext.Response;
            response.ContentType = mediaType;
            response.StatusCode = hasValidRange ? 206 : 200;

            if (hasValidRange)
            {
                long end = to.HasValue ? to.Value : fileData.Length - 1,
                     count = end + 1 - from.Value;

                response.Headers.Add("Content-Range", $"bytes {from}-{end}/{count}");
                response.OutputStream.Write(fileData, (int)from, (int)count);
            }
            else
            {
                response.OutputStream.Write(fileData, 0, fileData.Length);
            }
        }
    }
}