using Microsoft.Extensions.Logging;
using working_good.business.application.CQRS.Abstractions;

namespace working_good.business.infrastructure.Logging.Decorators;

internal sealed class LoggingCommandHandlerDecorator<T> : ICommandHandler<T> where T : class, ICommand
{
    private readonly ILogger<T> _logger;
    private readonly ICommandHandler<T> _handler;

    public LoggingCommandHandlerDecorator(ILogger<T> logger, ICommandHandler<T> handler)
    {
        _logger = logger;
        _handler = handler;
    }
    
    public async Task HandleAsync(T command, CancellationToken token)
    {
        try
        {
            _logger.LogInformation($"Handling {typeof(T)}");
            await _handler.HandleAsync(command, token);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }
}