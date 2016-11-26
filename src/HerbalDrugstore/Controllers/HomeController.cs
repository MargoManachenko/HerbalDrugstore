using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HerbalDrugstore.Data;
using HerbalDrugstore.Models;
using Microsoft.AspNetCore.Mvc;

namespace HerbalDrugstore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public HomeController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult HerbsList()
        {
            return View(_db.Herb.ToList());
        }

        public IActionResult AddHerb(Herb herb)
        {
            if (ModelState.IsValid)
            {
                _db.Herb.Add(herb);
                _db.SaveChanges();
            }
            return RedirectToAction("HerbsList", "Home");
        }

        public IActionResult EditHerb(int id)
        {
            var herbToEdit = _db.Herb.Single(h => h.HerbId == id);
            return View(herbToEdit);
        }

        public IActionResult ApplyEditingHerb(Herb herb)
        {
            if (ModelState.IsValid)
            {
                _db.Herb.Update(herb);
                _db.SaveChanges();
            }
            return RedirectToAction("HerbsList", "Home");
        }

        public IActionResult DeleteHerb(int id)
        {
            var herbToDelete = _db.Herb.Single(h => h.HerbId == id);
            _db.Herb.Remove(herbToDelete);
            _db.SaveChanges();
            return RedirectToAction("HerbsList", "Home");
        }

        public IActionResult DrugsList()
        {
            return View(_db.Drug.ToList());
        }

        [HttpGet]
        public IActionResult AddDrug()
        {
            return View();
        }


        //think
        [HttpPost]
        public IActionResult AddDrug(DrugAndHerbViewModel drugAndHerbs, IEnumerable<string> TBoxes)
        {
            var herbsList = new List<Herb>();

            foreach (var str in TBoxes)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    var herb = new Herb { Name = str.Trim(), Description = "", Species = "" };
                    herbsList.Add(herb);
                    _db.Herb.Add(herb);
                }
            }

            var drugToAdd = drugAndHerbs.Drug;

            _db.Drug.Add(drugToAdd);

            _db.SaveChanges();

            var drug = _db.Drug.OrderBy(d => d.DrugId).LastOrDefault();

            if (herbsList.Count != 0)
            {
                foreach (var herb in herbsList)
                {
                    var compound = new Compound() { Drug = drug, Herb = herb, DrugId = drug.DrugId, HerbId = herb.HerbId };
                    _db.Compound.Add(compound);
                }
            }
            _db.SaveChanges();

            return RedirectToAction("DrugsList", "Home");

        }

        public IActionResult DetailsDrug(int id)
        {
            
            var drug = _db.Drug.Single(d => d.DrugId == id);

            var compound = _db.Compound.Where(c => c.DrugId == id).ToList();

            //var comp = new List<Compound>();

            //foreach (var v in _db.Compound.ToList())
            //{
            //    if (v.DrugId == id)
            //    {
            //        comp.Add(v);
            //    }
            //}

            var model = new DrugAndHerbViewModel { Compound = compound, Drug = drug };

            return View(model);
        }

        public IActionResult EditDrug(int id)
        {
            var drug = _db.Drug.Single(d => d.DrugId == id);
            //var compound = _db.Compound.Where(c => c.DrugId == id).ToList();

            var comp = new List<Compound>();

            foreach (var compound in _db.Compound.ToList())
            {
                if (compound.DrugId == id)
                {
                    comp.Add(compound);
                }
            }

            var model = new DrugAndHerbViewModel { Compound = comp, Drug = drug };

            return View(model);
        }


        public IActionResult ApplyEditDrug(Drug drug, IEnumerable<string> TBoxes)
        {
            if (ModelState.IsValid)
            {
                _db.Drug.Update(drug);
            }

            var comp = new List<Compound>();

            foreach (var herb in TBoxes)
            {
                if (!string.IsNullOrEmpty(herb))
                {
                    
                }
            }

                _db.SaveChanges();

            return RedirectToAction("DrugsList", "Home");
        }

        public IActionResult DeleteDrug(int id)
        {
            var drugToDelete = _db.Drug.Single(h => h.DrugId == id);
            _db.Drug.Remove(drugToDelete);
            _db.SaveChanges();
            return RedirectToAction("DrugsList", "Home");
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
