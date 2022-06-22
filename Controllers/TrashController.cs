using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class TrashController : ControllerBase
{
    private static List<Trash> _trashList = new();
    private readonly ILogger<TrashController> _logger;

    public TrashController(ILogger<TrashController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    [Route("/get")]
    public List<Trash> Get()
    {
        try
        {
            return _trashList;
        }
        catch (System.Exception exception)
        {
            _logger.LogError(exception,"");
            throw;
        }
    }

    
    [HttpPost]
    [Route("/post")]
    public IActionResult Post([FromBody] Trash trash)
    {
        try
        {
            var lastItem = _trashList.OrderByDescending(x => x.Id).FirstOrDefault();
            trash.Id = lastItem is null ? trash.Id : lastItem.Id + 1;
            _trashList.Add(trash);
            return Ok();
        }
        catch (System.Exception exception)
        {
            _logger.LogError(exception,"");
            return StatusCode(500);
        }
    }

    [HttpDelete]
    [Route("/delete")]
    public Trash? Delete([FromQuery] int id)
    {
        try
        {
            var trash = _trashList.Where(x => x.Id == id).FirstOrDefault();
            _trashList.RemoveAt(_trashList.FindIndex(x => x.Id == id));
            return trash;
        }
        catch (System.Exception exception)
        {
            _logger.LogError(exception,"");
            throw;
        }
    }


    [HttpPatch]
    [Route("/patch")]
    public Trash? Patch([FromBody] Trash trash)
    {
        try
        {
            if(!_trashList.Where(x => x.Id == trash.Id).Any())
            {
                return null;
            }
            
            _trashList[_trashList.FindIndex(x => x.Id == trash.Id)] = trash;
            return trash;
        }
        catch (System.Exception exception)
        {
            _logger.LogError(exception,"");
            throw;
        }
    }
}
