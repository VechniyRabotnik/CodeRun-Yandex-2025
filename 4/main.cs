using System;

public class Solution 
{
    public int[] Solve(int n, int[] a) 
    {
        int[] result = new int[n + 1];
        long prev = -1_000_000_000_000_000L; 

        for (int i = 0; i < n; i++)
        {
            int pos = a[i];
            int neg = -a[i];

            bool okPos = pos >= prev;
            bool okNeg = neg >= prev;

            if (!okPos && !okNeg)
            {
                return new int[] { 0 };
            }

            int chosen;
            if (okPos && okNeg)
            {
                chosen = Math.Min(pos, neg);
            }
            else if (okPos)
            {
                chosen = pos;
            }
            else
            {
                chosen = neg;
            }

            result[i + 1] = chosen;
            prev = chosen;
        }

        result[0] = 1;
        return result;
    }
}