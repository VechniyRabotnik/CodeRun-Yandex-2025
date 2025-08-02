using System;

public class Solution 
{
    public int Solve(int n, int[] a) 
    {
        Array.Sort(a);

        int minMoves = n;
        int left = 0;

        for (int right = 0; right < n; right++)
        {
            while (a[right] - a[left] >= n)
            {
                left++;
            }

            int count = right - left + 1;
            minMoves = Math.Min(minMoves, n - count);
        }

        return minMoves;
    }
}