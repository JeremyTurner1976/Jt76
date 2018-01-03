using System;
using System.Collections.Generic;
using System.Text;

namespace Jt76.Data.Interfaces
{
	using System.Threading.Tasks;

	public interface IDatabaseInitializer
	{
		Task SeedAsync();
	}
}
