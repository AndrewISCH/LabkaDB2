using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LabkaDB2;
using LabkaDB2.Models;

namespace LabkaDB2.Controllers
{
    public class RentsController : Controller
    {
        private readonly CarSharingDbContext _context;

        public RentsController(CarSharingDbContext context)
        {
            _context = context;
        }

        // GET: Rents
        public async Task<IActionResult> Index()
        {
            var carSharingDbContext = _context.Rents.Include(r => r.Car).Include(r => r.Car.Model).Include(r => r.Csz).Include(r => r.Cust);
            return View(await carSharingDbContext.ToListAsync());
        }

        

        // GET: Rents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rent = await _context.Rents
                .Include(r => r.Car)
                .Include(r => r.Csz)
                .Include(r => r.Cust)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rent == null)
            {
                return NotFound();
            }

            return View(rent);
        }

        // GET: Rents/Create
        public IActionResult Create()
        {
            ViewData["CarId"] = new SelectList(_context.Cars.Select(c => new { Text = $"{c.Model.ModelName} ({c.Id})", Value = c.Id }), "Value", "Text");
            ViewData["Cszid"] = new SelectList(_context.CarSharingZones.Select(c => new { Text = $"{c.Town} {c.Id}", Value = c.Id }), "Value", "Text");
            ViewData["CustId"] = new SelectList(_context.Customers.Select(c => new { Text = $"{c.FirstName} {c.LastName}", Value = c.Id }), "Value", "Text");

            return View();
        }

        // POST: Rents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CustId,CarId,Cszid,StartDate,FinishDate,CostPerDay")] Rent rent)
        {
            ModelState.Remove("Csz");
            ModelState.Remove("Car");
            ModelState.Remove("Cust");
            if (ModelState.IsValid)
            {
                _context.Add(rent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CarId"] = new SelectList(_context.Cars, "Id", "Id", rent.CarId);
            ViewData["Cszid"] = new SelectList(_context.CarSharingZones, "Id", "Id", rent.Cszid);
            ViewData["CustId"] = new SelectList(_context.Customers, "Id", "Id", rent.CustId);
            return View(rent);
        }

        // GET: Rents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rent = await _context.Rents.FindAsync(id);
            if (rent == null)
            {
                return NotFound();
            }
            ViewData["CarId"] = new SelectList(_context.Cars.Select(c => new {Text = $"{c.Model.ModelName} ({c.Id})", Value = c.Id }), "Value", "Text");
            ViewData["Cszid"] = new SelectList(_context.CarSharingZones.Select(c => new {Text = $"{c.Town} {c.Id}", Value = c.Id}), "Value", "Text");
            ViewData["CustId"] = new SelectList(_context.Customers.Select(c => new {Text = $"{c.FirstName} {c.LastName}", Value = c.Id}), "Value", "Text");
            return View(rent);
        }

        // POST: Rents/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CustId,CarId,Cszid,StartDate,FinishDate,CostPerDay")] Rent rent)
        {
            if (id != rent.Id)
            {
                return NotFound();
            }
            ModelState.Remove("Csz");
            ModelState.Remove("Car");
            ModelState.Remove("Cust");
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RentExists(rent.Id))
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
            ViewData["CarId"] = new SelectList(_context.Cars, "Id", "Id", rent.CarId);
            ViewData["Cszid"] = new SelectList(_context.CarSharingZones, "Id", "Id", rent.Cszid);
            ViewData["CustId"] = new SelectList(_context.Customers, "Id", "Id", rent.CustId);
            return View(rent);
        }

        // GET: Rents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rent = await _context.Rents
                .Include(r => r.Car)
                .Include(r => r.Csz)
                .Include(r => r.Cust)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rent == null)
            {
                return NotFound();
            }

            return View(rent);
        }

        // POST: Rents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rent = await _context.Rents.FindAsync(id);
            if (rent != null)
            {
                _context.Rents.Remove(rent);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RentExists(int id)
        {
            return _context.Rents.Any(e => e.Id == id);
        }
    }
}
