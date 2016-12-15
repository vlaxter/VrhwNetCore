using Microsoft.AspNetCore.Mvc;
using VrhwNetCore.Shared.Interfaces;
using VrhwNetCore.Shared.Models;

namespace VrhwNerCore.Api.Controllers
{
    [Route("v1/[controller]")]
    public class DiffController : Controller
    {
        private readonly IDiffService _diffService;

        public DiffController(IDiffService diffService)
        {
            _diffService = diffService;
        }

        [Route("{id:int}/left")]
        [HttpPut]
        public IActionResult Left(int id, [FromBody]RequestModel request)
        {
            if (request != null && !string.IsNullOrWhiteSpace(request.Data))
            {
                var diff = _diffService.Left(id, request.Data);
                if (diff != null)
                {
                    return Created(string.Format($"v1/diff/{id}"), diff);
                }
                else
                {
                    return BadRequest();
                }
            }

            return BadRequest();
        }

        [Route("{id:int}/right")]
        [HttpPut]
        public IActionResult Right(int id, [FromBody]RequestModel request)
        {
            if (request != null && !string.IsNullOrWhiteSpace(request.Data))
            {
                var diff = _diffService.Right(id, request.Data);
                if (diff != null)
                {
                    return Created(string.Format($"v1/diff/{id}"), diff);
                }
                else
                {
                    return BadRequest();
                }
            }

            return BadRequest();
        }

        [Route("{id:int}")]
        [HttpGet]
        public IActionResult Diff(int id)
        {
            var diff = _diffService.GetDiff(id);
            if (diff == null)
            {
                return BadRequest();
            }

            return Ok(diff);
        }
    }
}