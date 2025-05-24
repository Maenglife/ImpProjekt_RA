class Program
{    
	static void Main()
    {
        Random rand = new();
		int l = rand.Next(1, 65); //random number between 1 and 64
		
		int n = 1000000; //key amount
		
		Hash.time_hash(Hash.shift_hash, l, n);
		Hash.time_hash(Hash.mod_prime_hash, l, n);
    }
}
