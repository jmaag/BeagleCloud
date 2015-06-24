using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BeagleCloud.Models;

namespace BeagleCloud.Controllers
{
    public class BaseController : Controller
    {
        public DBContext ZDB = new DBContext();
    }
}