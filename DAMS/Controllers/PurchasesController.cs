using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAMS.Data;
using DAMS.Models;
using AspNetCoreGeneratedDocument;

namespace DAMS.Controllers
{
    public class PurchasesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PurchasesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Purchases
        public async Task<IActionResult> Index(int id, string Choice)
        {
            var Data = await _context.User.Where(u => u.User_ID == id).FirstOrDefaultAsync();
            ViewBag.Data = Data;
            var Sell = await _context.Sell.ToListAsync();
            ViewBag.Sell = Sell;
            ViewBag.Choice = Choice;
            return View(await _context.Purchase.ToListAsync());
        }

        // GET: Purchases/Details/5
        public async Task<IActionResult> Details(int? id, int User_ID)
        {
            var Data = await _context.User.Where(u => u.User_ID == User_ID).FirstOrDefaultAsync();
            ViewBag.Data = Data;
            if (id == null)
            {
                return NotFound();
            }

            var purchase = await _context.Purchase
                .FirstOrDefaultAsync(m => m.Purchase_ID == id);
            if (purchase == null)
            {
                return NotFound();
            }

            return View(purchase);
        }

        // GET: Purchases/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Purchases/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Purchase_ID,Selling_ID,Patient_ID,Requested_Quantity,Request_Status,Delivery_Date,Delivery_Time")] Purchase purchase)
        {
            if (ModelState.IsValid)
            {
                _context.Add(purchase);
                await _context.SaveChangesAsync();
                TempData["SM_13"] = "Successfully ordered user a medicine.";
                return RedirectToAction(nameof(Layout), new { User_ID = purchase.Patient_ID });
            }
            return View(purchase);
        }

        // GET: Purchases/Edit/5
        public async Task<IActionResult> Edit(int? id, int User_ID, string Status)
        {
            var Data = await _context.User.Where(u => u.User_ID == User_ID).FirstOrDefaultAsync();
            ViewBag.Data = Data;
            ViewBag.Status = Status;
            if (id == null)
            {
                return NotFound();
            }

            var purchase = await _context.Purchase.FindAsync(id);
            if (purchase == null)
            {
                return NotFound();
            }
            return View(purchase);
        }

        // POST: Purchases/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Purchase_ID,Selling_ID,Patient_ID,Requested_Quantity,Request_Status,Delivery_Date,Delivery_Time")] Purchase purchase, int User_ID)
        {
            if (id != purchase.Purchase_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(purchase);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PurchaseExists(purchase.Purchase_ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["SM_14"] = "Successfully accepted a order.";
                return RedirectToAction(nameof(Layout), new { User_ID = User_ID });
            }
            return View(purchase);
        }

        // GET: Purchases/Delete/5
        public async Task<IActionResult> Delete(int? id, int User_ID)
        {
            var Data = await _context.User.Where(u => u.User_ID == User_ID).FirstOrDefaultAsync();
            ViewBag.Data = Data;
            if (id == null)
            {
                return NotFound();
            }

            var purchase = await _context.Purchase
                .FirstOrDefaultAsync(m => m.Purchase_ID == id);
            if (purchase == null)
            {
                return NotFound();
            }

            return View(purchase);
        }

        // POST: Purchases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var purchase = await _context.Purchase.FindAsync(id);
            if (purchase != null)
            {
                _context.Purchase.Remove(purchase);
            }

            await _context.SaveChangesAsync();
            TempData["SM_15"] = "Successfully cancled an order.";
            return RedirectToAction(nameof(Layout), new { User_ID = purchase.Patient_ID });
        }

        private bool PurchaseExists(int id)
        {
            return _context.Purchase.Any(e => e.Purchase_ID == id);
        }

        // Custom Built-in Function

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
        public async Task<IActionResult> Get_Sell_ID(int User_ID, int Medicine_ID)
        {
            var Data = await _context.User.Where(u => u.User_ID == User_ID).FirstOrDefaultAsync();
            ViewBag.Data = Data;
            var Sells = await _context.Sell.Where(u => u.Medicine_ID == Medicine_ID).ToListAsync();
            ViewBag.Sells = Sells;
            if (Sells == null || !Sells.Any())
            {
                TempData["DM_03"] = "No medicines Found";
                ViewBag.Medicines = null;
                return View("Search");
            }
            return View("Sells");
        }
        [HttpPost]
        public async Task<IActionResult> Get_Purchase_ID(int User_ID, int Selling_ID)
        {
            var Data = await _context.User.Where(u => u.User_ID == User_ID).FirstOrDefaultAsync();
            ViewBag.Data = Data;
            var Sells = await _context.Sell.Where(u => u.Selling_ID == Selling_ID).ToListAsync();
            ViewBag.Sells = Sells;
            ViewBag.Patient_ID = User_ID;
            return View("Create");
        }
        public async Task<IActionResult> Layout(int User_ID)
        {
            var Data = await _context.User.Where(u => u.User_ID == User_ID).FirstOrDefaultAsync();
            ViewBag.Data = Data;
            return View("~/Views/Users/Layout.cshtml");
        }
        public async Task<IActionResult> History(int id, string Choice)
        {
            var Data = await _context.User.Where(u => u.User_ID == id).FirstOrDefaultAsync();
            ViewBag.Data = Data;
            var Sell = await _context.Sell.ToListAsync();
            ViewBag.Sell = Sell;
            ViewBag.Choice = Choice;
            return View(await _context.Purchase.ToListAsync());
        }
    }
}
