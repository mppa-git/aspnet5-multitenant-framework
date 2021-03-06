﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Shell.Models;
using Microsoft.AspNet.Http.Features;
using Bah.Core.Site.Multitenancy;
using Microsoft.Framework.OptionsModel;
using Microsoft.Data.Entity;

namespace Shell.Controllers
{
    public class HomeController : Controller
    {
        public TestDbContext TestDbContext { get; private set; }
        public ITenantService TenantService { get; private set; }
        private MyOptions Options { get; set; }

        public HomeController(TestDbContext db, ITenantService tenantService, IOptions<MyOptions> options)
        {
            this.TestDbContext = db;
            this.TenantService = tenantService;
            this.Options = options.Value;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            this.TestDbContext.Database.Migrate();
            var row = new Test() { Name = "test" };
            this.TestDbContext.Add(row);
            this.TestDbContext.SaveChanges();

            //var tenant = this.HttpContext.Features.Get<ITenantFeature>();
            var tenantName = this.TenantService.Tenant.Name;

            ViewData["Message"] = string.Format(
                "Your {0} application description page.  {1} tests run.",
                tenantName,
                this.TestDbContext.Tests.Count());

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View("~/Views/Shared/Error.cshtml");
        }
    }
}
