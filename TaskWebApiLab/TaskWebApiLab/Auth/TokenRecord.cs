using Microsoft.CodeAnalysis.Elfie.Extensions;

namespace TaskWebApiLab.Auth
{
    public record TokenRecord(string Token, DateTime Expiration);
}
