using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.ML.Data;

public class PatientData
{
    public string Gender { get; set; } = string.Empty;
    public float PatientAge { get; set; } 
    public string Answer1 { get; set; } = string.Empty;
    public string Answer2 { get; set; } = string.Empty;
    public string Answer3 { get; set; } = string.Empty;
    public string Answer4 { get; set; } = string.Empty;
    public string Answer5 { get; set; } = string.Empty;
    public string Answer6 { get; set; } = string.Empty;
    public string Answer7 { get; set; } = string.Empty;
    public string Answer8 { get; set; } = string.Empty;
    public string Answer9 { get; set; } = string.Empty;
    public string Answer10 { get; set; } = string.Empty;
    public string Answer11 { get; set; } = string.Empty;
    public string Answer12 { get; set; } = string.Empty;
    public string Answer13 { get; set; } = string.Empty;
    public string Answer14 { get; set; } = string.Empty;
    public string Answer15 { get; set; } = string.Empty;
    public string Answer16 { get; set; } = string.Empty;
    public string Answer17 { get; set; } = string.Empty;
    public string Answer18 { get; set; } = string.Empty;
    public string Answer19 { get; set; } = string.Empty;
    public string Answer20 { get; set; } = string.Empty;
    public string RiskLevel { get; set; } =string.Empty;
    public PatientData() { }

    public PatientData(string gender, float patientAge, string answer1, string answer2, string answer3, string answer4, string answer5, string answer6, string answer7, string answer8, string answer9, string answer10, string answer11, string answer12, string answer13, string answer14, string answer15, string answer16, string answer17, string answer18, string answer19, string answer20, string riskLevel)
    {
        Gender = gender;
        PatientAge = patientAge;
        Answer1 = answer1;
        Answer2 = answer2;
        Answer3 = answer3;
        Answer4 = answer4;
        Answer5 = answer5;
        Answer6 = answer6;
        Answer7 = answer7;
        Answer8 = answer8;
        Answer9 = answer9;
        Answer10 = answer10;
        Answer11 = answer11;
        Answer12 = answer12;
        Answer13 = answer13;
        Answer14 = answer14;
        Answer15 = answer15;
        Answer16 = answer16;
        Answer17 = answer17;
        Answer18 = answer18;
        Answer19 = answer19;
        Answer20 = answer20;
        RiskLevel = riskLevel;
    }
}
