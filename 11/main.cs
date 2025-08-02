using System;
using System.Collections.Generic;

public class Solution {
    public long Solve(int n, long[] a) {
        if (n < 2) return 0;

        var freq = new Dictionary<long, int>(2);
        int left = 0;
        long maxLen = 0;

        for (int right = 0; right < n; right++) {
            long id = a[right];
            if (freq.ContainsKey(id)) {
                freq[id]++;
            } else {
                freq[id] = 1;
            }

            while (freq.Count > 2) {
                long leftId = a[left++];
                if (--freq[leftId] == 0) {
                    freq.Remove(leftId);
                }
            }

            if (freq.Count == 2) {
                long len = right - left + 1;
                if (len > maxLen) maxLen = len;
            }
        }

        return maxLen;
    }
}