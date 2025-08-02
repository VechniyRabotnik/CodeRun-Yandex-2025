using System;
using System.IO;
using System.Collections.Generic;

class Solution
{
    const int MOD = 998244353;

    static int ModPow(int x, int e)
    {
        long r = 1, b = x % MOD;
        while (e > 0)
        {
            if ((e & 1) != 0) r = r * b % MOD;
            b = b * b % MOD;
            e >>= 1;
        }
        return (int)r;
    }

    int Calculate(int n, string s)
    {
        int S = 0;
        for (int len = 2; len <= n; len += 2)
            S += (n - len + 1);
        var regionLeft = new int[S];
        var regionLen  = new int[S];
        var regionMask = new int[S];
        {
            int idx = 0;
            for (int len = 2; len <= n; len += 2)
                for (int l = 0; l + len <= n; l++)
                {
                    regionLeft[idx] = l;
                    regionLen[idx]  = len;
                    regionMask[idx] = ((1 << len) - 1) << l;
                    idx++;
                }
        }

        var offsets = new int[S + 1];
        {
            int sum = 0;
            for (int i = 0; i < S; i++)
            {
                offsets[i] = sum;
                sum += (1 << regionLen[i]);
            }
            offsets[S] = sum;
        }
        int totalP = offsets[S];
        var pContFlat = new int[totalP];

        int maxHalf = n / 2;
        var inv2pow = new long[maxHalf + 1];
        inv2pow[0] = 1;
        long inv2 = (MOD + 1) / 2;
        for (int i = 1; i <= maxHalf; i++)
            inv2pow[i] = inv2pow[i - 1] * inv2 % MOD;

        for (int i = 0; i < S; i++)
        {
            int l   = regionLeft[i];
            int len = regionLen[i];
            int half= len / 2;
            int off = offsets[i];
            for (int id = 0, full = 1 << len; id < full; id++)
            {
                bool alwaysAttr = false;
                int fixedPairs = 0;
                for (int j = 0; j < half; j++)
                {
                    bool ra = ((id >> j) & 1) != 0;
                    bool rb = ((id >> (len - 1 - j)) & 1) != 0;
                    if (!ra && !rb)
                    {
                        fixedPairs++;
                        if (s[l + j] == s[l + len - 1 - j])
                        {
                            alwaysAttr = true;
                            break;
                        }
                    }
                }
                if (alwaysAttr)
                    pContFlat[off + id] = 0;
                else
                {
                    int rnd = half - fixedPairs;
                    pContFlat[off + id] = (int)inv2pow[rnd];
                }
            }
        }

        int Rcount = 1 << n;
        var f = new int[Rcount];

        for (int R = Rcount - 1; R >= 0; R--)
        {
            int sumNL = 0, sumLoop = 0;
            for (int i = 0; i < S; i++)
            {
                int l   = regionLeft[i];
                int len = regionLen[i];
                int off = offsets[i];
                int mask= regionMask[i];

                int id  = (R >> l) & ((1 << len) - 1);
                int pc  = pContFlat[off + id];

                if ((R & mask) == mask)
                {
                    sumLoop += pc;
                    if (sumLoop >= MOD) sumLoop -= MOD;
                }
                else
                {
                    int prod = (int)((long)pc * f[R | mask] % MOD);
                    sumNL += prod;
                    if (sumNL >= MOD) sumNL -= MOD;
                }
            }

            int numer = sumNL + S;
            if (numer >= MOD) numer -= MOD;
            int denom = S - sumLoop;
            if (denom < 0) denom += MOD;

            int invD = ModPow(denom, MOD - 2);
            f[R] = (int)((long)numer * invD % MOD);
        }

        return f[0];
    }

    public void Solve()
    {
        using var inp = new FastInput();
        string s = inp.ReadToken();
        Console.WriteLine(Calculate(s.Length, s));
    }

    class FastInput : IDisposable
    {
        readonly StreamReader _r;
        readonly Queue<string> _q = new();
        public FastInput() => _r = new StreamReader(Console.OpenStandardInput());
        void Fill()
        {
            while (_q.Count == 0)
            {
                var line = _r.ReadLine();
                if (line == null) return;
                foreach (var tok in line.Split((char[])null, StringSplitOptions.RemoveEmptyEntries))
                    _q.Enqueue(tok);
            }
        }
        public string ReadToken() { Fill(); return _q.Count > 0 ? _q.Dequeue() : null; }
        public void Dispose() => _r.Dispose();
    }
}