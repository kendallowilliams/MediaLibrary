using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;

namespace MediaLibraryWebUI.ActionResults
{
    public class FileRangeResult : ActionResult
    {
        private string mediaType,
                       fileName;
        private long? from,
                      to;
        private bool hasValidRange = false;

        public FileRangeResult(string fileName, string range, string mediaType)
        {
            this.fileName = fileName;
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

            using (var stream = File.OpenRead(fileName))
            {
                if (hasValidRange)
                {
                    long end = to.HasValue ? to.Value : stream.Length - 1,
                         count = end + 1 - from.Value;
                    byte[] data = new byte[count];

                    stream.Write(data, (int)from.Value, (int)count);
                    response.Headers.Add("Content-Range", $"bytes {from}-{end}/{count}");
                    response.OutputStream.Write(data, 0, (int)count);
                }
                else
                {
                    File.OpenRead(this.fileName).CopyTo(response.OutputStream);
                }
            }
        }
    }
}