using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace KioskApplication
{
	public class BadWordFilter
	{
		public static Boolean FindBadWord(string input,Boolean excludeNumbers, int minLength)
		{
			try
			{
				int recordCount = 0;

				// This code is set to exclude numbers from Firstname and Lastname. Other fields may be added later.
				if (excludeNumbers == true && IsAlpha(input) == false && input.Length != 0)
				{
					return true;
				}

				 //If length of text in field is 1 character then likely to be rubbish registration.
				if (input.Length != 0 && input.Length <= minLength)
				{
				    return true;
				}
				
				SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["DefaultConnectionString"]);
				SqlCommand command = new SqlCommand("usp_getBadWordFilter_Find", conn);
				command.CommandType = CommandType.StoredProcedure;
				command.Parameters.Add("@inputString", SqlDbType.VarChar).Value = input;
				conn.Open();

				SqlDataReader reader = command.ExecuteReader();

				while (reader.Read())
				{
					recordCount = (int)reader["RecCount"];
				}

				//reader.Close();
				conn.Close();

				//if (recordCount == 0)
				//{
				//    SqlCommand command2 = new SqlCommand("usp_getBadWordFilter_Find", conn);
				//    command2.CommandType = CommandType.StoredProcedure;
				//    command2.Parameters.Add("@inputString", SqlDbType.VarChar).Value = input;

				//    SqlDataReader reader2 = command.ExecuteReader();
				//    while (reader2.Read())
				//    {
				//        recordCount = (int)reader2["RecCount"];
				//    }
				//}

				if (recordCount > 0)
					return true;
				else
					return false;
			}
			catch
			{
				return false;
			}
		}

		public static Boolean ContactPhoneLength(string input)
		{
			try 
			{
				if (input.Length < 7)
					return true;
				else
					return false;
			}
			catch 
			{
				return false;
			}
		}

		private static bool IsAlpha(string input)
		{
			//return Regex.IsMatch(input, "^[a-zA-Z]+$");
			return Regex.IsMatch(input, "^[A-Za-z-\\s\\']*$");
		}

		public static Boolean FindBadWordX(string input, Boolean excludeNumbers, int minLength, SqlConnection conn, SqlCommand command)
		{
			try
			{
				int recordCount = 0;

				// This code is set to exclude numbers from Firstname and Lastname. Other fields may be added later.
				if (excludeNumbers == true && IsAlpha(input) == false && input.Length != 0)
				{
					return true;
				}

				//If length of text in field is 1 character then likely to be rubbish registration.
				if (input.Length != 0 && input.Length <= minLength)
				{
					return true;
				}

				//SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["DefaultConnectionString"]);
				//SqlCommand command = new SqlCommand("usp_getBadWordFilter_Find", conn);
				command.CommandType = CommandType.StoredProcedure;
				command.Parameters.Add("@inputString", SqlDbType.VarChar).Value = input;
				conn.Open();

				SqlDataReader reader = command.ExecuteReader();

				while (reader.Read())
				{
					recordCount = (int)reader["RecCount"];
				}

				command.Parameters.Remove("@inputString");
				//reader.Close();

				//if (recordCount == 0)
				//{
				//    SqlCommand command2 = new SqlCommand("usp_getBadWordFilter_Find", conn);
				//    command2.CommandType = CommandType.StoredProcedure;
				//    command2.Parameters.Add("@inputString", SqlDbType.VarChar).Value = input;

				//    SqlDataReader reader2 = command.ExecuteReader();
				//    while (reader2.Read())
				//    {
				//        recordCount = (int)reader2["RecCount"];
				//    }
				//}

				conn.Close();

				if (recordCount > 0)
					return true;
				else
					return false;
			}
			catch
			{
				return true;
			}
		}

	}
}