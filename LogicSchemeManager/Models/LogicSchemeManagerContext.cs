using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogicSchemeManager.Models
{
	public class LogicSchemeManagerContext : DbContext
	{
		public LogicSchemeManagerContext(DbContextOptions<LogicSchemeManagerContext> options) : base(options) {
			Database.EnsureCreated();
		}
		
		public LogicSchemeManagerContext() : base() {

		}
		public DbSet<Port> Ports
		{
			get; set;
		}
		public DbSet<ElementType> ElementTypes
		{
			get; set;
		}
		public DbSet<Schema> Scheme
		{
			get; set;
		}
		public DbSet<ElementPort> ElementPorts
		{
			get; set;
		}
		public DbSet<Combination> Combinations
		{
			get; set;
		}
		public DbSet<Element> Elements
		{
			get; set;
		}
		public DbSet<CombinationPort> CombinationPorts
		{
			get; set;
		}
	}
}
