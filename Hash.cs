using System.Numerics;
using System.Diagnostics;
using static System.Math;
using System.Xml;
class Hash {
	static readonly byte[] a = new byte[] { 0b10001101, 0b01110010, 0b01011000, 0b10101100, 0b10000000,
                            0b01100011, 0b00001100, 0b10010011 }; //random uneven 64 bit number
	static readonly byte[] a88 = new byte[] {0b10001101,0b01001000,0b10110011,0b01100011,0b11011111,
							0b00110010,0b11111110,0b11010101,0b11111010,0b01011111,
							0b00100000}; //random 88 bit number (not all 1's)
	static readonly byte[] b88 = new byte[] {0b10010100,0b01101011,0b01100110,0b10000000,0b10100010,
							0b00001000,0b10110011,0b10001000,0b01001111,0b01110000,
							0b01001101};
	static readonly Int64 int_a = BitConverter.ToInt64(a,0);
	static readonly BigInteger big_a = new(a88);
	static readonly BigInteger big_b = new(b88);
	
	
	public static Int64 shift_hash(Int64 x, int l)
    {
        return (int_a*x)>>(64-l);
    }

    public static Int64 mod_prime_hash(Int64 x, int l) 
	{
		int q = 89;
        BigInteger p = BigInteger.Pow(2,q)-1;
		BigInteger axb = big_a * new BigInteger(x) + big_b;
		
		BigInteger y =(axb&p)+(axb>>q); // (a*x+b) mod p
		if (y>=p) {
			y-=p;	
		} 
		BigInteger lpow = BigInteger.Pow(2,l);
		return (Int64)(y % lpow); 
    }
	
	public static void time_hash(Func<Int64,int,Int64> hash, int l, int n)
	{
		IEnumerable<Tuple<ulong, int>> rand_Stream = Stream.CreateStream(n,l); 
		BigInteger hash_sum = 0;
		
		System.Console.WriteLine($"Starting {hash.Method.Name} hash:");
		Stopwatch watch = Stopwatch.StartNew();
		foreach (var pair in rand_Stream) {
			hash_sum += hash((long)pair.Item1, l);
		}
		watch.Stop();
		System.Console.WriteLine($"Hash Sum: {hash_sum}, time taken: {watch.ElapsedMilliseconds} ms");	
	}
}