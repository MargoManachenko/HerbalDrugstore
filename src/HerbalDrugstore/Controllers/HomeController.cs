using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HerbalDrugstore.Data;
using HerbalDrugstore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Cli.Utils.CommandParsing;
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

        public IActionResult HerbsList(string notFoundMessage, List<int> foundIds, string searchString)
        {
            ViewBag.NotForundMessage = notFoundMessage;
            ViewBag.FoundIds = foundIds;
            ViewBag.SearchString = searchString;

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
                var ids = new List<int>();

                foreach (var herb in _db.Herb)
                {
                    if (herb.Name.ToLower().Contains(nameToSearch.ToLower().Trim()))
                    {
                        ids.Add(herb.HerbId);
                    }
                }

                if (ids.Count == 0)
                {
                    ViewBag.Message = "No matches for ' " + nameToSearch + " '";
                    return RedirectToAction("HerbsList", "Home", new { notFoundMessage = ViewBag.Message });
                }

                ViewBag.FoundIds = ids;
                ViewBag.SearchString = nameToSearch;

                return RedirectToAction("HerbsList", "Home", new { notFoundMessage = "", foundIds = ViewBag.FoundIds, searchString = ViewBag.SearchString });

            }

            return RedirectToAction("HerbsList", "Home");
        }

        public IActionResult FilterHerbs(int value, int value2, string command)
        {
            if (command.Equals("Sort"))
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

            if (command.Equals("Filter"))
            {
                if (value2 == 1)
                {
                    var fullyFilled = _db.Herb.Where(h => h.Species != "" || h.Description != "").ToList();
                    return View(fullyFilled);
                }
                if (value2 == 2)
                {
                    var partlyFilled = _db.Herb.Where(h => h.Species == "" && h.Description == "").ToList();
                    return View(partlyFilled);
                }

                return RedirectToAction("HerbsList", "Home");
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


        public IActionResult SuppliersList()
        {
            return View(_db.Supplier.ToList());
        }

        [HttpGet]
        public IActionResult AddSupplier()
        {
            return View();
        }


        [HttpPost]
        public IActionResult AddSupplier(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                _db.Supplier.Add(supplier);
                _db.SaveChanges();
            }
            return RedirectToAction("SuppliersList", "Home");
        }

        public IActionResult EditSupplier(int id)
        {
            var supplier = _db.Supplier.Single(s => s.SupplierId == id);
            return View(supplier);
        }

        public IActionResult ApplyEditSupplier(Supplier supplier)
        {
            if (ModelState.IsValid)
            {
                _db.Supplier.Update(supplier);
                _db.SaveChanges();
            }
            return RedirectToAction("SuppliersList", "Home");
        }

        public IActionResult DeleteSupplier(int id)
        {
            var supplierToDelete = _db.Supplier.Single(h => h.SupplierId == id);
            _db.Supplier.Remove(supplierToDelete);
            _db.SaveChanges();
            return RedirectToAction("SuppliersList", "Home");
        }

        public IActionResult SuppliesList()
        {

            return View(_db.Supply.Include(p => p.Supplier).ToList());
        }

        [HttpGet]
        public IActionResult AddSupplyStep1()
        {
            return View(_db.Supplier.ToList());
        }

        //добавляем поставку в бд, передаем список препаратов
        public IActionResult AddSupplyStep2(int supplierId, bool repeat)
        {
            var supplier = _db.Supplier.Single(s => s.SupplierId == supplierId);
            var drugs = _db.Drug.ToList();
            var supply = new Supply() { SupplierId = supplierId, Supplier = supplier };

            if (!repeat)
            {
                _db.Supply.Add(supply);
                _db.SaveChanges();
            }

            var supplyToPass = _db.Supply.OrderByDescending(s => s.SupplyId).FirstOrDefault();

            var model = new SupplyAndLotViewModel() { Supplier = supplier, Drugs = drugs, Supply = supplyToPass };

            return View(model);
        }


        public IActionResult AddSupplyStep3(int drugId, int quantity, float price, string command)
        {
            var drug = _db.Drug.Single(d => d.DrugId == drugId);
            var supply = _db.Supply.OrderByDescending(s => s.SupplyId).FirstOrDefault();

            var lot = new Lot()
            {
                DrugId = drug.DrugId,
                Grug = drug,
                Quantity = quantity,
                Price = price,
                Supply = supply,
                SupplyId = supply.SupplyId
            };

            _db.Lot.Add(lot);
            _db.SaveChanges();

            if (command.Equals("Finish"))
            {
                return RedirectToAction("AddSupplyStep4", "Home", new { id = supply.SupplyId });
            }
            var supplier = _db.Supplier.OrderByDescending(s => s.SupplierId).FirstOrDefault();


            return RedirectToAction("AddSupplyStep2", "Home", new { supplierId = supplier.SupplierId, repeat = true });
        }

        [HttpGet]
        public IActionResult AddSupplyStep4(int id)
        {
            ViewBag.Id = id;

            return View();
        }

        [HttpPost]
        public IActionResult AddSupplyStep4(float price, DateTime date, int id)
        {
            var supply = _db.Supply.Single(s => s.SupplyId == id);
            supply.Price = price;
            supply.DateOfSupply = date;

            _db.Supply.Update(supply);
            _db.SaveChanges();

            return RedirectToAction("SuppliesList", "Home");
        }


        public IActionResult DeleteSupply(int id)
        {

            var lotsToDelete = _db.Lot.Where(l => l.SupplyId == id).ToList();

            foreach (var lot in lotsToDelete)
            {
                _db.Lot.Remove(lot);
            }

            var supplyToDelete = _db.Supply.Single(s => s.SupplyId == id);
            _db.Supply.Remove(supplyToDelete);

            _db.SaveChanges();

            return RedirectToAction("SuppliesList", "Home");
        }

        public IActionResult Reports()
        {
            return View();
        }

        public IActionResult Statistics()
        {
            return View();
        }

        public IActionResult ReportSuppliesForPastWeek()
        {
            var now = DateTime.Now;
            var suppliesList = _db.Supply.Where(s => s.DateOfSupply.Year == now.Year || s.DateOfSupply.Year == (now.Year - 1) && s.DateOfSupply.DayOfYear <= now.DayOfYear && s.DateOfSupply.DayOfYear >= (now.DayOfYear - 7)).Include(s => s.Supplier).ToList();

            var lotsList = (from l in _db.Lot from s in suppliesList where l.SupplyId == s.SupplyId select l).Include(d => d.Grug).ToList();

            var model = new RepoSupplyAndLots() { Supply = suppliesList, Lots = lotsList };

            return View(model);
        }

        public IActionResult ReportSuppliesForPastMonth()
        {
            var now = DateTime.Now;
            var suppliesList = _db.Supply.Where(s => s.DateOfSupply.Year == now.Year || s.DateOfSupply.Year == (now.Year - 1) && s.DateOfSupply.DayOfYear <= now.DayOfYear && s.DateOfSupply.DayOfYear >= (now.DayOfYear - 31)).Include(s => s.Supplier).ToList();

            var lotsList = (from l in _db.Lot from s in suppliesList where l.SupplyId == s.SupplyId select l).Include(d => d.Grug).ToList();

            var model = new RepoSupplyAndLots() { Supply = suppliesList, Lots = lotsList };

            return View(model);
        }

        public IActionResult SuppliersStatistic()
        {
            var supplies = _db.Supply.Include(s => s.Supplier).OrderBy(s => s.Supplier.SupplierId).ToList();

            var suppliers = _db.Supplier.ToList();

            var a =
                supplies.GroupBy(x => x.Supplier.SupplierId)
                    .OrderByDescending(y => y.Count())
                    .Take(suppliers.Count)
                    .Select(z => z.Key)
                    .ToList();

            var supplRes = new List<Supplier>();

            for (int i = 0; i < a.Count; i++)
            {
                foreach (var sup in suppliers)
                {
                    if (sup.SupplierId == a[i])
                    {
                        supplRes.Add(sup);
                    }
                }
            }

            var quant = new List<int>();

            for (int i = 0; i < supplRes.Count; i++)
            {
                quant.Add(0);;

                foreach (var supl in supplies)
                {
                    if (supl.SupplierId == supplRes[i].SupplierId)
                    {
                        quant[i] += 1;
                    }

                }
            }

            var model = new List<SuppliersChart>();

            for (int i = 0; i < quant.Count; i++)
            {
                var qR = quant[i];
                var sR = supplRes[i];
                var m = new SuppliersChart() {Supplier = sR,Quanntity = qR};
                model.Add(m);
            }

            //var popularSupplier = _db.Supplier.Single(s => s.SupplierId == a[0]);


            return View(model);

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
