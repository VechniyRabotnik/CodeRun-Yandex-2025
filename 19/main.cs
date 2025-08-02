using System;
using System.IO;
using System.Collections.Generic;

public class Solution
{
    private long CalculateAnswer(int n, int[] a, int[] b)
    {
        var visited = new bool[n];
        long sumMin = 0;
        int cycles = 0;

        for (int i = 0; i < n; i++)
        {
            if (!visited[i])
            {
                cycles++;
                int cur = i;
                long minB = long.MaxValue;
                while (!visited[cur])
                {
                    visited[cur] = true;
                    if (b[cur] < minB) minB = b[cur];
                    cur = a[cur] - 1;  
                }
                sumMin += minB;
            }
        }

        return cycles <= 1 ? 0 : sumMin;
    }

    public void Solve()
    {
        using (var input = new FastInput())
        {
            int t = input.ReadInt();
            var answers = new long[t];

            for (int test = 0; test < t; ++test)
            {
                int n = input.ReadInt();
                int[] a = input.ReadIntArray(n);
                int[] b = input.ReadIntArray(n);
                answers[test] = CalculateAnswer(n, a, b);
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
                foreach (var tok in line.Split((char[])null, StringSplitOptions.RemoveEmptyEntries))
                    _tokens.Enqueue(tok);
            }
        }

        public string ReadToken()
        {
            EnsureTokens();
            return _tokens.Count > 0 ? _tokens.Dequeue() : null;
        }

        public int ReadInt()
        {
            var tok = ReadToken();
            if (tok == null) throw new EndOfStreamException();
            return int.Parse(tok);
        }

        public int[] ReadIntArray(int n)
        {
            var arr = new int[n];
            for (int i = 0; i < n; i++)
                arr[i] = ReadInt();
            return arr;
        }

        public void Dispose() => _reader.Dispose();
    }
}