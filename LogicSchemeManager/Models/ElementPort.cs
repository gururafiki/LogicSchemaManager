using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogicSchemeManager.Models
{
	public class ElementPort {
		public int ElementPortId
		{
			get; set;
		}
		public int ElementId
		{
			get; set;
		}
		public Element Element
		{
			get; set;
		}
		public int? ParentId
		{
			get; set;
		}
		public ElementPort Parent
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
		public string Name
		{
			get; set;
		}
		public double? x
		{
			get;set;
		}
		public double? y
		{
			get; set;
		}
		public override string ToString() {
			return Name;
		}
	}
}
