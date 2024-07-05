using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DotNetAPI2.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ImagesController : ControllerBase
  {

    //POST {apibaseurl}/api/images
    public async Task<IActionResult> UploadImage([FromForm] IFormFile file, [FromForm] string filename, [FromForm] string title) 
    {


    }

  }
}
