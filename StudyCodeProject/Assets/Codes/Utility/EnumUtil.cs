using System.Collections.Generic;

public class EnumUtil<TEnum> where TEnum : struct
{
	protected static EnumUtil<TEnum> _instance;
	protected readonly TEnum[] _values;
	protected readonly string[] _stringValues;
	protected readonly Dictionary<string, TEnum> _stringEnumMap = new Dictionary<string, TEnum>();
	protected readonly Dictionary<TEnum, string> _enumStringMap = new Dictionary<TEnum, string>();

	public static EnumUtil<TEnum> Instance
	{
		get
		{
			if( _instance == null )
			{
				_instance = new EnumUtil<TEnum>();
			}

			return _instance;
		}
	}

	public int EnumCount { get { return _values.Length; } }
	public int EnumCountContainsInvalidValue { get { return _values.Length - 1; } }
	public TEnum[ ] EnumValues { get { return _values; } }
	public string[ ] EnumStringValues { get { return _stringValues; } }

	protected EnumUtil()
	{
		if( false == typeof( TEnum ).IsEnum )
		{
			throw new System.ArgumentException( "TEnum must be an enumerated type" );
		}

		System.Array array = System.Enum.GetValues(typeof(TEnum));

		_values = new TEnum[ array.Length ];
		for( int i = 0; i < array.Length; ++i )
		{
			TEnum e = (TEnum)array.GetValue(i);
			_values[ i ] = e;

			if( false == _enumStringMap.ContainsKey( e ) )
			{
				_enumStringMap.Add( e, e.ToString() );
			}
		}

		_stringValues = new string[ _values.Length ];
		for( int i = 0; i < _values.Length; ++i )
		{
			string str = _values[i].ToString();
			_stringValues[ i ] = str;

			if( false == _stringEnumMap.ContainsKey( str ) )
			{
				_stringEnumMap.Add( str, _values[ i ] );
			}
		}
	}
	public bool IsDefined( TEnum enumValue )
	{
		return System.Array.IndexOf( _values, enumValue ) != -1;
	}
	public bool IsDefined( string stringValue )
	{
		if( string.IsNullOrEmpty( stringValue ) )
		{
			return false;
		}

		return _stringEnumMap.ContainsKey( stringValue );
	}

	public TEnum Parse( string stringValue )
	{
		return _stringEnumMap[ stringValue ];
	}

	public bool TryParse( string stringValue, out TEnum enumValue )
	{
		if( IsDefined( stringValue ) )
		{
			enumValue = Parse( stringValue );
			return true;
		}
		else
		{
			enumValue = default( TEnum );
			return false;
		}
	}

	public string GetEnumString( TEnum enumValue )
	{
		return _enumStringMap[ enumValue ];
	}
}