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
    public class ElementTypesController : Controller
    {
        private readonly LogicSchemeManagerContext _context;

        public ElementTypesController(LogicSchemeManagerContext context)
        {
            _context = context;
        }

        // GET: ElementTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.ElementTypes.ToListAsync());
        }

        // GET: ElementTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var elementType = await _context.ElementTypes
                .FirstOrDefaultAsync(m => m.ElementTypeId == id);
            if (elementType == null)
            {
                return NotFound();
            }

            return View(elementType);
        }

        // GET: ElementTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ElementTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ElementTypeId,Name")] ElementType elementType)
        {
            if (ModelState.IsValid)
            {
                _context.Add(elementType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(elementType);
        }

        // GET: ElementTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var elementType = await _context.ElementTypes.FindAsync(id);
            if (elementType == null)
            {
                return NotFound();
            }
            return View(elementType);
        }

        // POST: ElementTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ElementTypeId,Name")] ElementType elementType)
        {
            if (id != elementType.ElementTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(elementType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ElementTypeExists(elementType.ElementTypeId))
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
            return View(elementType);
        }

        // GET: ElementTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var elementType = await _context.ElementTypes
                .FirstOrDefaultAsync(m => m.ElementTypeId == id);
            if (elementType == null)
            {
                return NotFound();
            }

            return View(elementType);
        }

        // POST: ElementTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var elementType = await _context.ElementTypes.FindAsync(id);
            _context.ElementTypes.Remove(elementType);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ElementTypeExists(int id)
        {
            return _context.ElementTypes.Any(e => e.ElementTypeId == id);
        }
    }
}
