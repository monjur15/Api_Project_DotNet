namespace Assesment_one.Models
{
    public class SubscriptionRequest
    {
        public string Amount { get; set; } = "1";
        public string FirstPaymentIncludedInCycle { get; set; } = "True";
        public string ServiceId { get; set; } = "100001";
        public string Currency { get; set; } = "BDT";
        public string StartDate { get; set; }
        public string ExpiryDate { get; set; }
        public string Frequency { get; set; }
        public string SubscriptionType { get; set; } = "BASIC";
        public string MaxCapRequired { get; set; } = "False";
        public string MerchantShortCode { get; set; } = "01307153119";
        public string PayerType { get; set; } = "CUSTOMER";
        public string PaymentType { get; set; } = "FIXED";
        public string RedirectUrl { get; set; }
        public string SubscriptionRequestId { get; set; }
        public string SubscriptionReference { get; set; }
        public string CKey { get; set; } = "000001";
    }
}
