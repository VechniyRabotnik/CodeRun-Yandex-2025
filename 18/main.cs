public class Solution 
{
    public long Solve(int n, int m) 
    {
        if (m <= 1) 
            return 1;
        long M = m;
        long lo = 1, hi = M, ans = M;
        while (lo <= hi) 
        {
            long mid = lo + ((hi - lo) >> 1);
            if (SumAtLeast(n, M, mid)) 
            {
                ans = mid;
                hi = mid - 1;
            } 
            else 
            {
                lo = mid + 1;
            }
        }
        return ans;
    }

    private static bool SumAtLeast(int n, long m, long w) 
    {
        long prev = 1;      
        long sumLow = 1;   
        int s = 0;
        int half = n >> 1;  

        for (int k = 1; k <= n; k++) 
        {
            if (prev > w) break;
            prev = prev * (long)(n - k + 1) / k;
            if (prev > w) break;
            sumLow += prev;
            s = k;
        }

        if (s >= half) 
            return true;

        long cntMid = (n + 1L) - 2L * (s + 1L);
        long total = sumLow * 2 + cntMid * w;
        return total >= m;
    }
}