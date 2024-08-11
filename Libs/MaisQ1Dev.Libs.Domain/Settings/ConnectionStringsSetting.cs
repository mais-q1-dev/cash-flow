using System.ComponentModel.DataAnnotations;

namespace MaisQ1Dev.Libs.Domain.Settings;

public sealed record ConnectionStringsSetting
{
    public static readonly string Section = "ConnectionStrings";

    [Required]
    public string Database { get; init; } = string.Empty;
    [Required]
    public string Redis { get; init; } = string.Empty;
}