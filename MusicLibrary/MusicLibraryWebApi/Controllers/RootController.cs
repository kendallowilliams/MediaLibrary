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
    [Export, PartCreationPolicy(CreationPolicy.NonShared)]
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
            Transaction transaction = null,
                        existingTransaction = null;

            try
            {
                string path = inData["path"]?.ToString();
                bool copyFiles = false,
                     recursive = false,
                     validData = !string.IsNullOrWhiteSpace(path) &&
                                 bool.TryParse(inData["copy"]?.ToString(), out copyFiles) &&
                                 bool.TryParse(inData["recursive"]?.ToString(), out recursive);
                string responseMessage = $"Invalid Data: [{inData}]",
                       transactionType = Enum.GetName(typeof(TransactionTypes), TransactionTypes.Read);

                transaction = await transactionService.GetNewTransaction(TransactionTypes.Read);
                existingTransaction = await transactionService.GetActiveTransactionByType(TransactionTypes.Read);
                
                if (validData && existingTransaction== null)
                {
                    await transactionService.UpdateTransactionInProcess(transaction);
                    HostingEnvironment.QueueBackgroundWorkItem(ct => fileService.ReadDirectory(transaction, path, recursive, copyFiles));
                }
                else
                {
                    if (existingTransaction != null)
                    {
                        responseMessage = $"{transactionType} is already running. See transaction #{existingTransaction.Id}";
                        response = Request.CreateResponse(HttpStatusCode.Conflict, responseMessage);
                    }
                    else if (!validData)
                    {
                        responseMessage = $"Invalid Data: [{inData}]";
                        response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, responseMessage);
                    }

                    await transactionService.UpdateTransactionCompleted(transaction, responseMessage);
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
