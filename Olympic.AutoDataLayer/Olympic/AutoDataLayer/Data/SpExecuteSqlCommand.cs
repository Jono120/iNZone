namespace Olympic.AutoDataLayer.Data
{
    using System;

    internal class SpExecuteSqlCommand : Command
    {
        public SpExecuteSqlCommand(string connectionString, string statement, string parameters, ClassContext context) : base(connectionString, "sp_executesql", context)
        {
            base.UseSpExecuteSql = true;
            base.SpExecuteSqlStatement = statement;
            base.SpExecuteSqlDeclarations = parameters;
        }
    }
}

