using MusicLibraryBLL.Models;
using MusicLibraryBLL.Services.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;
using static MusicLibraryBLL.Enums.TransactionEnums;

namespace MusicLibraryWebApi.Controllers
{
    [Export]
    public class RootController : ApiController
    {
        private readonly IFileService fileService;
        private readonly ITransactionService transactionService;

        [ImportingConstructor]
        public RootController(IFileService fileService, ITransactionService transactionService)
        {
            this.fileService = fileService;
            this.transactionService = transactionService;
        }

        public async Task<HttpResponseMessage> Read([FromBody] JObject inData)
        {
            HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Accepted);
            Transaction transaction = null;

            try
            {
                string path = inData["path"]?.ToString();
                bool copyFiles = false,
                     recursive = false,
                     validData = !string.IsNullOrWhiteSpace(path) &&
                                 bool.TryParse(inData["copy"]?.ToString(), out copyFiles) &&
                                 bool.TryParse(inData["recursive"]?.ToString(), out recursive);
                string invalidDataMessage = $"Invalid Data: [{inData}]";

                transaction = await transactionService.GetNewTransaction(TransactionTypes.Read);

                if (validData)
                {
                    await transactionService.UpdateTransactionInProcess(transaction);
                    HostingEnvironment.QueueBackgroundWorkItem(ct => fileService.ReadDirectory(transaction, path, recursive, copyFiles));
                }
                else
                {
                    await transactionService.UpdateTransactionCompleted(transaction, invalidDataMessage);
                    response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, invalidDataMessage);
                }
            }
            catch (Exception ex)
            {
                await transactionService.UpdateTransactionErrored(transaction, ex);
                response = Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
            }

            return response;
        }
    }
}
