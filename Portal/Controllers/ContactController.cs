using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Portal.Common;
using Portal.Models;
using WebGrease;

namespace Portal.Controllers
{
    public class ContactController : Controller
    {

        // GET: Contact
        public ActionResult Index()
        {
            return View();
        }

    }
}