namespace Olympic.AutoDataLayer
{
    using Olympic.AutoDataLayer.Data;
    using System;
    using System.Collections;
    using System.Data.SqlClient;

    public class SqlTransactionScope : IDisposable
    {
        private System.Data.IsolationLevel? _isolationLevel;
        private SqlTransactionScope _previousTransactionScope = TransactionManager.ThreadScopeTransaction;
        private Hashtable _transactions = new Hashtable();

        public SqlTransactionScope()
        {
            TransactionManager.ThreadScopeTransaction = this;
        }

        public void Complete()
        {
            foreach (object obj2 in this._transactions.Values)
            {
                ((SqlTransactionElements) obj2).Transaction.Commit();
            }
        }

        public void Dispose()
        {
            foreach (object obj2 in this._transactions.Values)
            {
                SqlTransactionElements elements = (SqlTransactionElements) obj2;
                elements.Connection.Close();
            }
            TransactionManager.ThreadScopeTransaction = this._previousTransactionScope;
        }

        internal SqlTransaction GetTransaction(string connectionString)
        {
            if (!this._transactions.ContainsKey(connectionString))
            {
                SqlTransaction transaction;
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                if (this.IsolationLevel.HasValue)
                {
                    transaction = connection.BeginTransaction(this.IsolationLevel.Value);
                }
                else
                {
                    transaction = connection.BeginTransaction();
                }
                SqlTransactionElements elements = new SqlTransactionElements(transaction, connection);
                this._transactions.Add(connectionString, elements);
            }
            return ((SqlTransactionElements) this._transactions[connectionString]).Transaction;
        }

        public void Rollback()
        {
            foreach (object obj2 in this._transactions.Values)
            {
                ((SqlTransactionElements) obj2).Transaction.Rollback();
            }
        }

        public System.Data.IsolationLevel? IsolationLevel
        {
            get
            {
                return this._isolationLevel;
            }
            set
            {
                this._isolationLevel = value;
            }
        }

        private class SqlTransactionElements
        {
            private SqlConnection _connection;
            private SqlTransaction _transaction;

            public SqlTransactionElements(SqlTransaction transaction, SqlConnection connection)
            {
                this._transaction = transaction;
                this._connection = connection;
            }

            public SqlConnection Connection
            {
                get
                {
                    return this._connection;
                }
            }

            public SqlTransaction Transaction
            {
                get
                {
                    return this._transaction;
                }
            }
        }
    }
}

