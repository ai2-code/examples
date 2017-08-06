using System;

namespace base32example1
{
  /// <summary>
  /// Base 32 encoding as defined in rfc4648
  ///
  /// http://tools.ietf.org/html/rfc4648#section-6
  ///
  /// original base32 implementation from Stackoverflow
  /// https://stackoverflow.com/questions/641361/base32-decoding/7135008#7135008
  /// by Shane (https://stackoverflow.com/users/904128/shane)
  /// 
  /// modified by Michael Lakerveld (https://github.com/lakerfield)
  /// </summary>
  public static class Base32Extensions
  {
    public static string ToBase32String(this byte[] input)
    {
      if (input == null || input.Length == 0)
        return string.Empty;

      int charCount = (int)Math.Ceiling(input.Length / 5d * 8);
      char[] returnArray = new char[charCount];

      byte nextChar = 0, bitsRemaining = 5;
      int arrayIndex = 0;

      foreach (byte b in input)
      {
        nextChar = (byte)(nextChar | (b >> (8 - bitsRemaining)));
        returnArray[arrayIndex++] = ValueToChar(nextChar);

        if (bitsRemaining < 4)
        {
          nextChar = (byte)((b >> (3 - bitsRemaining)) & 31);
          returnArray[arrayIndex++] = ValueToChar(nextChar);
          bitsRemaining += 5;
        }

        bitsRemaining -= 3;
        nextChar = (byte)((b << bitsRemaining) & 31);
      }

      //if we didn't end with a full char
      if (arrayIndex != charCount)
        returnArray[arrayIndex++] = ValueToChar(nextChar);

      return new string(returnArray);
    }

    public static string ToBase32StringWithRfc4648Padding(this byte[] input)
    {
      if (input == null || input.Length == 0)
        return string.Empty;

      int charCount = (int)Math.Ceiling(input.Length / 5d) * 8;
      char[] returnArray = new char[charCount];

      byte nextChar = 0, bitsRemaining = 5;
      int arrayIndex = 0;

      foreach (byte b in input)
      {
        nextChar = (byte)(nextChar | (b >> (8 - bitsRemaining)));
        returnArray[arrayIndex++] = ValueToChar(nextChar);

        if (bitsRemaining < 4)
        {
          nextChar = (byte)((b >> (3 - bitsRemaining)) & 31);
          returnArray[arrayIndex++] = ValueToChar(nextChar);
          bitsRemaining += 5;
        }

        bitsRemaining -= 3;
        nextChar = (byte)((b << bitsRemaining) & 31);
      }

      //if we didn't end with a full char
      if (arrayIndex != charCount)
      {
        returnArray[arrayIndex++] = ValueToChar(nextChar);
        while (arrayIndex != charCount) returnArray[arrayIndex++] = '='; //padding
      }

      return new string(returnArray);
    }



    public static byte[] FromBase32StringToBytes(this string input)
    {
      if (string.IsNullOrEmpty(input))
        return new byte[0];

      input = input.TrimEnd('='); //remove padding characters
      int byteCount = input.Length * 5 / 8; //this must be TRUNCATED
      byte[] returnArray = new byte[byteCount];

      byte curByte = 0, bitsRemaining = 8;
      int mask = 0, arrayIndex = 0;

      foreach (char c in input)
      {
        int cValue = CharToValue(c);

        if (bitsRemaining > 5)
        {
          mask = cValue << (bitsRemaining - 5);
          curByte = (byte)(curByte | mask);
          bitsRemaining -= 5;
        }
        else
        {
          mask = cValue >> (5 - bitsRemaining);
          curByte = (byte)(curByte | mask);
          returnArray[arrayIndex++] = curByte;
          curByte = (byte)(cValue << (3 + bitsRemaining));
          bitsRemaining += 3;
        }
      }

      //if we didn't end with a full byte
      if (arrayIndex != byteCount)
      {
        returnArray[arrayIndex] = curByte;
      }

      return returnArray;
    }



    // The Base 32 Alphabet

    // Value Encoding  Value Encoding  Value Encoding  Value Encoding
    //     0 A             9 J            18 S            27 3
    //     1 B            10 K            19 T            28 4
    //     2 C            11 L            20 U            29 5
    //     3 D            12 M            21 V            30 6
    //     4 E            13 N            22 W            31 7
    //     5 F            14 O            23 X
    //     6 G            15 P            24 Y         (pad) =
    //     7 H            16 Q            25 Z
    //     8 I            17 R            26 2		

    private static char ValueToChar(byte value)
    {
      // A-Z
      if (value < 26)
        return (char)('A' + value);

      // 2-7
      if (value < 32)
        return (char)('2' + value - 26);

      throw new ArgumentOutOfRangeException("value", "Invalid value for base32 character");
    }

    private static int CharToValue(char value)
    {
      // Uppercase
      if (value >= 'A' && value <= 'Z')
        return value - 'A';

      // Numbers
      if (value >= '2' && value <= '7')
        return value - '2' + 26;

      // Lowercase for being case invariant
      if (value >= 'a' && value <= 'z')
        return value - 'a';

      throw new ArgumentOutOfRangeException("value", "Invalid base32 character");
    }


  }
}
