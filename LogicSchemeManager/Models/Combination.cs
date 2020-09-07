using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogicSchemeManager.Models
{
	public class Combination
	{
		public int CombinationId
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
		public string Name
		{
			get; set;
		}

		public List<CombinationPort> CombinationPorts
		{
			get; set;
		}
		public Combination() {
			CombinationPorts = new List<CombinationPort>();
		}
	}
}
