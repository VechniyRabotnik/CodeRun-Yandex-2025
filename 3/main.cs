public class Solution 
{
    public int[] Solve(int n, int m, int[] swaps) 
    {
        int totalGuards = 2 * n;
        int[] positions = new int[totalGuards];
        for (int i = 0; i < totalGuards; i++)
            positions[i] = i + 1;

        int methodsInLeft = 0;
        for (int i = 0; i < n; i++)
            if (positions[i] <= n)
                methodsInLeft++;

        int[] result = new int[m];

        for (int i = 0; i < m; i++)
        {
            int i1 = swaps[2 * i] - 1;
            int i2 = swaps[2 * i + 1] - 1;

            int guard1 = positions[i1];
            int guard2 = positions[i2];

            if (i1 < n && guard1 <= n) methodsInLeft--;
            if (i2 < n && guard2 <= n) methodsInLeft--;

            positions[i1] = guard2;
            positions[i2] = guard1;

            if (i1 < n && positions[i1] <= n) methodsInLeft++;
            if (i2 < n && positions[i2] <= n) methodsInLeft++;

            result[i] = methodsInLeft;
        }

        return result;
    }
}