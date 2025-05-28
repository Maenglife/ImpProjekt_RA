using System.Diagnostics;
using System.Numerics;
class Countsketch {
	Int64[] sketch;
	private Int64 m;
	public Countsketch(int t, IEnumerable<Tuple<ulong, int>> stream, BigInteger[] rand_array) {
		this.m = 1L<<t;
		this.sketch = new Int64[m];
		(Func<UInt64,UInt64> h,Func<UInt64,int> s) = Hash.cs_hash(rand_array, t);
		
		foreach (Tuple<ulong, int> pair in stream) {
			UInt64 x = pair.Item1;
			int val = pair.Item2;
			sketch[h(x)] += s(x)*val; 
		}
	}
	
	public Int64 cubic_estimate() {
		Int64 cube_sum = 0;
		foreach (Int64 val in this.sketch) {
			cube_sum += val * val;
		}
		return cube_sum;
	}
}

class Experiment {
	public static Int64[] cubic_experiments(List<BigInteger[]> big_ints,
											IEnumerable<Tuple<ulong, int>> rand_stream,
											int amount, int t) {
		Int64[] cube_sum_array = new Int64[amount];
		System.Console.WriteLine($"Starting {amount} experiments:");
		Stopwatch watch = Stopwatch.StartNew();
		for (int i = 0; i<amount; i++) {
			BigInteger[] rand_a_vals = big_ints[i];
			Countsketch new_experiment = new(t, rand_stream, rand_a_vals);
			cube_sum_array[i] = new_experiment.cubic_estimate();
		}
		watch.Stop();
		System.Console.WriteLine($"Experiments took {watch.ElapsedMilliseconds} ms");
		return cube_sum_array;
	}
}