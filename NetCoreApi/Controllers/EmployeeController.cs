using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace NetCoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public EmployeeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"SELECT EmployeeId, EmployeeName, Department, 
                             CONVERT(varchar(10),DateOfJoining, 120) AS 'DateOfJoining', PhotoFileName 
                             FROM dbo.Employee ORDER BY 1 DESC";
            string dataSource = _configuration.GetConnectionString("MyConnectionString");

            SqlDataReader sqlDataReader;
            DataTable dataTable = new DataTable();

            using(SqlConnection sqlConnection = new SqlConnection(dataSource))
            {
                sqlConnection.Open();
                using(SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlDataReader = sqlCommand.ExecuteReader();
                    dataTable.Load(sqlDataReader);
                    sqlDataReader.Close();
                }
                sqlConnection.Close();
            }

            return new JsonResult(dataTable);
        }
    }
}
