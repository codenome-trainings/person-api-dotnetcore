using Microsoft.AspNetCore.Mvc;
using PersonApi.Business;
using PersonApi.Models;

namespace PersonApi.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PersonsController : ControllerBase
    {

        private readonly IPersonBusiness _personBusiness;

        public PersonsController(IPersonBusiness personService) => _personBusiness = personService;
        
        [HttpGet]
        public ActionResult Get() => Ok(_personBusiness.FindAll());
        
        
        [HttpGet("{id}")]
        public ActionResult Get(long id)
        {
            var response = _personBusiness.FindById(id);
            
            if (response == null)
            {
                return NotFound();
            }
            
            return Ok(response);
        }

        [HttpPost]
        public ActionResult Post([FromBody] Person person)
        {
            if (Request == null)
                return BadRequest();
            
            
            return Ok(new ObjectResult(_personBusiness.Create(person)));
        }

        [HttpPut]
        public Person Put([FromBody] Person person) => _personBusiness.Update(person);

        [HttpDelete("{id}")]
        public void Delete(long id) => _personBusiness.Delete(id);
    }

    
}