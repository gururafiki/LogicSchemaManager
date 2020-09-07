using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LogicSchemeManager.Utilities;

namespace LogicSchemeManager.Models
{
	public class TruthTable {
		public List<TruthTableRow> Rows = new List<TruthTableRow>();
		public TruthTable() {

		}

		public bool GetIsMinimal() {
			bool isMinimal = true;
			
			var singleOutputRows = Rows.SelectMany(row => row.Outputs
					.Where(output => output.Value)
					.Select(output => new SingleOutputTruthTableRow() {
						OutputPort = output.Key,
						Inputs = row.Inputs
					}).ToList()).ToList();

			//isMinimal = rows.Any(row => );

			/* для каждой комбинации входов в группе 
				* найти другую комбинацию входов, 
				* где при исключении повторяющихся елементов
				* остаются только пары елементов с одинаковыми портами 
			*/
			foreach (var groupedSingleOutputRows in singleOutputRows.GroupBy(row => row.OutputPort)) {
				var combinationsInputs = groupedSingleOutputRows.ToList()
					.Select(combination => combination.Inputs).ToList();

				combinationsInputs.ForEach(combinationInputs => {
					combinationsInputs.FindAll(checkedCombinationInputs => !checkedCombinationInputs.Equals(combinationInputs)).ForEach(checkedCombinationInputs => {
						var differentInputs = checkedCombinationInputs.Where(checkedInput => checkedCombinationInputs[checkedInput.Key] != checkedInput.Value);
						//differentInputs.Where(differentInput => differentInput.);
					});
				});
			}
			//.Any(group => group.Any(input => input) );
			/*
			 * ElementPort:
			 * [
				 * outputValue: inputsValue
				 * outputValue: inputsValue
				 * outputValue: inputsValue
			 * ]
			 */

			return isMinimal;
		}
		internal class SingleOutputTruthTableRow {
			public ElementPort OutputPort
			{
				get; set;
			}
			public Dictionary<ElementPort, bool> Inputs
			{
				get;set;
			}
		}
	}

	public class TruthTableRow
	{
		public Dictionary<ElementPort, bool> Inputs
		{
			get; set;
		}

		public Dictionary<ElementPort, bool> Outputs
		{
			get; set;
		}

		public TruthTableRow(Dictionary<ElementPort, bool> inputPorts, Dictionary<ElementPort, bool> outputPorts) {
			Inputs = inputPorts;
			Outputs = outputPorts;
		}
	}

	public class TruthTableView
	{
		public List<TruthTableRowView> Rows = new List<TruthTableRowView>();
		public TruthTableView(TruthTable truthTable, bool skipInternalValues) {
			truthTable.Rows.ForEach(row => {
				Rows.Add(new TruthTableRowView(row, skipInternalValues));
			});
		}
	}

	public class TruthTableRowView
	{
		public Dictionary<ElementPort, int> Inputs = new Dictionary<ElementPort, int>();
		public Dictionary<ElementPort, int> Outputs = new Dictionary<ElementPort, int>();
		public TruthTableRowView(TruthTableRow truthTableRow, bool skipInternalValues) {
			foreach (var input in truthTableRow.Inputs) {
				if(!skipInternalValues || input.Key.Parent == null)
					Inputs.Add(input.Key, input.Value ? 1 : 0);
			}

			foreach (var output in truthTableRow.Outputs) {
				if (!skipInternalValues || !output.Key.Element.Schema.Elements.SelectMany(el => el.ElementPorts).Any(ep => ep.ParentId == output.Key.ElementPortId))
					Outputs.Add(output.Key, output.Value ? 1 : 0);

			}
		}
	}
}

