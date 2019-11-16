using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace pi1.Tests
{
	public class RPNCalculatorTests
	{
		[Fact]
		public void Tokenize_SingleOperation_SholdReturnThreeTokens()
		{
			//arrange
			
			var calculator = new RPNCalculator();
			string[] expectedTokens = new[] { "4", "5", "+" };
			string input = "4+5";

			//act
			var actualTokens = calculator.Tokenize(input);

			//assert
			Assert.Equal(expectedTokens, actualTokens);
		}

		[Fact]
		public void Tokenize_TwoOperationsWithSamePriority_SholdReturnThreeTokens()
		{
			//arrange
			
			var calculator = new RPNCalculator();
			string[] expectedTokens = new[] { "4", "5", "+", "1", "-" };
			string input = "4+5-1";

			//act
			var actualTokens = calculator.Tokenize(input);

			//assert
			Assert.Equal(expectedTokens, actualTokens);
		}

		[Fact]
		public void Tokenize_TwoOperationsWithLargerPriorityInFirstOperation_SholdReturnSuccess()
		{
			//arrange
			
			var calculator = new RPNCalculator();
			string[] expectedTokens = new[] { "4", "5", "*", "1", "-" };
			string input = "4*5-1";

			//act
			var actualTokens = calculator.Tokenize(input);

			//assert
			Assert.Equal(expectedTokens, actualTokens);
		}

		[Fact]
		public void Tokenize_TwoOperationsWithLargerPriorityInSecondOperation_SholdReturnSuccess()
		{
			//arrange
			
			var calculator = new RPNCalculator();
			string[] expectedTokens = new[] { "4", "5", "2", "*", "+" };
			string input = "4+5*2";

			//act
			var actualTokens = calculator.Tokenize(input);

			//assert
			Assert.Equal(expectedTokens, actualTokens);
		}

		[Fact]
		public void Tokenize_TwoOperationsWithBrackets_SholdReturnSuccess()
		{
			//arrange
			
			var calculator = new RPNCalculator();
			string[] expectedTokens = new[] { "4", "5", "2", "+", "*" };
			string input = "4*(5+2)";

			//act
			var actualTokens = calculator.Tokenize(input);

			//assert
			Assert.Equal(expectedTokens, actualTokens);
		}

		[Fact]
		public void Tokenize_TwoOperationsInsideWithSamePriorityBrackets_SholdReturnSuccess()
		{
			//arrange
			
			var calculator = new RPNCalculator();
			string[] expectedTokens = new[] { "4", "5", "2", "+", "1", "-", "*" };
			string input = "4*(5+2-1)";

			//act
			var actualTokens = calculator.Tokenize(input);

			//assert
			Assert.Equal(expectedTokens, actualTokens);
		}

		[Fact]
		public void Tokenize_TwoOperationsInsideWithGreaterFirstPriorityInBrackets_SholdReturnSuccess()
		{
			//arrange
			
			var calculator = new RPNCalculator();
			string[] expectedTokens = new[] { "4", "5", "2", "*", "1", "-", "*" };
			string input = "4*(5*2-1)";

			//act
			var actualTokens = calculator.Tokenize(input);

			//assert
			Assert.Equal(expectedTokens, actualTokens);
		}

		[Fact]
		public void Tokenize_PowerOfTwoBracketGroups_SholdReturnSuccess()
		{
			//arrange
			
			var calculator = new RPNCalculator();
			string[] expectedTokens = new[] { "4", "2", "*", "2", "-", "7", "2", "2", "*", "-", "^" };
			string input = "(4*2-2)^(7-2*2)";

			//act
			var actualTokens = calculator.Tokenize(input);

			//assert
			Assert.Equal(expectedTokens, actualTokens);
		}



		//------------

		[Fact]
		public void Calculate_SingleOperation_SholdReturnThreeTokens()
		{
			//arrange
			
			var calculator = new RPNCalculator();
			double rightAnswer = 9;
			string input = "4+5";

			//act
			var answer = calculator.Calculate(input);

			//assert
			double eps = 0.000001;
			Assert.True(Math.Abs(rightAnswer - answer) < eps);
		}

		[Fact]
		public void Calculate_TwoOperationsWithSamePriority_SholdReturnThreeTokens()
		{
			//arrange
			
			var calculator = new RPNCalculator();
			double rightAnswer = 8;
			string input = "4+5-1";

			//act
			var answer = calculator.Calculate(input);

			//assert
			double eps = 0.000001;
			Assert.True(Math.Abs(rightAnswer - answer) < eps);
		}

		[Fact]
		public void Calculate_TwoOperationsWithLargerPriorityInFirstOperation_SholdReturnSuccess()
		{
			//arrange
			
			var calculator = new RPNCalculator();
			string input = "4*5-1";
			double rightAnswer = 19;

			//act
			var answer = calculator.Calculate(input);

			//assert
			double eps = 0.000001;
			Assert.True(Math.Abs(rightAnswer - answer) < eps);
		}

		[Fact]
		public void Calculate_TwoOperationsWithLargerPriorityInSecondOperation_SholdReturnSuccess()
		{
			//arrange
			
			var calculator = new RPNCalculator();
			string input = "4+5*2";
			double rightAnswer = 14;

			//act
			var answer = calculator.Calculate(input);

			//assert
			double eps = 0.000001;
			Assert.True(Math.Abs(rightAnswer - answer) < eps);
		}

		[Fact]
		public void Calculate_TwoOperationsWithBrackets_SholdReturnSuccess()
		{
			//arrange
			
			var calculator = new RPNCalculator();
			string input = "4*(5+2)";
			double rightAnswer = 28;

			//act
			var answer = calculator.Calculate(input);

			//assert
			double eps = 0.000001;
			Assert.True(Math.Abs(rightAnswer - answer) < eps);
		}

		[Fact]
		public void Calculate_TwoOperationsInsideWithSamePriorityBrackets_SholdReturnSuccess()
		{
			//arrange
			
			var calculator = new RPNCalculator();
			string input = "4*(5+2-1)";
			var rightAnswer = 24;

			//act
			var answer = calculator.Calculate(input);

			//assert
			double eps = 0.000001;
			Assert.True(Math.Abs(rightAnswer - answer) < eps);
		}

		[Fact]
		public void Calculate_TwoOperationsInsideWithGreaterFirstPriorityInBrackets_SholdReturnSuccess()
		{
			//arrange
			
			var calculator = new RPNCalculator();
			string input = "4*(5*2-1)";
			double rightAnswer = 36;
			

			//act
			var answer = calculator.Calculate(input);

			//assert
			double eps = 0.000001;
			Assert.True(Math.Abs(rightAnswer - answer) < eps);
		}

		[Fact]
		public void Calculate_PowerOfTwoBracketGroups_SholdReturnSuccess()
		{
			//arrange
			
			var calculator = new RPNCalculator();
			string input = "(4*2-2)^(7-2*2)";
			double rightAnswer = 216;
			

			//act
			var answer = calculator.Calculate(input);

			//assert
			double eps = 0.000001;
			Assert.True(Math.Abs(rightAnswer - answer) < eps);
		}
	}
}
