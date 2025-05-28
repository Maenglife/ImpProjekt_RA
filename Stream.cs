using System.Numerics;
class Stream {
	public static IEnumerable<Tuple<ulong,int>> CreateStream(int n, int l) {
		// We generate a random uint64 number.
		Random rnd = new System.Random();
		ulong a = 0UL;
		Byte[] b = new Byte[8];
		rnd.NextBytes(b);
		for(int i = 0; i<8; ++i) {
			a = (a<<8)+(ulong)b[i];
		}
		
		// We demand that our random number has 30 zeros on the least significant bits and then a one.
		a = (a|((1UL<<31)-1UL))^((1UL<<30)-1UL);
		ulong x = 0UL;
		for(int i = 0; i<n/3; ++i) {
			x += a;
			yield return Tuple.Create(x&(((1UL<<l)-1UL)<<30),1);
		}
		for(int i = 0; i<(n+1)/3; ++i) {
			x += a;
			yield return Tuple.Create(x&(((1UL<<l)-1UL)<<30),-1);
		}
		for(int i = 0; i<(n+2)/3; ++i) {
			x += a;
			yield return Tuple.Create(x&(((1UL<<l)-1UL)<<30),1);
		}
	}
	
	public static BigInteger[] GenerateRandomAs() {
		Random rnd = new();
		BigInteger[] output = new BigInteger[4]; 
		for (int i = 0; i<4; i++) {
			Byte[] b = new Byte[11];
			rnd.NextBytes(b);
			BigInteger a_i = new(b, isUnsigned: true); //88 bits
			output[i] = a_i;
		}
		return output;
	}
	
	public static List<BigInteger[]> CreateRandomAs(string filepath) {
		string all_lines = File.ReadAllText(filepath);
		all_lines = all_lines.Replace("\n", "");
		string[] binary = all_lines.Split(" ");
		List<byte[]> all_a_bytes = new List<byte[]>();
		for (int i = 0; i<binary.Length; i+=11) {
			byte[] a_bytes = new byte[11];
			for (int j = 0; j<11; j++) {
				a_bytes[j] = Convert.ToByte(binary[i+j], 2);
			}
			all_a_bytes.Add(a_bytes);
		}
		List<BigInteger> all_big_ints = new List<BigInteger>(); 
		foreach (byte[] a_byte_array in all_a_bytes) {
			all_big_ints.Add(new BigInteger(a_byte_array, isUnsigned: true));
		}
		List<BigInteger[]> output = new List<BigInteger[]>();
		for (int i=0; i<all_big_ints.Count(); i+=4) {
			output.Add(all_big_ints.Skip(i).Take(4).ToArray());
		}
		return output;
	}
	
}