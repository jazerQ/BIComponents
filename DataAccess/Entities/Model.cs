namespace DataAccess.Entities;

public class Model
{
    public long Article { get; set; }

    public string? PartNumber { get; set; }

    public string? Color { get; set; }

    public string? Transport { get; set; }

    public string? TypeOfControl { get; set; }

    public string? Scale { get; set; }

    public string? ActionRadius { get; set; }

    public string? Peculiarities { get; set; }

    public int? TypeId { get; set; }

    public virtual Product ArticleNavigation { get; set; } = null!;

    public virtual TypeOfModel? Type { get; set; }
}
