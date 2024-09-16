using System;
using System.Collections.Generic;

namespace JWT_API_BD.Models;

public partial class News
{
    public long IdNews { get; set; }

    public long? PublishedBy { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public DateTime? Date { get; set; }

    public virtual User? PublishedByNavigation { get; set; }
}
