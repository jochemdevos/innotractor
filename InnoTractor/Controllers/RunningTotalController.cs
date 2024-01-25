using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace InnoTractor.Controllers
{
  [Route("[controller]")]
  [ApiController]
  public class RunningTotalController : ControllerBase
  {
    private readonly IMemoryCache _memoryCache;

    public RunningTotalController(IMemoryCache memoryCache)
    {
      _memoryCache = memoryCache;
    }

    [HttpPut(Name = "runningTotal")]
    public IActionResult UpdateRunningTotal([FromBody] int number)
    {
      // Retrieve the current state from the cache
      var state = _memoryCache.GetOrCreate("RunningTotalState", entry =>
      {
        entry.SetSlidingExpiration(TimeSpan.FromMinutes(10)); // Adjust expiration time as needed
        return new RunningTotalState();
      });

      // Update the running total
      state.UpdateRunningTotal(number);

      // Store the updated state back in the cache
      _memoryCache.Set("RunningTotalState", state);

      // Return the current running total as plaintext
      return Ok(state.RunningTotal.ToString());
    }
    public class RunningTotalState
    {
      public Queue<int> LastThreeNumbers { get; private set; } = new Queue<int>();
      public int RunningTotal { get; private set; } = 0;

      public void UpdateRunningTotal(int number)
      {
        // Add the new number to the queue
        LastThreeNumbers.Enqueue(number);

        // If the queue size exceeds 3, remove the oldest number
        if (LastThreeNumbers.Count > 3)
        {
          RunningTotal -= LastThreeNumbers.Dequeue();
        }

        // Update the running total
        RunningTotal += number;
      }
    }
  }
}
