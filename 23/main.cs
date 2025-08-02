using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

public class Solution
{
    private int[] CalculateAnswer(int n, int[] aOrig)
    {
        if (n == 1)
        {
            int G = (aOrig[0] == 0 ? 1 : 0);
            return new[] { G, G, G, G };
        }

        int[] a = new int[n];
        for (int i = 0; i < n; i++)
            a[i] = aOrig[i] <= n ? aOrig[i] : n + 1;

        int origMax = 0;
        foreach (int v in aOrig)
            if (v > origMax) origMax = v;

        var used = new bool[n + 2];
        for (int i = 0; i < n; i++)
            if (a[i] <= n) used[a[i]] = true;
        int G0 = 0;
        while (used[G0]) G0++;

        var posList = new List<int>[n + 2];
        for (int v = 0; v < posList.Length; v++)
            posList[v] = new List<int>();
        for (int i = 0; i < n; i++)
            posList[a[i]].Add(i);

        var sPos = new SortedSet<int>();
        for (int i = 0; i < n; i++)
            if (a[i] > 0)
                sPos.Add(i);

        MexTree seg = null;
        int curL = 0, curR = -1;
        int[] freq = null;

        int minMax = -1;

        for (int K = 0; K <= G0; K++)
        {
            if (K > 0)
            {
                foreach (int idx in posList[K])
                    sPos.Remove(idx);

                if (sPos.Count == 0)
                {
                    minMax = K;
                    break;
                }

                int newL = sPos.Min;
                int newR = sPos.Max;

                if (seg == null)
                {
                    curL = newL;
                    curR = newR;
                    seg = new MexTree(G0 + 1);
                    freq = new int[G0 + 1];
                    for (int i = curL; i <= curR; i++)
                        if (a[i] <= G0)
                            freq[a[i]]++;
                    seg.Build(freq);
                }
                else
                {
                    if (newL > curL)
                        for (int i = curL; i < newL; i++)
                            if (a[i] <= G0 && --freq[a[i]] == 0)
                                seg.Update(a[i], 1);
                    if (newR < curR)
                        for (int i = curR; i > newR; i--)
                            if (a[i] <= G0 && --freq[a[i]] == 0)
                                seg.Update(a[i], 1);
                    curL = newL;
                    curR = newR;
                }

                if (seg.GetMex() <= K)
                {
                    minMax = K;
                    break;
                }
            }
            else 
            {
                if (sPos.Count == 0)
                    continue;

                int newL = sPos.Min;
                int newR = sPos.Max;
                curL = newL;
                curR = newR;
                seg = new MexTree(G0 + 1);
                freq = new int[G0 + 1];
                for (int i = curL; i <= curR; i++)
                    if (a[i] <= G0)
                        freq[a[i]]++;
                seg.Build(freq);
                if (seg.GetMex() == 0)
                {
                    minMax = 0;
                    break;
                }
            }
        }

        if (minMax == -1)
            minMax = G0;

        int maxMax = Math.Max(origMax, G0);
        int minMin = 0;
        int maxMin = G0;

        return new[] { minMax, maxMax, minMin, maxMin };
    }

    class MexTree
    {
        int size, n;
        int[] t;
        public MexTree(int n)
        {
            this.n = n;
            size = 1;
            while (size < n) size <<= 1;
            t = new int[size << 1];
        }
        public void Build(int[] freq)
        {
            for (int i = 0; i < n; i++)
                t[size + i] = freq[i] == 0 ? 1 : 0;
            for (int i = n; i < size; i++)
                t[size + i] = 0;
            for (int i = size - 1; i >= 1; i--)
                t[i] = t[i << 1] + t[(i << 1) | 1];
        }
        public void Update(int x, int v)
        {
            int i = size + x;
            if (t[i] == v) return;
            t[i] = v;
            for (i >>= 1; i >= 1; i >>= 1)
                t[i] = t[i << 1] + t[(i << 1) | 1];
        }
        public int GetMex()
        {
            if (t[1] == 0) return n;
            int i = 1;
            while (i < size)
                i = t[i << 1] > 0 ? (i << 1) : ((i << 1) | 1);
            return i - size;
        }
    }

    class FastInput : IDisposable
    {
        StreamReader _r = new StreamReader(Console.OpenStandardInput());
        Queue<string> _q = new Queue<string>();
        void Ensure()
        {
            while (_q.Count == 0)
            {
                var line = _r.ReadLine();
                if (line == null) return;
                foreach (var tok in line.Split((char[])null, StringSplitOptions.RemoveEmptyEntries))
                    _q.Enqueue(tok);
            }
        }
        public string ReadToken() { Ensure(); return _q.Count > 0 ? _q.Dequeue() : null; }
        public int ReadInt() => int.Parse(ReadToken());
        public int[] ReadIntArray(int n)
        {
            var a = new int[n];
            for (int i = 0; i < n; i++) a[i] = ReadInt();
            return a;
        }
        public void Dispose() => _r.Dispose();
    }

    public void Solve()
    {
        using (var inp = new FastInput())
        {
            int t = inp.ReadInt();
            var outp = new string[t];
            for (int i = 0; i < t; i++)
            {
                int n = inp.ReadInt();
                int[] a = inp.ReadIntArray(n);
                outp[i] = string.Join(" ", CalculateAnswer(n, a));
            }
            Console.WriteLine(string.Join("\n", outp));
        }
    }
}