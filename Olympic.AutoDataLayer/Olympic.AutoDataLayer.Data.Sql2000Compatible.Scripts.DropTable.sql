/*
	Used to drop a table in the database
*/

if exists (select * from dbo.sysobjects where id = object_id(N'[{TableName}]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [{TableName}]

