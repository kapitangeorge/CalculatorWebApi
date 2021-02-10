using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;

namespace CalculatorWebApi.Middlewares
{
    public class MaxConcurrentRequestsMiddleware
    {
        private readonly MaxConcurrentRequestsOption _options;

        private readonly RequestDelegate _next;

        private int _concurrentRequestCount;

        public MaxConcurrentRequestsMiddleware(RequestDelegate next, IOptions<MaxConcurrentRequestsOption> options)
        {
            _next = next;
            _options = options.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            if (Interlocked.Increment(ref _concurrentRequestCount) > _options.Limit)
            {
                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
            }
            else
            {
                await _next(context);

                Interlocked.Decrement(ref _concurrentRequestCount);
            }
        }
    }
}
