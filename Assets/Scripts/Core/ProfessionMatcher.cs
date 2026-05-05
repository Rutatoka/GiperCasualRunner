using System.Collections.Generic;

public class ProfessionMatcher
{
    private List<ProfessionData> professions;

    public ProfessionMatcher(List<ProfessionData> professions)
    {
        this.professions = professions;
    }

    public ProfessionData GetBestMatch(PlayerStats stats)
    {
        ProfessionData best = null;
        int bestScore = int.MinValue;

        foreach (var p in professions)
        {
            int score =
                stats.tech * p.tech +
                stats.human * p.human +
                stats.manager * p.manager +
                stats.worker * p.worker +
                stats.introvert * p.introvert +
                stats.extrovert * p.extrovert +
                stats.analyst * p.analyst +
                stats.intuitive * p.intuitive +
                stats.stability * p.stability +
                stats.openness * p.openness;

            if (score > bestScore)
            {
                bestScore = score;
                best = p;
            }
        }

        return best;
    }
}
