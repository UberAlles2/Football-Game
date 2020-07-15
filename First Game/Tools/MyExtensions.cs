using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Drake.Tools
{
  public static class MyExtensions
  {
    #region String Extensions -------------------------------------------------------

    /// <summary>
    /// Get string value between [first] a and [next] b.
    /// </summary>
    public static string _Between(this string value, string betweenBegin, string betweenEnd, bool caseSensitive = true)
    {
      int posA;
      int posB;

      if (caseSensitive)
        posA = value.IndexOf(betweenBegin);
      else
        posA = value.ToLower().IndexOf(betweenBegin.ToLower());

      if (caseSensitive)
        posB = value.IndexOf(betweenEnd, posA + betweenBegin.Length);
      else
        posB = value.ToLower().IndexOf(betweenEnd.ToLower(), posA + betweenBegin.Length);

      if (posA == -1)
      {
        return "";
      }
      if (posB == -1)
      {
        return "";
      }
      int adjustedPosA = posA + betweenBegin.Length;
      if (adjustedPosA >= posB)
      {
        return "";
      }
      return value.Substring(adjustedPosA, posB - adjustedPosA);
    }


    /// <summary>
    /// Get string value between [first] a and [last] b.
    /// </summary>
    public static string _BetweenFirstLast(this string value, string a, string b)
    {
      int posA = value.IndexOf(a);
      int posB = value.LastIndexOf(b);
      if (posA == -1)
      {
        return "";
      }
      if (posB == -1)
      {
        return "";
      }
      int adjustedPosA = posA + a.Length;
      if (adjustedPosA >= posB)
      {
        return "";
      }
      return value.Substring(adjustedPosA, posB - adjustedPosA);
    }

    /// <summary>
    /// Get string value between [first] a and [first after] b.
    /// </summary>
    public static string _RemoveBetween(this string value, string a, string b)
    {
      int posA = value.IndexOf(a);
      int posB = value.IndexOf(b, posA + 1);
      if (posA == -1)
      {
        return value;
      }
      if (posB == -1)
      {
        return value;
      }
      int adjustedPosA = posA + a.Length;
      if (adjustedPosA > posB)
      {
        return "";
      }
      return value.Remove(adjustedPosA, posB - adjustedPosA);
    }

    /// <summary>
    /// Get string value between [first] a and [first after] b.
    /// </summary>
    public static string _ReplaceAtPosition(this string value, int startPosition, int length, string replacementString)
    {
      var aStringBuilder = new StringBuilder(value);
      aStringBuilder.Remove(startPosition, length);
      aStringBuilder.Insert(startPosition, replacementString);

      return aStringBuilder.ToString();
    }


    /// <summary>
    /// Capitalizes the first letter of a string. This is not camelcase.
    /// </summary>
    /// <returns>The modified string.</returns>
    public static string _CapitalizeFirstCharacter(this string value)
    {
      if (String.IsNullOrEmpty(value))
        return value;

      return value.First().ToString().ToUpper() + String.Join("", value.Skip(1));
    }

    /// <summary>
    /// Removes all instances of a character value from a string.
    /// </summary>
    /// <param name="stripChar">Character to remove from string</param>
    /// <returns>The modified string.</returns>
    public static string _Strip(this string value, char stripChar)
    {
      if (String.IsNullOrEmpty(value))
        return value;

      return new String(value.ToCharArray().Where(x => x != stripChar).ToArray());
    }

    public static Dictionary<string, string> ToDictionary(this string s, char valueDelim, char pairDelim)
    {
      var segments = s.Split(new char[] { pairDelim }, StringSplitOptions.RemoveEmptyEntries);
      var entries = segments.Select(item => item.Split(new char[] { valueDelim }, StringSplitOptions.RemoveEmptyEntries));
      var kvps = entries.Select(kvp => new KeyValuePair<string, string>(kvp[0].Trim(), kvp.Length > 1 ? kvp[1] : string.Empty));
      return kvps.ToDictionary(k => k.Key, v => v.Value, StringComparer.OrdinalIgnoreCase);
    }

    #endregion

    public static void _Add<T, T2>(this List<KeyValuePair<T, T2>> list, T key, T2 value)
    {
      KeyValuePair<T, T2> kvp = new KeyValuePair<T, T2>(key, value);
      list.Add(kvp);
    }

    #region Object Extensions -------------------------------------------------------

    //public static U CloneAndUpcast<U, B>(this B b) where U : B, new()
    //{
    //  U clone = new U();

    //  var members = b.GetType().GetMembers(BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance);
    //  for (int i = 0; i < members.Length; i++)
    //  {
    //    if (members[i].MemberType == MemberTypes.Property)
    //    {
    //      clone
    //          .GetType()
    //          .GetProperty(members[i].Name)
    //          .SetValue(clone, b.GetType().GetProperty(members[i].Name).GetValue(b, null), null);
    //    }

    //  }
    //  return clone;
    //}

    #endregion
  }
}
