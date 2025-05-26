using System;
using System.Numerics;
using System.Diagnostics;

namespace MultiplyShiftHashing
{
    class Program
    {
        static void Main(string[] args)
        {
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
            byte[] randomBytes89a = new byte[]
            {
                0b11001011,
                0b10001110,
                0b11011001,
                0b10101111,
                0b01110010,
                0b10011110,
                0b10010010,
                0b00011001,
                0b10011000,
                0b10101001,
                0b01101110,
                0b00100000
            };
            byte[] randomBytes89b = new byte[]
            {
                0b00011010,
                0b01101110,
                0b00001100,
                0b00110011,
                0b10000000,
                0b01011001,
                0b00011111,
                0b00100100,
                0b00010011,
                0b10000110,
                0b11110010,
                0b01111101
            };

            Console.WriteLine("Starting task 1.a...\n");
            foreach (byte b in random_Bytes)
            {
                Console.WriteLine(Convert.ToString(b, 2));
            }
            ulong a = BitConverter.ToUInt64(random_Bytes, 0);
            Console.WriteLine(a);
            Console.WriteLine(Convert.ToString((long)a, 2).PadLeft(64, '0'));
            Random rand = new Random();
            int l = rand.Next(1, 64);
            ulong x = 1234567890123456789UL;
            ulong hashValue = MultiplyShiftHashing(x, a, l);
            Console.WriteLine($"Hashing {x} gives: {hashValue}");
            Console.WriteLine($"Hashing {x} in binary: {Convert.ToString((long)hashValue, 2).PadLeft(1, '0')}");
            Console.WriteLine("Starting task 1.b...\n");
            BigInteger bigInt89a = BytesToInteger89Bit(randomBytes89a);
            BigInteger bigInt89b = BytesToInteger89Bit(randomBytes89b);
            BigInteger hashValue89 = MultiModPrimeHashing(x, bigInt89a, bigInt89b, l);
            Console.WriteLine($"MultiModPrimeHashing with a = {bigInt89a} and b = {bigInt89b}.");
            Console.WriteLine($"Hashing {x} gives: {hashValue89}");
            Console.WriteLine($"Hashing {x} in binary: {BigIntegerToBinary(hashValue89, l)}");
            Console.WriteLine("*****Missing check for all 1s*****");
            int n = 10000;
            Stopwatch watch = Stopwatch.StartNew();
            ulong mulShiftCount = 0UL;
            foreach (var tup in CreateStream(n, l))
            {
                ulong key = tup.Item1;
                ulong hash1 = MultiplyShiftHashing(key, a, l);
                mulShiftCount += hash1;
            }
            watch.Stop();
            Console.WriteLine($"Sum of MultiplyShiftHashing: {mulShiftCount} (Time: {watch.ElapsedMilliseconds} ms)");

            watch.Restart();
            BigInteger mulPrimeCount = BigInteger.Zero;
            foreach (var tup in CreateStream(n, l))
            {
                ulong key = tup.Item1;
                BigInteger hash2 = MultiModPrimeHashing(key, bigInt89a, bigInt89b, l);
                mulPrimeCount += hash2;
            }
            Console.WriteLine($"Sum of MultiModPrimeHashing: {mulPrimeCount} (Time: {watch.ElapsedMilliseconds} ms)");
        }
        static ulong MultiplyShiftHashing(ulong x, ulong a, int l)
        {
            return (a * x) >> (64 - l);
        }
        static BigInteger MultiModPrimeHashing(ulong x, BigInteger a, BigInteger b, int l)
        {
            BigInteger p = BigInteger.Pow(2, 89) - 1;
            BigInteger val = (a * new BigInteger(x) + b) % p;
            BigInteger modL = BigInteger.Pow(2, l);
            return (val % modL);
        }
        static BigInteger BytesToInteger89Bit(byte[] bytes)
        {
            bytes[11] &= 0b00000001;
            byte[] extended = new byte[bytes.Length + 1];
            Array.Copy(bytes, extended, bytes.Length);
            return new BigInteger(extended);
        }
        static string BigIntegerToBinary(BigInteger value, int bits = 0)
        {
            if (value == 0) return new string('0', bits == 0 ? 1 : bits);
            var sb = new System.Text.StringBuilder();
            BigInteger v = value;
            while (v > 0)
            {
                sb.Insert(0, (v % 2).ToString());
                v /= 2;
            }
            string binary = sb.ToString();
            if (bits > 0)
                binary = binary.PadLeft(bits, '0');
            return binary;
        }

        public static IEnumerable<Tuple<ulong, int>> CreateStream(int n, int l)
        {
            // We generate a random uint64 number .
            Random rnd = new System.Random();
            ulong a = 0UL;
            Byte[] b = new Byte[8];
            rnd.NextBytes(b);
            for (int i = 0; i < 8; ++i)
            {
                a = (a << 8) + (ulong)b[i];
            }
            // We demand that our random number has 30 zeros on the least
            // significant bits and then a one.
            a = (a | ((1UL << 31) - 1UL)) ^ ((1UL << 30) - 1UL);
            ulong x = 0UL;
            for (int i = 0; i < n / 3; ++i)
            {
                x = x + a;
                yield return Tuple.Create(x & (((1UL << l) - 1UL) << 30), 1);
            }
            for (int i = 0; i < (n + 1) / 3; ++i)
            {
                x = x + a;
                yield return Tuple.Create(x & (((1UL << l) - 1UL) << 30), -1);
            }
            for (int i = 0; i < (n + 2) / 3; ++i)
            {
                x = x + a;
                yield return Tuple.Create(x & (((1UL << l) - 1UL) << 30), 1);
            }
        }
        class Hashtable
        {
            List<KeyValuePair<ulong, long>>[] hashPairs;
            ulong a;
            int l;
            public Hashtable(ulong a, int l)
            {
                this.a = a;
                this.l = l;
                int size = 1 << l;
                hashPairs = new List<KeyValuePair<ulong, long>>[size];
                for (int i = 0; i < size; i++)
                    hashPairs[i] = new List<KeyValuePair<ulong, long>>();
            }

            public int Hash(ulong x)
            {
                return (int)(MultiplyShiftHashing(x, a, l));
            }
            public long Get(ulong x) {
                int idx = Hash(x);
                foreach (var pair in hashPairs[idx])
                    if (pair.key == x)
                        return pair.value;
                return 0;
            }
            public void Set(ulong x, long v) {
                int idx = Hash(x);
                for (int i = 0; i < hashPairs[idx].Count; i++)
                {
                    if (hashPairs[idx][i].Key == x)
                    {
                        hashPairs[idx][i] = new KeyValuePair<ulong, long>(x, v);
                        return;
                    }
                }
                hashPairs[idx].Add(new KeyValuePair<ulong, long>(x, v));
                
            }
        }
    }
}