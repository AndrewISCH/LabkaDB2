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
    public class CustomersController : Controller
    {
        private readonly CarSharingDbContext _context;

        public CustomersController(CarSharingDbContext context)
        {
            _context = context;
        }

        // GET: Customers
        public async Task<IActionResult> Index()
        {
            return View(await _context.Customers.ToListAsync());
        }

        public async Task<IActionResult> CustomersByRentedCar()
        {
            return View(await _context.Customers.ToListAsync());
        }
        public async Task<IActionResult> CustomersByTown()
        {
            return View(await _context.Customers.ToListAsync());
        }

        public async Task<IActionResult> CustomersWithSameCars()
        {
            return View(await _context.Customers.ToListAsync());
        }
        

        public async Task<IActionResult> CustomersWithAllCarsFromZone()
        {
            return View(await _context.Customers.ToListAsync());
        }

        public async Task<IActionResult> CustomersWithSameOrMoreCars()
        {
            return View(await _context.Customers.ToListAsync());
        }

        public async Task<ActionResult<List<int>>> SearchCustsWithSameCars(int custId)
        {
            var customers = await _context.Customers
                .FromSqlRaw(@"
                WITH CustCars AS (
                    SELECT carID
                    FROM Rents
                    WHERE custID = {0}
                ),
                OtherCustCars AS (
                    SELECT custID, carID
                    FROM Rents
                    WHERE custID != {0}
                ),
                GroupedCustCars AS (
                    SELECT custID, COUNT(carID) AS CarCount
                    FROM OtherCustCars
                    GROUP BY custID
                    HAVING COUNT(carID) = (SELECT COUNT(*) FROM CustCars)
                )
                SELECT DISTINCT c.*
                FROM Customers c
                WHERE EXISTS (
                    SELECT 1
                    FROM GroupedCustCars gcc
                    WHERE gcc.custID = c.ID
                    AND NOT EXISTS (
                        SELECT cc.carID
                        FROM CustCars cc
                        WHERE NOT EXISTS (
                            SELECT 1
                            FROM Rents r
                            WHERE r.custID = gcc.custID
                            AND r.carID = cc.carID
                        )
                    )
                )
            ", custId)
                .AsNoTracking()
                .ToListAsync(); 

            var customerIds = customers
                .AsEnumerable() 
                .Select(c => c.Id)
                .ToList(); 

            return Json(customerIds);
        }

        public async Task<ActionResult<List<Customer>>> SearchCustomersWhoRentedAllCarsInZone(int cszId)
        {
            var customers = await _context.Customers
                .FromSqlRaw(@"
                SELECT DISTINCT cu.*
                FROM Customers cu
                WHERE NOT EXISTS (
                    SELECT 1
                    FROM Cars c
                    WHERE c.CSZID = {0} AND NOT EXISTS (
                        SELECT 1
                        FROM Rents r
                        WHERE r.carID = c.ID AND r.custID = cu.ID
                    )
                )
            ", cszId)
                .AsNoTracking()
                .Select(c => c.Id)
                .ToListAsync();

            return Json(customers);
        }

        public async Task<ActionResult<List<int>>> SearchCustsWithSameOrMoreCars(int custId)
        {
            var customers = await _context.Customers
             .FromSqlRaw(@"
                SELECT cu.*
                FROM Customers cu
                WHERE cu.ID != {0} AND NOT EXISTS (
                    SELECT r.carID
                    FROM Rents r
                    WHERE r.custID = {0} AND NOT EXISTS (
                        SELECT 1
                        FROM Rents r2
                        WHERE r2.carID = r.carID AND r2.custID = cu.ID
                    )
                ) AND EXISTS (
                    SELECT 1
                    FROM Rents r3
                    WHERE r3.custID = cu.ID AND r3.carID IN (
                        SELECT r4.carID
                        FROM Rents r4
                        WHERE r4.custID = {0}
                    )
                )
            ", custId)
             .AsNoTracking()
             .Select(c => c.Id)
             .ToListAsync();

            return Json(customers);
        }


        public async Task<ActionResult<List<int>>> SearchByTown(string town)
        {
            var customers = await _context.Customers
            .FromSqlRaw("SELECT DISTINCT cu.* FROM Customers cu INNER JOIN Rents r ON cu.Id = r.CustId INNER JOIN CarSharingZones csz ON r.CszId = csz.Id WHERE csz.Town = {0}", town)
            .Select(c => c.Id)
            .ToListAsync();

            return Json(customers);

        }

        public async Task<ActionResult<List<int>>> SearchByRentedCar(int carId)
        {
            var customers = await _context.Customers
            .FromSqlRaw("SELECT cu.* FROM Customers cu INNER JOIN Rents r ON cu.Id = r.CustId WHERE r.CarId = {0}", carId)
            .Select(c => c.Id)
            .ToListAsync();

            return Json(customers);

        }
        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,CreationDate,Phone,Email,NumOfOrders,Password")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,CreationDate,Phone,Email,NumOfOrders,Password")] Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.Id))
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
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}
