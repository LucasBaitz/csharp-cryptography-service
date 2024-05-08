namespace Cryptography.Services.DTOs
{
    public record CryptDataDTO
    { 
        public long Id { get; set; }
        public string UserDocument { get; init; } = string.Empty;
        public string CreditCardToken { get; init; } = string.Empty;
        public long Value { get; init; }
    }
}
