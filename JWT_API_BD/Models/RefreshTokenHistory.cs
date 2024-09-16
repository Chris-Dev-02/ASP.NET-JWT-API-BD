using System;
using System.Collections.Generic;

namespace JWT_API_BD.Models;

public partial class RefreshTokenHistory
{
    public long IdTokenRefresh { get; set; }

    public long? IdUser { get; set; }

    public string? Token { get; set; }

    public string? RefreshToken { get; set; }

    public DateTime? CreationDate { get; set; }

    public DateTime? ExpirationDate { get; set; }

    public virtual User? IdUserNavigation { get; set; }
}
