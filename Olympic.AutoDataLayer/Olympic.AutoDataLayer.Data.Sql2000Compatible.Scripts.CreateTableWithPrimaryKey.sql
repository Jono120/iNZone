CREATE TABLE [dbo].[{TableName}] (
	{TableFieldDeclarations} ,
	CONSTRAINT [PK_{TableName}] PRIMARY KEY  {Clustered} 
	(
		{TableKeyFieldList}
	)  ON [PRIMARY] 
) ON [PRIMARY]
