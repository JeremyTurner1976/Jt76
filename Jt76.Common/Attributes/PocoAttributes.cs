namespace Jt76.Common.Attributes
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;
	using Interfaces;

	public class PocoAttributes
	{
		public class DisplayName : Attribute, IAttribute
		{
			public DisplayName(string value)
			{
				Identifier = value;
			}

			public string Identifier { get; set; }
		}

		public class DateTimeKindSetting : Attribute
		{
			public DateTimeKindSetting(DateTimeKind kind)
			{
				Kind = kind;
			}

			public DateTimeKind Kind { get; }

			public static void Apply(object entity)
			{
				if (entity == null)
					return;

				IEnumerable<PropertyInfo> properties = entity.GetType()
					.GetProperties()
					.Where(x => x.PropertyType == typeof(DateTime) || x.PropertyType == typeof(DateTime?));

				foreach (PropertyInfo property in properties)
				{
					DateTimeKindSetting attr = property.GetCustomAttribute<DateTimeKindSetting>();
					if (attr == null)
						continue;

					DateTime? dt = property.PropertyType == typeof(DateTime?)
						? (DateTime?) property.GetValue(entity)
						: (DateTime) property.GetValue(entity);

					if (dt == null)
						continue;

					property.SetValue(entity, DateTime.SpecifyKind(dt.Value, attr.Kind));
				}
			}
		}

		public class CleanedHtmlString : Attribute
		{
			public static void Apply(object entity)
			{
				if (entity == null)
					return;

				IEnumerable<PropertyInfo> properties = entity.GetType()
					.GetProperties()
					.Where(x => x.PropertyType == typeof(string));

				foreach (PropertyInfo property in properties)
				{
					string strItem = (string) property.GetValue(entity);

					string[] lines = strItem.Split(
							new[] {Environment.NewLine},
							StringSplitOptions.None)
						.ToArray();

					string cleanedHtmlString = string.Join("<br/>", lines);

					property.SetValue(entity, cleanedHtmlString);
				}
			}
		}
	}
}