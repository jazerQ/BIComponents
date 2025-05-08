namespace DataAccess.Entities;

public class Product
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Link { get; set; }

    public int? Defaultprice { get; set; }

    public int? Pricewithcard { get; set; }

    public float? Productrating { get; set; }

    public int? Countofcomments { get; set; }

    public int? Countofquestions { get; set; }

    public long? Article { get; set; }

    public string? Country { get; set; }

    public virtual Model? Model { get; set; }
}
