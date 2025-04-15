namespace Core.Models;

public class Damper
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;
    
    public string Link { get; set; } = string.Empty;

    public string DefaultPrice { get; set; }

    public string? PriceWithCard { get; set; }

    public float? ProductRating { get; set; }

    public int? CountOfComments { get; set; }

    public int? CountOfQuestions { get; set; }

    public long Article { get; set; }

    public string Type { get; set; }

    public string? PartNumber { get; set; }

    public string? Color { get; set; }

    public string? Country { get; set; }

    public string? Transport { get; set; }

    public string? TypeOfControl { get; set; }
    
    public string? Scale { get; set; }

    public int? ActionRadius { get; set; }

    public string? Peculiarities { get; set; }
}

public static class DamperType
{
    public static List<string> Types =
    [
        "Запчасть для радиоуправляемых моделей",
        "Машинка радиоуправляемая",
        "Игрушка радиоуправляемая",
        "Аксессуар для радиоуправляемых моделей"
    ];
}