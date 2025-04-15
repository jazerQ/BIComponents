using System;
using System.Collections.Generic;

namespace DataAccess;

public partial class Type
{
    public int Id { get; set; }

    public string? Type1 { get; set; }

    public virtual ICollection<Model> Models { get; set; } = new List<Model>();
}
