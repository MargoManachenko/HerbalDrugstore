using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HerbalDrugstore.Data;
using HerbalDrugstore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
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
        public IActionResult HerbsList(string notFoundMessage, List<int> foundIds, string commandSort, string command)
        {

            if (commandSort == "sortedByNameAsc")
            {
                var sortedByNameAsc = _db.Herb.OrderBy(h => h.Name).ToList();
                ViewBag.Message = "Растения упорядочены в алфавитном порядке по возрастанию по имени";
                return View(sortedByNameAsc);
            }
            if (commandSort == "sortedBySpeciesAsc")
            {

                var sortedBySpeciesAsc = _db.Herb.OrderBy(h => h.Species == "").ThenBy(h => h.Species).ToList();
                ViewBag.Message = "Растения упорядочены в алфавитном порядке по возрастанию по виду";
                return View(sortedBySpeciesAsc);
            }
            if (commandSort == "sortedByNameDesc")
            {
                var sortedByNameDesc = _db.Herb.OrderByDescending(h => h.Name).ToList();
                ViewBag.Message = "Растения упорядочены в алфавитном порядке по убыванию по виду";
                return View(sortedByNameDesc);
            }
            if (commandSort == "sortedBySpeciesDesc")
            {
                var sortedBySpeciesDesc = _db.Herb.OrderBy(h => h.Species == "").OrderByDescending(h => h.Species).ToList();
                ViewBag.Message = "Растения упорядочены в алфавитном порядке по убыванию по имени";
                return View(sortedBySpeciesDesc);
            }

            if (commandSort == "filteredFullyFilled")
            {
                var fullyFilled = _db.Herb.Where(h => h.Species != "" || h.Description != "").ToList();
                ViewBag.Message = "Растения с полностью заполненной информацией";
                return View(fullyFilled);
            }
            if (commandSort == "filteredPartlyFilled")
            {
                var partlyFilled = _db.Herb.Where(h => h.Species == "" && h.Description == "").ToList();
                ViewBag.Message = "Растения с частично заполненной информацией";
                return View(partlyFilled);
            }

            ViewBag.NotForundMessage = notFoundMessage;

            if (foundIds.Count != 0)
            {
                ViewBag.FoundIds = foundIds;
            }
            return View(_db.Herb.ToList());
        }

        public IActionResult FilterHerbs(int value, int value2, string command)
        {
            if (command.Equals("Sort"))
            {
                if (value == 1)
                {
                    return RedirectToAction("HerbsList", "Home", new { commandSort = "sortedByNameAsc" });
                }
                if (value == 2)
                {
                    return RedirectToAction("HerbsList", "Home", new { commandSort = "sortedBySpeciesAsc" });
                }
                if (value == 3)
                {
                    return RedirectToAction("HerbsList", "Home", new { commandSort = "sortedByNameDesc" });
                }
                if (value == 4)
                {
                    return RedirectToAction("HerbsList", "Home", new { commandSort = "sortedBySpeciesDesc" });
                }
                return RedirectToAction("HerbsList", "Home");
            }

            if (command.Equals("Filter"))
            {
                if (value2 == 1)
                {
                    return RedirectToAction("HerbsList", "Home", new { commandSort = "filteredFullyFilled" });
                }
                if (value2 == 2)
                {
                    return RedirectToAction("HerbsList", "Home", new { commandSort = "filteredPartlyFilled" });
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

            var drugchanges = new DrugChanges()
            {
                Date = DateTime.Now,
                DrugId = drugToC.DrugId,
                Drug = drugToC,
                Increasing = false,
                Quantity = drugToC.Quantity,
                SupplierName = ""
            };

            _db.DrugChanges.Add(drugchanges);

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

            var compList = _db.Compound.Include(c => c.Drug).Include(c => c.Herb).ToList();

            foreach (var compound in compList)
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

        public IActionResult ConsumeDrug(int id, int quantity)
        {
            var drugToConsume = _db.Drug.Single(d => d.DrugId == id);

            if (drugToConsume.Quantity >= quantity)
            {
                drugToConsume.Quantity -= quantity;

                var drugchanges = new DrugChanges()
                {
                    Date = DateTime.Now,
                    DrugId = drugToConsume.DrugId,
                    Drug = drugToConsume,
                    Increasing = false,
                    Quantity = quantity,
                    SupplierName = ""
                };

                _db.DrugChanges.Add(drugchanges);
                _db.Drug.Update(drugToConsume);
                _db.SaveChanges();

                return RedirectToAction("DrugsList", "Home");
            }

            if (drugToConsume.Quantity < quantity)
            {
                return RedirectToAction("ErrorDrugConsuming", "Home", new { consuming = quantity, drugQuantity = drugToConsume.Quantity, drugName = drugToConsume.Name });
            }

            return RedirectToAction("DrugsList", "Home");
        }

        public IActionResult ErrorDrugConsuming(int drugQuantity, int consuming, string drugName)
        {
            ViewBag.drugQuantity = drugQuantity;
            ViewBag.consuming = consuming;
            ViewBag.drugName = drugName;

            return View();
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

            drug.Quantity += quantity;

            _db.Drug.Update(drug);

            _db.Lot.Add(lot);

            var supplier = _db.Supplier.OrderByDescending(s => s.SupplierId).FirstOrDefault();

            var drugchanges = new DrugChanges()
            {
                DrugId = drug.DrugId,
                Drug = drug,
                Increasing = true,
                Quantity = quantity,
                SupplierName = supplier.ContactName
            };

            _db.DrugChanges.Add(drugchanges);

            _db.SaveChanges();

            if (command.Equals("Finish"))
            {
                return RedirectToAction("AddSupplyStep4", "Home", new { id = supply.SupplyId });
            }

            return RedirectToAction("AddSupplyStep2", "Home", new { supplierId = supplier.SupplierId, repeat = true });
        }

        [HttpGet]
        public IActionResult AddSupplyStep4(int id, int drugId, int quantity, float price)
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

            var change = _db.DrugChanges.OrderByDescending(s => s.ChangeId).FirstOrDefault();
            change.Date = date;
            _db.DrugChanges.Update(change);
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

            var supplRes = (from t in a from sup in suppliers where sup.SupplierId == t select sup).ToList();

            //var supplRes = new List<Supplier>();

            //for (int i = 0; i < a.Count; i++)
            //{
            //    foreach (var sup in suppliers)
            //    {
            //        if (sup.SupplierId == a[i])
            //        {
            //            supplRes.Add(sup);
            //        }
            //    }
            //}

            var quant = new List<int>();

            for (int i = 0; i < supplRes.Count; i++)
            {
                quant.Add(0); ;

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
                var m = new SuppliersChart() { Supplier = sR, Quanntity = qR };
                model.Add(m);
            }

            //var popularSupplier = _db.Supplier.Single(s => s.SupplierId == a[0]);


            return View(model);

        }

        public IActionResult DrugChangesGrid()
        {
            return View(_db.DrugChanges.Include(c => c.Drug).OrderBy(c => c.Date).ToList());
        }

        public IActionResult DeleteDrugChange(int id)
        {
            var change = _db.DrugChanges.Single(c => c.ChangeId == id);
            _db.DrugChanges.Remove(change);
            _db.SaveChanges();
            return RedirectToAction("DrugChangesGrid", "Home");
        }

        public IActionResult Calculation()
        {
            var model = new List<PromptViewModel>();

            var changes = _db.DrugChanges.Where(c => c.Increasing).Include(c => c.Drug).ToList();

            var drugs = _db.DrugChanges.Select(c => c.Drug).Distinct().ToList();

            for (int i = 0; i < drugs.Count; i++)
            {
                var supplierName = "";
                var dates = new List<DateTime>();
                var quantities = new List<int>();

                foreach (var drugChangese in changes)
                {
                    if (drugs[i].Name == drugChangese.Drug.Name && drugChangese.Increasing && drugChangese.SupplierName != "")
                    {
                        dates.Add(drugChangese.Date);
                        quantities.Add(drugChangese.Quantity);
                        supplierName = drugChangese.SupplierName;
                    }
                }
                if (dates.Count != 0 && quantities.Count != 0)
                {
                    int totalDays;
                    var thisYear = new List<int>();
                    var prevYear = new List<int>();

                    foreach (var date in dates)
                    {
                        if (date.Year == DateTime.Now.Year)
                            thisYear.Add(date.DayOfYear);
                        if (date.Year == DateTime.Now.Year - 1)
                            prevYear.Add(date.DayOfYear);

                    }
                    
                    if (prevYear.Count != 0)
                    {
                        
                        var daysSumThisYear = thisYear.Max() - thisYear.Min();
                        var daysSumPrevYear = prevYear.Max() - prevYear.Min();
                        totalDays = daysSumPrevYear + daysSumThisYear;
                        if (totalDays == 0) continue;

                    }
                    else
                    {
                        var maxDay = thisYear.Max();
                        var minDay = thisYear.Min();
                        totalDays = maxDay - minDay;
                        if(totalDays == 0) continue;
                    }

                    
                    var totalQuantities = quantities.Sum();

                    //if (totalQuantities == 0 || totalDays ==0)
                    //{
                    //    return View(model);
                    //}
                    var perDay = totalQuantities / totalDays;

                    var daysLeft = DateTime.Now.DayOfYear - thisYear.Max() - 7;
                    var daysLeftForinfo = DateTime.Now.DayOfYear - thisYear.Max();
                    var available = drugs[i].Quantity;

                    var needed = perDay*daysLeft;
                    
                    var enought = needed <= available;

                    if (!enought)
                    {
                        var days = " for less than 1 day";

                        if (drugs[i].Quantity / (perDay * daysLeft) > 1)
                        {
                            days = " for " + drugs[i].Quantity / perDay + " days";
                        }

                        var infoStr = "The drug decreases by " + perDay +
                            " items a day in average. The last order was shipped " + daysLeftForinfo + " days ago."; 

                        model.Add(new PromptViewModel()
                        {
                            Drug = drugs[i],
                            ConsumePerDay = perDay,
                            Days = days,
                            SupplierName = supplierName,
                            Info = infoStr
                        });
                    }
                }
            }
            return View(model);
        }

        //public IActionResult Calculate()
        //{
        //    var currentDate = DateTime.Now;

        //    var suppliesForMonth = _db.Supply.Where(s => s.DateOfSupply.Year == currentDate.Year 
        //    && s.DateOfSupply.DayOfYear >= currentDate.DayOfYear - 30
        //    || s.DateOfSupply.Year == currentDate.Year - 1).ToList();
        //}

        public ActionResult SortingAscPartialView()
        {

            return PartialView(_db.Herb.ToList());
        }

        public IActionResult About()
        {

            ViewData["Message"] = "Your application description page.";
            ViewBag.Message = "Это частичное представление.";
            return View(_db.Herb.ToList());
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
