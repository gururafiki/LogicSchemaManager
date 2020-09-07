using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogicSchemeManager.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LogicSchemeManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VisController : ControllerBase
	{
		private readonly LogicSchemeManagerContext _context;

		public VisController(LogicSchemeManagerContext context) {
			_context = context;
		}
		
		[HttpGet("{schemaid}")]
		public VisSchema GetVisElementsData(int schemaid) {
			var nodes = new List<VisNode>();
			_context.Elements
						.Where(element => element.SchemaId == schemaid)
						.Include(element => element.ElementType)
						.ToList().ForEach(element => {
							if (element.x.HasValue && element.y.HasValue) {
								nodes.Add(new VisNodeWithPositions {
									id = element.ElementId,
									label = $"{element.ElementType.Name}\n\n{element.Name}",
									x = element.x.Value,
									y = element.y.Value
								});
							} else {
								nodes.Add(new VisNode {
									id = element.ElementId,
									label = $"{element.ElementType.Name}\n\n{element.Name}"
								});
							}
						});

			int idCounter = nodes.Max(node => node.id);

			List<VisPortNode> visPorts = new List<VisPortNode>();

			_context.ElementPorts.Include(elementPort => elementPort.Port)
						.Where(elementPort => elementPort.Element.SchemaId == schemaid
												&& ((elementPort.Port.IsOutput
														&& !_context.ElementPorts.Any(checkedPort => checkedPort.ParentId == elementPort.ElementPortId)
															|| (!elementPort.Port.IsOutput && !elementPort.ParentId.HasValue)
													)) 
												)
						.ToList()
						.ForEach(elementPort => {
							if (elementPort.x.HasValue && elementPort.y.HasValue) {
								visPorts.Add(new VisPortNodeWithPositions {
									id = ++idCounter,
									label = $"{elementPort.Name}",
									connectedTo = elementPort.ElementId,
									elementPortId = elementPort.ElementPortId,
									isOutput = elementPort.Port.IsOutput,
									x = elementPort.x.Value,
									y = elementPort.y.Value
								});
							} else {
								visPorts.Add(new VisPortNode{
									id = ++idCounter,
									label = $"{elementPort.Name}",
									connectedTo = elementPort.ElementId,
									elementPortId = elementPort.ElementPortId,
									isOutput = elementPort.Port.IsOutput
								});
							}

						});

			nodes.AddRange(visPorts);

			var edges = _context.ElementPorts
									.Where(elementPort => elementPort.Element.SchemaId == schemaid && elementPort.ParentId.HasValue)
									.Select(elementPort => new VisEdge {
										id = $"{elementPort.Name}_{elementPort.Parent.Name}",
										label = $"{elementPort.Parent.Name} => {elementPort.Name}",
										to = elementPort.ElementId,
										from = elementPort.Parent.ElementId
									}).ToList();

			edges.AddRange(visPorts.Select(visPort => new VisEdge {
												id = $"{visPort.label}",
												label = visPort.label,
												to = visPort.isOutput ? visPort.id : visPort.connectedTo,
												from = visPort.isOutput ? visPort.connectedTo : visPort.id
			}).ToList());


			return new VisSchema {
				nodes = nodes,
				edges = edges
			};
		}

		//{"1":{"x":34,"y":151},"2":{"x":-78,"y":-141},"3":{"x":0,"y":-7},"4":{"x":132,"y":-85},"5":{"x":-111,"y":257},"6":{"x":141,"y":272},"7":{"x":-261,"y":-149},"8":{"x":-102,"y":-289},"9":{"x":260,"y":-254}}

		// PUT: api/vis/5
		[HttpPut("{schemaid}")]
		public async Task<IActionResult> SavePositions([FromRoute] int schemaid, [FromBody] List<VisPosition> positions) {
			if (!ModelState.IsValid) {
				return BadRequest(ModelState);
			}

			var elements = await _context.Elements
				.Include(e => e.ElementPorts)
				.ThenInclude(ep => ep.Port)
				.Where(e => e.SchemaId == schemaid).ToListAsync();

			positions.ForEach(position => {
				if (position.isPort) {
					var elementPort = elements.SelectMany(e => e.ElementPorts).Where(ep => ep.ElementPortId == position.itemId).FirstOrDefault();
					elementPort.x = position.x;
					elementPort.y = position.y;

				} else {
					var element = elements.Where(e => e.ElementId == position.itemId).FirstOrDefault();
					element.x = position.x;
					element.y = position.y;
				}
			});

			await _context.SaveChangesAsync();

			return Ok();
		}

		public class VisPosition {
			public double x
			{
				get;
				set;
			}
			public double y
			{
				get;set;
			}
			public bool isPort
			{
				get;set;
			}
			public int itemId
			{
				get;set;
			}
		}

		public class VisSchema
		{
			public List<VisNode> nodes
			{
				get;
				set;
			}
			public List<VisEdge> edges
			{
				get;
				set;
			}
		}

		public class VisEdge
		{
			public string id
			{
				get; set;
			}
			public string label
			{
				get;set;
			}
			public int from
			{
				get; set;
			}
			public int to
			{
				get; set;
			}
		}
		
		public class VisNode
		{
			public int id
			{
				get; set;
			}
			public string label
			{
				get; set;
			}
		}

		public class VisPortNode : VisNode
		{
			public int size = 20;
			public bool isOutput
			{
				get;
				set;
			}
			public int connectedTo
			{
				get;set;
			}
			public int elementPortId
			{
				get;
				set;
			}
		}

		public class VisPortNodeWithPositions : VisPortNode
		{
			public double x
			{
				get; set;
			}
			public double y
			{
				get; set;
			}
		}

		public class VisNodeWithPositions : VisNode
		{
			public double x
			{
				get; set;
			}
			public double y
			{
				get; set;
			}
		}
	}
}