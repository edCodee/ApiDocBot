namespace ApiDocBot.ML_MechanicalARM
{
    public class PatientDataMechanicalArm
    {
        public string Gender { get; set; } = string.Empty;
        public float PatientAge { get; set; }
        public float SustainedAttention { get; set; }
        public float Planning { get; set; }
        public float Categorization { get; set; }
        public float ExecutiveFunction { get; set; }
        public float EyeHandCoordination { get; set; }
        public float FineMotorPrecision { get; set; }
        public float ToleranceFrustration { get; set; }
        public float Perseverance { get; set; }
        public float TotalScore { get; set; }
        public string RiskLevel { get; set; } = string.Empty;

        public PatientDataMechanicalArm()
        {

        }

        public PatientDataMechanicalArm(string gender, float patientAge, float sustainedAttention, float planning, float categorization, float executiveFunction, float eyeHandCoordination, float fineMotorPrecision, float toleranceFrustration, float perseverance, float totalScore, string riskLevel)
        {
            Gender = gender;
            PatientAge = patientAge;
            SustainedAttention = sustainedAttention;
            Planning = planning;
            Categorization = categorization;
            ExecutiveFunction = executiveFunction;
            EyeHandCoordination = eyeHandCoordination;
            FineMotorPrecision = fineMotorPrecision;
            ToleranceFrustration = toleranceFrustration;
            Perseverance = perseverance;
            TotalScore = totalScore;
            RiskLevel = riskLevel;
        }
    }
}
