using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogicSchemeManager.Models
{
	public class CombinationPort
	{
		public int CombinationPortId
		{
			get; set;
		}
		public int CombinationId
		{
			get; set;
		}
		public Combination Combination
		{
			get; set;
		}
		public int PortId
		{
			get; set;
		}
		public Port Port
		{
			get; set;
		}
		public bool Value
		{
			get; set;
		}
	}
}
