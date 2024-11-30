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
        public async Task<IActionResult> Index()
        {
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
            if (ModelState.IsValid)
            {
                _context.Add(sell);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sell);
        }

        // GET: Sells/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
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
                return RedirectToAction(nameof(Index));
            }
            return View(sell);
        }

        // GET: Sells/Delete/5
        public async Task<IActionResult> Delete(int? id)
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
            return RedirectToAction(nameof(Index));
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
            var data = _context.Medicine.ToList();
            ViewBag.Medicine_Name = data.Select(f => f.Medicine_Name).Distinct().ToList();
            ViewBag.Medicine_Weightage = data.Select(f => f.Medicine_Weightage).Distinct().ToList();
            return View();
        }
        public async Task<IActionResult> Search_01(int User_ID, string Medicine_Name, int Medicine_Weightage)
        {
            var Data = await _context.User.Where(u => u.User_ID == User_ID).FirstOrDefaultAsync();
            ViewBag.Data = Data;
            var Medicines = await _context.Medicine.Where(u => u.Medicine_Name.Contains(Medicine_Name)).ToListAsync();
            if (Medicines == null)
            {
                TempData["DM_03"] = "No medicines Found";
            }
            return RedirectToAction("Search", new { id = User_ID });
        }
    }
}
