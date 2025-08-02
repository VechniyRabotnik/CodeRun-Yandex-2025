using System;
using System.Collections.Generic;

public class Solution 
{

    private const long MOD = 1_000_000_000L - 7538;
    private readonly Dictionary<long,int> memo = new Dictionary<long,int>();

    public long Solve(long n) 
    {
        memo[0] = 1;
        return F(n);
    }

    private int F(long n)
    {
        if (memo.TryGetValue(n, out int val))
            return val;

        long n2 = n / 2;
        long n3 = n / 3;
        long n4 = n / 4;

        int a2 = F(n2);
        int a3 = F(n3);
        int a4 = F(n4);

        int pow = PowMod(a2, a3);

        long res = pow;
        res = (res + 5L * a4) % MOD;
        res = (res + (n % MOD)) % MOD;

        memo[n] = (int)res;
        return (int)res;
    }

    private int PowMod(long x, int exp)
    {
        long result = 1;
        long baseVal = x % MOD;
        long e = exp;
        while (e > 0)
        {
            if ((e & 1) == 1)
                result = (result * baseVal) % MOD;
            baseVal = (baseVal * baseVal) % MOD;
            e >>= 1;
        }
        return (int)result;
    }
}