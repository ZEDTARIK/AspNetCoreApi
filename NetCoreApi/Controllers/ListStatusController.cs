using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetCoreApi.Models;
using System.Data;
using System.Data.SqlClient;

namespace NetCoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListStatusController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ListStatusController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"SELECT StatusId, StatusName from dbo.ListStatus ORDER BY 1 DESC";
            string dataSource = _configuration.GetConnectionString("MyConnectionString");

            SqlDataReader sqlDataReader;
            DataTable dataTable = new DataTable();

            using (SqlConnection sqlConnection = new SqlConnection(dataSource))
            {
                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlDataReader = sqlCommand.ExecuteReader();
                    dataTable.Load(sqlDataReader);
                    sqlDataReader.Close();
                }

                sqlConnection.Close();
            }

            return new JsonResult(dataTable);
        }

        [HttpGet("{id}")]
        public JsonResult Get(int id)
        {
            string query = @"SELECT StatusId, StatusName from dbo.ListStatus WHERE StatusId = @StatusId";
            string dataSource = _configuration.GetConnectionString("MyConnectionString");

            SqlDataReader sqlDataReader;
            DataTable dataTable = new DataTable();

            using (SqlConnection sqlConnection = new SqlConnection(dataSource))
            {
                sqlConnection.Open();

                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@StatusId", id);
                    sqlDataReader = sqlCommand.ExecuteReader();
                    dataTable.Load(sqlDataReader);
                    sqlDataReader.Close();
                }

                sqlConnection.Close();
            }

            return new JsonResult(dataTable);
        }

        [HttpPost]
        public JsonResult Post(ListStatus listStatus)
        {
            string query = @"INSERT INTO dbo.ListStatus VALUES(@StatusName)";
            string dataSource = _configuration.GetConnectionString("MyConnectionString");

            using(SqlConnection sqlConnection = new SqlConnection(dataSource))
            {
                sqlConnection.Open();
                using(SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@StatusName", listStatus.StatusName);
                    sqlCommand.ExecuteReader();
                }

                sqlConnection.Close();
            }

            return new JsonResult(listStatus.StatusName);
        }

        [HttpPut]
        public JsonResult Put(ListStatus listStatus)
        {
            string query = @"UPDATE dbo.ListStatus 
                                SET StatusName= @StatusName
                            WHERE StatusId = @StatusId";

            string dataSource = _configuration.GetConnectionString("MyConnectionString");

            using (SqlConnection sqlConnection = new SqlConnection(dataSource))
            {
                sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@StatusId", listStatus.StatusId);
                    sqlCommand.Parameters.AddWithValue("@StatusName", listStatus.StatusName);
                    sqlCommand.ExecuteReader();
                }

                sqlConnection.Close();
            }

            return new JsonResult(listStatus);
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"DELETE FROM dbo.ListStatus WHERE StatusId =@StatusId";
            string dataSource = _configuration.GetConnectionString("MyConnectionString");

            using (SqlConnection sqlConnection = new SqlConnection(dataSource))
            {
                sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@StatusId", id);
                    sqlCommand.ExecuteReader();
                }

                sqlConnection.Close();
            }

            return new JsonResult("Deleted SuccessFully");
        }
    }
}
