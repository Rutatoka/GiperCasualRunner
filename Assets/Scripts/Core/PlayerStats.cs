using System;
using UnityEngine;

[System.Serializable]
public class PlayerStats
{
    private float[] categoryStats = new float[10];
    private float[] directionStats = new float[10];

    private float[] categoryExposurePrimary = new float[10];
    private float[] categoryExposureSecondary = new float[10];

    private float[] directionExposurePrimary = new float[10];
    private float[] directionExposureSecondary = new float[10];

    // =========================
    // ADD STAT
    // =========================
    public void AddStat(StatType type, int value, bool isPrimary, FlowState state)
    {
        int i = (int)type;
        if (i < 0 || i >= 10) return;
        if (state != FlowState.CategoryTest && state != FlowState.DirectionTest)
        {
            Debug.LogError("Invalid state in AddStat: " + state);
        }
        if (state == FlowState.CategoryTest)
        {
            categoryStats[i] += value;

            if (isPrimary)
                categoryExposurePrimary[i] += 1f;
            else
                categoryExposureSecondary[i] += 1f;
        }
        else if (state == FlowState.DirectionTest)
        {
            directionStats[i] += value;

            if (isPrimary)
                directionExposurePrimary[i] += 1f;
            else
                directionExposureSecondary[i] += 1f;
        }
    }

    // =========================
    // VECTOR
    // =========================
    public float[] GetVector(FlowState state)
    {
        return state == FlowState.CategoryTest ? categoryStats : directionStats;
    }

    // =========================
    // CONFIDENCE
    // =========================
    public float GetConfidence()
    {
        float total = 0f;

        for (int i = 0; i < 10; i++)
        {
            total += categoryExposurePrimary[i] + categoryExposureSecondary[i];
            total += directionExposurePrimary[i] + directionExposureSecondary[i];
        }

        return total;
    }

    // =========================
    // OPTIONAL: NORMALIZED VECTOR (если захочешь потом)
    // =========================
    public float[] GetNormalized(FlowState state)
    {
        float[] source = GetVector(state);
        float[] norm = new float[10];

        for (int i = 0; i < 10; i++)
        {
            float exp = 1f; // защита от деления на 0

            if (state == FlowState.CategoryTest)
                exp = categoryExposurePrimary[i] + categoryExposureSecondary[i];
            else
                exp = directionExposurePrimary[i] + directionExposureSecondary[i];

            norm[i] = exp > 0 ? source[i] / exp : 0f;
        }

        return norm;
    }
}