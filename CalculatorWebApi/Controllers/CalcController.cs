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
        public  ActionResult<double> Post([FromBody] ArgumentsModel model) 
        {
            double? result = null;

            if (model == null) return BadRequest("Неверно указаны аргументы");

            switch (model.Operation)
            {
                case "sum":
                    result =  CalculatorFunctions.Sum(model.FirstDigit, model.SecondDigit);
                    break;
                case "multiply":
                    result = CalculatorFunctions.Multiply(model.FirstDigit, model.SecondDigit);
                    break;

                    //TODO: Добавление новых операций 
                default:
                    return BadRequest("Неправильно указана операция");
                    
            }

            return result;
        }

       
    }
}
