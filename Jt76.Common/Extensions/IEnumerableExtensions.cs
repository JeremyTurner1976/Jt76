using System;
using System.Collections.Generic;
using System.Text;

namespace Jt76.Common.Extensions
{
    public static class IEnumerableExtensions
    {
	    public static IEnumerable<T> TakeLast<T>(
			this IEnumerable<T> collection,
		    int count)
	    {
		    if (collection == null)
			    throw new ArgumentNullException(nameof(collection));
		    if (count < 0)
			    throw new ArgumentOutOfRangeException(
					nameof(count), 
					"count must be 0 or greater");

		    LinkedList<T> temp = new LinkedList<T>();

		    foreach (T value in collection)
		    {
			    temp.AddLast(value);
			    if (temp.Count > count)
				    temp.RemoveFirst();
		    }

		    return temp;
	    }
	}
}
