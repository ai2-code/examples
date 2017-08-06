using System;

namespace base32example1
{
  public static class GuidExtensions
  {
    public static string ToBase32String(this Guid guid)
    {
      var msBytes = guid.ToByteArray();

      if (!BitConverter.IsLittleEndian)
        return msBytes.ToBase32String();

      // Reorder first 8 bytes to expected order
      var bytes = new byte[16];
      bytes[3] = msBytes[0];
      bytes[2] = msBytes[1];
      bytes[1] = msBytes[2];
      bytes[0] = msBytes[3];
      bytes[5] = msBytes[4];
      bytes[4] = msBytes[5];
      bytes[6] = msBytes[7];
      bytes[7] = msBytes[6];

      for (var i = 8; i < 16; i++)
        bytes[i] = msBytes[i];

      return bytes.ToBase32String();
    }

  }
}
