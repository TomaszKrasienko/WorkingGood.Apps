using working_good.business.core.Exceptions;

namespace working_good.business.core.Models.Company;

public sealed class SlaTimeMustBeGreaterThanZeroException()
    : CustomException("SLA time must be greater than zero", "sla_time_must_be_greater_than_zero");