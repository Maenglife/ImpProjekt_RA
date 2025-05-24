class Program
{    
	static void Main()
    {
        Random rand = new();
		int l = rand.Next(1, 21); //random number between 1 and 21 (should be 64, but would run out
								  //of memory with current implementation)
		
		int n = 1000000; //key amount
		
		System.Console.WriteLine("Task 1:");
		Hash.time_hash(Hash.shift_hash, l, n);
		Hash.time_hash(Hash.mod_prime_hash, l, n);
		System.Console.WriteLine();
		
		System.Console.WriteLine("Task 2:");
		Hashtable table = new(Hash.shift_hash, l);
		System.Console.WriteLine(Hash.shift_hash(3,l));
		
		System.Console.WriteLine("Short test of get/set/increment, should give values 5,0,11:");
		table.set(3,5);
		Int64 test1 = table.get(3);
		Int64 test2 = table.get(1);
		table.increment(3,6);
		Int64 test3 = table.get(3);
		
		System.Console.WriteLine($"get x = 3: {test1} \nget x = 1: {test2} \nget x = 3(after increment): {test3}");
    }
}
