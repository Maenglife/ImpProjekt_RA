using System.Numerics;

class Countsketch {
	Int64[] sketch;
	private int t;
	private Int64 m;
	private IEnumerable<Tuple<ulong, int>> stream;
	public Countsketch(int t, IEnumerable<Tuple<ulong, int>> stream, BigInteger[] rand_array) {
		this.t = t;
		this.m = ((Int64) 1)<<t;
		this.sketch = new Int64[m];
		this.stream = stream;
		
		foreach (Tuple<ulong, int> pair in stream) {
			(Func<UInt64,UInt64> h,Func<UInt64,int> s) = Hash.cs_hash(rand_array, t);
			UInt64 x = pair.Item1;
			int val = pair.Item2;
			sketch[h(x)] += s(x)*val; 
		}
	}
	
	public Int64 cubic_estimate() {
		Int64 cube_sum = 0;
		foreach (Int64 val in this.sketch) {
			cube_sum += (Int64)Math.Pow(val,2);
		}
		return cube_sum;
	}
}