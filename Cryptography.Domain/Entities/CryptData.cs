namespace Cryptography.Domain.Entities
{
    public class CryptData
    {
        public long Id { get; set; }
        public string UserDocument { get; set; } = string.Empty;
        public string CreditCardToken { get; set; } = string.Empty;
        public long Value { get; set; }
    }
}
