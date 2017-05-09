DECLARE @Results TABLE (
	[Row] INT IDENTITY(1,1) PRIMARY KEY,
{PrimaryKeyFieldDeclarations}
)

INSERT INTO @Results
({PrimaryKeyFieldList})
SELECT {PrimaryKeyFieldList} -- (4 - Distinct is not supported for keyed objects)
FROM {TableName}
{0} -- WHERE
-- Group by is not valid for a keyed object, so no GROUP BY or HAVING
{1} -- ORDER BY

SELECT 
	{TableFieldList}
FROM 
	@Results r
		INNER JOIN {TableName} t
			ON {PrimaryKeyInnerJoinList}
WHERE
	(([Row] - 1) / {2}) + 1 = {3} -- (2 = PageSize, 3 = PageNumber)
ORDER BY
	[Row] 

SELECT 
	COUNT([Row]) [Count]
FROM
	@Results

