namespace Jt76.Common.CommonData.Interfaces
{
	using System.Threading.Tasks;

	public interface IDatabaseInitializer
	{
		Task SeedAsync();
	}
}
