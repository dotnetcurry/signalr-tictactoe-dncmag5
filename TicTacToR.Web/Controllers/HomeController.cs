using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TicTacToR.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Login to Challenge your Friends to a 'Mind-bending' game of Tic-Tac-ToR";
            
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = @"Tic-Tac-ToR is the online incarnation of the age old Tic-Tac-Toe (a.k.a. Noughts and crosses, Xs and Os).
                                It uses HTML5 Canvas, JavaScript on the client-side and SignalR+ASP.NET at the backend.";
            return View();
        }
    }
}
