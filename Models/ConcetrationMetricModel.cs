using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace ApiDocBot.Models
{
    public class ConcetrationMetricModel
    {
        [Key]
        public int concentrationMetrics_id { get; set; }
        public int concentrationMetrics_patientProfileFreeId { get; set; }
        public int concentrationMetrics_durationMs { get; set; }
        public decimal concentrationMetrics_percentMoving { get; set; }
        public decimal concentrationMetrics_avgMovement { get; set; }
        public DateTime concentrationMetrics_createdAt { get; set; }

        public ConcetrationMetricModel()
        {

        }

        public ConcetrationMetricModel(int concentrationMetrics_id, int concentrationMetrics_patientProfileFreeId, int concentrationMetrics_durationMs, decimal concentrationMetrics_percentMoving, decimal concentrationMetrics_avgMovement, DateTime concentrationMetrics_createdAt)
        {
            this.concentrationMetrics_id = concentrationMetrics_id;
            this.concentrationMetrics_patientProfileFreeId = concentrationMetrics_patientProfileFreeId;
            this.concentrationMetrics_durationMs = concentrationMetrics_durationMs;
            this.concentrationMetrics_percentMoving = concentrationMetrics_percentMoving;
            this.concentrationMetrics_avgMovement = concentrationMetrics_avgMovement;
            this.concentrationMetrics_createdAt = concentrationMetrics_createdAt;
        }
    }
}
