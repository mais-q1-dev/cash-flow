using CommunityToolkit.Diagnostics;
using System.Text.RegularExpressions;

namespace MaisQ1Dev.Libs.Domain.Entities;

public sealed partial record Email
{
    private const string Pattern = @"^(?=.{1,255}$)([a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,})$";

    private Email(string address) => Address = address;

    public string Address { get; init; } = null!;

    public static Email Create(string address)
    {
        Guard.IsNotNullOrWhiteSpace(address, nameof(address));
        Guard.IsGreaterThanOrEqualTo(address.Length, 6, nameof(address));
        Guard.IsTrue(EmailRegex().IsMatch(address));

        return new Email(address.Trim().ToLower());
    }

    public static implicit operator string(Email email)
        => email.Address;

    [GeneratedRegex(Pattern)]
    private static partial Regex EmailRegex();
}
