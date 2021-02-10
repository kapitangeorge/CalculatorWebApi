using System.Threading.Tasks;
using CalculatorWebApi.Models;
using Microsoft.AspNetCore.Mvc;



namespace CalculatorWebApi.Controllers
{
    [Route("api/Calc")]
    public class CalcController : Controller
    {
       

        // POST api/Calc
        [HttpPost]
        public  async Task<ActionResult<double>> Post([FromBody] Calculator calculator) 
        {
            double? result = null;

            if (calculator == null) return BadRequest("Неверно указаны аргументы");

            switch (calculator.Operation)
            {
                case "sum":
                    result =  calculator.FirstDigit + calculator.SecondDigit;
                    break;
                case "multiply":
                    result = calculator.FirstDigit * calculator.SecondDigit;
                    break;

                    //TODO: Добавление новых операций 
                default:
                    break;
            }

            if (result == null) return BadRequest("Неправильно указана операция.");
            else return await Task.FromResult(result);
        }

       
    }
}
