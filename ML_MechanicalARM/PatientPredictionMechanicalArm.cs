using Microsoft.ML.Data;

namespace ApiDocBot.ML_MechanicalARM
{
    public class PatientPredictionMechanicalArm
    {
        [ColumnName("PredictedLabel")]
        public string PredictedRiskLevel { get; set; } = string.Empty;

        [ColumnName("Score")]
        public float[] Scores { get; set; } = Array.Empty<float>();
    }
}
