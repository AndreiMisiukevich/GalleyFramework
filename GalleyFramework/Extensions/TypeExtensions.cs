using System;
using System.Reflection;

namespace GalleyFramework.Extensions
{
    public static class TypeExtensions
    {
		public static Type GetNullableType(this Type type)
		{
			type = Nullable.GetUnderlyingType(type);
			return type.GetTypeInfo().IsValueType
					   ? typeof(Nullable<>).MakeGenericType(type)
						   : type;
		}
    }
}
