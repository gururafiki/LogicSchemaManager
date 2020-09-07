using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogicSchemeManager.Models
{
	public class Port
	{
		public int PortId
		{
			get; set;
		}
		public string Name
		{
			get; set;
		}
		public bool IsOutput
		{
			get; set;
		}
		public List<ElementPort> ElementPorts
		{
			get; set;
		}
		public List<CombinationPort> CombinationPorts
		{
			get; set;
		}
		public Port() {
			ElementPorts = new List<ElementPort>();
			CombinationPorts = new List<CombinationPort>();
		}
	}
}
