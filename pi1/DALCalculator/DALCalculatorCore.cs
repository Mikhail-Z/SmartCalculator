using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace pi1
{
	public partial class DALCalculator : ICalculator
	{
		private readonly ILogger<DALCalculator> _logger;
		private readonly IReadOnlyDictionary<string, int> _operationPriorities;
		private Stack<string> operands;
		private Stack<string> operations;
		private Stack<int> currentPriorities;
		private int currentPriority;

		public DALCalculator(ILogger<DALCalculator> logger)
		{
			_logger = logger;
			_operationPriorities = new ReadOnlyDictionary<string, int>(
				new Dictionary<string, int>
				{
					["+"] = 1,
					["-"] = 1,
					["*"] = 2,
					["/"] = 2,
					["^"] = 3
				});
			operands = new Stack<string>();
			operations = new Stack<string>();
			currentPriorities = new Stack<int>();
		}

		public double Calculate(string s)
		{
			var tokens = Tokenize(s);
			foreach (var token in tokens)
			{
				if (IsOperand(s))
				{
					operands.Push(Double.Parse(token).ToString());
				}
				else
				{
					if (IsLeftBracket(token))
					{
						operands.Push(token);
						currentPriorities.Push(currentPriority);
						currentPriority = 0;
					}
					else if (IsRightBracket(token))
					{
						operands.Push(CalculateExpressionInBrackets().ToString());
						currentPriority = currentPriorities.Pop();
					}
					else if (IsOperation(token))
					{
						if (_operationPriorities[token] >= currentPriority)
						{
							currentPriority = _operationPriorities[token];
							operations.Push(token);
						}
						else if (_operationPriorities[token] < currentPriority)
						{
							operands.Push(CalculateExpressionTillLessPriorityOperation(_operationPriorities[token]).ToString());
							operations.Push(token);
						}
					}
				}
			}

			var result = CalculateRemainedExpression();

			return result;
		}
	}
}
