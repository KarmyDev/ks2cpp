namespace Ks2Cpp
{
	public class Token
	{
		public object Value {get; private set;}
		public string Type {get; private set;}
		
		public Token (string type, object value)
		{
			Type = type;
			Value = value;
		}
	}
}