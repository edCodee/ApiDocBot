using Microsoft.ML;

namespace ApiDocBot.ML_MechanicalARM
{
    public class MLServiceMechanicalArm
    {
        private readonly PredictionEngine<PatientDataMechanicalArm, PatientPredictionMechanicalArm> _predictionEngine;

        public MLServiceMechanicalArm()
        {
            var mlContext = new MLContext();

            // obtener la ruta relativa desde donde se ejecuta la app
            var modelPath = Path.Combine(Directory.GetCurrentDirectory(), "ML_MechanicalARM", "PatientRiskModel_mechanicalarmV3.zip");

            var model = mlContext.Model.Load(modelPath, out var _);
            _predictionEngine = mlContext.Model.CreatePredictionEngine<PatientDataMechanicalArm, PatientPredictionMechanicalArm>(model);
        }

        public string PredictRiskMechanicalArm(PatientDataMechanicalArm dataMechanicalArm)
        {
            try
            {
                var prediction = _predictionEngine.Predict(dataMechanicalArm);
                return prediction.PredictedRiskLevel;
            }
            catch (Exception ex)
            {
                // Aquí atrapamos la excepción real y la propagamos con más detalle
                throw new InvalidOperationException($"Error en predicción ML: {ex.Message}", ex);
            }
        }
    }
}
