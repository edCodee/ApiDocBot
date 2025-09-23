using Microsoft.ML;
using System.IO;
    public class MLServiceMechanicalArm
    {
    public MLServiceMechanicalArm()
        {

            var modelPathConfig = configuration["MLModel2:Path"];
            if (string.IsNullOrWhiteSpace(modelPathConfig))
                throw new ArgumentException("Falta la configuración MLModel2:Path en appsettings.json");

            // Resuelve con ContentRootPath (seguro en local y en Azure)
            var modelPath = Path.Combine(env.ContentRootPath, modelPathConfig);

            _logger.LogInformation("Buscando modelo ML en: {path}", modelPath);

            if (!File.Exists(modelPath))
                throw new FileNotFoundException($"No se encontró el modelo ML en {modelPath}");

            // Cargamos el modelo y también capturamos el schema para inspección
            using (var fs = File.OpenRead(modelPath))
            {
                _mlModel = _mlContext.Model.Load(fs, out _schema);
            }
        }

        public PatientPredictionMechanicalArm Predict(PatientDataMechanicalArm input)
            {
                // Crear PredictionEngine por petición (es seguro y evita problemas de concurrencia)
                var engine = _mlContext.Model.CreatePredictionEngine<PatientDataMechanicalArm, PatientPredictionMechanicalArm>(_mlModel);
                var prediction = engine.Predict(input);
                return prediction;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al predecir con ML.");
                throw;
            }
        }
    }
}
