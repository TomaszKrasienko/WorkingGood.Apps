namespace working_good.business.application.DTOs;

public record PaginationArgumentsDto
{
    private int _pageNumber;
    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = value < 1 ? 0 : value - 1;
    }
    
    private const int MaxPageSize = 50;
    private int _pageSize = 10;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }
};