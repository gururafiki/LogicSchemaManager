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
    public class CombinationPortsController : Controller
    {
        private readonly LogicSchemeManagerContext _context;

        public CombinationPortsController(LogicSchemeManagerContext context)
        {
            _context = context;
        }

        // GET: CombinationPorts
        public async Task<IActionResult> Index()
        {
            var logicSchemeManagerContext = _context.CombinationPorts.Include(c => c.Combination).Include(c => c.Port);
            return View(await logicSchemeManagerContext.ToListAsync());
        }

        // GET: CombinationPorts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var combinationPort = await _context.CombinationPorts
                .Include(c => c.Combination)
                .Include(c => c.Port)
                .FirstOrDefaultAsync(m => m.CombinationPortId == id);
            if (combinationPort == null)
            {
                return NotFound();
            }

            return View(combinationPort);
        }

        // GET: CombinationPorts/Create
        public IActionResult Create()
        {
            ViewData["CombinationId"] = new SelectList(_context.Combinations, "CombinationId", "Name");
            ViewData["PortId"] = new SelectList(_context.Ports, "PortId", "Name");
            return View();
        }

        // POST: CombinationPorts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CombinationPortId,CombinationId,PortId,Value")] CombinationPort combinationPort)
        {
            if (ModelState.IsValid)
            {
                _context.Add(combinationPort);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CombinationId"] = new SelectList(_context.Combinations, "CombinationId", "Name", combinationPort.CombinationId);
            ViewData["PortId"] = new SelectList(_context.Ports, "PortId", "Name", combinationPort.PortId);
            return View(combinationPort);
        }

        // GET: CombinationPorts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var combinationPort = await _context.CombinationPorts.FindAsync(id);
            if (combinationPort == null)
            {
                return NotFound();
            }
            ViewData["CombinationId"] = new SelectList(_context.Combinations, "CombinationId", "Name", combinationPort.CombinationId);
            ViewData["PortId"] = new SelectList(_context.Ports, "PortId", "Name", combinationPort.PortId);
            return View(combinationPort);
        }

        // POST: CombinationPorts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CombinationPortId,CombinationId,PortId,Value")] CombinationPort combinationPort)
        {
            if (id != combinationPort.CombinationPortId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(combinationPort);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CombinationPortExists(combinationPort.CombinationPortId))
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
            ViewData["CombinationId"] = new SelectList(_context.Combinations, "CombinationId", "Name", combinationPort.CombinationId);
            ViewData["PortId"] = new SelectList(_context.Ports, "PortId", "Name", combinationPort.PortId);
            return View(combinationPort);
        }

        // GET: CombinationPorts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var combinationPort = await _context.CombinationPorts
                .Include(c => c.Combination)
                .Include(c => c.Port)
                .FirstOrDefaultAsync(m => m.CombinationPortId == id);
            if (combinationPort == null)
            {
                return NotFound();
            }

            return View(combinationPort);
        }

        // POST: CombinationPorts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var combinationPort = await _context.CombinationPorts.FindAsync(id);
            _context.CombinationPorts.Remove(combinationPort);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CombinationPortExists(int id)
        {
            return _context.CombinationPorts.Any(e => e.CombinationPortId == id);
        }
    }
}
