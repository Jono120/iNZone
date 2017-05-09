namespace Olympic.AutoDataLayer.Data
{
    using Olympic.AutoDataLayer;
    using System;

    internal class TransactionManager
    {
        [ThreadStatic]
        private static SqlTransactionScope _threadScopeTransaction;

        public static SqlTransactionScope ThreadScopeTransaction
        {
            get
            {
                return _threadScopeTransaction;
            }
            set
            {
                _threadScopeTransaction = value;
            }
        }
    }
}

