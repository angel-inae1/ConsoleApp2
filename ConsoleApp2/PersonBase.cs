namespace ConsoleApp2;

public class PersonBase
{
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public DateTime Birthday { get; set; }
    public double Height { get; set; }
    public double Weight { get; set; }
    public double DoneWeight { get; set; }
    public DateTime DateOfRecording { get; set; }
    public double ActualWeight { get; set; }

    public (double Index, string Decoding) Calculatebmi(double weight, double height)
    {
        double bmi = weight / (height * height);
        string decoding = bmi switch
        {
            <= 16 => "Дефицит массы тела.",
            > 16 and <= 18 => "Недостаточная масса тела",
            > 18 and <= 25 => "Норма",
            > 25 and <= 30 => "Избыточный вес",
            > 30 and <= 35 => "Ожирение 1 стадия",
            > 35 and <= 40 => "Ожирение 2 стадия",
            > 40 => "Ожирение 3 стадия",
            _=> "Неизвестное значение."
        };
        return (bmi, decoding);
    }
}