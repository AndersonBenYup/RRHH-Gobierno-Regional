using Microsoft.AspNetCore.Mvc.Filters;

namespace Proyecto_Evo_RRLL.Filtros;

public class RegistrarExcepcionFilter : IAsyncExceptionFilter
{
    private readonly ILogger<RegistrarExcepcionFilter> _logger;

    public RegistrarExcepcionFilter(ILogger<RegistrarExcepcionFilter> logger) => _logger = logger;

    public Task OnExceptionAsync(ExceptionContext context)
    {
        _logger.LogError(context.Exception, "Excepción no controlada en {Ruta}",
            context.ActionDescriptor?.DisplayName ?? context.HttpContext.Request.Path.Value ?? "?");
        return Task.CompletedTask;
    }
}