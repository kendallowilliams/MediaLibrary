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

            FileInfo info = new FileInfo(this.fileName);
            bool isPartial = hasValidRange && !(from == 0 && (to == info.Length - 1 || !to.HasValue));

            response.StatusCode = isPartial ? 206 : 200;
            response.ContentType = mediaType;
            response.Headers.Add("Accept-Ranges", "bytes");

            if (isPartial)
            {
                long end = to.HasValue ? to.Value : info.Length - 1,
                     count = end + 1 - from.Value;

                response.Headers.Add("Content-Range", $"bytes {from}-{end}/{info.Length}");
                response.TransmitFile(this.fileName, from.Value, count);
            }
            else
            {
                response.TransmitFile(this.fileName);
            }
        }
    }
}