namespace ApiDocBot.DTO.ConcetrationMetricModelDTO
{
    public class ConcetrationMetricReadDTO
    {
        public int ConcetrationMetricId { get; set; }
        public int ConcetrationMetricPatientProfileFreeId { get; set; }
        public int ConcetrationMetricDurationMs { get; set; }
        public decimal ConcetrationMetricPercentMoving { get; set; }
        public decimal ConcetrationMetricAvgMovement { get; set; }
        public DateTime ConcetrationMetricCreateAt { get; set; }
    }
}
