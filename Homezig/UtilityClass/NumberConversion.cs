using System;
using System.Text;

namespace Homezig
{
	public class NumberConversion
	{
		public static string hex2binary(string hexvalue)
		{
			string binaryval = Convert.ToString(Convert.ToInt32(hexvalue, 16), 2);
			for (var i = binaryval.Length; i < 8; i++) 
			{
				var aStringBuilder = new StringBuilder(binaryval);
				aStringBuilder.Insert(0, "0");
				binaryval = aStringBuilder.ToString();
			}	
			return binaryval;
		}

		public static string binary2hex(string strBinary)
		{
			string strHex = Convert.ToInt32(strBinary,2).ToString("X");
			return strHex;
		}
	}
}

