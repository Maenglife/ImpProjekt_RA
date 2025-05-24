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
}