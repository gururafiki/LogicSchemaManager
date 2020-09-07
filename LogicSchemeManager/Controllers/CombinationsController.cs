using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LogicSchemeManager.Models;

namespace LogicSchemeManager.Controllers
{
    public class CombinationsController : Controller
    {
        private readonly LogicSchemeManagerContext _context;

        public CombinationsController(LogicSchemeManagerContext context)
        {
            _context = context;
        }

        // GET: Combinations
        public async Task<IActionResult> Index()
        {
            var logicSchemeManagerContext = _context.Combinations.Include(c => c.ElementType);
            return View(await logicSchemeManagerContext.ToListAsync());
        }

        // GET: Combinations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var combination = await _context.Combinations
                .Include(c => c.ElementType)
                .FirstOrDefaultAsync(m => m.CombinationId == id);
            if (combination == null)
            {
                return NotFound();
            }

            return View(combination);
        }

        // GET: Combinations/Create
        public IActionResult Create()
        {
            ViewData["ElementTypeId"] = new SelectList(_context.ElementTypes, "ElementTypeId", "Name");
            return View();
        }

        // POST: Combinations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CombinationId,ElementTypeId,Name")] Combination combination)
        {
            if (ModelState.IsValid)
            {
                _context.Add(combination);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ElementTypeId"] = new SelectList(_context.ElementTypes, "ElementTypeId", "Name", combination.ElementTypeId);
            return View(combination);
        }

        // GET: Combinations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var combination = await _context.Combinations.FindAsync(id);
            if (combination == null)
            {
                return NotFound();
            }
            ViewData["ElementTypeId"] = new SelectList(_context.ElementTypes, "ElementTypeId", "Name", combination.ElementTypeId);
            return View(combination);
        }

        // POST: Combinations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CombinationId,ElementTypeId,Name")] Combination combination)
        {
            if (id != combination.CombinationId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(combination);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CombinationExists(combination.CombinationId))
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
            ViewData["ElementTypeId"] = new SelectList(_context.ElementTypes, "ElementTypeId", "Name", combination.ElementTypeId);
            return View(combination);
        }

        // GET: Combinations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var combination = await _context.Combinations
                .Include(c => c.ElementType)
                .FirstOrDefaultAsync(m => m.CombinationId == id);
            if (combination == null)
            {
                return NotFound();
            }

            return View(combination);
        }

        // POST: Combinations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var combination = await _context.Combinations.FindAsync(id);
            _context.Combinations.Remove(combination);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CombinationExists(int id)
        {
            return _context.Combinations.Any(e => e.CombinationId == id);
        }
    }
}
