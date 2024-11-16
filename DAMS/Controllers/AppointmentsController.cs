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
    public class AppointmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AppointmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Appointments
        public async Task<IActionResult> Index(int User_ID)
        {
            var Data = await _context.User.Where(u => u.User_ID == User_ID).FirstOrDefaultAsync();
            ViewBag.Data = Data;
            return View(await _context.Appointment.ToListAsync());
        }

        // GET: Appointments/Details/5
        public async Task<IActionResult> Details(int? id, int User_ID)
        {
            var Data = await _context.User.Where(u => u.User_ID == User_ID).FirstOrDefaultAsync();
            ViewBag.Data = Data;
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointment
                .FirstOrDefaultAsync(m => m.Appointment_ID == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // GET: Appointments/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Appointments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Appointment_ID,Patient_ID,Doctor_ID,Appointment_Date,Appointment_Time,Appointment_Status")] Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(appointment);
                await _context.SaveChangesAsync();
                TempData["SM_04"] = "Successfully sent a reservation request.";
                return RedirectToAction(nameof(Layout), new { User_ID = appointment.Patient_ID });
            }
            return View(appointment);
        }

        // GET: Appointments/Edit/5
        public async Task<IActionResult> Edit(int? id, int User_ID, string Choice)
        {
            var Data = await _context.User.Where(u => u.User_ID == User_ID).FirstOrDefaultAsync();
            ViewBag.Data = Data;
            ViewBag.Choice = Choice;
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointment.FindAsync(id);
            if (appointment == null)
            {
                return NotFound();
            }
            return View(appointment);
        }

        // POST: Appointments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Appointment_ID,Patient_ID,Doctor_ID,Appointment_Date,Appointment_Time,Appointment_Status")] Appointment appointment)
        {
            if (id != appointment.Appointment_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appointment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppointmentExists(appointment.Appointment_ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["SM_06"] = "Accepted a reservation request.";
                return RedirectToAction(nameof(Layout_01), new { Doctor_ID = appointment.Doctor_ID });
            }
            return View(appointment);
        }

        // GET: Appointments/Delete/5
        public async Task<IActionResult> Delete(int? id, int User_ID)
        {
            var Data = await _context.User.Where(u => u.User_ID == User_ID).FirstOrDefaultAsync();
            ViewBag.Data = Data;
            if (id == null)
            {
                return NotFound();
            }

            var appointment = await _context.Appointment
                .FirstOrDefaultAsync(m => m.Appointment_ID == id);
            if (appointment == null)
            {
                return NotFound();
            }

            return View(appointment);
        }

        // POST: Appointments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appointment = await _context.Appointment.FindAsync(id);
            if (appointment != null)
            {
                _context.Appointment.Remove(appointment);
            }

            await _context.SaveChangesAsync();
            TempData["SM_05"] = "Successfully canceled a reservation request.";
            return RedirectToAction(nameof(Layout), new { User_ID = appointment.Patient_ID });
        }

        private bool AppointmentExists(int id)
        {
            return _context.Appointment.Any(e => e.Appointment_ID == id);
        }

        // Custom built-in functions
        public async Task<IActionResult> Layout(int User_ID)
        {
            var Data = await _context.User.Where(u => u.User_ID == User_ID).FirstOrDefaultAsync();
            ViewBag.Data = Data;
            return View("~/Views/Users/Layout.cshtml");
        }
        public async Task<IActionResult> Layout_01(int Doctor_ID)
        {
            var Doctor = await _context.Doctor.Where(u => u.Doctor_ID == Doctor_ID).FirstOrDefaultAsync();
            ViewBag.Doctor = Doctor;
            var Data = await _context.User.Where(u => u.User_ID == Doctor.User_ID).FirstOrDefaultAsync();
            ViewBag.Data = Data;
            return View("Requests", await _context.Appointment.ToListAsync());
        }
        public async Task<IActionResult> Requests(int id)
        {
            var Data = await _context.User.Where(u => u.User_ID == id).FirstOrDefaultAsync();
            ViewBag.Data = Data;
            var Doctor = await _context.Doctor.Where(u => u.User_ID == id).FirstOrDefaultAsync();
            ViewBag.Doctor = Doctor;
            return View(await _context.Appointment.ToListAsync());
        }
        public async Task<IActionResult> History(int User_ID, string Choice)
        {
            var Data = await _context.User.Where(u => u.User_ID == User_ID).FirstOrDefaultAsync();
            ViewBag.Data = Data;
            var Doctor = await _context.Doctor.Where(u => u.User_ID == User_ID).FirstOrDefaultAsync();
            ViewBag.Doctor = Doctor;
            ViewBag.Choice = Choice;
            return View(await _context.Appointment.ToListAsync());
        }
    }
}
