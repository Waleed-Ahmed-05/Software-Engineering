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
    public class RidesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RidesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Rides
        public async Task<IActionResult> Index(int User_ID)
        {
            var Data = await _context.User.Where(u => u.User_ID == User_ID).FirstOrDefaultAsync();
            ViewBag.Data = Data;
            return View(await _context.Ride.ToListAsync());
        }

        // GET: Rides/Details/5
        public async Task<IActionResult> Details(int? id, int User_ID)
        {
            var Data = await _context.User.Where(u => u.User_ID == User_ID).FirstOrDefaultAsync();
            ViewBag.Data = Data;
            if (id == null)
            {
                return NotFound();
            }

            var ride = await _context.Ride
                .FirstOrDefaultAsync(m => m.Ride_ID == id);
            if (ride == null)
            {
                return NotFound();
            }

            return View(ride);
        }

        // GET: Rides/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Rides/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Ride_ID,Driver_ID,Patient_ID,Pickup_Location,Dropoff_Location,Pickup_Time,Pickup_Date,Ride_Status")] Ride ride)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ride);
                await _context.SaveChangesAsync();
                TempData["SM_04"] = "Successfully sent a reservation request.";
                return RedirectToAction(nameof(Layout), new { User_ID = ride.Patient_ID });
            }
            return View(ride);
        }

        // GET: Rides/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ride = await _context.Ride.FindAsync(id);
            if (ride == null)
            {
                return NotFound();
            }
            return View(ride);
        }

        // POST: Rides/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Ride_ID,Driver_ID,Patient_ID,Pickup_Location,Dropoff_Location,Pickup_Time,Pickup_Date,Ride_Status")] Ride ride)
        {
            if (id != ride.Ride_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ride);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RideExists(ride.Ride_ID))
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
            return View(ride);
        }

        // GET: Rides/Delete/5
        public async Task<IActionResult> Delete(int? id, int User_ID)
        {
            var Data = await _context.User.Where(u => u.User_ID == User_ID).FirstOrDefaultAsync();
            ViewBag.Data = Data;
            if (id == null)
            {
                return NotFound();
            }

            var ride = await _context.Ride
                .FirstOrDefaultAsync(m => m.Ride_ID == id);
            if (ride == null)
            {
                return NotFound();
            }

            return View(ride);
        }

        // POST: Rides/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ride = await _context.Ride.FindAsync(id);
            if (ride != null)
            {
                _context.Ride.Remove(ride);
            }

            await _context.SaveChangesAsync();
            TempData["SM_07"] = "Successfully canceled a reservation request.";
            return RedirectToAction(nameof(Layout), new { User_ID = ride.Patient_ID });
        }

        private bool RideExists(int id)
        {
            return _context.Ride.Any(e => e.Ride_ID == id);
        }

        // Custom built-in functions
        public async Task<IActionResult> Layout(int User_ID)
        {
            var Data = await _context.User.Where(u => u.User_ID == User_ID).FirstOrDefaultAsync();
            ViewBag.Data = Data;
            return View("~/Views/Users/Layout.cshtml");
        }
        public async Task<IActionResult> History(int User_ID, string Choice)
        {
            var Data = await _context.User.Where(u => u.User_ID == User_ID).FirstOrDefaultAsync();
            ViewBag.Data = Data;
            var Driver = await _context.Driver.Where(u => u.User_ID == User_ID).FirstOrDefaultAsync();
            ViewBag.Driver = Driver;
            ViewBag.Choice = Choice;
            return View(await _context.Ride.ToListAsync());
        }
    }
}
