namespace Jt76.Common.Extensions
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using Interfaces;

	public static class EnumExtensions
	{
		public static string ToNameString(this Enum enumSource)
		{
			Type type = enumSource.GetType();
			return Enum.GetName(type, enumSource);
		}

		public static string ToAttributeString<T>(this Enum enumSource) where T : Attribute, IAttribute
		{
			Type type = enumSource.GetType();
			MemberInfo[] memInfo = type.GetMember(enumSource.ToString());

			if (memInfo != null && memInfo.Length > 0)
			{
				IEnumerable<Attribute> attrs = (IEnumerable < Attribute >)
					memInfo[0].GetCustomAttributes(typeof (T), false);

				Attribute[] attributes = attrs as Attribute[] ?? attrs.ToArray();
				if (attrs != null && attributes.Any())
				{
					return ((T) attributes[0]).Identifier;
				}
			}

			return enumSource.ToNameString();
			;
		}
	}
}