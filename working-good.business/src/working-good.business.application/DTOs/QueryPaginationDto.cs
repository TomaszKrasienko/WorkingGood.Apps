namespace working_good.business.application.DTOs;

public record QueryPaginationDto<T>(T Data, PaginationDto Pagination);