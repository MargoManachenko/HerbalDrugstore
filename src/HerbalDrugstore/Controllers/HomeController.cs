using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HerbalDrugstore.Data;
using HerbalDrugstore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpGet]
        public IActionResult AddHerb()
        {
            return View();
        }

        [HttpPost]
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

        public IActionResult AddDrug()
        {
            return View();
        }

        public IActionResult SearchHerb(string nameToSearch)
        {

            if (!string.IsNullOrEmpty(nameToSearch))
            {
                foreach (var herb in _db.Herb)
                {
                    if (herb.Name.ToLower() == nameToSearch.ToLower().Trim())
                    {
                        return View(herb);
                    }
                }
            }
            return RedirectToAction("HerbsList", "Home");
        }

        public IActionResult FilterHerbs(int value)
        {
            if (value == 1)
            {
                var sortedByName = _db.Herb.OrderBy(h => h.Name).ToList();
                return View(sortedByName);
            }
            if (value == 2)
            {
                var sortedBySpecies = _db.Herb.OrderBy(h => h.Species == "").ThenBy(h => h.Species).ToList();
                return View(sortedBySpecies);
            }

            return RedirectToAction("HerbsList", "Home");
        }

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

                    if (_db.Herb.FirstOrDefault(h => string.Equals(h.Name, herb.Name, StringComparison.CurrentCultureIgnoreCase)) == null)
                    {
                        _db.Herb.Add(herb);
                    }
                }
            }

            var drugToAdd = drugAndHerbs.Drug;
            _db.Drug.Add(drugToAdd);

            _db.SaveChanges();

            var drugToC = _db.Drug.OrderBy(d => d.DrugId).LastOrDefault();

            var herbsToC = new List<Herb>();

            foreach (var h in herbsList)
            {
                herbsToC.Add(_db.Herb.Single(p => p.Name == h.Name));
            }

            if (herbsToC.Count != 0)
            {
                foreach (var herb in herbsToC)
                {
                    var compound = new Compound() { Drug = drugToC, Herb = herb, DrugId = drugToC.DrugId, HerbId = herb.HerbId };
                    _db.Compound.Add(compound);
                }
            }
            _db.SaveChanges();

            return RedirectToAction("DrugsList", "Home");

        }

        public IActionResult DetailsDrug(int id)
        {

            var drug = _db.Drug.Single(d => d.DrugId == id);

            var compound = _db.Compound.Include(c => c.Herb).Include(c => c.Drug).Where(c => c.DrugId == id).ToList();

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
