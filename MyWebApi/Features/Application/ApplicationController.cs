using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyWebApi.Features.Application
{ 
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ApplicationController(IMediator mediator)
        {
            _mediator = mediator;
        }
        // GET: api/Application
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Application/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<Application.GetAppliction.GetApplictionResponse> Get(int id)
        {
            return await _mediator.Send(new Application.GetAppliction.GetApplictionQuery()
            {
                Id =id
            });
        }

        // POST: api/Application
        [HttpPost]
        public async Task<UpsertApplication.InsertApplicationResponse> Post([FromBody] UpsertApplication.InsertApplicationCommand application)
        {
            return await _mediator.Send(application);
        }

        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<DeleteApplication.DeleteApplicationResponse> Delete(int id)
        {
            return await _mediator.Send(new DeleteApplication.DeleteApplicationCommand()
            {
                Id = id
            });
        }
    }
}
