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
	
	public Int64 square_estimate() {
		Int64 cube_sum = 0;
		foreach (Int64 val in this.sketch) {
			cube_sum += val * val;
		}
		return cube_sum;
	}
}

class Experiment {
	public static Countsketch time_sketch(int t, IEnumerable<Tuple<ulong, int>> randstream) {
		Stopwatch cs_watch = Stopwatch.StartNew();
		Countsketch sketch = new(t, randstream, Hash.a_array);
		cs_watch.Stop();
		System.Console.WriteLine($"took {cs_watch.ElapsedMilliseconds} ms to create sketch with m=2^{t}");
		return sketch;
	}
	public static Int64[] square_experiments(List<BigInteger[]> big_ints,
											IEnumerable<Tuple<ulong, int>> rand_stream,
											int amount, int t) {
		Int64[] cube_sum_array = new Int64[amount];
		System.Console.WriteLine($"Starting {amount} experiments:");
		Stopwatch watch = Stopwatch.StartNew();
		for (int i = 0; i<amount; i++) {
			BigInteger[] rand_a_vals = big_ints[i];
			Countsketch new_experiment = new(t, rand_stream, rand_a_vals);
			cube_sum_array[i] = new_experiment.square_estimate();
		}
		watch.Stop();
		System.Console.WriteLine($"Experiments took {watch.ElapsedMilliseconds} ms");
		return cube_sum_array;
	}
	
	public static void analyse_experiements(Int64[] experiments, BigInteger exact) {
		BigInteger mean_square_error = 0;
		foreach (Int64 experiment in experiments) {
			BigInteger diff = (BigInteger)experiment-exact;
			mean_square_error += diff*diff;
		}
		
		List<Int64[]> partitions = new List<Int64[]>();
		for (int i = 0; i<99; i+=11) {
			Int64[] partition = experiments.Skip(i).Take(i+11).ToArray();
			Array.Sort(partition);
			partitions.Add(partition);
		}
		
		Array.Sort(experiments);
		(int,Int64)[] all_coordinates = new (int,Int64)[100];
		for (int i = 0; i<100; i++) {
			(int,Int64) coordinate = (i, experiments[i]);
			all_coordinates[i] = (i, experiments[i]);
		} 
		string coordinate_format = string.Join(", ", all_coordinates.Select(p => $"[{p.Item1 + 1}, {p.Item2}]")); // to be inserted in maple
		System.Console.WriteLine($"All coordinates : [{coordinate_format}]");
		
		System.Console.WriteLine($"Mean square error was: {mean_square_error}");
		Int64[] all_medians = new Int64[9];
		System.Console.WriteLine($"Exact S value from Task 3: {exact}");
		for (int i = 0; i<9; i++) {
			Int64[] current = partitions[i];
			System.Console.WriteLine($"List G_{i+1} = from {current[0]}-{current[10]}, Median value = {current[5]}");
			all_medians[i] = current[5];
		}
		Array.Sort(all_medians);
		(int,Int64)[] median_coordinates = new (int,Int64)[9];
		for (int i = 0; i<9; i++) {
			(int,Int64) coordinate = (i, all_medians[i]);
			median_coordinates[i] = (i, all_medians[i]);
		} 
		string median_format = string.Join(", ", median_coordinates.Select(p => $"[{p.Item1 + 1}, {p.Item2}]")); // to be inserted in maple
		System.Console.WriteLine($"All median values : [{median_format}]");
	}
}