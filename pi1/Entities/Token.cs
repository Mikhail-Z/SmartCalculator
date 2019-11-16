using System;
using System.Collections.Generic;
using System.Text;

namespace pi1.Entities
{
	public interface IToken
	{
		bool IsOperation();
		bool IsOperand();
	}
}
