using System.Numerics;
using System.Diagnostics;
class Hash
{
	static readonly byte[] a = new byte[] { 0b10001101, 0b01110010, 0b01011000, 0b10101100, 0b10000000,
							0b01100011, 0b00001100, 0b10010011 }; //random uneven 64 bit number
	static readonly byte[] a88 = new byte[] {0b10001101,0b01001000,0b10110011,0b01100011,0b11011111,
							0b00110010,0b11111110,0b11010101,0b11111010,0b01011111,
							0b00100000}; //random 88 bit number (not all 1's)
	static readonly byte[] b88 = new byte[] {0b10010100,0b01101011,0b01100110,0b10000000,0b10100010,
							0b00001000,0b10110011,0b10001000,0b01001111,0b01110000,
							0b01001101};

	static readonly byte[] a0 = new byte[] {0b00100100, 0b10011101, 0b01001010, 0b00100010, 0b11100011, 0b10100011, 0b01101001, 0b11111100, 0b01100110,
							0b01010110, 0b00010111};
	static readonly byte[] a1 = new byte[] {0b10110011, 0b10111111, 0b01101010, 0b10100001, 0b10101110, 0b00010010, 0b11001110, 0b11110010,
							0b11110101, 0b00010101, 0b11101101};
	static readonly byte[] a2 = new byte[] {0b10011000, 0b10111100, 0b10111000, 0b00100101, 0b00110110, 0b00011110, 0b01101001, 0b11001011,
							0b01000011, 0b01011111, 0b00111000};
	static readonly byte[] a3 = new byte[] {0b01100111, 0b10111110, 0b00001010, 0b00010111, 0b00011000, 0b00100000, 0b00001001, 0b11111000,
							0b01101001, 0b00111011, 0b10111011};
	static readonly UInt64 int_a = BitConverter.ToUInt64(a, 0);
	static readonly BigInteger big_a = new(a88, isUnsigned: true);
	static readonly BigInteger big_b = new(b88, isUnsigned: true);
	static readonly BigInteger big_a0 = new(a0, isUnsigned: true);
	static readonly BigInteger big_a1 = new(a1, isUnsigned: true);
	static readonly BigInteger big_a2 = new(a2, isUnsigned: true);
	static readonly BigInteger big_a3 = new(a3, isUnsigned: true);

	static  BigInteger[] a_array = {big_a0, big_a1, big_a2, big_a3};
	public static UInt64 shift_hash(UInt64 x, int l)
	{
		return (int_a * x) >> (64 - l);
	}

	public static UInt64 mod_prime_hash(UInt64 x, int l)
	{
		int q = 89;
		BigInteger p = (BigInteger.One << q) - 1;
		BigInteger axb = big_a * x + big_b;

		BigInteger x_mod_p = (axb & p) + (axb >> q); // (a*x+b) mod p
		if (x_mod_p >= p)
		{
			x_mod_p -= p;
		}

		BigInteger m = (BigInteger.One << l) - 1;

		BigInteger output = x_mod_p & m; // ((a*x+b) mod p) mod m
		if (output >= m)
		{
			output -= m;
		}
		return (UInt64)output;
	}

	public static void time_hash(Func<UInt64, int, UInt64> hash, int l, int n)
	{
		IEnumerable<Tuple<ulong, int>> rand_stream = Stream.CreateStream(n, l);
		BigInteger hash_sum = 0;

		System.Console.WriteLine($"Starting {hash.Method.Name} hash:");
		Stopwatch watch = Stopwatch.StartNew();
		foreach (var pair in rand_stream)
		{
			hash_sum += hash(pair.Item1, l);
		}
		watch.Stop();
		System.Console.WriteLine($"Hash Sum: {hash_sum}, time taken: {watch.ElapsedMilliseconds} ms");
	}
	public static BigInteger poly_hash(UInt64 x)
	{
		int k = 4;
		int b = 89;
		BigInteger p = (BigInteger.One << b) - 1;
		BigInteger y = a_array[k - 1];
		for (int i = k - 2; i >= 0; i--)
		{
			y = y * x + a_array[i];
			y = (y & p) + (y >> b);
			if (y >= p)
			{
				y -= p;
			}
		}
		return y;
	}
	public static BigInteger cs_hash(Func<UInt64, BigInteger>, UInt64 x, int l) {
		int b = 89;
		BigInteger h = poly_hash(x) & ((BigInteger.One << b)-1);
		BigInteger s = 1 - 2 * (poly_hash(x) >> b);
	}
	public static void time_poly_hash(int l, int n)
	{
		IEnumerable<Tuple<ulong, int>> rand_stream = Stream.CreateStream(n, l);
		BigInteger hash_sum = 0;

		System.Console.WriteLine($"Starting poly_hash:");
		Stopwatch watch = Stopwatch.StartNew();
		foreach (var pair in rand_stream)
		{
			hash_sum += poly_hash(pair.Item1);
		}
		watch.Stop();
		System.Console.WriteLine($"Hash Sum: {hash_sum}, time taken: {watch.ElapsedMilliseconds} ms");
	}
}