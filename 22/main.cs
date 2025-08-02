using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

public class Solution
{
    private void CalculateAnswer(int n, int[] a)
    {
        var to = new int[n];
        for (int i = 0; i < n; i++)
            to[i] = a[i] == -1 ? -1 : a[i] - 1;

        var color = new byte[n];       
        var compId = new int[n];      
        Array.Fill(compId, -1);
        var cycleSizes = new List<int>();
        var stack = new List<int>();

        for (int i = 0; i < n; i++)
        {
            if (color[i] != 0) continue;
            stack.Clear();
            int u = i;
            while (u != -1 && color[u] == 0)
            {
                color[u] = 1;
                stack.Add(u);
                u = to[u];
            }
            if (u != -1 && color[u] == 1)
            {
                int idx = stack.IndexOf(u);
                int sz = stack.Count - idx;
                int cid = cycleSizes.Count;
                cycleSizes.Add(sz);
                for (int j = idx; j < stack.Count; j++)
                    compId[stack[j]] = cid;
            }
            foreach (var x in stack)
                color[x] = 2;
        }
        for (int i = 0; i < n; i++)
        {
            if (compId[i] == -1)
            {
                compId[i] = cycleSizes.Count;
                cycleSizes.Add(1);
            }
        }

        var children = new List<int>[n];
        for (int i = 0; i < n; i++) children[i] = new List<int>();
        var hasParent = new bool[n];

        for (int v = 0; v < n; v++)
        {
            int p = to[v];
            if (p != -1 && compId[p] != compId[v])
            {
                children[p].Add(v);
                hasParent[v] = true;
            }
        }

        var subtree = new long[n];
        var dfs = new List<(int v, int idx)>();
        for (int i = 0; i < n; i++)
        {
            if (hasParent[i]) continue;
            dfs.Add((i, 0));
            while (dfs.Count > 0)
            {
                var (v, idx) = dfs[dfs.Count - 1];
                if (idx < children[v].Count)
                {
                    var w = children[v][idx];
                    dfs[dfs.Count - 1] = (v, idx + 1);
                    dfs.Add((w, 0));
                }
                else
                {
                    dfs.RemoveAt(dfs.Count - 1);
                    long sum = 1;
                    foreach (var w in children[v])
                        sum += subtree[w];
                    subtree[v] = sum;
                }
            }
        }

        int compCount = cycleSizes.Count;
        var compTotal = new long[compCount];
        for (int i = 0; i < n; i++)
        {
            int cid = compId[i];
            if (cycleSizes[cid] > 1)
                compTotal[cid] += subtree[i];
        }

        var sb = new StringBuilder();
        for (int i = 0; i < n; i++)
        {
            if (i > 0) sb.Append(' ');
            int cid = compId[i];
            sb.Append(cycleSizes[cid] > 1 ? compTotal[cid] : subtree[i]);
        }
        Console.WriteLine(sb);
    }

    public void Solve()
    {
        using (var input = new FastInput())
        {
            int t = input.ReadInt();
            for (int test = 0; test < t; test++)
            {
                int n = input.ReadInt();
                int[] a = input.ReadIntArray(n);
                CalculateAnswer(n, a);
            }
        }
    }
}

public class FastInput : IDisposable
{
    private readonly StreamReader _reader;
    private readonly Queue<string> _tokens = new Queue<string>();
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
    public string ReadToken() { EnsureTokens(); return _tokens.Count > 0 ? _tokens.Dequeue() : null; }
    public int ReadInt() { var t = ReadToken(); return t == null ? 0 : int.Parse(t); }
    public int[] ReadIntArray(int n)
    {
        var arr = new int[n];
        for (int i = 0; i < n; i++) arr[i] = ReadInt();
        return arr;
    }
    public void Dispose() => _reader.Dispose();
}