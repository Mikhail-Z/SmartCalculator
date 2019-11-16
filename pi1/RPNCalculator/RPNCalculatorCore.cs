using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace pi1
{
	public partial class RPNCalculator : ICalculator
	{
		private readonly Logger _logger = NLog.LogManager.GetCurrentClassLogger();
		private static readonly IReadOnlyDictionary<string, int> _operationPriorities
			= new ReadOnlyDictionary<string, int>(
				new Dictionary<string, int>
				{
					["+"] = 1,
					["-"] = 1,
					["*"] = 2,
					["/"] = 2,
					["^"] = 3
				});
		List<string> tokensInRPNOrder;
		private Stack<string> _operations;
		private int currentPriority;

		public RPNCalculator()
		{
			tokensInRPNOrder = new List<string>();
			_operations = new Stack<string>();
			currentPriority = 0;
		}

		public double Calculate(string s)
		{
			var tokens = Tokenize(s);
			tokens.Reverse();
			var tree = BuildBinaryTree(tokens);
			return tree.Calculate();
		}
	}
}
