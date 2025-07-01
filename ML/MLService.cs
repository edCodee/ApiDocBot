using Microsoft.ML;
using System.IO;

public class MLService
{
    private readonly PredictionEngine<PatientData, PatientPrediction> _predictionEngine;

    public MLService()
    {
        var mlContext = new MLContext();

        // Obtener ruta relativa desde donde se ejecuta la app
        var modelPath = Path.Combine(Directory.GetCurrentDirectory(), "ML", "PatientRiskModel.zip");

        var model = mlContext.Model.Load(modelPath, out var _);
        _predictionEngine = mlContext.Model.CreatePredictionEngine<PatientData, PatientPrediction>(model);
    }

    public string PredictRisk(PatientData data)
    {
        var prediction = _predictionEngine.Predict(data);
        return prediction.PredictedRiskLevel;
    }
}
