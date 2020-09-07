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
    public class SchemeController : Controller
    {
        private readonly LogicSchemeManagerContext _context;

        public SchemeController(LogicSchemeManagerContext context)
        {
            _context = context;
        }

        // GET: Scheme
        public async Task<IActionResult> Index()
        {
            return View(await _context.Scheme.ToListAsync());
        }

        // GET: Scheme/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schema = await _context.Scheme
                .FirstOrDefaultAsync(m => m.SchemaId == id);
            if (schema == null)
            {
                return NotFound();
			}
			ViewData["OutptElementPorts"] = new SelectList(_context.ElementPorts.Where(ep => (ep.Port.IsOutput || (!ep.Port.IsOutput && ep.ParentId == null)) && ep.Element.SchemaId == id), "ElementPortId", "Name");
			return View(schema);
        }

        // GET: Scheme/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Scheme/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SchemaId,Name")] Schema schema)
        {
            if (ModelState.IsValid)
            {
                _context.Add(schema);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(schema);
        }

        // GET: Scheme/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schema = await _context.Scheme.FindAsync(id);
            if (schema == null)
            {
                return NotFound();
            }
            return View(schema);
        }

        // POST: Scheme/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SchemaId,Name")] Schema schema)
        {
            if (id != schema.SchemaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(schema);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SchemaExists(schema.SchemaId))
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
            return View(schema);
        }

        // GET: Scheme/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schema = await _context.Scheme
                .FirstOrDefaultAsync(m => m.SchemaId == id);
            if (schema == null)
            {
                return NotFound();
            }

            return View(schema);
        }

        // POST: Scheme/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var schema = await _context.Scheme.FindAsync(id);
            _context.Scheme.Remove(schema);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SchemaExists(int id)
        {
            return _context.Scheme.Any(e => e.SchemaId == id);
        }
    }
}
