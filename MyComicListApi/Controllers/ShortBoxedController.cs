using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyComicListApi.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyComicListApi.Controllers
{
    [Route("[controller]")]
    public class ShortBoxedController : Controller
    {
        public IShortBoxedRepository NewComics { get; set; }

        public ShortBoxedController(IShortBoxedRepository newComics)
        {
            NewComics = newComics;
        }
        // GET: api/values
        [Route("GetNewComics")]
        [HttpGet]
        public async Task<IActionResult> GetNewComics()
        {
            var list = await NewComics.GetCurrentWeeksComicsAsync();

            if (list == null)
            {
                return NotFound();
            }

            return Ok(list);
        }
    }
}
