using System;
using Xunit;
using pi1;
using Moq;
using Microsoft.Extensions.Logging;

namespace pi1.Tests
{
	public class DALCalculatorTests
	{
		[Fact]
		public void Tokenize_SingleOperation_SholdReturnThreeTokens()
		{
			//arrange
			var mockLogger = new Mock<ILogger<DALCalculator>>();
			var calculator = new DALCalculator(mockLogger.Object);
			string[] expectedTokens = new[] { "4", "+", "5" };
			string input = "4+5";

			//act
			var actualTokens = calculator.Tokenize(input);

			//assert
			Assert.Equal(expectedTokens, actualTokens);
		}

		[Fact]
		public void Tokenize_TwoOperationWithSamePriority_SholdReturnFiveTokens()
		{
			//arrange
			var mockLogger = new Mock<ILogger<DALCalculator>>();
			var calculator = new DALCalculator(mockLogger.Object);
			string[] expectedTokens = new[] { "4", "+", "5", "-", "1" };
			string input = "4+5-1";

			//act
			var actualTokens = calculator.Tokenize(input);

			//assert
			Assert.Equal(expectedTokens, actualTokens);
		}

		[Fact]
		public void Tokenize_TwoOperations_SholdReturnFiveTokens()
		{
			//arrange
			var mockLogger = new Mock<ILogger<DALCalculator>>();
			var calculator = new DALCalculator(mockLogger.Object);
			string[] expectedTokens = new[] { "4", "+", "5", "-", "1" };
			string input = "4+5-1";

			//act
			var actualTokens = calculator.Tokenize(input);

			//assert
			Assert.Equal(expectedTokens, actualTokens);
		}

		[Fact]
		public void Tokenize_TwoOperationsAndBigInts_SholdReturnFiveTokens()
		{
			//arrange
			var mockLogger = new Mock<ILogger<DALCalculator>>();
			var calculator = new DALCalculator(mockLogger.Object);
			string[] expectedTokens = new[] { "43", "+", "52", "-", "1" };
			string input = "43+52-1";

			//act
			var actualTokens = calculator.Tokenize(input);

			//assert
			Assert.Equal(expectedTokens, actualTokens);
		}

		[Fact]
		public void Tokenize_TwoOperationsAndDoubles_SholdReturnFiveTokens()
		{
			//arrange
			var mockLogger = new Mock<ILogger<DALCalculator>>();
			var calculator = new DALCalculator(mockLogger.Object);
			string[] expectedTokens = new[] { ".43", "+", "52.2", "-", "1." };
			string input = ".43+52.2-1.";

			//act
			var actualTokens = calculator.Tokenize(input);

			//assert
			Assert.Equal(expectedTokens, actualTokens);
		}

		[Fact]
		public void Tokenize_TwoOperationsAndDoublesAndBrackets_SholdReturnSevenTokens()
		{
			//arrange
			var mockLogger = new Mock<ILogger<DALCalculator>>();
			var calculator = new DALCalculator(mockLogger.Object);
			string[] expectedTokens = new[] { ".43", "*", "(", "52.2", "-", "1.", ")"};
			string input = ".43*(52.2-1.)";
			//act
			var actualTokens = calculator.Tokenize(input);
			//assert
			Assert.Equal(expectedTokens, actualTokens);
		}

		[Fact]
		public void Tokenize_FourOperationsAndDoublesAndBracketsInsideBrackets_SholdReturnNineTokens()
		{
			//arrange
			var mockLogger = new Mock<ILogger<DALCalculator>>();
			var calculator = new DALCalculator(mockLogger.Object);
			string[] expectedTokens = new[] { ".43", "*", "(", "52.2", "/", "(", "1", "+", "1", ")", ")"};
			string input = ".43*(52.2/(1+1))";
			//act
			var actualTokens = calculator.Tokenize(input);
			//assert
			Assert.Equal(expectedTokens, actualTokens);
		}

		[Fact]
		public void Tokenize_WrongBracketsCount_ShouldThrowException()
		{
			//arrange
			var mockLogger = new Mock<ILogger<DALCalculator>>();
			var calculator = new DALCalculator(mockLogger.Object);
			string input = ".43*(52.2)/(1+1))";
			//act
			void action() { calculator.Tokenize(input); }
			//assert
			Assert.Throws<ArgumentException>(action);
		}

		[Fact]
		public void Tokenize_WrongBracketsOrder_ShouldThrowException()
		{
			//arrange
			var mockLogger = new Mock<ILogger<DALCalculator>>();
			var calculator = new DALCalculator(mockLogger.Object);
			string input = ".43*52.2/)(1+1";
			//act
			void action() { calculator.Tokenize(input); }
			//assert
			Assert.Throws<ArgumentException>(action);
		}

		[Fact]
		public void Tokenize_NothingInsideBrackets_ShouldThrowException()
		{
			//arrange
			var mockLogger = new Mock<ILogger<DALCalculator>>();
			var calculator = new DALCalculator(mockLogger.Object);
			string input = ".43*52.2/()";
			//act
			void action() { calculator.Tokenize(input); }
			//assert
			Assert.Throws<ArgumentException>(action);
		}

		[Fact]
		public void Tokenize_WrongOperationOrder_ShouldThrowException()
		{
			//arrange
			var mockLogger = new Mock<ILogger<DALCalculator>>();
			var calculator = new DALCalculator(mockLogger.Object);
			string input = ".43*52.2/(4+*3)";
			//act
			void action() { calculator.Tokenize(input); }
			//assert
			Assert.Throws<ArgumentException>(action);
		}

		[Fact]
		public void Tokenize_UnacceptableOperation_ShouldThrowException()
		{
			//arrange
			var mockLogger = new Mock<ILogger<DALCalculator>>();
			var calculator = new DALCalculator(mockLogger.Object);
			string input = ".43*52.2&3";
			//act
			void action() { calculator.Tokenize(input); }
			//assert
			Assert.Throws<ArgumentException>(action);
		}
	}
}
