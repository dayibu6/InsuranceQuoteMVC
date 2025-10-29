using System;
using System.Linq;
using System.Web.Mvc;
using Insurance_Quote.Models;

namespace Insurance_Quote.Controllers
{
    public class InsureeController : Controller
    {
        private InsuranceEntities db = new InsuranceEntities();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Insuree insuree)
        {
            if (ModelState.IsValid)
            {
                decimal quote = 50m; // base rate

                var today = DateTime.Today;
                int age = today.Year - insuree.DateOfBirth.Year;
                if (insuree.DateOfBirth > today.AddYears(-age)) age--;

                if (age <= 18)
                {
                    quote += 100m;
                }
                else if (age >= 19 && age <= 25)
                {
                    quote += 50m;
                }
                else if (age >= 26)
                {
                    quote += 25m;
                }

                if (insuree.CarYear < 2000)
                {
                    quote += 25m;
                }
                else if (insuree.CarYear > 2015)
                {
                    quote += 25m;
                }

                if (insuree.CarMake.ToLower() == "porsche")
                {
                    quote += 25m;
                    if (insuree.CarModel.ToLower() == "911 carrera")
                    {
                        quote += 25m;
                    }
                }

                quote += insuree.SpeedingTickets * 10m;

                if (insuree.DUI)
                {
                    quote *= 1.25m;
                }

                if (insuree.CoverageType.ToLower() == "full" || insuree.CoverageType.ToLower() == "full coverage")
                {
                    quote *= 1.5m;
                }

                insuree.Quote = quote;

                db.Insurees.Add(insuree);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(insuree);
        }

        public ActionResult Admin()
        {
            var insurees = db.Insurees.ToList();
            return View(insurees);
        }
    }
}
