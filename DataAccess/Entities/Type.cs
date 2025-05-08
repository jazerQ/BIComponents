namespace DataAccess.Entities;

public partial class TypeOfModel
{
    public int Id { get; set; }

    public string? Type1 { get; set; }

    public virtual ICollection<Model> Models { get; set; } = new List<Model>();
}
