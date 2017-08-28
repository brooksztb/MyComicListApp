using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyComicListApi.Services;
using MyComicListApi.Models;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MyComicListApi.Controllers
{
    [Route("api/[controller]")]
    public class PullListController : Controller
    {
        public IPullListRepository PullLists { get; set; }

        public PullListController(IPullListRepository pulllists)
        {
            PullLists = pulllists;
        }
        // GET: api/values
        [Route("GetAllActiveLists")]
        [HttpGet]
        public IEnumerable<PullList> GetAllActiveLists()
        {
            return PullLists.GetAllActiveLists();
        }

        // GET api/values/5
        [Route("FindList")]
        [HttpGet("{name}")]
        public IActionResult FindList(string name)
        {
            var list = PullLists.FindListByName(name);

            if(list == null)
            {
                return NotFound();
            }

            return Ok(list);
        }

        // POST api/values
        [Route("AddNewList")]
        [HttpPost]
        public async Task<IActionResult> AddNewList([FromBody]PullList pulllist)
        {
            PullListResult result = new PullListResult();

            try
            {
                string id = await PullLists.Add(pulllist);

                if(id != null)
                {
                    result.Id = id;
                }

                return Ok(string.Format("New List Added for {0}", pulllist.Name));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Route("UpdateListStatus")]
        [HttpPatch("{name}")]
        public async Task<IActionResult> UpdateListStatus(string name, [FromBody]bool status)
        {
            try
            {
                string result = null;
                string oldStatus = null;
                string newStatus = null;

                if (name != null)
                {
                    var list = PullLists.FindListByName(name);

                    if (list.Active == false)
                    {
                        oldStatus = "Inactive";
                    }
                    else
                    {
                        oldStatus = "Active";
                    }
                    if (status == false)
                    {
                        newStatus = "Inactive";
                    }
                    else
                    {
                        newStatus = "Active";
                    }
                    result = await PullLists.UpdateListStatus(name, status);
                }

                if (result == null)
                {
                    return NotFound();
                }

                return Ok(string.Format("List for {0} Updated From {1} to {2}", name, oldStatus, newStatus));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Route("AddNewComicToList")]
        [HttpPut("{name}")]
        public async Task<IActionResult> AddNewComicToList(string name, [FromBody]List<Comic> comics)
        {
            try
            {
                string result = null;

                if(comics != null && comics.Count > 1)
                {
                    result = await PullLists.AddBulkComicsToList(name, comics);
                }
                else if (comics != null && comics.Count == 1)
                {
                    result = await PullLists.AddComicToList(name, comics[0]);
                }

                if (result == null)
                {
                    return NotFound();
                }

                return Ok(string.Format("List Updated for {0}", name));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }


        // DELETE api/values/5
        [Route("DeleteList")]
        [HttpDelete("{name}")]
        public async Task<IActionResult> DeleteList(string name)
        {
            try
            {

                string result = await PullLists.DeleteList(name);

                if(result == null)
                {
                    return NotFound();
                }

                return Ok(string.Format("List Deleted for {0}", name));
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Route("DeleteComicFromList")]
        [HttpPut("{name}")]
        public async Task<IActionResult> DeleteComicFromList(string name, [FromBody]List<string> nameofcomics)
        {
            try
            {
                string result = null;
                string message = null;
                if (nameofcomics != null && nameofcomics.Count > 1)
                {
                    result = await PullLists.DeleteBulkComicsFromList(nameofcomics, name);
                    foreach(string comics in nameofcomics)
                    {
                        message += string.Format("{0} Deleted From List ", comics);
                    }
                }
                else if(nameofcomics != null && nameofcomics.Count == 1)
                {
                    result = await PullLists.DeleteComicFromList(nameofcomics[0], name);
                    message = string.Format("{0} Deleted From List", nameofcomics[0]);
                }

                if (result == null)
                {
                    return NotFound();
                }


                return Ok(message);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
