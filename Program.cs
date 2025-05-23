using System.Numerics;
using System.Diagnostics;
using static System.Math;

class Program
{
    static Int64 shift_hash(Int64 a, Int64 x, int l)
    {
        return (a*x)>>(64-l);
    }
    static Int64 mod_prime_hash(BigInteger a, Int64 x, BigInteger b, int l) 
	{
		int q = 89;
        BigInteger p = BigInteger.Pow(2,q)-1;
		BigInteger axb = a * new BigInteger(x) + b;
		
		BigInteger y =(axb&p)+(axb>>q); // (a*x+b) mod p
		if (y>=p) {
			y-=p;	
		} 
		BigInteger lpow = BigInteger.Pow(2,l);
		return (Int64)(y % lpow); 
    }
	
	static void time_shift_hash(Int64 a, int l, int n)
	{
		IEnumerable<Tuple<ulong, int>> rand_Stream = Stream.CreateStream(n,l); 
		BigInteger s_hash_sum = 0;
		
		System.Console.WriteLine("Starting shift hash:");
		Stopwatch watch = Stopwatch.StartNew();
		foreach (var pair in rand_Stream) {
			s_hash_sum += shift_hash(a, (long)pair.Item1, l);
		}
		watch.Stop();
		System.Console.WriteLine($"Hash Sum: {s_hash_sum}, time taken: {watch.ElapsedMilliseconds} ms");	
	}
	static void time_modp_hash(BigInteger a, BigInteger b, int l, int n)
	{
		IEnumerable<Tuple<ulong, int>> rand_Stream = Stream.CreateStream(n,l); 
		BigInteger mp_hash_sum = 0;
		
		System.Console.WriteLine("Starting mod prime hash:");
		Stopwatch watch = Stopwatch.StartNew();
		foreach (var pair in rand_Stream) {
			mp_hash_sum += mod_prime_hash(a, (long)pair.Item1, b, l);
		}
		watch.Stop();
		System.Console.WriteLine($"Hash Sum: {mp_hash_sum}, time taken: {watch.ElapsedMilliseconds} ms");	
	}
    static void Main()
    {
        byte[] a = new byte[] { 0b10001101, 0b01110010, 0b01011000, 0b10101100, 0b10000000,
                                0b01100011, 0b00001100, 0b10010011 }; //random uneven 64 bit number
		Int64 int_a = BitConverter.ToInt64(a,0);
        Random rand = new Random();
		int l = rand.Next(1, 65); //random number between 1 and 64
		
		byte[] a88 = new byte[] {0b10001101,0b01001000,0b10110011,0b01100011,0b11011111,0b00110010,
								0b11111110,0b11010101,0b11111010,0b01011111,0b00100000}; //random 88 bit number (not all 1's)
		BigInteger big_a = new(a88);
		byte[] b88 = new byte[] {0b10010100,0b01101011,0b01100110,0b10000000,0b10100010,0b00001000,
								0b10110011,0b10001000,0b01001111,0b01110000,0b01001101};
		BigInteger big_b = new(b88);
		
		int n = 1000000; // key amount
		
		time_shift_hash(int_a, l, n);
		time_modp_hash(big_a, big_b, l, n);
    }
}
