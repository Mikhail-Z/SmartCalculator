using pi1.Entities;
using pi1.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace pi1
{
	partial class RPNCalculator : ICalculator
	{
		private class BinaryTree
		{
			private Item _root;
			private RPNCalculator _calculator;
			public int Count { private set; get; } = 0;

			public BinaryTree(RPNCalculator calculator)
			{
				_calculator = calculator;
			}

			public void Add(string value)
			{
				if (_root == null)
				{
					_root = new Item(value);
				}
				else
				{
					Add(_root, value);
				}

				Count++;
			}

			public double Calculate()
			{
				if (Count == 0)
				{
					return 0;
				}

				return Calculate(_root);
			}

			private double Calculate(Item curItem)
			{
				if (IsOperation(curItem.Value))
				{
					return EvaluateOperation(curItem.Value, Calculate(curItem.Left), Calculate(curItem.Right));
				}
				else
				{
					return Double.Parse(curItem.Value);
				}
			}

			private bool Add(Item curItem, string value)
			{
				if (curItem.Right != null)
				{
					if (RPNCalculator.IsOperand(curItem.Right.Value))
					{
						if (curItem.Left == null)
						{
							curItem.Left = new Item(value);
							return true;
						}
						else
						{
							if (RPNCalculator.IsOperand(curItem.Left.Value))
							{
								return false;
							}
							else
							{
								return Add(curItem.Left, value);
							}
						}
					}
					else
					{
						var result = Add(curItem.Right, value);
						if (result == false)
						{
							if (curItem.Left != null)
							{
								if (RPNCalculator.IsOperand(curItem.Left.Value))
								{
									return false;
									
								}
								else
								{
									return Add(curItem.Left, value);
								}
								
							}
							else
							{
								curItem.Left = new Item(value);
								return true;
							}
						}
						else
						{
							return true;
						}
					}
				}
				else
				{
					curItem.Right = new Item(value);
					return true;
				}
			}

			private class Item
			{
				public Item(string value)
				{
					Value = value;
				}

				public string Value { private set; get; }
				public Item Left { get; set; }
				public Item Right { get; set; }
			}
		}

		private BinaryTree BuildBinaryTree(IList<string> tokens)
		{
			var tree = new BinaryTree(this);
			foreach (var token in tokens)
			{
				tree.Add(token);
			}

			return tree;
		}

		public List<string> Tokenize([NotNull] string expression)
		{
			var regex = new Regex(@"\s+");
			regex.Replace(expression, "");
			if (Validate(expression) == false)
			{
				throw new ArgumentException("Найдены недопустимые символы или кол-во отрывающихся скобок не равно кол-ву закрывающихся скобок");
			}

			string currentOperand = "";
			bool mayBeDigit = true;
			bool mayBeDot = true;
			bool mayBeOperation = false;
			bool mayBeLeftBracket = true;
			bool mayBeRightBracket = false;
			IList<string> tokens = new List<string>();

			int i = 0;
			foreach (var symb in expression)
			{
				if (mayBeDigit && IsDigit(symb))
				{
					currentOperand += symb;
					mayBeOperation = true;
					mayBeDot = true;
					mayBeLeftBracket = false;
					mayBeRightBracket = true;
				}
				else if (mayBeDot && symb == ',')
				{
					mayBeDot = false;
					currentOperand += symb;
				}
				else if (mayBeLeftBracket && symb == '(')
				{
					tokens.Add(symb.ToString());
					mayBeRightBracket = false;
				}
				else if (mayBeRightBracket && symb == ')')
				{
					if (currentOperand != "")
					{
						tokens.Add(currentOperand);
						currentOperand = "";
					}

					tokens.Add(symb.ToString());
					mayBeLeftBracket = false;
				}
				else if (mayBeOperation && IsOperation(symb))
				{
					mayBeOperation = false;
					mayBeDot = true;
					if (currentOperand != "")
					{
						tokens.Add(currentOperand);
					}
					currentOperand = "";
					tokens.Add(symb.ToString());
					mayBeLeftBracket = true;
					mayBeRightBracket = false;
				}
				else
				{
					throw new ArgumentException($"Неверный символ {symb} на {i}-ой позиции");
				}

				i++;
			}

			if (currentOperand == "" && !expression.EndsWith(")"))
			{
				throw new ArgumentException($"В конце выражения найдена операция {expression.Last()}, а не операнд или закрывающаяся скобка");
			}

			if (currentOperand != "")
			{
				tokens.Add(currentOperand);
			}

			return ChangeTokensOrderToRPN(tokens);
		}

		internal List<string> ChangeTokensOrderToRPN(IList<string> tokens)
		{
			var tokensRPNOrder = new List<string>();

			foreach (var token in tokens)
			{
				if (IsOperation(token))
				{
					if (_operationPriorities[token] <= currentPriority && _operations.Count > 0)
					{
						var targetPriority = _operationPriorities[token];
						var lastOperationInStack = _operations.Peek();

						while (lastOperationInStack != "(" && _operationPriorities[lastOperationInStack] >= targetPriority)
						{
							_operations.Pop();

							tokensInRPNOrder.Add(lastOperationInStack);
							if (_operations.Count == 0)
							{
								break;
							}

							lastOperationInStack = _operations.Peek();
						}

						_operations.Push(token);
					}
					else
					{
						_operations.Push(token);
					}

					currentPriority = _operationPriorities[token];
				}
				else if (IsOperand(token))
				{
					tokensInRPNOrder.Add(token);
				}
				else if (IsLeftBracket(token))
				{
					_operations.Push(token);
					currentPriority = 0;
				}
				else if (IsRightBracket(token))
				{
					string lastOperationInStack = _operations.Pop();
					while (lastOperationInStack != "(")
					{
						tokensInRPNOrder.Add(lastOperationInStack);
						if (_operations.Count() == 0)
						{
							break;
						}

						lastOperationInStack = _operations.Pop();
					}
					if (_operations.Count > 0)
					{
						currentPriority = _operationPriorities[_operations.Peek()];
					}
					else
					{
						currentPriority = 0;
					}
				}
			}

			while (_operations.Count() > 0)
			{
				tokensInRPNOrder.Add(_operations.Pop());
			}

			return tokensInRPNOrder;
		}

		private bool Validate(string expression)
		{
			return ValidateSymbols(expression) &&
				ValidateBrackets(expression) &&
				expression.Length > 0;
		}

		private bool ValidateSymbols(string s)
		{
			var possibleSymbols = _operationPriorities.Keys.ToList().ToCharList();
			possibleSymbols.AddRange(new List<char> {
				'(', ')', ',',
				'0', '1', '2', '3', '4', '5', '6', '7', '8', '9'
			});

			foreach (var symb in s)
			{
				if (possibleSymbols.Contains(symb) == false)
				{
					return false;
				}
			}

			return true;
		}

		private bool ValidateBrackets(string s)
		{
			var unclosedLestBracketsCount = 0;
			foreach (var symb in s)
			{
				if (symb == '(')
				{
					unclosedLestBracketsCount++;
				}
				else if (symb == ')')
				{
					unclosedLestBracketsCount--;
				}
				if (unclosedLestBracketsCount < 0)
				{
					return false;
				}
			}

			if (unclosedLestBracketsCount != 0)
			{
				return false;
			}

			return true;
		}

		private static bool IsDigit(char c)
		{
			return c >= '0' && c <= '9';
		}

		private static bool IsOperation(char c)
		{
			return _operationPriorities.Keys.Contains(c.ToString());
		}

		private static bool IsOperation(string s)
		{
			return _operationPriorities.Keys.ToList().Contains(s);
		}

		private static bool IsOperand(string s)
		{
			return _operationPriorities.Keys.ToList().Contains(s) == false 
				&& IsLeftBracket(s) == false 
				&& IsRightBracket(s) == false;
		}

		private static double EvaluateOperation(string operation, double operand1, double operand2)
		{
			if (operation == "+")
			{
				return operand1 + operand2;
			}
			else if (operation == "-")
			{
				return operand1 - operand2;
			}
			else if (operation == "*")
			{
				return operand1 * operand2;
			}
			else if (operation == "/")
			{
				return operand1 / operand2;
			}
			else if (operation == "^")
			{
				return Math.Pow(operand1, operand2);
			}
			else
			{
				throw new ArgumentException("Несуществующая операция");
			}
		}

		private static bool IsLeftBracket(string s)
		{
			return s == "(";
		}

		private static bool IsRightBracket(string s)
		{
			return s == ")";
		}
	}
}
