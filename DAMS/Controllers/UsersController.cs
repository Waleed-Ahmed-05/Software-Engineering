using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DAMS.Data;
using DAMS.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace DAMS.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _context.User.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.User_ID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("User_ID,First_Name,Last_Name,Gender,Age,DOB,Contact,CNIC,Role,Email,Password")] User user)
        {
            var Data = await _context.User.Where(u => u.Email == user.Email).FirstOrDefaultAsync();
            if (Data == null)
            {
                if (ModelState.IsValid)
                {
                    _context.Add(user);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Additional_Details), new { User_ID = user.User_ID });
                }
                return View(user);
            }
            TempData["DM_02"] = "A user with this email already exists. Try using another email.";
            return View("Signup");
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var Data = await _context.User.Where(u => u.User_ID == id).FirstOrDefaultAsync();
            ViewBag.Data = Data;
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("User_ID,First_Name,Last_Name,Gender,Age,DOB,Contact,CNIC,Role,Email,Password")] User user)
        {
            if (id != user.User_ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.User_ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["SM_02"] = "Successfully changed user credentials.";
                return RedirectToAction(nameof(Layout), new { User_ID = user.User_ID });
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var Data = await _context.User.Where(u => u.User_ID == id).FirstOrDefaultAsync();
            ViewBag.Data = Data;
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FirstOrDefaultAsync(m => m.User_ID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user != null)
            {
                _context.User.Remove(user);
                var appointment = await _context.Appointment.ToListAsync();
                foreach (var Data in appointment)
                {
                    if (Data.Appointment_Status != "Done" && user.User_ID == Data.Patient_ID)
                    {
                        var Data_01 = await _context.Appointment.FindAsync(Data.Appointment_ID);
                        if (Data_01 != null)
                        {
                            _context.Appointment.Remove(Data_01);
                        }
                    }
                }
                var ride = await _context.Ride.ToListAsync();
                foreach (var Data in ride)
                {
                    if (Data.Ride_Status != "Done" && user.User_ID == Data.Patient_ID)
                    {
                        var Data_01 = await _context.Ride.FindAsync(Data.Ride_ID);
                        if (Data_01 != null)
                        {
                            _context.Ride.Remove(Data_01);
                        }
                    }
                }
                if (user.Role == "Doctor")
                {
                    var doctor = await _context.Doctor.FirstOrDefaultAsync(d => d.User_ID == id);
                    if (doctor != null)
                    {
                        _context.Doctor.Remove(doctor);
                    }
                }
                else if (user.Role == "Driver")
                {
                    var driver = await _context.Driver.FirstOrDefaultAsync(d => d.User_ID == id);
                    if (driver != null)
                    {
                        _context.Driver.Remove(driver);
                    }
                }
                await _context.SaveChangesAsync();
                TempData["SM_01"] = "Account Deletion Successful.";
            }
            return RedirectToAction(nameof(Login));
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.User_ID == id);
        }

        // Custom built-in functions

        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Signup()
        {
            return View();
        }
        public async Task<IActionResult> Layout(int User_ID)
        {
            var Data = await _context.User.Where(u => u.User_ID == User_ID).FirstOrDefaultAsync();
            ViewBag.Data = Data;
            return View();
        }
        public async Task<IActionResult> Validate(string Email, string Password)
        {
            var Data = await _context.User.Where(u => u.Email == Email && u.Password == Password).FirstOrDefaultAsync();
            if (Data != null)
            {
                ViewBag.Data = Data;
                return View("Layout");
            }
            TempData["DM_01"] = "Login failed. Either the E-mail or Password is Invlaid.";
            return View("Login");
        }
        public async Task<IActionResult> Additional_Details(int User_ID)
        {
            var Data = await _context.User.Where(u => u.User_ID == User_ID).FirstOrDefaultAsync();
            if (Data.Role == "Doctor")
            {
                return View("~/Views/Doctors/Create.cshtml");
            }
            else if(Data.Role == "Driver")
            {
                return View("~/Views/Drivers/Create.cshtml");
            }
            return View("Login");
        }
        public async Task<IActionResult> Edit_Additional_Details(int id)
        {
            var Data = await _context.User.Where(u => u.User_ID == id).FirstOrDefaultAsync();
            ViewBag.Data = Data;
            if(Data.Role == "Doctor")
            {
                var Data_01 = await _context.Doctor.Where(u => u.User_ID == id).FirstOrDefaultAsync();
                ViewBag.Data_01 = Data_01;
                return View("~/Views/Doctors/Edit.cshtml", Data_01);
            }
            else if(Data.Role == "Driver")
            {
                var Data_01 = await _context.Driver.Where(u => u.User_ID == id).FirstOrDefaultAsync();
                ViewBag.Data_01 = Data_01;
                return View("~/Views/Drivers/Edit.cshtml", Data_01);
            }
            return View("Layout");
        }
        public async Task<IActionResult> Reserve_Appointment(int id)
        {
            var Data = await _context.User.Where(u => u.User_ID == id).FirstOrDefaultAsync();
            ViewBag.Data = Data;
            var Doctors = await _context.Doctor.ToListAsync();
            ViewBag.Doctor = Doctors;
            return View("Appointments", await _context.User.ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> Get_Appointment_ID(int User_ID, int Doctor_ID)
        {
            var Data = await _context.User.Where(u => u.User_ID == User_ID).FirstOrDefaultAsync();
            ViewBag.Data = Data;
            var Doctor = await _context.Doctor.Where(u => u.Doctor_ID == Doctor_ID).FirstOrDefaultAsync();
            ViewBag.Doctor = Doctor;
            return View("~/Views/Appointments/Create.cshtml");
        }
        public async Task<IActionResult> Reserve_Ride(int id)
        {
            var Data = await _context.User.Where(u => u.User_ID == id).FirstOrDefaultAsync();
            ViewBag.Data = Data;
            var Appointment = await _context.Appointment.Where(u => u.Patient_ID == id).Where(u => u.Appointment_Status == "Accepted").ToListAsync();
            ViewBag.Appointment = Appointment;
            var Doctor = await _context.Doctor.ToListAsync();
            ViewBag.Doctor = Doctor;
            return View("Rides", await _context.User.ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> Get_Driver_ID(int User_ID, int Doctor_ID, int Appointment_ID)
        {
            var Data = await _context.User.Where(u => u.User_ID == User_ID).FirstOrDefaultAsync();
            ViewBag.Data = Data;
            ViewBag.Doctor_ID = Doctor_ID;
            ViewBag.Appointment_ID = Appointment_ID;
            var Driver = await _context.Driver.ToListAsync();
            ViewBag.Driver = Driver;
            return View("Drivers", await _context.User.ToListAsync());
        }
        [HttpPost]
        public async Task<IActionResult> Get_Ride_ID(int User_ID, int Doctor_ID, int Driver_ID, int Appointment_ID)
        {
            var Data = await _context.User.Where(u => u.User_ID == User_ID).FirstOrDefaultAsync();
            ViewBag.Data = Data;
            var Doctor = await _context.Doctor.Where(u => u.Doctor_ID == Doctor_ID).FirstOrDefaultAsync();
            ViewBag.Doctor = Doctor;
            var Appointment = await _context.Appointment.Where(u => u.Appointment_ID == Appointment_ID).FirstOrDefaultAsync();
            ViewBag.Appointment = Appointment;
            ViewBag.Driver_ID = Driver_ID;
            return View("~/Views/Rides/Create.cshtml");
        }
    }
}
