namespace Jt76.Data.Abstract
{
	using System.ComponentModel.DataAnnotations;

	public abstract class ModelBase
	{
		[Key]
		public int Id { get; protected set; }
	}
}

