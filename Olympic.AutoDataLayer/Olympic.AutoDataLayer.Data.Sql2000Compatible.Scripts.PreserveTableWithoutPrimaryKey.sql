if exists (select * from dbo.sysobjects where id = object_id(N'[__ADLTemp_{TableName}]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
	DROP TABLE [__ADLTemp_{TableName}]

CREATE TABLE [dbo].[__ADLTemp_{TableName}] (
	{TableFieldDeclarations} 
) ON [PRIMARY]

if exists (select * from dbo.sysobjects where id = object_id(N'[{TableName}]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
BEGIN

	IF 1 = {IdentityInsert} -- Does this need an identity insert?
		SET IDENTITY_INSERT [__ADLTemp_{TableName}] ON

	INSERT INTO [__ADLTemp_{TableName}] (
		{PreserveFieldList}
	) 
	SELECT
		{PreserveFieldList}
	FROM
		[{TableName}]

	IF 1 = {IdentityInsert} -- Does this need an identity insert?
		SET IDENTITY_INSERT [__ADLTemp_{TableName}] OFF


END

if exists (select * from dbo.sysobjects where id = object_id(N'[{TableName}]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [{TableName}]

EXEC sp_rename '[__ADLTemp_{TableName}]', '{TableName}'

