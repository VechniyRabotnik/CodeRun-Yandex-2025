using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

public class Solution
{

    private static int Gcd(int x, int y)
    {
        while (y != 0)
        {
            int t = x % y;
            x = y;
            y = t;
        }
        return x;
    }

    const int INF = 1000000005;
    int n, q;
    int[] a;
    int[] d;          
    long[] ans;
    struct Query { public int k, L, R, idx; }
    struct Event { public int g, L, R, j; }  

    public void Solve()
    {
        var inp = new FastInput();
        n = inp.ReadInt(); q = inp.ReadInt();
        a = inp.ReadIntArray(n);
        if (n == 1)
        {
            var sb0 = new StringWriter();
            for (int i = 0; i < q; i++) sb0.WriteLine(0);
            Console.Write(sb0);
            return;
        }
        int N = n - 1;
        d = new int[N+1];
        for (int i = 1; i <= N; i++)
            d[i] = Math.Abs(a[i] - a[i-1]);

        var events = new List<Event>(N * 20);
        var prev = new List<(int g, int l)>();
        for (int j = 1; j <= N; j++)
        {
            var cur = new List<(int g, int l)>();
            cur.Add((d[j], j));
            foreach (var p in prev)
            {
                int g2 = Gcd(p.g, d[j]);
                if (cur[cur.Count-1].g == g2)
                    cur[cur.Count-1] = (g2, p.l);
                else
                    cur.Add((g2, p.l));
            }
            for (int t = 0; t < cur.Count; t++)
            {
                int l0 = cur[t].l;
                int r0 = (t == 0 ? j : cur[t-1].l - 1);
                int gg = cur[t].g == 0 ? INF : cur[t].g;
                events.Add(new Event { g = gg, L = l0, R = r0, j = j });
            }
            prev = cur;
        }
        ans = new long[q];
        var qs = new Query[q];
        for (int i = 0; i < q; i++)
        {
            int L = inp.ReadInt();
            int R = inp.ReadInt();
            int K = inp.ReadInt();
            qs[i] = new Query { L = L, R = R, k = K, idx = i };
        }
        events.Sort((e1,e2) => e2.g.CompareTo(e1.g));
        Array.Sort(qs, (x,y) => y.k.CompareTo(x.k));

        var st = new SegmentTreeBeats(N);
        st.Build();

        int ptr = 0;
        foreach (var qu in qs)
        {
            while (ptr < events.Count && events[ptr].g >= qu.k)
            {
                var ev = events[ptr++];
                st.RangeChmax(ev.L, ev.R, ev.j);
            }
            int Lq = qu.L, Rq = qu.R - 1;
            long res = (Lq <= Rq
                ? st.Query(Lq, Rq, Rq)
                : 0);
            ans[qu.idx] = res;
        }

        var sw = new StringWriter();
        for (int i = 0; i < q; i++)
            sw.WriteLine(ans[i]);
        Console.Write(sw);
    }

    class SegmentTreeBeats
    {
        int n;
        int[] lson, rson;
        int[] lc, rc;
        long[] minF, sminF, sumF;
        int[] cntMin;
        long[] maxF;
        long[] sumI; 
        int[] cnt;   
        public SegmentTreeBeats(int N)
        {
            n = N;
            int sz = 4 * (n + 5);
            lson = new int[sz]; rson = new int[sz];
            lc = new int[sz]; rc = new int[sz];
            minF = new long[sz]; sminF = new long[sz];
            sumF = new long[sz];
            cntMin = new int[sz];
            maxF = new long[sz];
            sumI = new long[sz];
            cnt = new int[sz];
            Build(1, 1, n);
        }
        public void Build() {  }

        void Build(int v, int L, int R)
        {
            lc[v] = L; rc[v] = R;
            if (L == R)
            {
                long f0 = L - 1;
                minF[v] = maxF[v] = f0;
                sminF[v] = long.MaxValue;
                sumF[v] = f0;
                cntMin[v] = 1;
                sumI[v] = L;
                cnt[v] = 1;
            }
            else
            {
                int m = (L + R) >> 1;
                Build(v<<1, L, m);
                Build(v<<1|1, m+1, R);
                PullUp(v);
            }
        }

        void PullUp(int v)
        {
            int l = v<<1, r = v<<1|1;
            sumF[v] = sumF[l] + sumF[r];
            sumI[v] = sumI[l] + sumI[r];
            cnt[v] = cnt[l] + cnt[r];
            if (minF[l] < minF[r])
            {
                minF[v] = minF[l];
                cntMin[v] = cntMin[l];
                sminF[v] = Math.Min(sminF[l], minF[r]);
            }
            else if (minF[l] > minF[r])
            {
                minF[v] = minF[r];
                cntMin[v] = cntMin[r];
                sminF[v] = Math.Min(minF[l], sminF[r]);
            }
            else
            {
                minF[v] = minF[l];
                cntMin[v] = cntMin[l] + cntMin[r];
                sminF[v] = Math.Min(sminF[l], sminF[r]);
            }
            maxF[v] = Math.Max(maxF[l], maxF[r]);
        }

        void PushChmax(int v, long x)
        {
            long delta = x - minF[v];
            sumF[v] += delta * cntMin[v];
            minF[v] = x;
            if (maxF[v] < x) maxF[v] = x;
        }

        void PushDown(int v)
        {
            int l = v<<1, r = v<<1|1;
            if (minF[v] > minF[l])
                PushChmax(l, minF[v]);
            if (minF[v] > minF[r])
                PushChmax(r, minF[v]);
        }

        public void RangeChmax(int L, int R, long x) => RangeChmax(1, L, R, x);
        void RangeChmax(int v, int L, int R, long x)
        {
            if (rc[v] < L || R < lc[v] || minF[v] >= x) return;
            if (L <= lc[v] && rc[v] <= R && sminF[v] > x)
            {
                PushChmax(v, x);
                return;
            }
            PushDown(v);
            RangeChmax(v<<1, L, R, x);
            RangeChmax(v<<1|1, L, R, x);
            PullUp(v);
        }

        public long Query(int L, int R, int bound) => Query(1, L, R, bound);
        long Query(int v, int L, int R, int bd)
        {
            if (rc[v] < L || R < lc[v]) return 0;
            if (L <= lc[v] && rc[v] <= R)
            {
                if (maxF[v] <= bd)
                {
                    return sumF[v] - (sumI[v] - cnt[v]);
                }
                if (minF[v] > bd)
                {
                    return cnt[v] * (bd + 1L) - sumI[v];
                }
            }
            PushDown(v);
            long s1 = Query(v<<1, L, R, bd);
            long s2 = Query(v<<1|1, L, R, bd);
            return s1 + s2;
        }
    }

    public class FastInput : IDisposable
    {
        private readonly StreamReader _r;
        private Queue<string> _q = new Queue<string>();
        public FastInput() => _r = new StreamReader(Console.OpenStandardInput());
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
        public string NextToken() { Ensure(); return _q.Count>0?_q.Dequeue():null; }
        public int ReadInt() => int.Parse(NextToken());
        public int[] ReadIntArray(int n)
        {
            var a = new int[n];
            for (int i = 0; i < n; i++) a[i] = ReadInt();
            return a;
        }
        public void Dispose() => _r.Dispose();
    }

}