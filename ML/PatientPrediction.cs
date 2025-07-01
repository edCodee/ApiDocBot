using Microsoft.ML.Data;

public class PatientPrediction
{
    [ColumnName("PredictedLabel")]
    public string PredictedRiskLevel { get; set; } = string.Empty;

    [ColumnName("Score")]
    public float[] Scores { get; set; } = Array.Empty<float>();
}
