using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonApi.Business;
using PersonApi.Data.VO;
using PersonApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PersonApi.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/[controller]/v{version:apiVersion}")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginBusiness _loginBusiness;

        public LoginController(ILoginBusiness loginBusiness)
        {
            _loginBusiness = loginBusiness;
        }

        [SwaggerResponse(201, Type = typeof(PersonVO))]
        [SwaggerResponse(400)]
        [SwaggerResponse(401)]
        [AllowAnonymous]
        [HttpPost]
        public object Post([FromBody] UserVO user)
        {
            if (user == null) return BadRequest();
            return _loginBusiness.FindByLogin(user);
        }
    }
}