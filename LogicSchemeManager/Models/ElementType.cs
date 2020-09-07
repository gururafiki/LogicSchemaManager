using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogicSchemeManager.Models
{
	public class ElementType
	{
		public int ElementTypeId
		{
			get; set;
		}
		public string Name
		{
			get; set;
		}
		public List<Element> Elements
		{
			get; set;
		}
		public List<Combination> Combinations
		{
			get; set;
		}
		public ElementType() {
			Elements = new List<Element>();
			Combinations = new List<Combination>();
		}
	}
}
