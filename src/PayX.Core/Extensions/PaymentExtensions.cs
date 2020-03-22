namespace PayX.Core.Extensions
{
    public static class PaymentExtensions
    {
        public static string GetMaskedCardNumber(this string cardNumber)
        {
            return $"**** **** **** {cardNumber.Substring(cardNumber.Length - 4)}";
        }
    }
}