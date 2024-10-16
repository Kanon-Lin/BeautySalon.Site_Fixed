using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace BeautySalon.FrontEnd.Site.Models.Infra
{
	public class HashUtility
	{
		public static string GetSalt()
		{
			return System.Configuration.ConfigurationManager.AppSettings["Salt"];
		}
		public static string ToSHA256(string plainText, string salt = null)
		{
			//檢查是否有傳入salt,若無則取得web.config中的Salt
			salt = salt ?? GetSalt();

			//使用SHA256加密
			using (var mySHA256 = SHA256.Create())
			{
				//將輸入值與salt結合後轉為byte
				var bytes = System.Text.Encoding.UTF8.GetBytes(plainText + salt);
				//取得加密後的byte
				var hash = mySHA256.ComputeHash(bytes);

				//將byte轉為16進制字串
				var sb = new StringBuilder();
				foreach (var b in hash)
				{
					sb.Append(b.ToString("X2")); //把byte转化为16进制,字母為大写
				}
				return sb.ToString();

			}
		}
	}
}