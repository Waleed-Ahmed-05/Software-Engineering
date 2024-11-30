using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAMS.Data;
using DAMS.Models.Medicine_Model_;
using DAMS.Models;

namespace DAMS.Controllers
{
    public class MedicinesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MedicinesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Medicines
        public async Task<IActionResult> Index()
        {
            return View(await _context.Medicine.ToListAsync());
        }

        // GET: Medicines/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicine = await _context.Medicine
                .FirstOrDefaultAsync(m => m.Medicine_ID == id);
            if (medicine == null)
            {
                return NotFound();
            }

            return View(medicine);
        }

        // GET: Medicines/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Medicines/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Medicine_ID,Medicine_Name,Medicine_Description,Category,Weightage_ID,Price_ID")] Medicine medicine, int User_ID)
        {
            if (ModelState.IsValid)
            {
                _context.Add(medicine);
                await _context.SaveChangesAsync();
                TempData["SM_08"] = "Successfully added a new medicine.";
                return RedirectToAction(nameof(Layout), new { User_ID = User_ID });
            }
            return View(medicine);
        }

        // GET: Medicines/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicine = await _context.Medicine.FindAsync(id);
            if (medicine == null)
            {
                return NotFound();
            }
            return View(medicine);
        }

        // POST: Medicines/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Medicine_ID,Medicine_Name,Medicine_Description,Category,Weightage_ID,Price_ID")] Medicine medicine)
        {
            if (id != medicine.Medicine_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(medicine);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MedicineExists(medicine.Medicine_ID))
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
            return View(medicine);
        }

        // GET: Medicines/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var medicine = await _context.Medicine
                .FirstOrDefaultAsync(m => m.Medicine_ID == id);
            if (medicine == null)
            {
                return NotFound();
            }

            return View(medicine);
        }

        // POST: Medicines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var medicine = await _context.Medicine.FindAsync(id);
            if (medicine != null)
            {
                _context.Medicine.Remove(medicine);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MedicineExists(int id)
        {
            return _context.Medicine.Any(e => e.Medicine_ID == id);
        }

        // Custom Built-in Functions
        public async Task<IActionResult> Validate(int id)
        {
            var Data = await _context.User.Where(u => u.User_ID == id).FirstOrDefaultAsync();
            ViewBag.Data = Data;
            return View();
        }
        public async Task<IActionResult> Validation([Bind("Price_ID", "Medicine_Price")] Price price, [Bind("Weightage_ID", "Medicine_Weightage")] Weightage weightage, int User_ID, string Medicine_Name, string Medicine_Description, string Category, int Medicine_Weightage, float Medicine_Price)
        {
            bool Availability = false;
            var Medicines = await _context.Medicine.ToListAsync();
            var Weightage = await _context.Weightage.Where(u => u.Medicine_Weightage == Medicine_Weightage).FirstOrDefaultAsync();
            var Price = await _context.Price.Where(u => u.Medicine_Price == Medicine_Price).FirstOrDefaultAsync();
            var Data = await _context.User.Where(u => u.User_ID == User_ID).FirstOrDefaultAsync();
            ViewBag.Data = Data;
            if (Weightage != null && Price != null)
            {
                foreach (var Medicine in Medicines)
                {
                    if (Medicine.Medicine_Name == Medicine_Name && Medicine.Weightage_ID == Weightage.Weightage_ID && Medicine.Price_ID == Price.Price_ID)
                    {
                        Availability = true;
                    }
                }
            }
            if (!Availability)
            {
                if (Weightage == null)
                {
                    _context.Add(weightage);
                    await _context.SaveChangesAsync();
                    Weightage = await _context.Weightage.Where(u => u.Medicine_Weightage == Medicine_Weightage).FirstOrDefaultAsync();
                }
                if (Price == null)
                {
                    _context.Add(price);
                    await _context.SaveChangesAsync();
                    Price = await _context.Price.Where(u => u.Medicine_Price == Medicine_Price).FirstOrDefaultAsync();
                }
                ViewBag.User_ID = User_ID;
                ViewBag.Medicine_Name = Medicine_Name;
                ViewBag.Medicine_Description = Medicine_Description;
                ViewBag.Category = Category;
                ViewBag.Price_ID = Price.Price_ID;
                ViewBag.Weightage_ID = Weightage.Weightage_ID;
                return View("Create");
            }
            TempData["SM_09"] = "A medicine with the same details already exists.";
            return RedirectToAction(nameof(Layout), new { User_ID = User_ID });
        }
        public async Task<IActionResult> Layout(int User_ID)
        {
            var Data = await _context.User.Where(u => u.User_ID == User_ID).FirstOrDefaultAsync();
            ViewBag.Data = Data;
            return View("~/Views/Users/Layout.cshtml");
        }
    }
}
