using pi1.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace pi1
{
	public interface IOperationsHistoryRepository
	{
		public void Add(OperationItem operationItem);
		public IList<OperationItem> GetAll();
	}
}
