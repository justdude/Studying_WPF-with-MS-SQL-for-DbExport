using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DbExport.Database
{
	public static class Generator
	{
		public static string GenerateID(string name)
		{
			//System.Threading.Thread.Sleep(1);
			//return DateTime.Now.GetHashCode().ToString("x");
			return Guid.NewGuid().ToString("N") + name + DateTime.Now.ToString("ddMMyy");
		}

		private static string GenerateIDByRNGCrypto()
		{ 
			int maxSize  = 8 ;
			int minSize = 5 ;
			char[] chars = new char[62];
			string a;
			a = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
			chars = a.ToCharArray();
			int size  = maxSize ;
			byte[] data = new byte[1];
			RNGCryptoServiceProvider  crypto = new RNGCryptoServiceProvider();
			crypto.GetNonZeroBytes(data) ;
			size =  maxSize ;
			data = new byte[size];
			crypto.GetNonZeroBytes(data);
			StringBuilder result = new StringBuilder(size) ;
			foreach(byte b in data )
			{ 
				result.Append(chars[b % (chars.Length)]); 
			}
			return result.ToString();
		}

	}
}
