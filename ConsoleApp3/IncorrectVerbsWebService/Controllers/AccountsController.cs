using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AdoHelper;
using Microsoft.AspNetCore.Mvc;

namespace IncorrectVerbsWebService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<(int id, string namehash, string passwordhash, int score, int left)>> Get()
        {
            SqlConnection connection = new SqlConnection(DataAccess.ConnectionString);
            connection.Open();
            var rezult = new AdoHelper<(int id, string namehash, string passwordhash, int score, int left)>(connection)
                .Query("SELECT * FROM Accounts")
                .ExecuteReader();
            connection.Close();
            return new ActionResult<IEnumerable<(int, string, string, int, int)>>(rezult);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<(int, string, string, int, int)> Get(int id)
        {
            SqlConnection connection = new SqlConnection(DataAccess.ConnectionString);
            connection.Open();
            var rezult = new AdoHelper<(int id, string namehash, string passwordhash, int score, int left)>(connection)
                .Query("SELECT * FROM Accounts WHERE Id=@id")
                .Parameters(("@id", id))
                .ExecuteReader().FirstOrDefault();
            connection.Close();
            return new ActionResult<(int, string, string, int, int)>(rezult);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] (int id, string namehash, string passwordhash, int score, int left) value)
        {
            
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] (int id, string name, string password, int score, int left) value)
        {
            SqlConnection connection = new SqlConnection(DataAccess.ConnectionString);
            connection.Open();
            if (new AdoHelper<int>(connection).Query("SELECT COUNT(id) FROM Accounts WHERE Id=@id").Parameters(("@id", value.id)).ExecuteScalar() == 1)
            {
                // Update
                new AdoHelper<int>(connection)
                    .Query("UPDATE Accounts SET (score = @score, left = @left")
                    .Parameters(("@score", value.score),
                    ("@left", value.left))
                    .ExecuteNonQuery();
            }
            else
            {
                // Create
                new AdoHelper<int>(connection)
                    .Query("INSERT INTO Accounts (name, password, score, left) VALUES (@name, @password, @score, @left")
                    .Parameters(("@name",value.name),
                    ("@password", value.password),
                    ("@score", value.score),
                    ("@left", value.left))
                    .ExecuteNonQuery();
            }
            connection.Close();
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
