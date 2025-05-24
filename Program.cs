class Program
{    
	static void Main()
    {
        Random rand = new();
		int l = rand.Next(1,21); //random number between 1 and 20 (should be 64, but would run out
								 //of memory with current implementation)
		
		int n = 1000000; //key amount
		
		System.Console.WriteLine("Task 1:");
		Hash.time_hash(Hash.shift_hash, l, n);
		Hash.time_hash(Hash.mod_prime_hash, l, n);
		System.Console.WriteLine();
		
		System.Console.WriteLine("Task 2:\nShort test of get/set/increment, should give values 5,0,11:");
		Hashtable shift_table = new(Hash.shift_hash, l);
				
		shift_table.set(3,5);
		Int64 test1 = shift_table.get(3);
		Int64 test2 = shift_table.get(1);
		shift_table.increment(3,6);
		Int64 test3 = shift_table.get(3);
		
		System.Console.WriteLine($"get x = 3: {test1} \nget x = 1: {test2} \nget x = 3 (after increment): {test3}");
		System.Console.WriteLine();
		
		System.Console.WriteLine("Task 3:");
		Hashtable shift_table_2 = new(Hash.shift_hash, l);
		shift_table_2.cubic_sums(n);
		Hashtable modp_table_2 = new(Hash.mod_prime_hash, l);
		modp_table_2.cubic_sums(n);
	}
}
