using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAMS.Data;
using DAMS.Models;

namespace DAMS.Controllers
{
    public class SellsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SellsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Sells
        public async Task<IActionResult> Index(int id)
        {
            var Data = await _context.User.Where(u => u.User_ID == id).FirstOrDefaultAsync();
            ViewBag.Data = Data;
            return View(await _context.Sell.ToListAsync());
        }

        // GET: Sells/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sell = await _context.Sell
                .FirstOrDefaultAsync(m => m.Selling_ID == id);
            if (sell == null)
            {
                return NotFound();
            }

            return View(sell);
        }

        // GET: Sells/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Sells/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Selling_ID,Medicine_ID,Seller_ID,Quantity,Medicine_Price")] Sell sell)
        {
            bool Availability = false;
            var Sells = _context.Sell.ToList();
            foreach (var Sell in Sells)
            {
                if (sell.Medicine_ID == Sell.Medicine_ID && sell.Seller_ID == Sell.Seller_ID)
                {
                    Availability = true;
                }
            }

            if (!Availability)
            {
                if (ModelState.IsValid)
                {
                    _context.Add(sell);
                    await _context.SaveChangesAsync();
                    TempData["SM_10"] = "Successfully added a new medicine in selling category.";
                }
            }
            else
            {
                TempData["DM_02"] = "A medicine with same details already exists in selling category.";
            }
            return RedirectToAction(nameof(Layout), new { User_ID = sell.Seller_ID });
        }

        // GET: Sells/Edit/5
        public async Task<IActionResult> Edit(int? id, int User_ID)
        {
            var Data = await _context.User.Where(u => u.User_ID == User_ID).FirstOrDefaultAsync();
            ViewBag.Data = Data;
            if (id == null)
            {
                return NotFound();
            }

            var sell = await _context.Sell.FindAsync(id);
            if (sell == null)
            {
                return NotFound();
            }
            return View(sell);
        }

        // POST: Sells/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Selling_ID,Medicine_ID,Seller_ID,Quantity,Medicine_Price")] Sell sell)
        {
            if (id != sell.Selling_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sell);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SellExists(sell.Selling_ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["SM_11"] = "Successfully changed medcicine details in selling category.";
                return RedirectToAction(nameof(Layout), new { User_ID = sell.Seller_ID });
            }
            return View(sell);
        }

        // GET: Sells/Delete/5
        public async Task<IActionResult> Delete(int? id, int User_ID)
        {
            var Data = await _context.User.Where(u => u.User_ID == User_ID).FirstOrDefaultAsync();
            ViewBag.Data = Data;
            var Sell = await _context.Sell.Where(u => u.Selling_ID == id).FirstOrDefaultAsync();
            var Medicine = await _context.Medicine.Where(u => u.Medicine_ID == Sell.Medicine_ID).FirstOrDefaultAsync();
            ViewBag.Medicine = Medicine;
            if (id == null)
            {
                return NotFound();
            }

            var sell = await _context.Sell
                .FirstOrDefaultAsync(m => m.Selling_ID == id);
            if (sell == null)
            {
                return NotFound();
            }

            return View(sell);
        }

        // POST: Sells/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sell = await _context.Sell.FindAsync(id);
            if (sell != null)
            {
                _context.Sell.Remove(sell);
            }

            await _context.SaveChangesAsync();
            TempData["SM_12"] = "Successfully removed a medcicine from selling category.";
            return RedirectToAction(nameof(Layout), new { User_ID = sell.Seller_ID });
        }

        private bool SellExists(int id)
        {
            return _context.Sell.Any(e => e.Selling_ID == id);
        }

        // Custom built-in functions
        public async Task<IActionResult> Search(int id)
        {
            var Data = await _context.User.Where(u => u.User_ID == id).FirstOrDefaultAsync();
            ViewBag.Data = Data;
            return View();
        }
        public async Task<IActionResult> Search_01(int User_ID, string Medicine_Name)
        {
            var Data = await _context.User.Where(u => u.User_ID == User_ID).FirstOrDefaultAsync();
            ViewBag.Data = Data;
            if (Medicine_Name != null)
            {
                var Medicines = await _context.Medicine.Where(u => u.Medicine_Name.Contains(Medicine_Name)).ToListAsync();
                ViewBag.Medicines = Medicines;
                if (Medicines == null || !Medicines.Any())
                {
                    TempData["DM_03"] = "No medicines Found";
                    ViewBag.Medicines = null;
                    return View("Search");
                }
            }
            return View("Medicines");
        }
        [HttpPost]
        public async Task<IActionResult> Get_Sell_ID (int User_ID, int Medicine_ID)
        {
            var Data = await _context.User.Where(u => u.User_ID == User_ID).FirstOrDefaultAsync();
            ViewBag.Data = Data;
            ViewBag.Seller_ID = User_ID;
            return View("Create");
        }
        public async Task<IActionResult> Layout(int User_ID)
        {
            var Data = await _context.User.Where(u => u.User_ID == User_ID).FirstOrDefaultAsync();
            ViewBag.Data = Data;
            return View("~/Views/Users/Layout.cshtml");
        }
    }
}
