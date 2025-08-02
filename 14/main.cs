public class Answer {
    public int Sum;
    public string[] Field;

    public Answer(int sum, string[] field) {
        Sum   = sum;
        Field = field;
    }
}

public class Solution {
    public Answer solution(int n) {
        int sum = n*(n-1) + 2*(n-1)*(n-1);

        var field = new string[n];
        for (int i = 0; i < n; i++) {
            if ((i & 1) == 0) {
                field[i] = new string('x', n);
            } else {
                field[i] = new string('-', n);
            }
        }

        return new Answer(sum, field);
    }
}