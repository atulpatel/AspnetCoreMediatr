using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyWebApi.Features.Customers
{
    [Route("api/[controller]")]
    public class CustomerController : Controller
    {
        private readonly IMediator _mediarR;

        public CustomerController(IMediator mediarR)
        {
            _mediarR = mediarR;
        }

        [HttpGet]
        // GET: Customer
        public ActionResult Get()
        {
            return Ok(new { Key ="Key", Value="Value" });
        }

        [HttpGet]
        [Route("{id}")]
        // GET: Customer/Details/5
        public ActionResult Get(int id)
        {
            return Ok(new { Key = "Key", Value = "Value" });
        }

        // POST: Customer/Create
        [HttpPost]
        public async Task<CustomerUpsert.Response> Post([FromBody]CustomerUpsert.Command command)
        {
            return await _mediarR.Send(command);
        }


        // POST: Customer/Edit/5
        [HttpPut]
        public ActionResult Put(int id, IFormCollection collection)
        {
            // TODO: Add update logic here
            return Ok();
        }


        // POST: Customer/Delete/5
        [HttpDelete]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            return Ok();
        }
    }
}