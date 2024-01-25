using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace InnoTractor.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class ValuesController : ControllerBase
  {
    [HttpGet(Name = "values")]
    public int GetValues() => SumValues(new ValuesClass().Values());

    public static int SumValues(string?[] inputValues)
    {
      int sum = 0;
      NumberStyles styles = NumberStyles.Integer | NumberStyles.AllowExponent | NumberStyles.AllowThousands;

      foreach (string? value in inputValues)
      {
        int? parsedValue = CallTryParse(value,styles);
        if (parsedValue.HasValue)
        {
          sum += (int)parsedValue;
        }
      }

      return sum;
    }
    private static int? CallTryParse(string stringToConvert, NumberStyles styles)
    {
      CultureInfo provider = new CultureInfo("nl-NL");

      bool success = int.TryParse(stringToConvert, styles, provider, out int number);
      if (success)
      {
        return number;
      }
      else
      {
        Console.WriteLine($"Attempted conversion of '{stringToConvert}' failed.");
        return null;
      }
    }
  }
}
