using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace OneShopCharts.Controllers
{
    public class HelloWorldController : Controller
    {
        public string Index()
        {
            return "this is my default action...";
        }

        public string Welcome()
        {
            return "this is the Welcome action method...";
        }
    }
}