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
    public class CarSharingZonesController : Controller
    {
        private readonly CarSharingDbContext _context;

        public CarSharingZonesController(CarSharingDbContext context)
        {
            _context = context;
        }

        // GET: CarSharingZones
        public async Task<IActionResult> Index()
        {
            return View(await _context.CarSharingZones.ToListAsync());
        }

        // GET: CarSharingZones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carSharingZone = await _context.CarSharingZones
                .FirstOrDefaultAsync(m => m.Id == id);
            if (carSharingZone == null)
            {
                return NotFound();
            }

            return View(carSharingZone);
        }

        // GET: CarSharingZones/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CarSharingZones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Town,CarCapacity,Latitude,Longtitude")] CarSharingZone carSharingZone)
        {
            if (ModelState.IsValid)
            {
                _context.Add(carSharingZone);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(carSharingZone);
        }

        // GET: CarSharingZones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carSharingZone = await _context.CarSharingZones.FindAsync(id);
            if (carSharingZone == null)
            {
                return NotFound();
            }
            return View(carSharingZone);
        }

        // POST: CarSharingZones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Town,CarCapacity,Latitude,Longtitude")] CarSharingZone carSharingZone)
        {
            if (id != carSharingZone.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(carSharingZone);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarSharingZoneExists(carSharingZone.Id))
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
            return View(carSharingZone);
        }

        // GET: CarSharingZones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var carSharingZone = await _context.CarSharingZones
                .FirstOrDefaultAsync(m => m.Id == id);
            if (carSharingZone == null)
            {
                return NotFound();
            }

            return View(carSharingZone);
        }

        // POST: CarSharingZones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var carSharingZone = await _context.CarSharingZones.FindAsync(id);
            if (carSharingZone != null)
            {
                _context.CarSharingZones.Remove(carSharingZone);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarSharingZoneExists(int id)
        {
            return _context.CarSharingZones.Any(e => e.Id == id);
        }
    }
}
