using System.Numerics;
class Program
{    
	static readonly byte[] a = new byte[] { 0b10001101, 0b01110010, 0b01011000, 0b10101100, 0b10000000,
                            0b01100011, 0b00001100, 0b10010011 }; //random uneven 64 bit number
	static readonly byte[] a88 = new byte[] {0b10001101,0b01001000,0b10110011,0b01100011,0b11011111,
							0b00110010,0b11111110,0b11010101,0b11111010,0b01011111,
							0b00100000}; //random 88 bit number (not all 1's)
	static readonly byte[] b88 = new byte[] {0b10010100,0b01101011,0b01100110,0b10000000,0b10100010,
							0b00001000,0b10110011,0b10001000,0b01001111,0b01110000,
							0b01001101};
	
	static void Main()
    {
        Random rand = new();
		int l = rand.Next(1, 65); //random number between 1 and 64
		
		Int64 int_a = BitConverter.ToInt64(a,0);

		BigInteger big_a = new(a88);
		BigInteger big_b = new(b88);
		
		int n = 1000000; // key amount
		
		Hash.time_shift_hash(int_a, l, n);
		Hash.time_modp_hash(big_a, big_b, l, n);
    }
}
