using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalculatorWebApi.Middlewares
{
    public class MaxConcurrentRequestsOption
    {
        public int Limit { get; set; }
    }
}
