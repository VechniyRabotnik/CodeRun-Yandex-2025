public class Solution 
{
    public int[] Solve(int n, int m, int[] p) 
    {

        long[] P = new long[n + 1];
        for (int i = 1; i <= n; i++)
            P[i] = p[i - 1];

     
        long lastIdx = 0, lastVal = 0;
        for (int i = 1; i <= n; i++)
        {
            if (P[i] != -1)
            {
                long minAllowed = lastVal + (long)m * (i - lastIdx);
                if (P[i] < minAllowed)
                    return new int[] { -1 };
                lastIdx = i;
                lastVal = P[i];
            }
        }


        long[] filled = new long[n + 1];
        filled[0] = 0;
        lastIdx = 0;
        lastVal = 0;
        for (int i = 1; i <= n; i++)
        {
            if (P[i] != -1)
            {
                filled[i] = P[i];
                lastIdx = i;
                lastVal = P[i];
            }
            else
            {
                filled[i] = lastVal + (long)m * (i - lastIdx);
            }
        }

        int[] result = new int[n];
        for (int i = 1; i <= n; i++)
            result[i - 1] = (int)(filled[i] - filled[i - 1]);

        return result;
    }
}