using working_good.business.application.DTOs;

namespace working_good.business.application.Services.QueryRepositories;

public interface ICompanyQueryRepository
{
    Task<QueryPaginationDto<IEnumerable<CompanyDto>>> GetCompaniesList(int pageNumber, int pageSize, 
        string name = null, bool? isOwner = null);

    Task<CompanyDto> GetCompanyById(Guid id);
}