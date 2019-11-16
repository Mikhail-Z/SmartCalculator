using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using pi1.Utils;
using System.Diagnostics.CodeAnalysis;

namespace pi1
{
	public partial class DALCalculator
	{
		public IList<string> Tokenize([NotNull] string expression)
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
				else if (mayBeDot && symb == '.')
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
					tokens.Add(currentOperand);
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
			
			return tokens;
		}

		private bool Validate(string expression)
		{
			return ValidateSymbols(expression) &&
				ValidateBrackets(expression) &&
				expression.Length > 0;
		}

		private bool ValidateSymbols(string s)
		{
			var possibleSymbols = _operationPriorities.Keys.ToList().
				ToCharList();
			possibleSymbols.AddRange(new List<char> { 
				'(', ')', '.', 
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

		private bool IsDigit(char c)
		{
			return c >= '0' && c <= '9';
		}

		private bool IsOperation(char c)
		{
			return _operationPriorities.Keys.Contains(c.ToString());
		}

		private bool IsOperation(string s)
		{
			return _operationPriorities.Keys.ToList().Contains(s);
		}

		private bool IsOperand(string s)
		{
			return _operationPriorities.Keys.ToList().Contains(s) == false;
		}

		private double EvaluateOperation(string operation, double operand1, double operand2)
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

		private bool IsLeftBracket(string s)
		{
			return s == "(";
		}

		private bool IsRightBracket(string s)
		{
			return s == ")";
		}

		private double CalculateExpressionInBrackets()
		{
			double curNumber = Double.Parse(operands.Pop());
			double operand1;
			double operand2;
			string curOperation;

			do
			{
				operand2 = curNumber;
				curOperation = operations.Pop();
				operand1 = Double.Parse(operands.Pop());

				curNumber = EvaluateOperation(curOperation, operand1, operand2);
			}
			while (operations.Count > 0 && operands.Count > 0 && operands.Peek() != "(");
			
			operands.Pop();
			
			return curNumber;
		}

		private double CalculateExpressionTillLessPriorityOperation(int targetPriority)
		{
			double curNumber = Double.Parse(operands.Pop());
			double operand1;
			double operand2;
			string curOperation;

			do
			{
				operand2 = curNumber;
				curOperation = operations.Pop();
				operand1 = Double.Parse(operands.Pop());

				curNumber = EvaluateOperation(curOperation, operand1, operand2);
			}
			while (operands.Count > 0 && operands.Peek() != "(" && operations.Count > 0 && _operationPriorities[operations.Peek()] > targetPriority);

			return curNumber;
		}

		private double CalculateRemainedExpression()
		{
			double curNumber = Double.Parse(operands.Pop());
			double operand1;
			string operand1Str;
			double operand2;
			string operand2Str;
			string curOperation;

			while (operations.Count > 0)
			{
				curOperation = operations.Pop();
				operand2 = Double.Parse(operands.Pop());
				operand1 = Double.Parse(operands.Pop());
				curNumber = EvaluateOperation(curOperation, operand1, operand2);
			}

			return curNumber;
		}
	}
}
