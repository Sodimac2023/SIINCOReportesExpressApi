using ConsultasWeb.Model;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;

namespace ConsultasWeb.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ConsultasWebController : ControllerBase
	{
		private readonly OracleConnection _oracleConnection;



		public ConsultasWebController(OracleConnection oracleConnection)
		{
			_oracleConnection = oracleConnection;
		}



		[HttpPost]
		[Route("ExecuteQuery")]
		public IActionResult ExecuteQuery([FromBody] Query query)
		{
			try
			{
				if (string.IsNullOrEmpty(query.queryString))
				{
					return BadRequest("La consulta no puede estar vacía.");
				}



				using (var command = new OracleCommand(query.queryString, _oracleConnection))
				{
					_oracleConnection.Open();



					using (var reader = command.ExecuteReader())
					{
						var results = new List<Dictionary<string, object>>();



						while (reader.Read())
						{
							var row = new Dictionary<string, object>();



							for (var i = 0; i < reader.FieldCount; i++)
							{
								row[reader.GetName(i)] = reader[i];
							}



							results.Add(row);
						}



						return Ok(results);
					}
				}
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"Error al ejecutar la consulta: {ex.Message}");
			}
			finally
			{
				_oracleConnection.Close();
			}
		}
	}
}