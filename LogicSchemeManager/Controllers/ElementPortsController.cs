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
    public class ElementPortsController : Controller
    {
        private readonly LogicSchemeManagerContext _context;

        public ElementPortsController(LogicSchemeManagerContext context)
        {
            _context = context;
        }

        // GET: ElementPorts
        public async Task<IActionResult> Index()
        {
            var logicSchemeManagerContext = _context.ElementPorts.Include(e => e.Element).Include(e => e.Parent).Include(e => e.Port);
            return View(await logicSchemeManagerContext.ToListAsync());
        }

        // GET: ElementPorts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var elementPort = await _context.ElementPorts
                .Include(e => e.Element)
                .Include(e => e.Parent)
                .Include(e => e.Port)
                .FirstOrDefaultAsync(m => m.ElementPortId == id);
            if (elementPort == null)
            {
                return NotFound();
            }

            return View(elementPort);
        }

        // GET: ElementPorts/Create
        public IActionResult Create()
        {
            ViewData["ElementId"] = new SelectList(_context.Elements, "ElementId", "Name");
            ViewData["ParentId"] = new SelectList(_context.ElementPorts.Where(ep => ep.Port.IsOutput).ToList(), "ElementPortId", "Name");
            ViewData["PortId"] = new SelectList(_context.Ports, "PortId", "Name");
            return View();
        }

        // POST: ElementPorts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ElementPortId,ElementId,ParentId,PortId,Name")] ElementPort elementPort)
        {
            if (ModelState.IsValid)
            {
                _context.Add(elementPort);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ElementId"] = new SelectList(_context.Elements, "ElementId", "Name", elementPort.ElementId);
            ViewData["ParentId"] = new SelectList(_context.ElementPorts.Where(ep => ep.Port.IsOutput).ToList(), "ElementPortId", "Name", elementPort.ParentId);
            ViewData["PortId"] = new SelectList(_context.Ports, "PortId", "Name", elementPort.PortId);
            return View(elementPort);
        }

        // GET: ElementPorts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var elementPort = await _context.ElementPorts.FindAsync(id);
            if (elementPort == null)
            {
                return NotFound();
            }
            ViewData["ElementId"] = new SelectList(_context.Elements, "ElementId", "Name", elementPort.ElementId);
            ViewData["ParentId"] = new SelectList(_context.ElementPorts.Where(ep => ep.Port.IsOutput).ToList(), "ElementPortId", "Name", elementPort.ParentId);
            ViewData["PortId"] = new SelectList(_context.Ports, "PortId", "Name", elementPort.PortId);
            return View(elementPort);
        }

        // POST: ElementPorts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ElementPortId,ElementId,ParentId,PortId,Name")] ElementPort elementPort)
        {
            if (id != elementPort.ElementPortId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(elementPort);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ElementPortExists(elementPort.ElementPortId))
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
            ViewData["ElementId"] = new SelectList(_context.Elements, "ElementId", "Name", elementPort.ElementId);
            ViewData["ParentId"] = new SelectList(_context.ElementPorts.Where(ep => ep.Port.IsOutput).ToList(), "ElementPortId", "Name", elementPort.ParentId);
            ViewData["PortId"] = new SelectList(_context.Ports, "PortId", "Name", elementPort.PortId);
            return View(elementPort);
        }

        // GET: ElementPorts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var elementPort = await _context.ElementPorts
                .Include(e => e.Element)
                .Include(e => e.Parent)
                .Include(e => e.Port)
                .FirstOrDefaultAsync(m => m.ElementPortId == id);
            if (elementPort == null)
            {
                return NotFound();
            }

            return View(elementPort);
        }

        // POST: ElementPorts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var elementPort = await _context.ElementPorts.FindAsync(id);
            _context.ElementPorts.Remove(elementPort);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ElementPortExists(int id)
        {
            return _context.ElementPorts.Any(e => e.ElementPortId == id);
        }
    }
}
