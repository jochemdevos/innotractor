using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InnoTractor.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class FizzBuzzController : ControllerBase
  {

    private readonly ILogger<FizzBuzzController> _logger;

    public FizzBuzzController(ILogger<FizzBuzzController> logger)
    {
      _logger = logger;
    }

    [HttpGet(Name = "GetFizzBuzz")]
    public IEnumerable<FizzBuzz> Get()
    {
      return Enumerable.Range(0, 31).Select(index => new FizzBuzz
      {
        Input = index,
        Output = GetOutput(index)
      })
      .ToArray();
    }
    private string GetOutput(int index)
    {
      string output = "";

      if (index % 3 == 0)
      {
        output += "Fizz";
      }

      if (index % 5 == 0)
      {
        output += "Buzz";
      }

      if (string.IsNullOrEmpty(output))
      {
        output = index.ToString();
      }

      return output;
    }
  }
}
