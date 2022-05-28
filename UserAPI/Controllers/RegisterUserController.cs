using Microsoft.AspNetCore.Mvc;
using UserAPI.Data;

namespace UserAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterUserController : ControllerBase
    {
        //// GET: api/<RegisterUserController>
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/<RegisterUserController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        // POST api/<RegisterUserController>
        [HttpPost]
        public async Task Post([FromBody] User user)
        {

            await UserDatabase.RegisterUser(user);
        }

        //// PUT api/<RegisterUserController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<RegisterUserController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
