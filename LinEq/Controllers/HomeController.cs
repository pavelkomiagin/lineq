using LinEq.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LinEq.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            ViewBag.Title = "Linear Equation Solver";
            return View();
        }

        [HttpPost]
        public JsonResult Solve()
        {
            string equations = Request.Form["equations"];

            LinearEquationSolver solver = new LinearEquationSolver(equations);
            double[] solve = solver.Solve();
            return Json(solve);
        }

    }
}
