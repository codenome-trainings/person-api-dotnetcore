using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonApi.Business;
using PersonApi.Data.VO;
using Swashbuckle.AspNetCore.SwaggerGen;
using Tapioca.HATEOAS;

namespace PersonApi.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/[controller]/v{version:apiVersion}")]
    public class PersonsController : ControllerBase
    {
        private readonly IPersonBusiness _personBusiness;

        public PersonsController(IPersonBusiness personService)
        {
            _personBusiness = personService;
        }


        [HttpGet]
        [SwaggerResponse(200, Type = typeof(List<PersonVO>))]
        [SwaggerResponse(204)]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        [Authorize("Bearer")]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Get()
        {
            return Ok(_personBusiness.FindAll());
        }

        [HttpGet("{id}")]
        [SwaggerResponse(200, Type = typeof(PersonVO))]
        [SwaggerResponse(204)]
        [SwaggerResponse(400)]
        [SwaggerResponse(404)]
        [SwaggerResponse(401)]
        [Authorize("Bearer")]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Get(long id)
        {
            var response = Ok(_personBusiness.FindById(id));

            if (response == null) return NotFound();

            return response;
        }
        
        [HttpGet("{find-by-name}")]
        [SwaggerResponse(200, Type = typeof(PersonVO))]
        [SwaggerResponse(204)]
        [SwaggerResponse(400)]
        [SwaggerResponse(404)]
        [SwaggerResponse(401)]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult FindByName([FromQuery] string firstname, [FromQuery] string lastname)
        {
            var response = Ok(_personBusiness.FindByName(firstname, lastname));
            if (response == null) return NotFound();
            return response;
        }
        
        [HttpGet("find-with-paged-search/{sortDirection}/{pageSize}/{page}")]
        [SwaggerResponse(200, Type = typeof(PersonVO))]
        [SwaggerResponse(204)]
        [SwaggerResponse(400)]
        [SwaggerResponse(404)]
        [SwaggerResponse(401)]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult GetPagedSearch([FromQuery] string name, string sortDirection, int pageSize, int page)
        {
            return new ObjectResult(_personBusiness.FindWithPagedSearch(name, sortDirection, pageSize, page));
        }

        [HttpPost]
        [SwaggerResponse(201, Type = typeof(PersonVO))]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        [Authorize("Bearer")]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Post([FromBody] PersonVO person)
        {
            return person == null
                ? (IActionResult) BadRequest()
                : Ok(new ObjectResult(_personBusiness.Create(person)).Value);
        }


        [HttpPut]
        [SwaggerResponse(202, Type = typeof(PersonVO))]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        [Authorize("Bearer")]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Put([FromBody] PersonVO person)
        {
            return Ok(_personBusiness.Update(person));
        }


        [HttpDelete("{id}")]
        [SwaggerResponse(204)]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        [Authorize("Bearer")]
        [TypeFilter(typeof(HyperMediaFilter))]
        public IActionResult Delete(long id)
        {
            _personBusiness.Delete(id);
            return NoContent();
        }
    }
}