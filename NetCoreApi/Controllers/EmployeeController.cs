using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetCoreApi.Models;
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

        [HttpGet("{id}")]
        public JsonResult Get(int id)
        {
            string query = @"SELECT EmployeeName, Department, 
                             CONVERT(varchar(10),DateOfJoining, 120) AS 'DateOfJoining', PhotoFileName 
                             FROM dbo.Employee
                            WHERE EmployeeId=@EmployeeId";

            string dataSource = _configuration.GetConnectionString("MyConnectionString");
            SqlDataReader sqlDataReader;
            DataTable dataTable = new DataTable();

            using (SqlConnection sqlConnection = new SqlConnection(dataSource))
            {
                sqlConnection.Open();
                using (SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@EmployeeId", id);
                    sqlDataReader = sqlCommand.ExecuteReader();
                    dataTable.Load(sqlDataReader);
                    sqlDataReader.Close();
                }
                sqlConnection.Close();
            }

            return new JsonResult(dataTable);
        }

        [HttpPost]
        public JsonResult Post(Employee employee)
        {
            string query = @"INSERT INTO dbo.Employee (EmployeeName, Department, DateOfJoining, PhotoFileName)
                             VALUES (@EmployeeName, @Department, @DateOfJoining, @PhotoFileName)";
            string dataSource = _configuration.GetConnectionString("MyConnectionString");

            SqlDataReader sqlDataReader;

            using(SqlConnection sqlConnection = new SqlConnection(dataSource))
            {
                sqlConnection.Open();
                using(SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@EmployeeName", employee.EmployeeName);
                    sqlCommand.Parameters.AddWithValue("@Department", employee.Department);
                    sqlCommand.Parameters.AddWithValue("@DateOfJoining", employee.DateOfJoining);
                    sqlCommand.Parameters.AddWithValue("@PhotoFileName", employee.PhotoFileName);
                    sqlDataReader = sqlCommand.ExecuteReader();
                    sqlDataReader.Close();
                }
                sqlConnection.Close();
            }
            return new JsonResult(employee);
        }

        [HttpPut]
        public JsonResult Put(Employee employee)
        {
            string @query = @"UPDATE dbo.Employee 
                                SET EmployeeName = @EmployeeName
                                WHERE EmployeeId = @EmployeeId";
            string dataSource = _configuration.GetConnectionString("MyConnectionString");

            SqlDataReader sqlDataReader;
            DataTable dataTable = new DataTable();
            using(SqlConnection sqlConnection = new SqlConnection(dataSource))
            {
                sqlConnection.Open();
                using(SqlCommand sqlCommand = new SqlCommand(query, sqlConnection))
                {
                    sqlCommand.Parameters.AddWithValue("@EmployeeId", employee.EmployeeId);
                    sqlCommand.Parameters.AddWithValue("@EmployeeName", employee.EmployeeName);
                    sqlDataReader = sqlCommand.ExecuteReader();
                    dataTable.Load(sqlDataReader);
                    sqlDataReader.Close();
                }
            }
            return new JsonResult(dataTable);
        }

    }
}
