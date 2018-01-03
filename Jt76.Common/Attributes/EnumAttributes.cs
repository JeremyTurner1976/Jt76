namespace Jt76.Common.Attributes
{
	using System;
	using Interfaces;

	public class EnumAttributes
	{
		public class ClientSideString : Attribute, IAttribute
		{
			public ClientSideString(string value)
			{
				Identifier = value;
			}

			public string Identifier { get; set; }
		}

		public class FileName : Attribute, IAttribute
		{
			public FileName(string value)
			{
				Identifier = value;
			}

			public string Identifier { get; set; }
		}
	}
}