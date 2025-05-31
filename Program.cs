using System.Numerics;

class Program {
	static void Main() {
		Random rand = new();
		int l = rand.Next(1, 65); //random number between 1 and 64
		int l_small = rand.Next(1,21); //random number between 1 and 20. Used in task 2+. For values
		// above 24 we run out of memory with current hash table implementation, but we dont use values
		// above 20 to ensure run-time is not too long.
		int n = 1<<l; //number of elements in stream must be >=2^l (note this will overflow for l>32,
		// as standard int is 32-bit, but the CreateStream function takes an int)
		int n_small = 2000000; // we pick number above 2^20 (ca. 1 mil)
		IEnumerable<Tuple<ulong, int>> rand_stream = Stream.CreateStream(n,l); // for task 1
		IEnumerable<Tuple<ulong, int>> rand_stream_small = Stream.CreateStream(n_small,l_small); // for task 3+

		System.Console.WriteLine("Task 1:");
		Hash.time_hash(Hash.shift_hash, rand_stream, l);
		Hash.time_hash(Hash.mod_prime_hash, rand_stream, l);
		System.Console.WriteLine();

		System.Console.WriteLine("Task 2:\nShort test of get/set/increment, should give values 5,0,11:");
		Hashtable shift_table = new(Hash.shift_hash, l_small);

		shift_table.set(3, 5);
		Int64 test1 = shift_table.get(3);
		Int64 test2 = shift_table.get(1);
		shift_table.increment(3, 6);
		Int64 test3 = shift_table.get(3);

		System.Console.WriteLine($"get x = 3: {test1} \nget x = 1: {test2} \nget x = 3 (after increment): {test3}");
		System.Console.WriteLine();

		System.Console.WriteLine("Task 3:");
		Hashtable shift_table_2 = new(Hash.shift_hash, l_small);
		BigInteger exact = shift_table_2.sum_of_squares(rand_stream_small, n_small);
		Hashtable modp_table_2 = new(Hash.mod_prime_hash, l_small);
		modp_table_2.sum_of_squares(rand_stream_small, n_small);
		System.Console.WriteLine();

		System.Console.WriteLine("Task 4:");
		Hash.time_poly_hash(rand_stream_small, Hash.a_array);
		System.Console.WriteLine();
		
		System.Console.WriteLine("Task 5-6:");
		int t = 4; // m = 2^t, where m is the length of the sketch array and possible hash values.
		
		Countsketch sketch = new(t, rand_stream_small, Hash.a_array);
		Int64 estimate = sketch.cubic_estimate();
		System.Console.WriteLine($"Single sketch's estimate of cube_sum:{estimate}");
		System.Console.WriteLine();
		
		System.Console.WriteLine("Task 7:");
		List<BigInteger[]> random_bigints = Stream.CreateRandomAs("random_bytes.txt");// all random
		// bytes from random.org divided into lists of 4 BigInteger to be used in each poly_hash,
		// ensuring 4-independence.
		Int64[] experiments = Experiment.cubic_experiments(random_bigints, rand_stream_small, 100, t);
		Array.Sort(experiments);
		System.Console.WriteLine($"experiement values between {experiments[0]}-{experiments[99]}, " +
								 $"median: {experiments[49]}, exact from Task 3: {exact}");
	}
}
