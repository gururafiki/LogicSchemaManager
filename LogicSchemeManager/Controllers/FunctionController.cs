using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LogicSchemeManager.Models;

namespace LogicSchemeManager.Controllers
{
    [Route("api/function")]
    [ApiController]
    public class FunctionController : ControllerBase
    {
        private readonly LogicSchemeManagerContext _context;

        public FunctionController(LogicSchemeManagerContext context)
        {
            _context = context;
        }

        // GET: api/Function
        [HttpGet]
        public IEnumerable<Schema> GetScheme()
        {
            return _context.Scheme;
        }
		
		// GET: api/Function/5
		[HttpGet("{id}/{skipInternalValues}")]
        public async Task<IActionResult> GetTruthTable([FromRoute] int id, [FromRoute] bool skipInternalValues)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var schema = await _context.Scheme
				.Include(sch => sch.Elements)
				.ThenInclude(e => e.ElementPorts)
				.ThenInclude(ep => ep.Port)
				.FirstOrDefaultAsync(sch => sch.SchemaId == id);

            if (schema == null)
            {
                return NotFound();
            }

			var truthTable = schema.GetTruthTable(_context);
			
			return Ok(new TruthTableView(truthTable, skipInternalValues));
        }
		
        // PUT: api/Function/5
        [HttpPut("{id}")]
        public async Task<IActionResult> GetElementOutput([FromRoute] int id, [FromBody] Dictionary<int,bool> values)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
			
			var element = await _context.Elements
				.Include(e => e.ElementPorts)
				.ThenInclude(ep => ep.Port)
				.FirstOrDefaultAsync(e => e.ElementId == id);
			
			if (element == null) {
				return NotFound();
			}

			Dictionary<ElementPort, bool> inputValues = new Dictionary<ElementPort, bool>();
			
			element.ElementPorts.Where(ep => !ep.Port.IsOutput).ToList().ForEach(ep => {
				inputValues[ep] = values[ep.PortId];
			});

			var outputValues = element.GetElementOutput(_context, inputValues);

			return Ok(outputValues);
        }
		
        // POST: api/Function
        [HttpPost("{id}/{key}/{value}")]
        public async Task<IActionResult> GetTestVectors([FromRoute] int id, [FromRoute] int key, [FromRoute] bool value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
			

			var schema = await _context.Scheme
				.Include(sch => sch.Elements)
				.ThenInclude(e => e.ElementPorts)
				.ThenInclude(ep => ep.Port)
				.FirstOrDefaultAsync(sch => sch.SchemaId == id);

			if (schema == null) {
				return NotFound();
			}
			KeyValuePair<int, bool> error = new KeyValuePair<int, bool>(key, value);

			var truthTable = schema.GetErrorTruthTable(_context, error);

			return Ok(new TruthTableView(truthTable, true));
		}
		/*
        // DELETE: api/Function/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSchema([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var schema = await _context.Scheme.FindAsync(id);
            if (schema == null)
            {
                return NotFound();
            }

            _context.Scheme.Remove(schema);
            await _context.SaveChangesAsync();

            return Ok(schema);
        }

        private bool SchemaExists(int id)
        {
            return _context.Scheme.Any(e => e.SchemaId == id);
        }
		*/
    }
}