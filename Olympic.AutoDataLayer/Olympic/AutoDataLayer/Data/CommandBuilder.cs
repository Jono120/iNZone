namespace Olympic.AutoDataLayer.Data
{
    using Olympic.AutoDataLayer;
    using Olympic.AutoDataLayer.BuildSupport;
    using Olympic.AutoDataLayer.Data.Sql2000Compatible;
    using System;
    using System.Data;

    internal class CommandBuilder
    {
        public static Command GetCreateTableCommand(string connectionString, TableDefinition tableDefinition, bool preserveData, ClassContext context)
        {
            TableDefinition tableToPreserve = null;
            if (preserveData)
            {
                tableToPreserve = TableDefinition.LoadFromDatabase(connectionString, tableDefinition.TableName, context);
            }
            return new SpExecuteSqlCommand(connectionString, Scripting.GetCreateTableScript(tableDefinition, tableToPreserve), "", context);
        }

        public static Command GetCreateVersionHistoryTriggerCommand(string connectionString, TableDefinition tableDefinition, ClassContext context)
        {
            string createVersionHistoryTriggerScript = Scripting.GetCreateVersionHistoryTriggerScript(tableDefinition);
            Command command = new Command(connectionString, createVersionHistoryTriggerScript, context);
            command.Type = CommandType.Text;
            return command;
        }

        public static Command GetDropTableCommand(string connectionString, string tableName, ClassContext context)
        {
            return new SpExecuteSqlCommand(connectionString, Scripting.GetDropTableScript(tableName), "", context);
        }

        public static Command GetListContraintsCommand(string connectionString, string tableName, ClassContext context)
        {
            tableName = "[" + tableName + "]";
            string commandText = "EXEC sp_helpconstraint " + new SearchFilter("@objname", tableName).ToSQLWhereClause() + ", " + new SearchFilter("@nomsg", "nomsg").ToSQLWhereClause();
            Command command = new Command(connectionString, commandText, context);
            command.Type = CommandType.Text;
            return command;
        }

        public static Command GetListIndexesCommand(string connectionString, string tableName, ClassContext context)
        {
            tableName = "[" + tableName + "]";
            string commandText = "EXEC sp_helpindex " + new SearchFilter("@objname", tableName).ToSQLWhereClause();
            Command command = new Command(connectionString, commandText, context);
            command.Type = CommandType.Text;
            return command;
        }

        public static Command GetListTableColumnsCommand(string connectionString, string tableName, ClassContext context)
        {
            return new SpExecuteSqlCommand(connectionString, Scripting.GetListTableColumnsScript(tableName), "", context);
        }

        public static Command GetListTriggersCommand(string connectionString, string tableName, ClassContext context)
        {
            tableName = "[" + tableName + "]";
            string commandText = "EXEC sp_helptrigger " + new SearchFilter("@tabname", tableName).ToSQLWhereClause();
            Command command = new Command(connectionString, commandText, context);
            command.Type = CommandType.Text;
            return command;
        }
    }
}

