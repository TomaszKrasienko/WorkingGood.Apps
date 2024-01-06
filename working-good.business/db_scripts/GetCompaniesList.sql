SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [wg].[GetCompaniesList](
    @pageNumber     INT,
    @pageSize       INT,
    @isOwner        BIT = NULL,
    @companyName    VARCHAR(40) = NULL 
)
AS
BEGIN 

SELECT 
      [Id] AS Id
    , [Name]
    , [IsOwner]
    , [SlaTimeSpan]
    , [EmailDomain]
INTO #TmpCompanies
FROM wg.Companies comp
WHERE 1=1
  AND (comp.IsOwner = @isOwner or @isOwner is null)
  AND (comp.Name LIKE '%' + @companyName + '%' OR @companyName IS NULL)

SELECT 
      [Id] AS Id
    , [Name]
    , [IsOwner]
    , [SlaTimeSpan]
    , [EmailDomain]
FROM #TmpCompanies
ORDER BY [Id]
OFFSET (@pageSize * @pageNumber) ROWS FETCH NEXT @pageSize ROWS ONLY

DECLARE @totalCount FLOAT = (SELECT 
                                  COUNT(1)
                              FROM #TmpCompanies)
    SET @pageNumber = @pageNumber + 1;
    DECLARE @totalPages INT = CEILING(@totalCount / @pageSize)
    DECLARE @hasNext BIT 
    IF @totalPages <> @pageNumber
        SET @hasNext = 1
    ELSE
        SET @hasNext = 0

    DECLARE @hasPrevious BIT 
    IF @pageNumber != 1
        SET @hasPrevious = 1
    ELSE
        SET @hasPrevious = 0
SELECT
      @pageNumber AS 'CurrentPage'
    , @totalCount AS 'TotalCount'
    , @pageSize AS 'PageSize'
    , @totalPages AS 'TotalPages'
    , @hasNext AS 'HasNext'
    , @hasPrevious AS 'HasPrevious'
END
GO
