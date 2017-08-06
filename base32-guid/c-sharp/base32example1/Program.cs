using System;

namespace base32example1
{
  class Program
  {
    static void Main(string[] args)
    {
      var guid = Guid.NewGuid();
      Console.WriteLine("Hello World!! The Guid: {0}", guid);

      var encoded = guid.ToBase32String();
      Console.WriteLine("encoded with base32 is: {0}", encoded);
    }
  }
}