public class Solution 
{
    public int Solve(int n, int m) 
    {
        int left = 1;
        int right = n + m;
        int result = 0;

        while (left <= right) 
        {
            int mid = (left + right) / 2;
            long totalCells = (long)mid * mid;
            bool possible = false;

            if (mid % 2 == 0) 
            {
                long need = totalCells / 2;
                if ((n >= need && m >= need) || (m >= need && n >= need))
                    possible = true;
            }
            else 
            {
                long whiteNeeded = (totalCells + 1) / 2;
                long blackNeeded = (totalCells - 1) / 2;
                if ((n >= whiteNeeded && m >= blackNeeded) || (n >= blackNeeded && m >= whiteNeeded))
                    possible = true;
            }

            if (possible) 
            {
                result = mid;
                left = mid + 1;
            } 
            else 
            {
                right = mid - 1;
            }
        }

        return result;
    }
}