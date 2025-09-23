using ApiDocBot.ML_MechanicalARM;
using Microsoft.ML;
using System.IO;

public class MLServiceMechanicalArm
{
    private readonly PredictionEngine<PatientDataMechanicalArm, PatientPredictionMechanicalArm> _predictionEngine;

    public MLServiceMechanicalArm()
    {
        var mlContext=new MLContext();

        //Obtener la ruta relativa
        var modelPath = Path.Combine(Directory.GetCurrentDirectory(), "ML_MechanicalARM", "PatientRiskModel_mechanicalArmV4.zip");

        var model = mlContext.Model.Load(modelPath, out var _);
        _predictionEngine=mlContext.Model.CreatePredictionEngine<PatientDataMechanicalArm, PatientPredictionMechanicalArm>(model);
    }

    public string PredictRisk(PatientDataMechanicalArm data)
    {
        var prediction = _predictionEngine.Predict(data);
        return prediction.PredictedRiskLevel;
    }
}