using System;

namespace libAniDB.NET
{
	internal abstract class FieldData
	{
		protected FieldData(string name, Type type)
		{
			Name = name;
			DataType = type;
		}

		public readonly string Name;
		public readonly Type DataType;

		public abstract object GetValue();
		public abstract void SetValue(object value);
	}

	internal class FieldData<T> : FieldData
	{
		internal FieldData(string name)
			: base(name, typeof(T)) {}

		internal FieldData(string name, T value)
			: this(name)
		{
			Value = value;
		}

		internal FieldData() : this("") {}

		public T Value { get; set; }

		public override object GetValue()
		{
			return Value;
		}

		public override void SetValue(object value)
		{
			if (value != null && !(value is T))
				throw new InvalidCastException("Invalid parameter type: " + value.GetType() + Environment.NewLine
											   + "Expected type: " + typeof(T));

			Value = (T)value;
		}
	}

}