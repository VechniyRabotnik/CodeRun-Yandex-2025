public class Solution 
{
    public long Solve(int n) 
    {
        var isPrime = new bool[n + 1];
        for (int i = 2; i <= n; i++) isPrime[i] = true;
        int m = (int)Math.Sqrt(n);
        for (int i = 2; i <= m; i++)
            if (isPrime[i])
                for (int j = i * i; j <= n; j += i)
                    isPrime[j] = false;

        long c1 = 0, c3 = 0;
        for (int i = 3; i <= n; i++)
        {
            if (!isPrime[i]) continue;
            if ((i & 3) == 1) c1++;
            else c3++;
        }

        return c1 * c3;
    }
}