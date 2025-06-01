using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Net.Mime;
using System.Numerics;

class Program {
	static void Main() {
		Random rand = new();
		int l = rand.Next(1, 65); //random number between 1 and 64
		int l_small = 20; //random number between 1 and 20. Used in task 2+. For values
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
		int t = 10; // m = 2^t, where m is the length of the sketch array and possible hash values.
		Countsketch sketch_t5 = Experiment.time_sketch(5, rand_stream_small);
		Countsketch sketch_t10 = Experiment.time_sketch(t, rand_stream_small);
		Countsketch sketch_t15 = Experiment.time_sketch(15, rand_stream_small);
		Countsketch sketch_t20 = Experiment.time_sketch(20, rand_stream_small);
		
		Int64 estimate_t5 = sketch_t5.cubic_estimate();
		Int64 estimate_t10 = sketch_t10.cubic_estimate();
		Int64 estimate_t15 = sketch_t15.cubic_estimate();
		Int64 estimate_t20 = sketch_t20.cubic_estimate();
		System.Console.WriteLine($"Single t=5 sketch estimate of cube_sum:{estimate_t5}");
		System.Console.WriteLine($"Single t=10 sketch estimate of cube_sum:{estimate_t10}");
		System.Console.WriteLine($"Single t=15 sketch estimate of cube_sum:{estimate_t15}");
		System.Console.WriteLine($"Single t=20 sketch estimate of cube_sum:{estimate_t20}");
		System.Console.WriteLine();
		
		System.Console.WriteLine("Task 7:");
		List<BigInteger[]> random_bigints = Stream.CreateRandomAs("random_bytes.txt");// all random
		// bytes from random.org divided into lists of 4 BigInteger to be used in each poly_hash,
		// ensuring 4-independence.
		
		Int64[] experiments_t5 = Experiment.cubic_experiments(random_bigints, rand_stream_small, 100, 5);
		Int64[] experiments_t10 = Experiment.cubic_experiments(random_bigints, rand_stream_small, 100, t);
		Int64[] experiments_t15 = Experiment.cubic_experiments(random_bigints, rand_stream_small, 100, 15);
		Int64[] experiments_t20 = Experiment.cubic_experiments(random_bigints, rand_stream_small, 100, 20);
		
		Experiment.analyse_experiements(experiments_t5,exact);
		Experiment.analyse_experiements(experiments_t10,exact);
		Experiment.analyse_experiements(experiments_t15,exact);
		Experiment.analyse_experiements(experiments_t20,exact);
	}
}
