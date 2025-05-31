using System.Numerics;
using System.Diagnostics;

class Hashtable {
	private List<(UInt64,Int64)>[] table;
	private Func<UInt64,int,UInt64> hash;
	private int l;
	private Int64 pow_l;
	public Hashtable(Func<UInt64,int,UInt64> hash, int l) {
		this.l = l;
		this.pow_l = (Int64)Math.Pow(2,l);
		this.hash = hash;
		this.table = new List<(UInt64,Int64)>[this.pow_l];
		
		for (int i = 0; i<this.pow_l; i++) {
			table[i] = new List<(UInt64,Int64)>();
		}
	}
	
	public Int64 get(UInt64 x) {
		List<(UInt64,Int64)> bucket = this.table[hash(x, this.l)];
		int collisions = bucket.Count();
		if (collisions==0) {
			return 0;
		}
		for (int i = 0; i<collisions; i++) {
			if (bucket[i].Item1 == x) {
				return bucket[i].Item2;
			}
		}
		return 0;
	}
	public void set(UInt64 x, Int64 v) {
		List<(UInt64,Int64)> bucket = this.table[hash(x, this.l)];
		int collisions = bucket.Count();
		if (collisions==0) {
			bucket.Add((x, v));
			return;
		}
		for (int i = 0; i<collisions; i++) {
			if (bucket[i].Item1 == x) {
				bucket[i] = (x,v);
				return;
			}
		}
		bucket.Add((x,v));
		return;
	}
	
	public void increment(UInt64 x, Int64 d) {
		List<(UInt64,Int64)> bucket = this.table[hash(x, this.l)];
		int collisions = bucket.Count();
		if (collisions==0) {
			bucket.Add((x, d));
			return;
		}
		for (int i = 0; i<collisions; i++) {
			if (bucket[i].Item1 == x) {
				bucket[i] = (x,bucket[i].Item2+d);
				return;
			}
		}
		bucket.Add((x,d));
		return;
	}
	
	public BigInteger cubic_sums(IEnumerable<Tuple<ulong, int>> rand_stream, int n) {
		if (this.pow_l > n) {
			throw new ArgumentOutOfRangeException(nameof(n), "n must be greater than 2^l");
		}
		
		System.Console.WriteLine($"Calculate keys and insert into table using {this.hash.Method.Name}, n:{n}, l^2:{this.pow_l}");
		Stopwatch watch = Stopwatch.StartNew();
		foreach (Tuple<ulong, int> pair in rand_stream) {
			increment(pair.Item1,pair.Item2);
		}
		
		System.Console.WriteLine($"Calculate keys and insert into table took {watch.ElapsedMilliseconds} ms");
		Int64 cube_sum = 0;
		for (int i = 0; i<this.pow_l; i++) {
			List<(UInt64,Int64)> bucket = table[i];
			
			for (int j = 0; j<bucket.Count(); j++) {
				(UInt64,Int64) pair = bucket[j];
				cube_sum += (Int64)Math.Pow(pair.Item2, 2);
			}
		}
		watch.Stop();
		System.Console.WriteLine($"Final cube sum: {cube_sum}. Final runtime: {watch.ElapsedMilliseconds} ms");
		Console.WriteLine();

		// Measure distribution of keys across buckets
		int[] bucketSizes = new int[this.pow_l];
		for (int i = 0; i < this.pow_l; i++) {
    		bucketSizes[i] = this.table[i].Count;
		}

		double mean = bucketSizes.Average();
		double variance = bucketSizes.Select(size => Math.Pow(size - mean, 2)).Average();
		double stdDev = Math.Sqrt(variance);
		int maxSize = bucketSizes.Max();
		int minSize = bucketSizes.Min();
		int nonEmptyBuckets = bucketSizes.Count(size => size > 0);
		int emptyBuckets = (int)this.pow_l - nonEmptyBuckets;

		Console.WriteLine($"Bucket distribution stats:");
		Console.WriteLine($"  Mean size     : {mean:F2}");
		Console.WriteLine($"  Variance      : {variance:F2}");
		Console.WriteLine($"  Std deviation : {stdDev:F2}");
		Console.WriteLine($"  Max size      : {maxSize}");
		Console.WriteLine($"  Min size      : {minSize}");
		Console.WriteLine($"  Empty buckets : {emptyBuckets} / {this.pow_l} ({100.0 * emptyBuckets / this.pow_l:F2}%)");
		Console.WriteLine();

		return cube_sum;
	}
}