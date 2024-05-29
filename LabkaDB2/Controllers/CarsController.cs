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
    public class CarsController : Controller
    {
        private readonly CarSharingDbContext _context;

        public CarsController(CarSharingDbContext context)
        {
            _context = context;
        }

        // GET: Cars
        public async Task<IActionResult> Index()
        {
            var carSharingDbContext = _context.Cars.Include(c => c.Csz).Include(c => c.Model);
           

       
            return View(await carSharingDbContext.ToListAsync());
        }

        public async Task<IActionResult> CarsByTown()
        {
            var carSharingDbContext = _context.Cars.Include(c => c.Csz).Include(c => c.Model);



            return View(await carSharingDbContext.ToListAsync());
        }
        public async Task<IActionResult> CarsByUser()
        {
            var carSharingDbContext = _context.Cars.Include(c => c.Csz).Include(c => c.Model);



            return View(await carSharingDbContext.ToListAsync());
        }
        
        public async Task<IActionResult> CarsByBrand()
        {
            var carSharingDbContext = _context.Cars.Include(c => c.Csz).Include(c => c.Model);



            return View(await carSharingDbContext.ToListAsync());
        }


        public async Task<IActionResult> CarsNotRentedByUsersWhoRentedCar()
        {
            var carSharingDbContext = _context.Cars.Include(c => c.Csz).Include(c => c.Model);



            return View(await carSharingDbContext.ToListAsync());
        }


        public async Task<ActionResult<List<int>>> SearchByTown(string town)
        {
            var carIds = await _context.Cars
            .FromSqlRaw("SELECT c.Id FROM Cars c INNER JOIN CarSharingZones cz ON c.CSZID = cz.Id WHERE cz.Town = {0}", town)
            .Select(c => c.Id)
            .ToListAsync();

            return Json(carIds);

        }
        public async Task<ActionResult<List<Car>>> SearchCarsNotRentedByUsersWhoRentedCar(int carId)
        {
            var cars = await _context.Cars
                .FromSqlRaw(@"
                SELECT c.*
                FROM Cars c
                WHERE c.ID NOT IN (
                    SELECT DISTINCT r1.carID
                    FROM Rents r1
                    WHERE r1.custID IN (
                        SELECT r2.custID
                        FROM Rents r2
                        WHERE r2.carID = {0}
                    )
                )
            ", carId)
                .AsNoTracking()
                .Select(c=> c.Id)
                .ToListAsync();

            return Json(cars);
        }
    
    public async Task<ActionResult<List<int>>> SearchByBrand(string brand)
        {
            var carIds = await _context.Cars
            .FromSqlRaw("SELECT c.* FROM Cars c INNER JOIN Models m ON c.ModelId = m.Id WHERE m.Brand = {0}", brand)
            .Select(c => c.Id)
            .ToListAsync();

            return Json(carIds);

        }
        public async Task<ActionResult<List<int>>> SearchByUser(int id)
        {
            var carIds = await _context.Cars
            .FromSqlRaw("SELECT c.Id FROM Cars c INNER JOIN Rents r ON c.ID = r.carID WHERE r.custID = {0}", id)
            .Select(c => c.Id)
            .ToListAsync();

            return Json(carIds);

        }

        

        // GET: Cars/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars
                .Include(c => c.Csz)
                .Include(c => c.Model)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        public ActionResult GetAllCars()
        {
            return Json(_context.Cars);
        }

        // GET: Cars/Create
        public IActionResult Create()
        {
            ViewData["Cszid"] = new SelectList(_context.CarSharingZones.Select(c => new { Text = $"{c.Town} {c.Id}", Value = c.Id }), "Value", "Text");


            ViewData["ModelId"] = new SelectList(_context.Models.Select(c => new { Text = $"{c.ModelName} ({c.Id})", Value = c.Id }), "Value", "Text");
            
            return View();
        }

        // POST: Cars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ModelId,Cszid,IsRented,ProduceYear,TechInspirationDate,Color")] Car car)
        {
            ModelState.Remove("Csz");
            ModelState.Remove("Model");
            if (ModelState.IsValid)
            {
                _context.Add(car);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Cszid"] = new SelectList(_context.CarSharingZones, "Id", "Id", car.Cszid);
            ViewData["ModelId"] = new SelectList(_context.Models, "Id", "Id", car.ModelId);
            return View(car);
        }

        // GET: Cars/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            
            ViewData["Cszid"] = new SelectList(_context.CarSharingZones.Select(c => new { Text = $"{c.Town} {c.Id}", Value = c.Id }), "Value", "Text");
            
            
            ViewData["ModelId"] = new SelectList(_context.Models.Select(c => new {Text = $"{c.ModelName} ({c.Id})", Value = c.Id}), "Value", "Text");
            return View(car);
        }

        // POST: Cars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ModelId,Cszid,IsRented,ProduceYear,TechInspirationDate,Color")] Car car)
        {
            ModelState.Remove("Csz");
            ModelState.Remove("Model");

            if (id != car.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(car);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarExists(car.Id))
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
            ViewData["Cszid"] = new SelectList(_context.CarSharingZones, "Id", "Id", car.Cszid);
            ViewData["ModelId"] = new SelectList(_context.Models, "Id", "Id", car.ModelId);
            return View(car);
        }

        // GET: Cars/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars
                .Include(c => c.Csz)
                .Include(c => c.Model)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car != null)
            {
                _context.Cars.Remove(car);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CarExists(int id)
        {
            return _context.Cars.Any(e => e.Id == id);
        }
    }
}
