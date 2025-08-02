using System;
using System.Collections.Generic;

public class Solution {
    public int[] Solve(int n, int k, int[] a) {
        bool[] has = new bool[n + 1];
        has[0] = true;
        for (int i = 0; i < k; i++) {
            int pos = a[i];
            if (pos >= 0 && pos <= n) has[pos] = true;
        }
        has[n] = true;

        var visited = new bool[n + 1];
        var pred    = new int[n + 1];  
        var jump    = new int[n + 1];  
        var q       = new Queue<int>();

        visited[0] = true;
        q.Enqueue(0);

        while (q.Count > 0) {
            int u = q.Dequeue();
            if (u == n) break;  
            for (int d = 1; d <= 2; d++) {
                int v = u + d;
                if (v <= n && !visited[v] && has[v]) {
                    visited[v] = true;
                    pred[v]    = u;
                    jump[v]    = d;
                    if (v == n) {  
                        q.Clear();
                        break;
                    }
                    q.Enqueue(v);
                }
            }
        }

        if (!visited[n])
            return new int[] { -1 };

        var path = new List<int>();
        for (int cur = n; cur != 0; cur = pred[cur]) {
            path.Add(jump[cur]);
        }
        path.Reverse();

        int m = path.Count;
        var res = new int[m + 1];
        res[0] = m;
        for (int i = 0; i < m; i++)
            res[i + 1] = path[i];
        return res;
    }
}