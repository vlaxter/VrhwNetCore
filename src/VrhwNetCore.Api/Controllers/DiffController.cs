using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
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
        public void Left(int id, [FromBody]RequestModel request)
        {
            _diffService.Left(id, request.Data);
        }

        [Route("{id:int}/right")]
        [HttpPut]
        public void Right(int id, [FromBody]RequestModel request)
        {
            _diffService.Right(id, request.Data);
        }

        [Route("{id:int}")]
        [HttpGet]
        public object Diff(int id)
        {
            return _diffService.GetDiff(id);
        }
    }
}
