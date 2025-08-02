 
using System;
using System.Collections.Generic;

public class Solution
{

    class Node
    {
        public Node[] Next = new Node[26];
        public List<int> Indices; 
        public int Head;          
    }

    public int[,] Solve(int n, string[] words)
    {
        int total = words.Length; 
        var root = new Node();


        for (int i = 0; i < total; i++)
        {
            var node = root;
            var w = words[i];
            foreach (char ch in w)
            {
                int c = ch - 'a';
                if (node.Next[c] == null)
                    node.Next[c] = new Node();
                node = node.Next[c];
            }
            if (node.Indices == null)
                node.Indices = new List<int>();
            node.Indices.Add(i);
        }

        var matched = new bool[total];
        var result = new int[n, 2];
        int pairCount = 0;


        var order = new int[total];
        for (int i = 0; i < total; i++) order[i] = i;
        Array.Sort(order, (i, j) => words[j].Length - words[i].Length);

        foreach (int fi in order)
        {
            if (matched[fi]) 
                continue;

            var node = root;
            Node bestNode = null;
            int bestIdx = -1;
            var full = words[fi];


            for (int k = 0; k < full.Length; k++)
            {
                node = node.Next[full[k] - 'a'];
                if (node == null)
                    break;

                var list = node.Indices;
                if (list != null)
                {

                    while (node.Head < list.Count)
                    {
                        int idx = list[node.Head];
                        if (matched[idx] || idx == fi)
                            node.Head++;
                        else
                            break;
                    }

                    if (node.Head < list.Count)
                    {
                        bestNode = node;
                        bestIdx = list[node.Head];
                    }
                }
            }

            if (bestNode != null)
            {
                matched[fi] = true;
                matched[bestIdx] = true;
                bestNode.Head++;

                result[pairCount, 0] = bestIdx + 1; 
                result[pairCount, 1] = fi + 1;
                pairCount++;
                if (pairCount == n)
                    break;
            }
        }

        return result;
    }
}