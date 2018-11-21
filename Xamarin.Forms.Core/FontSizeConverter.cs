using System;
using System.Globalization;
using Xamarin.Forms.Xaml;

namespace Xamarin.Forms
{
	[TypeConversion(typeof(double))]
	public class FontSizeConverter : TypeConverter, IExtendedTypeConverter
	{
		[Obsolete("IExtendedTypeConverter.ConvertFrom is obsolete as of version 2.2.0. Please use ConvertFromInvariantString (string, IServiceProvider) instead.")]
		object IExtendedTypeConverter.ConvertFrom(CultureInfo culture, object value, IServiceProvider serviceProvider)
			=> ((IExtendedTypeConverter)this).ConvertFromInvariantString(value as string, serviceProvider);

		object IExtendedTypeConverter.ConvertFromInvariantString(string value, IServiceProvider serviceProvider)
		{
			if (value != null) {
				value = value.Trim();
				if (double.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out double size))
					return size;

				var ignoreCase = (serviceProvider?.GetService(typeof(IConverterOptions)) as IConverterOptions)?.IgnoreCase ?? false;
				var sc = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
				NamedSize? namedSize = null;

				if (value.Equals("Default", sc))
					namedSize = NamedSize.Default;
				else if (value.Equals("Micro", sc))
					namedSize = NamedSize.Micro;
				else if (value.Equals("Small", sc))
					namedSize = NamedSize.Small;
				else if (value.Equals("Medium", sc))
					namedSize = NamedSize.Medium;
				else if (value.Equals("Large", sc))
					namedSize = NamedSize.Large;
				else if (Enum.TryParse(value, ignoreCase, out NamedSize ns))
					namedSize = ns;

				if (namedSize.HasValue) {
					var type = serviceProvider.GetService(typeof(IProvideValueTarget)) is IProvideValueTarget valueTargetProvider ? valueTargetProvider.TargetObject.GetType() : typeof(Label);
					return Device.GetNamedSize(namedSize.Value, type, false);
				}
			}
			throw new InvalidOperationException(string.Format("Cannot convert \"{0}\" into {1}", value, typeof(double)));
		}

		public override object ConvertFromInvariantString(string value)
		{
			if (value != null) {
				if (double.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out double size))
					return size;
				value = value.Trim();

				if (value.Equals("Default", StringComparison.Ordinal))
					return Device.GetNamedSize(NamedSize.Default, typeof(Label), false);
				if (value.Equals("Micro", StringComparison.Ordinal))
					return Device.GetNamedSize(NamedSize.Micro, typeof(Label), false);
				if (value.Equals("Small", StringComparison.Ordinal))
					return Device.GetNamedSize(NamedSize.Small, typeof(Label), false);
				if (value.Equals("Medium", StringComparison.Ordinal))
					return Device.GetNamedSize(NamedSize.Medium, typeof(Label), false);
				if (value.Equals("Large", StringComparison.Ordinal))
					return Device.GetNamedSize(NamedSize.Large, typeof(Label), false);
				if (Enum.TryParse(value, out NamedSize namedSize))
					return Device.GetNamedSize(namedSize, typeof(Label), false);
			}
			throw new InvalidOperationException(string.Format("Cannot convert \"{0}\" into {1}", value, typeof(double)));
		}
	}
}