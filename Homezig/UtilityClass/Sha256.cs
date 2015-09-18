using System;
using System.Text;
using PCLCrypto;

namespace Homezig
{
	public class Sha256
	{
		public Sha256 ()
		{
			
		}

		public static String sha256_hash(String password) {
			
			StringBuilder Sb = new StringBuilder();
			Encoding enc = Encoding.UTF8;
			var hasher = WinRTCrypto.HashAlgorithmProvider.OpenAlgorithm(HashAlgorithm.Sha256);
			byte[] hash = hasher.HashData(enc.GetBytes(password));

			foreach (Byte b in hash)
				Sb.Append(b.ToString("x2"));
			
			return Sb.ToString ();
		}
	}
}

