﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BlackJack.BLL.Services;
using BlackJack.ViewModel;
using System.Threading.Tasks;

namespace BlackJack.MVC.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

    }
}