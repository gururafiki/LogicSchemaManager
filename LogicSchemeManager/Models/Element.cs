using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LogicSchemeManager.Models
{
	public class Element
	{
		public int ElementId
		{
			get;
			set;
		}
		public string Name
		{
			get;
			set;
		}
		public int SchemaId
		{
			get; set;
		}
		public Schema Schema
		{
			get; set;
		}
		public int ElementTypeId
		{
			get; set;
		}
		public ElementType ElementType
		{
			get; set;
		}
		public double? x
		{
			get; set;
		}
		public double? y
		{
			get; set;
		}
		public List<ElementPort> ElementPorts
		{
			get; set;
		}
		public Element() {
			ElementPorts = new List<ElementPort>();
		}

		public Dictionary<ElementPort, bool> GetElementOutput(LogicSchemeManagerContext context, Dictionary<ElementPort, bool> inputValues) {
			var outputValues = new Dictionary<ElementPort, bool>();
			
			//ToDo: check if ports exists

			var combination = context.Combinations
				.Include(c => c.CombinationPorts)
				.ThenInclude(cp => cp.Port)
				.Where(c => c.ElementTypeId == ElementTypeId
						&& !c.CombinationPorts
								.Where(cp => !cp.Port.IsOutput)
								.Any(cp => ElementPorts
												.Where(ep => !ep.Port.IsOutput)
												.Any(input => input.PortId == cp.PortId && inputValues[input] != cp.Value))
				).FirstOrDefault();
			
			if (combination == default(Combination)) {
				return null;
			}

			ElementPorts.Where(ep => ep.ElementId == ElementId && ep.Port.IsOutput).ToList().ForEach(ep => {
				outputValues[ep] = combination.CombinationPorts.FirstOrDefault(cp => cp.Port.IsOutput && cp.PortId == ep.PortId).Value;
			});
			
			return outputValues;
		}

	}
}
