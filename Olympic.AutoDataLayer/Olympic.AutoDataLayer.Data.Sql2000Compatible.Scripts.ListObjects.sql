DECLARE @Results TABLE (
	[Row] INT IDENTITY(1,1) PRIMARY KEY,
{TableFieldDeclarations}
)

INSERT INTO @Results
({ResultTableFieldList})
SELECT {4} {SelectFieldList} -- (4 - Distinct)
FROM {TableName}
{0} -- WHERE
{GroupByList}
{5} -- HAVING
{1} -- ORDER BY


SELECT 
	{TableFieldList}
FROM 
	@Results t
WHERE
	(([Row] - 1) / {2}) + 1 = {3} -- (2 = PageSize, 3 = PageNumber)
ORDER BY
	[Row] 

SELECT 
	COUNT([Row]) [Count]
FROM
	@Results

