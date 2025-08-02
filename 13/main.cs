using System;
using System.Collections.Generic;

public class Solution {
    public int Solve(int n, int[] a, int m, int[] b) {
        var zerosA = new List<int>(n);
        var zerosB = new List<int>(m);
        for (int i = 0; i < n; i++) if (a[i] == 0) zerosA.Add(i + 1);
        for (int i = 0; i < m; i++) if (b[i] == 0) zerosB.Add(i + 1);

        var pref1A = new int[n + 1];
        var pref1B = new int[m + 1];
        for (int i = 1; i <= n; i++)
            pref1A[i] = pref1A[i - 1] + (a[i - 1] == 1 ? 1 : 0);
        for (int i = 1; i <= m; i++)
            pref1B[i] = pref1B[i - 1] + (b[i - 1] == 1 ? 1 : 0);

        int total1A = pref1A[n];
        int total1B = pref1B[m];

        int maxLen = 0;
        int Z = Math.Min(zerosA.Count, zerosB.Count);

        for (int k = 0; k <= Z; k++) {
            int posA = k == 0 ? 0 : zerosA[k - 1];
            int posB = k == 0 ? 0 : zerosB[k - 1];

            int rem1A = total1A - pref1A[posA];
            int rem1B = total1B - pref1B[posB];

            int take1 = Math.Min(rem1A, rem1B);
            int curLen = k + take1;
            if (curLen > maxLen)
                maxLen = curLen;
        }

        return maxLen;
    }
}