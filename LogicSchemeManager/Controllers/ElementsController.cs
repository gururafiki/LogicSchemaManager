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
    public class ElementsController : Controller
    {
        private readonly LogicSchemeManagerContext _context;

        public ElementsController(LogicSchemeManagerContext context)
        {
            _context = context;
        }

        // GET: Elements
        public async Task<IActionResult> Index()
        {
            var logicSchemeManagerContext = _context.Elements.Include(e => e.ElementType).Include(e => e.Schema);
            return View(await logicSchemeManagerContext.ToListAsync());
        }

        // GET: Elements/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var element = await _context.Elements
                .Include(e => e.ElementType)
                .Include(e => e.Schema)
                .FirstOrDefaultAsync(m => m.ElementId == id);
            if (element == null)
            {
                return NotFound();
            }

            return View(element);
        }

		// GET: Elements/Nodes
		public IActionResult Nodes() {
			ViewData["ElementTypeId"] = new SelectList(_context.ElementTypes, "ElementTypeId", "Name");
			ViewData["SchemaId"] = new SelectList(_context.Scheme, "SchemaId", "Name");
			return View();
		}
		
		// GET: Elements/Create
		public IActionResult Create()
        {
            ViewData["ElementTypeId"] = new SelectList(_context.ElementTypes, "ElementTypeId", "Name");
            ViewData["SchemaId"] = new SelectList(_context.Scheme, "SchemaId", "Name");
            return View();
        }

        // POST: Elements/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ElementId,Name,SchemaId,ElementTypeId")] Element element)
        {
            if (ModelState.IsValid)
            {
                _context.Add(element);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ElementTypeId"] = new SelectList(_context.ElementTypes, "ElementTypeId", "Name", element.ElementTypeId);
            ViewData["SchemaId"] = new SelectList(_context.Scheme, "SchemaId", "Name", element.SchemaId);
            return View(element);
        }

        // GET: Elements/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var element = await _context.Elements.FindAsync(id);
            if (element == null)
            {
                return NotFound();
            }
            ViewData["ElementTypeId"] = new SelectList(_context.ElementTypes, "ElementTypeId", "Name", element.ElementTypeId);
            ViewData["SchemaId"] = new SelectList(_context.Scheme, "SchemaId", "Name", element.SchemaId);
            return View(element);
        }

        // POST: Elements/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ElementId,Name,SchemaId,ElementTypeId")] Element element)
        {
            if (id != element.ElementId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(element);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ElementExists(element.ElementId))
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
            ViewData["ElementTypeId"] = new SelectList(_context.ElementTypes, "ElementTypeId", "Name", element.ElementTypeId);
            ViewData["SchemaId"] = new SelectList(_context.Scheme, "SchemaId", "Name", element.SchemaId);
            return View(element);
        }

        // GET: Elements/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var element = await _context.Elements
                .Include(e => e.ElementType)
                .Include(e => e.Schema)
                .FirstOrDefaultAsync(m => m.ElementId == id);
            if (element == null)
            {
                return NotFound();
            }

            return View(element);
        }

        // POST: Elements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var element = await _context.Elements.FindAsync(id);
            _context.Elements.Remove(element);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ElementExists(int id)
        {
            return _context.Elements.Any(e => e.ElementId == id);
        }
    }
}
