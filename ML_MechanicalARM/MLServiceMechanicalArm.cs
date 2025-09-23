using Microsoft.ML;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;
using Microsoft.ML.Data;

namespace ApiDocBot.ML_MechanicalARM
{
    public class MLServiceMechanicalArm
    {
        private readonly MLContext _mlContext;
        private readonly ITransformer _mlModel;
        private readonly DataViewSchema _schema;
        private readonly ILogger<MLServiceMechanicalArm> _logger;

        public MLServiceMechanicalArm(IConfiguration configuration, IWebHostEnvironment env, ILogger<MLServiceMechanicalArm> logger)
        {
            _logger = logger;
            _mlContext = new MLContext();

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

            // Log del schema de entrada para validar columnas
            var columnNames = string.Join(",", _schema.Select(c => c.Name));
            _logger.LogInformation("Modelo cargado correctamente. Schema de columnas: {cols}", columnNames);
        }

        public PatientPredictionMechanicalArm Predict(PatientDataMechanicalArm input)
        {
            try
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
