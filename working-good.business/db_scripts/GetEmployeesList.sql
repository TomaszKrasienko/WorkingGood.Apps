SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER   PROCEDURE [wg].[GetEmployeesList](
    @pageNumber     INT,
    @pageSize       INT,
    @companyId    UNIQUEIDENTIFIER = NULL 
)
AS
BEGIN 

    SELECT 
          [Id]
        , [Email]
        , [CompanyId]
    INTO #TmpEmployees
    FROM wg.Employees emp
    WHERE 1=1
      AND (emp.CompanyId = @companyId OR @companyId IS NULL)

    SELECT 
          [Id]
        , [Email]
        , [CompanyId]
    FROM #TmpEmployees
    ORDER BY [Id]
    OFFSET (@pageSize * @pageNumber) ROWS FETCH NEXT @pageSize ROWS ONLY

    DECLARE @totalCount FLOAT = (SELECT 
                                    COUNT(1)
                                FROM #TmpEmployees)
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
