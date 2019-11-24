using System;
using System.Collections.Generic;
using System.Text;
using pi1.Entities;

namespace pi1.Repositories
{
	public class OperationsHistoryMemoryRepository : IOperationsHistoryRepository
	{
		private IList<OperationItem> operationItems;

		public OperationsHistoryMemoryRepository()
		{
			operationItems = new List<OperationItem>();
		}

		public void Add(OperationItem operationItem)
		{
			operationItems.Add(operationItem);
		}

		public IList<OperationItem> GetAll()
		{
			return operationItems;
		}
	}
}
