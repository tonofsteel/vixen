﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using Vixen.Attributes;
using Vixen.Module.Effect;
using Vixen.Sys;

namespace System.Windows.Controls.WpfPropertyGrid.Converters
{
	public class TargetElementDepthConverter: TypeConverter
	{
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return true;
		}

		public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
		{
			return true;
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			return Convert.ToSingle(value);	
		}

		public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
		{
			return value.ToString();
		}

		public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
		{
			return true;
		}

		public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
		{
			int depth = 0;
			if (context != null)
			{
				IEffect effect = (IEffect)((PropertyItem) context).UnwrappedComponent;
				
				if (effect.TargetNodes.FirstOrDefault() != null)
				{
					IEnumerable<ElementNode> leafs = effect.TargetNodes.SelectMany(x => x.GetLeafEnumerator());
					depth = leafs.Select(leaf => leaf.GetAllParentNodes().Count()).Concat(new[] {depth}).Max();
				}
			}

			PropertyDescriptor propertyDescriptor = context.PropertyDescriptor;
			var attribute = propertyDescriptor.Attributes[typeof(OffsetAttribute)] as OffsetAttribute;
			int offset = 0;
			if (attribute != null)
			{
				offset = attribute.Offset;
			}
			List<string> values = new List<string>();
			for (int i = 0; i < depth; i++)
			{
				values.Add((offset + i).ToString());	
			}

			return new StandardValuesCollection(values.ToArray());
		}
	}
}
