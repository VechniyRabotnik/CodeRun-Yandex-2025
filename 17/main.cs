using System;

public class Solution {
    public long Solve(int n, long[] a) {

        var Lmax = new int[n];
        var Rmax = new int[n];
        var Lmin = new int[n];
        var Rmin = new int[n];

        var st = new int[n];
        int top;

        top = -1;
        for (int i = 0; i < n; i++) {
            while (top >= 0 && a[st[top]] <= a[i]) top--;
            Lmax[i] = (top >= 0 ? st[top] : -1);
            st[++top] = i;
        }
        top = -1;
        for (int i = n - 1; i >= 0; i--) {
            while (top >= 0 && a[st[top]] < a[i]) top--;
            Rmax[i] = (top >= 0 ? st[top] : n);
            st[++top] = i;
        }

        top = -1;
        for (int i = 0; i < n; i++) {
            while (top >= 0 && a[st[top]] >= a[i]) top--;
            Lmin[i] = (top >= 0 ? st[top] : -1);
            st[++top] = i;
        }
        top = -1;
        for (int i = n - 1; i >= 0; i--) {
            while (top >= 0 && a[st[top]] > a[i]) top--;
            Rmin[i] = (top >= 0 ? st[top] : n);
            st[++top] = i;
        }

        long sumMax = 0, sumMin = 0;
        for (int i = 0; i < n; i++) {
            long leftMax = i - Lmax[i];
            long rightMax = Rmax[i] - i;
            sumMax += a[i] * leftMax * rightMax;

            long leftMin = i - Lmin[i];
            long rightMin = Rmin[i] - i;
            sumMin += a[i] * leftMin * rightMin;
        }

        return sumMax - sumMin;
    }
}