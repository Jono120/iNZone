/*
	Used to return a list of the fields in a table.  In the script {TableName}
	is replaced with the name of the table that you are querying.
*/

DECLARE @objname nvarchar(776)
SELECT @objname = '[{TableName}]'

-- Returns a row with a single column indicating if the table exists
SELECT CAST(ISNULL(object_id(@objname), 0) AS BIT) [Exists]

DECLARE @numtypes nvarchar(80)
SELECT @numtypes = N'tinyint,smallint,decimal,int,real,money,float,numeric,smallmoney'

-- Returns a list of the columns in the table
SELECT
	name [ColumnName],
	type_name(xusertype) [Type],
	convert(int, length) [Length],
	case when charindex(type_name(xtype), @numtypes) > 0
		then convert(char(5),ColumnProperty(id, name, 'precision'))
		else '' end [Precision],
	case when charindex(type_name(xtype), @numtypes) > 0
		then convert(char(5),OdbcScale(xtype,xscale))
		else '' end [Scale],
	CAST(isnullable AS BIT) [Nullable],
	CAST(colstat & 1 AS BIT) [Identity]

FROM syscolumns where id = object_id(@objname) and number = 0 order by colid


