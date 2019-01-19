using MediaLibraryBLL.Models;
using MediaLibraryBLL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;

namespace MediaLibraryWebApi.Controllers
{
    public abstract class ApiControllerBase : ApiController
    {
        protected ITransactionService transactionService;

        public ApiControllerBase() { }
    }
}