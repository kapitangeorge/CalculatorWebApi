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
        private readonly int _limit;

        private readonly RequestDelegate _next;

        private int _concurrentRequestCount;

        public MaxConcurrentRequestsMiddleware(RequestDelegate next, IOptions<MaxConcurrentRequestsOption> options)
        {
            _next = next;
            _limit = options.Value.Limit;
        }

        public async Task Invoke(HttpContext context)
        {
            if (CheckLimit())
            {
                context.Response.StatusCode = StatusCodes.Status503ServiceUnavailable;
            }
            else
            {
                await _next(context);

                Interlocked.Decrement(ref _concurrentRequestCount);
            }
        }

        private bool CheckLimit()
        {
            bool limitExceeded;

            int initialConcurrentRequestsCount, incrementedConcurrentRequestsCount;
            do
            {
                limitExceeded = true;

                initialConcurrentRequestsCount = _concurrentRequestCount;
                if (initialConcurrentRequestsCount >= _limit)
                {
                    break;
                }

                limitExceeded = false;
                incrementedConcurrentRequestsCount = initialConcurrentRequestsCount + 1;
            }
            while (initialConcurrentRequestsCount != Interlocked.CompareExchange(
                ref _concurrentRequestCount, incrementedConcurrentRequestsCount, initialConcurrentRequestsCount));

            return limitExceeded;
        }
    }
}
