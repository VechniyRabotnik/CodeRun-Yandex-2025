using System;

public class Solution {
    const int MOD = 1_000_000_007;

    public int Solve(int a, int k, int n) {
        int t = Math.Min(k, n);
        int u = k + n - t;      

        var inv = new int[t + 1];
        if (t >= 1) {
            inv[1] = 1;
            for (int i = 2; i <= t; i++) {
                inv[i] = (int)(MOD - (long)(MOD / i) * inv[MOD % i] % MOD);
            }
        }

        long C = 1;
        for (int i = 1; i <= t; i++) {
            C = C * ((u + (long)i) % MOD) % MOD;
            C = C * inv[i] % MOD;
        }

        return (int)ModPow(C, MOD - 2);
    }

    private long ModPow(long x, int e) {
        long r = 1;
        x %= MOD;
        while (e > 0) {
            if ((e & 1) == 1) r = r * x % MOD;
            x = x * x % MOD;
            e >>= 1;
        }
        return r;
    }
}