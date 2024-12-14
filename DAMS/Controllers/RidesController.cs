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
            var Ride = await _context.Ride.Where(u => u.Ride_ID == id).FirstOrDefaultAsync();
            ViewBag.Ride= Ride;
            var Vehicle = await _context.Driver.Where(u => u.Driver_ID == Ride.Driver_ID).FirstOrDefaultAsync();
            ViewBag.Vehicle = Vehicle;
            var Driver = await _context.User.Where(u => u.User_ID == Vehicle.User_ID).FirstOrDefaultAsync();
            ViewBag.Driver = Driver;
            var Patient = await _context.User.Where(u => u.User_ID == Ride.Patient_ID).FirstOrDefaultAsync();
            ViewBag.Patient = Patient;
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
        public async Task<IActionResult> Edit(int? id, int User_ID, string Choice)
        {
            var Data = await _context.User.Where(u => u.User_ID == User_ID).FirstOrDefaultAsync();
            ViewBag.Data = Data;
            ViewBag.Choice = Choice;
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
                TempData["SM_06"] = "Accepted a reservation request.";
                return RedirectToAction(nameof(Layout_01), new { Driver_ID = ride.Driver_ID });
            }
            return View(ride);
        }

        // GET: Rides/Delete/5
        public async Task<IActionResult> Delete(int? id, int User_ID)
        {
            var Data = await _context.User.Where(u => u.User_ID == User_ID).FirstOrDefaultAsync();
            ViewBag.Data = Data;
            var Ride = await _context.Ride.Where(u => u.Ride_ID == id).FirstOrDefaultAsync();
            ViewBag.Ride = Ride;
            var Vehicle = await _context.Driver.Where(u => u.Driver_ID == Ride.Driver_ID).FirstOrDefaultAsync();
            ViewBag.Vehicle = Vehicle;
            var Driver = await _context.User.Where(u => u.User_ID == Vehicle.User_ID).FirstOrDefaultAsync();
            ViewBag.Driver = Driver;
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
        public async Task<IActionResult> Requests(int id)
        {
            var Data = await _context.User.Where(u => u.User_ID == id).FirstOrDefaultAsync();
            ViewBag.Data = Data;
            var Driver = await _context.Driver.Where(u => u.User_ID == id).FirstOrDefaultAsync();
            ViewBag.Driver = Driver;
            return View(await _context.Ride.ToListAsync());
        }
        public async Task<IActionResult> Layout_01(int Driver_ID)
        {
            var Driver = await _context.Driver.Where(u => u.Driver_ID == Driver_ID).FirstOrDefaultAsync();
            ViewBag.Driver = Driver;
            var Data = await _context.User.Where(u => u.User_ID == Driver.User_ID).FirstOrDefaultAsync();
            ViewBag.Data = Data;
            return View("Requests", await _context.Ride.ToListAsync());
        }
    }
}
