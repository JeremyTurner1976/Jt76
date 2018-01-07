namespace Jt76.Common.Extensions
{
	using System;
	using System.Collections.Generic;
	using System.Text;

	public static class ErrorExtensions
	{
		private const string customHtmlCss =
			"style='min-width:800px; padding:40px; border: red 1px solid; border-radius: 15px; margin: 25px; padding-top:15px;'";

		public static string ToHtml(this Exception e)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool first = true;

			while (e != null)
			{
				stringBuilder.Append(first ? "<h3>Server Side Error</h3>" : "<br/>");
				first = false;
				stringBuilder.Append("|Message| " + e.Message + "<br/>");
				stringBuilder.Append("|Source| " + e.Source + "<br/>");
				stringBuilder.AppendLine("[Stack Trace|<br/>");
				foreach (string item in GetStackStraceStrings(e.StackTrace))
					stringBuilder.AppendLine("&nbsp;&nbsp;&nbsp;" + item + "<br/>");
				stringBuilder.AppendLine("<br/>");
				stringBuilder.Append(e.InnerException == null ? "<br/>|No Inner Exception|<br/>" : "<br/>|Inner Exception|: <br/>");
				e = e.InnerException;
			}

			return "<div " + customHtmlCss + ">" + stringBuilder + "</div>";
		}

		public static string ToEnhancedString(this Exception e)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool first = true;

			while (e != null)
			{
				if (!first)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine();
				}
				first = false;
				stringBuilder.AppendLine("|Message| " + e.Message);
				stringBuilder.AppendLine("|Time| " + DateTime.Now);
				stringBuilder.AppendLine("|Source| " + e.Source);
				stringBuilder.AppendLine("[Stack Trace|");
				foreach (string item in GetStackStraceStrings(e.StackTrace))
					stringBuilder.AppendLine("  " + item);
				stringBuilder.AppendLine("");
				stringBuilder.AppendLine(e.InnerException == null ? "|No Inner Exception|" : "\n|Inner Exception|: ");
				e = e.InnerException;
			}

			return stringBuilder.ToString();
		}

		public static string ToHtml(this AggregateException ae)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("|AGGREGATE ERROR|");
			stringBuilder.AppendLine(ToEnhancedString(ae.GetBaseException()));
			stringBuilder.AppendLine("");
			bool first = true;

			foreach (Exception e in ae.InnerExceptions)
			{
				stringBuilder.Append(first ? "<h3>Server Side Error</h3>" : "<br/>");
				first = false;
				stringBuilder.Append("|Message| " + e.Message + "<br/>");
				stringBuilder.Append("|Source| " + e.Source + "<br/>");
				stringBuilder.AppendLine("[Stack Trace|<br/>");
				foreach (string item in GetStackStraceStrings(e.StackTrace))
					stringBuilder.AppendLine("&nbsp;&nbsp;&nbsp;" + item + "<br/>");
				stringBuilder.AppendLine("<br/>");
				stringBuilder.Append(e.InnerException == null ? "<br/>|No Inner Exception|<br/>" : "<br/>|Inner Exception|: <br/>");
			}


			return "<div " + customHtmlCss + ">" + stringBuilder + "</div>";
		}

		public static string ToEnhancedString(this AggregateException ae)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("|AGGREGATE ERROR|");
			stringBuilder.AppendLine(ToEnhancedString(ae.GetBaseException()));
			stringBuilder.AppendLine("");

			foreach (Exception e in ae.InnerExceptions)
			{
				stringBuilder.AppendLine(ToEnhancedString(e));
				stringBuilder.AppendLine("");
			}

			return stringBuilder.ToString();
		}

		private static IEnumerable<string> GetStackStraceStrings(string strStackTrace)
		{
			return strStackTrace == null
				? new[] {""}
				: strStackTrace.Split(new[] {"\r\n", "\n"}, StringSplitOptions.None);
		}
	}
}