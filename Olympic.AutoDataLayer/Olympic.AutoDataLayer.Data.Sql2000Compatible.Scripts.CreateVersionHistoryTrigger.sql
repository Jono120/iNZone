
CREATE TRIGGER [{TriggerName}] ON [{TableName}]
	AFTER UPDATE
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
	INSERT INTO [{VersionHistoryTableName}] (
		{FieldList}
	)
	SELECT 
		{DeletedFieldList}
	FROM DELETED d
		INNER JOIN INSERTED i
			ON {PrimaryKeyInnerJoin}
	WHERE
		i.[_version] <> d.[_version]
END
