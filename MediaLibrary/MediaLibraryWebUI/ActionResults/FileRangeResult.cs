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
            RangeHeaderValue header = null;

            this.fileData = fileData;
            this.mediaType = mediaType;
            try { header = RangeHeaderValue.Parse(range); } catch (Exception) { };
            hasValidRange = header != null;

            if (hasValidRange)
            {
                RangeItemHeaderValue headerValue = header.Ranges.ElementAt(0);

                hasValidRange = true;
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
                long count = (to.HasValue ? to.Value + 1 : fileData.Length) - from.Value;

                response.OutputStream.Write(fileData, (int)from, (int)count);
            }
            else
            {
                response.OutputStream.Write(fileData, 0, fileData.Length);
            }
        }
    }
}