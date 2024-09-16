using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace JWT_API_BD.Models;

public partial class User
{
    public long IdUser { get; set; }

    public string? UserName { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    [JsonIgnore]
    public virtual ICollection<News> News { get; set; } = new List<News>();

    [JsonIgnore]
    public virtual ICollection<RefreshTokenHistory> RefreshTokenHistories { get; set; } = new List<RefreshTokenHistory>();
}
