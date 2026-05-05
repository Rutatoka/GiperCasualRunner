using System.Collections.Generic;
using UnityEngine;
using System;

public class ProfessionMatcher
{
    private List<ProfessionData> professions;

    public ProfessionMatcher(List<ProfessionData> professions)
    {
        this.professions = professions;
    }

    public (ProfessionData, List<(ProfessionData, float)>) GetBestMatch(float[] player)
    {
        ProfessionData best = null;
        float bestScore = -1f;

        List<(ProfessionData, float)> all = new();

        foreach (var p in professions)
        {
            float score = Cosine(player, p.vector);
            all.Add((p, score));

            if (score > bestScore)
            {
                bestScore = score;
                best = p;
            }
        }

        all.Sort((a, b) => b.Item2.CompareTo(a.Item2));

        return (best, all);
    }

    private float Cosine(float[] a, float[] b)
    {
        float dot = 0f;
        float magA = 0f;
        float magB = 0f;

        for (int i = 0; i < 10; i++)
        {
            dot += a[i] * b[i];
            magA += a[i] * a[i];
            magB += b[i] * b[i];
        }

        if (magA == 0 || magB == 0) return 0f;

        return dot / (Mathf.Sqrt(magA) * Mathf.Sqrt(magB));
    }
}