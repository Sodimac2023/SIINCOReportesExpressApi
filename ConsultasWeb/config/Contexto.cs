using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ConsultasWeb.config
{
	public class Contexto : DbContext
	{
		public Contexto(DbContextOptions<Contexto> options) : base(options)
		{
			Database.EnsureCreated();
		}

	}
}
