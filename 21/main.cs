using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

public class Solution
{
    class Point {
        public int x, y;
        public Point(int x, int y) {
            this.x = x;
            this.y = y;
        }
    }

    private (int, int) CalculateAnswer(int n, Point[] points)
    {
        if (n == 2)
            return (1, 2);

        var xs = new long[n];
        var ys = new long[n];
        for (int i = 0; i < n; i++) {
            xs[i] = points[i].x;
            ys[i] = points[i].y;
        }

        var evenSum = new List<int>(n);
        var oddSum  = new List<int>(n);
        for (int i = 0; i < n; i++) {
            if ( ((xs[i] + ys[i]) & 1) == 0 ) evenSum.Add(i);
            else                             oddSum.Add(i);
        }
        if (evenSum.Count > 0 && oddSum.Count > 0)
            return (evenSum[0] + 1, oddSum[0] + 1);

        var candidates = new List<(int,int)>();
        var idx = Enumerable.Range(0, n).ToArray();
        Array.Sort(idx, (a,b) => xs[a]!=xs[b]? xs[a].CompareTo(xs[b]) : ys[a].CompareTo(ys[b]));
        int L = Math.Min(n - 1, 5);
        for (int i = 0; i < L; i++)
            candidates.Add((idx[i], idx[i+1]));

        Array.Sort(idx, (a,b) => ys[a]!=ys[b]? ys[a].CompareTo(ys[b]) : xs[a].CompareTo(xs[b]));
        for (int i = 0; i < L; i++)
            candidates.Add((idx[i], idx[i+1]));

        var rnd = new Random();
        for (int t = 0; t < 5; t++) {
            int i = rnd.Next(n);
            int j = rnd.Next(n - 1);
            if (j >= i) j++;
            candidates.Add((i, j));
        }

        foreach (var (i, j) in candidates) {
            if (i == j) continue;
            long xi = xs[i], yi = ys[i], xj = xs[j], yj = ys[j];
            long dx = xj - xi, dy = yj - yi;
            long rhs = (xj*xj - xi*xi + yj*yj - yi*yi);
            bool ok = true;
            for (int k = 0; k < n; k++) {
                if (k==i||k==j) continue;
                if ((dx*xs[k] + dy*ys[k])*2 == rhs) {
                    ok = false;
                    break;
                }
            }
            if (ok) return (i + 1, j + 1);
        }

        return (1, 2);
    }

    public void Solve()
    {
        using var input = new FastInput();
        int n = input.ReadInt();
        var pts = new Point[n];
        for (int i = 0; i < n; i++) {
            pts[i] = new Point(input.ReadInt(), input.ReadInt());
        }
        var (a, b) = CalculateAnswer(n, pts);
        Console.WriteLine($"{a} {b}");
    }

    class FastInput : IDisposable
    {
        private readonly StreamReader _reader;
        private Queue<string> _tokens = new Queue<string>();
        public FastInput() => _reader = new StreamReader(Console.OpenStandardInput());
        private void EnsureTokens()
        {
            while (_tokens.Count == 0)
            {
                var line = _reader.ReadLine();
                if (line == null) return;
                foreach (var tok in line.Split((char[])null, StringSplitOptions.RemoveEmptyEntries))
                    _tokens.Enqueue(tok);
            }
        }
        public string ReadToken() { EnsureTokens(); return _tokens.Dequeue(); }
        public int ReadInt()    => int.Parse(ReadToken());
        public void Dispose()   => _reader.Dispose();
    }
}