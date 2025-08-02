using System;

public class Solution 
{
    public long Solve(int n, int q, long[] a, int[][] queries) 
    {
        var diff = new long[n + 2];  
        for (int i = 0; i < q; i++) 
        {
            int l = queries[i][0];
            int r = queries[i][1];
            diff[l]++;
            diff[r + 1]--;
        }

        var freq = new long[n];
        long curr = 0;
        for (int i = 1; i <= n; i++) 
        {
            curr += diff[i];
            freq[i - 1] = curr;
        }

        Array.Sort(a);
        Array.Sort(freq);

        long result = 0;
        for (int i = 0; i < n; i++) 
        {
            result += a[i] * freq[i];
        }

        return result;
    }
}