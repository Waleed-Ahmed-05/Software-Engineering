﻿using System;
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
                    return RedirectToAction(nameof(Login));
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
            }
            await _context.SaveChangesAsync();
            TempData["SM_01"] = "Account Deletion Successful.";
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
    }
}
