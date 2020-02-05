using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MediaLibraryWebUI.Controllers
{
#if !DEBUG
    [Authorize]
#endif
    public abstract class BaseController : Controller
    {
    }
}