using System;

public class Solution
{
    public long[] Solve(int n, int t, long[] a, long[] b)
    {
        long[] result = new long[t + 1];

        long total = 0;
        for (int i = 0; i < n; i++)
            total += a[i];
        result[0] = total;

        var diff = new long[t + 2];
        var rem  = new long[t + 2];

        for (int i = 0; i < n; i++)
        {
            long ai = a[i], bi = b[i];
            if (bi == 0) 
                continue; 

            long full = ai / bi;       
            long tail = ai - full * bi; 

            int endFull = (int)Math.Min(full, (long)t);
            diff[1] += bi;
            diff[endFull + 1] -= bi;

            if (tail > 0 && full + 1 <= t)
                rem[full + 1] += tail;
        }

        long runningB = 0;
        for (int minute = 1; minute <= t; minute++)
        {
            runningB += diff[minute];
            long release = runningB + rem[minute];
            total -= release;
            result[minute] = total;
        }

        return result;
    }
}