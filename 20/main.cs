using System;
using System.IO;
using System.Collections.Generic;

public class Solution
{
    private const int MAX = 200000;
    private static int[] mu;
    private static int[] freq;
    private static int[] cnt;
    private static bool muInit = false;

    private static void InitMobius()
    {
        if (muInit) return;
        muInit = true;
        
        mu   = new int[MAX + 1];
        freq = new int[MAX + 1];
        cnt  = new int[MAX + 1];

        var isPrime = new bool[MAX + 1];
        var primes  = new List<int>();

        mu[1] = 1;
        for (int i = 2; i <= MAX; i++)
            isPrime[i] = true;

        for (int i = 2; i <= MAX; i++)
        {
            if (isPrime[i])
            {
                primes.Add(i);
                mu[i] = -1;
            }
            foreach (int p in primes)
            {
                long x = (long)i * p;
                if (x > MAX) break;
                isPrime[x] = false;
                if (i % p == 0)
                {
                    mu[x] = 0;
                    break;
                }
                else
                {
                    mu[x] = -mu[i];
                }
            }
        }
    }

    private long CalculateAnswer(int n, int k, int[] a)
    {
        InitMobius();

        var bList = new List<int>(n);
        int maxB = 0;
        foreach (int ai in a)
        {
            if (ai % k == 0)
            {
                int v = ai / k;
                bList.Add(v);
                if (v > maxB) maxB = v;
            }
        }

        int m = bList.Count;
        if (m < 2)
            return 0;

        for (int i = 1; i <= maxB; i++)
        {
            freq[i] = 0;
            cnt[i]  = 0;
        }

        foreach (int v in bList)
            freq[v]++;

        for (int d = 1; d <= maxB; d++)
        {
            for (int j = d; j <= maxB; j += d)
                cnt[d] += freq[j];
        }

        long ans = 0;
        for (int d = 1; d <= maxB; d++)
        {
            int cd = cnt[d];
            if (cd >= 2 && mu[d] != 0)
                ans += mu[d] * ((long)cd * (cd - 1) / 2);
        }

        return ans;
    }

    public void Solve()
    {
        using (var input = new FastInput())
        {
            int t = input.ReadInt();
            var answers = new long[t];

            for (int test = 0; test < t; test++)
            {
                int n = input.ReadInt();
                int k = input.ReadInt();
                int[] a = input.ReadIntArray(n);
                answers[test] = CalculateAnswer(n, k, a);
            }

            Console.WriteLine(string.Join("\n", answers));
        }
    }

    public class FastInput : IDisposable
    {
        private readonly StreamReader _reader;
        private Queue<string> _tokens = new Queue<string>();

        public FastInput()
        {
            _reader = new StreamReader(Console.OpenStandardInput());
        }

        private void EnsureTokens()
        {
            while (_tokens.Count == 0)
            {
                var line = _reader.ReadLine();
                if (line == null) return;
                foreach (var token in line.Split((char[])null, StringSplitOptions.RemoveEmptyEntries))
                    _tokens.Enqueue(token);
            }
        }

        public string ReadToken()
        {
            EnsureTokens();
            return _tokens.Count > 0 ? _tokens.Dequeue() : null;
        }

        public int ReadInt()
        {
            var tk = ReadToken();
            if (tk == null) throw new EndOfStreamException();
            return int.Parse(tk);
        }

        public int[] ReadIntArray(int n)
        {
            var arr = new int[n];
            for (int i = 0; i < n; i++)
                arr[i] = ReadInt();
            return arr;
        }

        public void Dispose()
        {
            _reader.Dispose();
        }
    }
}