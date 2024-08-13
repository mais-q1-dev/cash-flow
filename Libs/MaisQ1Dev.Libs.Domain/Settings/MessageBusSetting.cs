using System.ComponentModel.DataAnnotations;

namespace MaisQ1Dev.Libs.Domain.Settings;

public sealed record MessageBusSetting
{
    public static readonly string Section = "MessageBus";

    [Required]
    public string Host { get; init; } = string.Empty;
    [Required]
    public string Port { get; init; } = string.Empty;
    [Required]
    public string Username { get; init; } = string.Empty;
    [Required]
    public string Password { get; init; } = string.Empty;
    [Required]
    public string VirtualHost { get; init; } = string.Empty;

    public string ConnectionString =>
        $"amqp://{Username}:{Password}@{Host}:{Port}/{VirtualHost}";
}