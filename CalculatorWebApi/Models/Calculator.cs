using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalculatorWebApi.Models
{
    public class Calculator
    {
        public double FirstDigit { get; set; }

        public double SecondDigit { get; set; }

        public string Operation { get; set; }
    }
}
