using System.Text;
using aocTools;
using day14;

var input = Helper.ReadFile("input.txt").Split("\n\n")[0];

var operationsString = Helper.ReadFile("input.txt").Split("\n\n")[1].Split('\n');

Polymer.CreateOperations(operationsString);
Polymer.ReadPolymers(input);
Polymer.ProcessPolymers(40);
Polymer.GetResult();

