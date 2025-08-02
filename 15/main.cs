 public class Solution {
    public long Solve(string ballad, int n) {
        long ans = 0;
        var sumPos = new long[26];
        var cnt    = new long[26];
        int pos = 0;  

        foreach (char ch in ballad) {
            if (ch == ' ') continue;
            pos++;
            int idx = ch - 'a';
            ans += cnt[idx] * (pos - 1L) - sumPos[idx];
            cnt[idx]++;
            sumPos[idx] += pos;
        }

        return ans;
    }
}