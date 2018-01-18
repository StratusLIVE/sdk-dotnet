using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthorizeNet.Utility
{
    public class CreditCardMetadata : CreditCard
    {
        public string Cvv { get; set; }
        public string ExpirationDate { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }
        public string CustomerId { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string CustomerIp { get; set; }
    }

    public partial class CreditCard
    {
        public Guid? AccountId { get; set; }
        public Guid? ContactId { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public CreditCard.Enums.Type? Type { get; set; }
        public CreditCard.Enums.ExpirationMonth? ExpirationMonth { get; set; }
        public string ExpirationYear { get; set; }
        public string Address { get; set; }
        public string CardholderName { get; set; }
        public string City { get; set; }
        public string StateOrProvince { get; set; }
        public string PostalCode { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Guid? CreditCardId { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string TransactionCode { get; set; }
        public string AuthCode { get; set; }
        public int? ResultCode { get; set; }
        public CreditCard.Enums.StatusCode? StatusCode { get; set; }
        public string ProfilePaymentCode { get; set; }
        public string ProfileCode { get; set; }
        public CreditCard.Enums.Result? Result { get; set; }
        public string ResultMessage { get; set; }
        public string VendorCode { get; set; }
    }

    public partial class CreditCard
    {
        public partial class Enums
        {
            public enum Result
            {
                Success = 0,
                Declined = 1,
                Expired = 2,
                Error = 3,
                Held = 4,
                Other = 5,
            }
            public enum Type
            {
                Unknown = 0,
                Visa = 1,
                MasterCard = 2,
                AmericanExpress = 3,
                Discover = 4
            }
            public enum StateCode
            {
                Active = 0,
                Inactive = 1,
            }
            public enum ExpirationMonth
            {
                January = 1,
                February = 2,
                March = 3,
                April = 4,
                May = 5,
                June = 6,
                July = 7,
                August = 8,
                September = 9,
                October = 10,
                November = 11,
                December = 12,
            }
            public enum StatusCode
            {
                Active = 1,
                Inactive = 2,
            }
        }
    }
}
