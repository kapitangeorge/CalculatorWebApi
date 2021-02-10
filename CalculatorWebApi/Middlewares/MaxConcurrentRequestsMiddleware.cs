using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CalculatorWebApi.Middlewares
{
    public class MaxConcurrentRequestsMiddleware
    {
        // Лимит одновременных запросов
        private const int Limit = 10; 

        private int _concurrentRequestCount;
        

        private readonly RequestDelegate _next;

        public MaxConcurrentRequestsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (CheckLimit())
            {
                IHttpResponseFeature responseFeature = context.Features.Get<IHttpResponseFeature>();

                responseFeature.StatusCode = StatusCodes.Status503ServiceUnavailable;
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
                if (initialConcurrentRequestsCount >= Limit)
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
