using System.Numerics;
using System.Diagnostics;
using static System.Math;
class Hash {
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
	
	public static void time_shift_hash(Int64 a, int l, int n)
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

	public static void time_modp_hash(BigInteger a, BigInteger b, int l, int n)
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
}