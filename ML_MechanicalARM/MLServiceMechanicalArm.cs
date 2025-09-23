using Microsoft.ML;
using Microsoft.Extensions.Configuration;

namespace ApiDocBot.ML_MechanicalARM
{
    public class MLServiceMechanicalArm
    {
        private readonly PredictionEngine<PatientDataMechanicalArm, PatientPredictionMechanicalArm> _predictionEngine;

        public MLServiceMechanicalArm(IConfiguration configuration)
        {
            var mlContext = new MLContext();

            var modelPathConfig = configuration["MLModel2:Path"];
            if (string.IsNullOrWhiteSpace(modelPathConfig))
                throw new ArgumentException("Falta la configuración MLModel2:Path en appsettings.json");

            var modelPath = Path.Combine(Directory.GetCurrentDirectory(), modelPathConfig);

            if (!File.Exists(modelPath))
                throw new FileNotFoundException($"No se encontró el modelo ML en {modelPath}");

            var model = mlContext.Model.Load(modelPath, out _);
            _predictionEngine = mlContext.Model.CreatePredictionEngine<PatientDataMechanicalArm, PatientPredictionMechanicalArm>(model);
        }

        public string PredictRiskMechanicalArm(PatientDataMechanicalArm dataMechanicalArm)
        {
            var prediction = _predictionEngine.Predict(dataMechanicalArm);
            return prediction.PredictedRiskLevel;
        }
    }
}
