using backend.DTOs;
using backend.services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        IPostService _titlesServices;

        public PostController(IPostService titlesServices)
        {
            _titlesServices = titlesServices;
        }
             
             [HttpGet]
             public async Task<IEnumerable<PostDto>> Get()=> await _titlesServices.Get();
    }
}
