using System;
using System.Security.Cryptography;
using System.Text;

namespace Webstream {
  /// <summary>
  /// Provides functionality for encoding strings for use within URLs
  /// </summary>
  public sealed class UrlEncoding {
    public UrlEncoding() {
    }
    /// <summary>
    /// Converts a UTF8 string to a Base64 string
    /// </summary>
    /// <param name="value">The string to encode</param>
    /// <returns>The encoded string</returns>
    public static string Base64UrlEncode(string value) {
      byte[] bytes = Encoding.UTF8.GetBytes(value);
      string base64encoded = Convert.ToBase64String(bytes).Replace("+", "-").Replace("/", "_").Replace("=", "");
      return base64encoded;
    }
    public static string Base64UrlEncode(byte[] bytes) {
      string base64encoded = Convert.ToBase64String(bytes).Replace("+", "-").Replace("/", "_").Replace("=", "");
      return base64encoded;
    }
    /// <summary>
    /// Converts a Base64 string to a UTF8 string
    /// </summary>
    /// <param name="value">The string to decode</param>
    /// <returns>The decoded string</returns>
    public static string Base64UrlDecode(string value) {
      string raw = value.Replace("-", "+").Replace("_", "/");
      int mod4 = raw.Length % 4;
      if (mod4 != 0)  // check for extra padding
        raw += "====".Substring(mod4);
      byte[] bytes = Convert.FromBase64String(raw);
      return Encoding.UTF8.GetString(bytes);
    }
    /// <summary>
    /// Generates an SHA1 hash of a UTF8 string, and converts
    /// it to a Base64 string
    /// </summary>
    /// <param name="value">The string to encode</param>
    /// <returns>The encoded hash string</returns>
    public static string HashBase64UrlEncode(string value) {
      byte[] bytes = Encoding.UTF8.GetBytes(value);
      byte[] hash = SHA1.Create().ComputeHash(bytes);
      string base64encoded = Convert.ToBase64String(hash);
      return base64encoded.Replace("+", "-").Replace("/", "_").Replace("=", "");
    }
    /// <summary>
    /// Converts a Base64 string to a UTF8 string, assumes it is a
    /// SHA1 hash, and converts it to a hexadecimal representation of that hash
    /// </summary>
    /// <param name="value">The string to decode</param>
    /// <returns>The decoded hash string</returns>
    public static string HashBase64UrlDecode(string value) {
      string raw = value.Replace("-", "+").Replace("_", "/");
      int mod4 = raw.Length % 4;
      if (mod4 != 0)  // check for extra padding
        raw += "====".Substring(mod4);
      byte[] hash = Convert.FromBase64String(raw);
      string decoded = string.Empty;
      for (int b = 0; b < hash.Length; b++)
        decoded += hash[b].ToString("x2");
      return decoded;
    }
  }
}
