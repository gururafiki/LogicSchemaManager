using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LogicSchemeManager.Models
{
    public class Schema
    {
        public int SchemaId { get; set; }
        public string Name { get; set; }
        public List<Element> Elements { get; set; }
        public Schema()
        {
            Elements = new List<Element>();
        }

		public TruthTable GetTruthTable(LogicSchemeManagerContext context) {
			var truthTable = new TruthTable();//new Dictionary<Dictionary<ElementPort, bool>, Dictionary<ElementPort, bool>>();
			GetAvailableInputCombinations().ForEach(availableInputCombination => {
				truthTable.Rows.Add(new TruthTableRow(availableInputCombination, GetSchemaOutput(context, availableInputCombination)));
			});

			return truthTable;
		}
		public TruthTable GetErrorTruthTable(LogicSchemeManagerContext context, KeyValuePair<int, bool> error) {
			var truthTable = new TruthTable();//new Dictionary<Dictionary<ElementPort, bool>, Dictionary<ElementPort, bool>>();
			GetAvailableInputCombinations().ForEach(availableInputCombination => {
				truthTable.Rows.Add(new TruthTableRow(availableInputCombination, GetSchemaOutputWithError(context, availableInputCombination, error)));
			});

			return truthTable;
		}
		public List<Dictionary<ElementPort, bool>> GetAvailableInputCombinations() {
			var inputPorts = Elements.SelectMany(e => e.ElementPorts)
				.Where(ep => !ep.Port.IsOutput && ep.Parent == null).ToList();

			List<Dictionary<ElementPort, bool>> availableInputCombinations = new List<Dictionary<ElementPort, bool>>();


			for (int i = 0; i < Math.Pow(2, inputPorts.Count); i++) {
				Dictionary<ElementPort, bool> availableInputCombination = new Dictionary<ElementPort, bool>();
				BitArray bitArray = new BitArray(BitConverter.GetBytes(i));
				var inputValues = bitArray.Cast<bool>().ToList();
				for(var j = 0; j < inputPorts.Count; j++) {
					availableInputCombination[inputPorts[j]] = inputValues[j];
				}
				availableInputCombinations.Add(availableInputCombination);
			}

			return availableInputCombinations;
		}


		public Dictionary<ElementPort, bool> GetSchemaOutputWithError(LogicSchemeManagerContext context, Dictionary<ElementPort, bool> inputValues, KeyValuePair<int,bool> error) {
			var outputValues = new Dictionary<ElementPort, bool>();
			var processedElements = new HashSet<int>();
			var processingNeeded = true;

			var errorValues = new Dictionary<ElementPort, bool>();
			foreach (var pair in inputValues) {
				errorValues.Add(pair.Key, pair.Value);
			}
			var errorInput = errorValues.FirstOrDefault(kvp => kvp.Key.ElementPortId == error.Key);
			if (!errorInput.Equals(default(KeyValuePair<ElementPort, bool>)))
				errorValues[errorInput.Key] = error.Value;

			while (processingNeeded) {
				processingNeeded = false;

				Elements.Where(element => !element.ElementPorts.Where(ep => !ep.Port.IsOutput).Any(ep => !errorValues.ContainsKey(ep)) && !processedElements.Contains(element.ElementId))
				.ToList()
				.ForEach(element => {

					var elementOutputValues = element.GetElementOutput(context, errorValues);
					foreach (var elementOutputValue in elementOutputValues) {
						//ToDo: create logic for output that connected to another element and also are outputs of scheme
						outputValues[elementOutputValue.Key] = elementOutputValue.Key.ElementPortId == error.Key ? error.Value : elementOutputValue.Value;

						Elements.SelectMany(e => e.ElementPorts)
						.Where(ep => ep.ParentId == elementOutputValue.Key.ElementPortId).ToList().ForEach(ep => {
							errorValues[ep] = outputValues[elementOutputValue.Key];
						});
					}
					processedElements.Add(element.ElementId);
					processingNeeded = true;
				});
			}


			return outputValues;
		}

		public Dictionary<ElementPort, bool> GetSchemaOutput(LogicSchemeManagerContext context, Dictionary<ElementPort, bool> inputValues) {
			var outputValues = new Dictionary<ElementPort, bool>();
			var processedElements = new HashSet<int>();
			var processingNeeded = true;

			while (processingNeeded) {
				processingNeeded = false;

				Elements.Where(element => !element.ElementPorts.Where(ep => !ep.Port.IsOutput).Any(ep => !inputValues.ContainsKey(ep)) && !processedElements.Contains(element.ElementId)) 
				.ToList()
				.ForEach(element => {
					var elementOutputValues = element.GetElementOutput(context, inputValues);
					foreach (var elementOutputValue in elementOutputValues) {
						//ToDo: create logic for output that connected to another element and also are outputs of scheme
						outputValues[elementOutputValue.Key] = elementOutputValue.Value;

						Elements.SelectMany(e => e.ElementPorts)
						.Where(ep => ep.ParentId == elementOutputValue.Key.ElementPortId).ToList().ForEach(ep => {
							inputValues[ep] = elementOutputValue.Value;
						});
					}
					processedElements.Add(element.ElementId);
					processingNeeded = true;
				});
			}
			

			return outputValues;
		}
	}
}
