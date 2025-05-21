using System;

namespace MultiplyShiftHashing
{
    class Program
    {
        static void Main(string[] args) {
            byte[] random_Bytes = new byte[]
            {   
                0b01001101,
                0b01101001,
                0b11011111,
                0b01110011,
                0b10001001,
                0b01101011,
                0b01011011, 
                0b10111111
            };
            foreach (byte b in random_Bytes) {
                Console.WriteLine(Convert.ToString(b,2));
            }
            ulong a = BitConverter.ToUInt64(random_Bytes, 0);
            Console.WriteLine(a);
            Console.WriteLine(Convert.ToString((long)a, 2).PadLeft(64, '0'));
            Random rand = new Random();
            int l = rand.Next(1,64);
            ulong x = 1234567890123456789UL;
            ulong hashValue = MultiplyShiftHashing(x, a, l);
            Console.WriteLine($"\nHashing {x} gives: {hashValue}");  
            Console.WriteLine($"\nHashing {x} (binary): {Convert.ToString((long)hashValue, 2).PadLeft(1, '0')}");          
        }
        static ulong MultiplyShiftHashing(ulong x, ulong a, int l) {
            return (a*x) >> (64-l);
        }
    }
}