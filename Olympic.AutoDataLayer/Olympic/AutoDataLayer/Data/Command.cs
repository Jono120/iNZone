namespace Olympic.AutoDataLayer.Data
{
    using Olympic.AutoDataLayer;
    using System;
    using System.Collections;
    using System.Data;
    using System.Data.SqlClient;
    using System.Text.RegularExpressions;

    internal class Command
    {
        private ClassContext _classContext;
        private string _commandText;
        private int _commandTimeout = AutoDataSupport.CommandTimeout;
        private string _connectionString;
        private Hashtable _parameters = new Hashtable();
        protected CommandType _type = CommandType.StoredProcedure;
        protected string SpExecuteSqlDeclarations = "";
        protected string SpExecuteSqlStatement = "";
        protected bool UseSpExecuteSql;

        public Command(string connectionString, string commandText, ClassContext context)
        {
            this.ConnectionString = connectionString;
            this._commandText = commandText;
            this._classContext = context;
        }

        public DataSet ExecuteDataSet()
        {
            this.FirePreQuery();
            DataSet dataSet = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommand command = new SqlCommand(this.CommandText);
            command.CommandTimeout = this.CommandTimeout;
            adapter.SelectCommand = command;
            bool flag = false;
            if (TransactionManager.ThreadScopeTransaction != null)
            {
                adapter.SelectCommand.Transaction = TransactionManager.ThreadScopeTransaction.GetTransaction(this.ConnectionString);
                adapter.SelectCommand.Connection = adapter.SelectCommand.Transaction.Connection;
                flag = true;
            }
            else
            {
                adapter.SelectCommand.Connection = new SqlConnection(this.ConnectionString);
                adapter.SelectCommand.Connection.Open();
            }
            adapter.SelectCommand.CommandType = this.Type;
            if ((this._parameters.Count > 0) || this.UseSpExecuteSql)
            {
                SqlParameter[] validatedSqlParameters = this.ValidatedSqlParameters;
                for (int i = 0; i < validatedSqlParameters.Length; i++)
                {
                    IDataParameter parameter = validatedSqlParameters[i];
                    adapter.SelectCommand.Parameters.Add(parameter);
                }
            }
            try
            {
                adapter.Fill(dataSet);
                if (!flag)
                {
                    adapter.SelectCommand.Connection.Close();
                }
                return dataSet;
            }
            catch (Exception exception)
            {
                if (!flag && (adapter.SelectCommand.Connection.State == ConnectionState.Open))
                {
                    adapter.SelectCommand.Connection.Close();
                }
                this.ThrowException(exception);
                return null;
            }
        }

        public int ExecuteNonQuery()
        {
            this.FirePreQuery();
            int num = 0;
            SqlCommand command = new SqlCommand();
            command.CommandTimeout = this.CommandTimeout;
            bool flag = false;
            if (TransactionManager.ThreadScopeTransaction != null)
            {
                command.Transaction = TransactionManager.ThreadScopeTransaction.GetTransaction(this.ConnectionString);
                command.Connection = command.Transaction.Connection;
                flag = true;
            }
            else
            {
                command.Connection = new SqlConnection(this.ConnectionString);
                command.Connection.Open();
            }
            command.CommandText = this.CommandText;
            command.CommandType = this.Type;
            if ((this._parameters.Count > 0) || this.UseSpExecuteSql)
            {
                SqlParameter[] validatedSqlParameters = this.ValidatedSqlParameters;
                for (int i = 0; i < validatedSqlParameters.Length; i++)
                {
                    IDataParameter parameter = validatedSqlParameters[i];
                    command.Parameters.Add(parameter);
                }
            }
            try
            {
                num = command.ExecuteNonQuery();
                if (!flag)
                {
                    command.Connection.Close();
                }
                return num;
            }
            catch (Exception exception)
            {
                if (!flag && (command.Connection.State == ConnectionState.Open))
                {
                    command.Connection.Close();
                }
                this.ThrowException(exception);
                return 0;
            }
        }

        public SqlDataReader ExecuteReader()
        {
            SqlConnection connection;
            this.FirePreQuery();
            SqlCommand command = new SqlCommand();
            command.CommandText = this.CommandText;
            command.CommandType = this.Type;
            command.CommandTimeout = this.CommandTimeout;
            bool flag = false;
            if (TransactionManager.ThreadScopeTransaction != null)
            {
                flag = true;
                command.Transaction = TransactionManager.ThreadScopeTransaction.GetTransaction(this.ConnectionString);
                connection = command.Transaction.Connection;
                command.Connection = connection;
            }
            else
            {
                connection = new SqlConnection(this.ConnectionString);
                connection.Open();
                command.Connection = connection;
            }
            if ((this._parameters.Count > 0) || this.UseSpExecuteSql)
            {
                SqlParameter[] validatedSqlParameters = this.ValidatedSqlParameters;
                for (int i = 0; i < validatedSqlParameters.Length; i++)
                {
                    IDataParameter parameter = validatedSqlParameters[i];
                    command.Parameters.Add(parameter);
                }
            }
            try
            {
                SqlDataReader reader;
                if (flag)
                {
                    reader = command.ExecuteReader();
                }
                else
                {
                    reader = command.ExecuteReader(CommandBehavior.CloseConnection);
                }
                return reader;
            }
            catch (Exception exception)
            {
                if (!flag && (connection.State == ConnectionState.Open))
                {
                    connection.Close();
                }
                this.ThrowException(exception);
                return null;
            }
        }

        public object ExecuteScalar()
        {
            this.FirePreQuery();
            SqlCommand command = new SqlCommand();
            command.CommandTimeout = this.CommandTimeout;
            bool flag = false;
            if (TransactionManager.ThreadScopeTransaction != null)
            {
                flag = true;
                command.Transaction = TransactionManager.ThreadScopeTransaction.GetTransaction(this.ConnectionString);
                command.Connection = command.Transaction.Connection;
            }
            else
            {
                command.Connection = new SqlConnection(this.ConnectionString);
                command.Connection.Open();
            }
            command.CommandText = this.CommandText;
            command.CommandType = this.Type;
            if ((this._parameters.Count > 0) || this.UseSpExecuteSql)
            {
                SqlParameter[] validatedSqlParameters = this.ValidatedSqlParameters;
                for (int i = 0; i < validatedSqlParameters.Length; i++)
                {
                    IDataParameter parameter = validatedSqlParameters[i];
                    command.Parameters.Add(parameter);
                }
            }
            try
            {
                object obj2 = command.ExecuteScalar();
                if (!flag)
                {
                    command.Connection.Close();
                }
                return obj2;
            }
            catch (Exception exception)
            {
                if (!flag && (command.Connection.State == ConnectionState.Open))
                {
                    command.Connection.Close();
                }
                this.ThrowException(exception);
                return null;
            }
        }

        private void FirePreQuery()
        {
            if (this._classContext == null)
            {
                throw new Exception("No context");
            }
            if (this is SpExecuteSqlCommand)
            {
                if (this._classContext != null)
                {
                    AutoDataSupport.OnPreExecuteCommand(this._classContext, ref this.SpExecuteSqlStatement);
                }
            }
            else if (this._classContext != null)
            {
                AutoDataSupport.OnPreExecuteCommand(this._classContext, ref this._commandText);
            }
        }

        protected void ThrowException(Exception exception)
        {
            string str4;
            string message = exception.GetBaseException().Message;
            string str2 = "";
            string str3 = "";
            Match match = new Regex(@"[\(]([^\.]*)[\.]([^\)]*)[\)]([^$]*)$").Match(message);
            if (match.Success)
            {
                str2 = match.Result("$1");
                match.Result("$2");
                str3 = match.Result("$3").Trim();
            }
            else
            {
                str2 = "Exception";
            }
            if (str3.Length > 0)
            {
                str3.Split(new char[] { ':' });
            }
            if (((str4 = str2) != null) && (str4 == "Exception"))
            {
                throw exception;
            }
            throw exception;
        }

        public string CommandText
        {
            get
            {
                return this._commandText;
            }
        }

        public int CommandTimeout
        {
            get
            {
                return this._commandTimeout;
            }
            set
            {
                this._commandTimeout = value;
            }
        }

        public string ConnectionString
        {
            get
            {
                return this._connectionString;
            }
            set
            {
                this._connectionString = value;
            }
        }

        public Hashtable Parameters
        {
            get
            {
                return this._parameters;
            }
        }

        public CommandType Type
        {
            get
            {
                return this._type;
            }
            set
            {
                this._type = value;
            }
        }

        private SqlParameter[] ValidatedSqlParameters
        {
            get
            {
                int count = this._parameters.Count;
                if (this.UseSpExecuteSql)
                {
                    count += 2;
                }
                if (count <= 0)
                {
                    return null;
                }
                SqlParameter[] parameterArray = new SqlParameter[count];
                int index = 0;
                if (this.UseSpExecuteSql)
                {
                    SqlParameter parameter = new SqlParameter("@statement", SqlDbType.NText);
                    parameter.Value = this.SpExecuteSqlStatement;
                    parameterArray[index++] = parameter;
                    SqlParameter parameter2 = new SqlParameter("@parameters", SqlDbType.NText);
                    parameter2.Value = this.SpExecuteSqlDeclarations;
                    parameterArray[index++] = parameter2;
                }
                foreach (DictionaryEntry entry in this._parameters)
                {
                    string parameterName = entry.Key.ToString();
                    if (parameterName.Substring(0, 1) != "@")
                    {
                        parameterName = "@" + parameterName;
                    }
                    bool flag = entry.Value == null;
                    if (!flag && BaseTypeNullHelper.IsNullableBaseType(entry.Value.GetType()))
                    {
                        flag = BaseTypeNullHelper.IsNull(entry.Value);
                    }
                    if (flag)
                    {
                        parameterArray[index] = new SqlParameter(parameterName, DBNull.Value);
                    }
                    else if (entry.Value is SqlParameter)
                    {
                        SqlParameter parameter3 = (SqlParameter) entry.Value;
                        SqlParameter parameter4 = new SqlParameter(parameterName, parameter3.SqlDbType);
                        parameter4.Value = parameter3.Value;
                        parameterArray[index] = parameter4;
                    }
                    else
                    {
                        parameterArray[index] = new SqlParameter(parameterName, entry.Value);
                    }
                    index++;
                }
                return parameterArray;
            }
        }
    }
}

