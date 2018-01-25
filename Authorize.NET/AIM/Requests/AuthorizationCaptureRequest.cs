using AuthorizeNet.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AuthorizeNet.AIM.Requests
{
    public class AuthorizationCaptureRequest : GatewayRequest
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationRequest"/> class.
        /// </summary>
        /// <param name="cardNumber">The card number.</param>
        /// <param name="expirationMonthAndYear">The expiration month and year.</param>
        /// <param name="amount">The amount.</param>
        /// <param name="description">The description.</param>
        public AuthorizationCaptureRequest(string cardNumber, string expirationMonthAndYear, decimal amount, string description)
        {
            //this.SetApiAction(RequestAction.AuthorizeAndCapture);
            SetQueue(cardNumber, expirationMonthAndYear, amount, description);
            this.Queue(ApiFields.CustomerIPAddress, this.CustomerIp);
        }
        private void SetApiAction(RequestAction action)
        {
            var apiValue = "AUTH_CAPTURE";

            switch (action)
            {
                case RequestAction.Authorize:
                    apiValue = "AUTH_ONLY";
                    break;
                case RequestAction.Capture:
                    apiValue = "CAPTURE_ONLY";
                    break;
                case RequestAction.UnlinkedCredit:
                case RequestAction.Credit:
                    apiValue = "CREDIT";
                    break;
                case RequestAction.Void:
                    apiValue = "VOID";
                    break;
                case RequestAction.PriorAuthCapture:
                    apiValue = "PRIOR_AUTH_CAPTURE";
                    break;
            }
            Queue(ApiFields.TransactionType, apiValue);
        }

        public AuthorizationCaptureRequest(CreditCardMetadata incomingCreditCard, decimal amount, bool includeCapture)
        {
            if (includeCapture)
            {
                SetApiAction(RequestAction.AuthorizeAndCapture);
            }
            else
            {
                SetApiAction(RequestAction.Authorize);
            }
            base.AddFraudCheck();
            SetQueue(incomingCreditCard.Number, incomingCreditCard.ExpirationDate, amount, incomingCreditCard.Description);
            AddCustomer(incomingCreditCard.CustomerId, incomingCreditCard.Email, FinancialHelpers.GetFirstName(incomingCreditCard.CardholderName), FinancialHelpers.GetLastName(incomingCreditCard.CardholderName), incomingCreditCard.Address, incomingCreditCard.City, incomingCreditCard.StateOrProvince, incomingCreditCard.PostalCode);
            AddCardCode(incomingCreditCard.Cvv);
            SetAdditonalFields(incomingCreditCard);
            this.CustomerIp = GetExternalIp();
            this.Queue(ApiFields.CustomerIPAddress, this.CustomerIp);
        }

        private void SetAdditonalFields(CreditCardMetadata incomingCreditCard)
        {
            if(!string.IsNullOrEmpty(incomingCreditCard.PhoneNumber))
                this.Queue(ApiFields.Phone, incomingCreditCard.PhoneNumber);
            if (!string.IsNullOrEmpty(incomingCreditCard.PhoneNumber))
                this.Queue(ApiFields.Fax, incomingCreditCard.Fax);
            if (!string.IsNullOrEmpty(incomingCreditCard.PhoneNumber))
                this.Queue(ApiFields.CustomerID, incomingCreditCard.Fax);
            if (!string.IsNullOrEmpty(incomingCreditCard.PhoneNumber))
                this.Queue(ApiFields.PONumber, incomingCreditCard.PONumber);
            if (!string.IsNullOrEmpty(incomingCreditCard.PhoneNumber))
                this.Queue(ApiFields.InvoiceNumber, incomingCreditCard.InvoiceNumber);
            this.Queue(ApiFields.CustomerID, incomingCreditCard.CustomerId);
            if (!string.IsNullOrEmpty(incomingCreditCard.PhoneNumber))
                this.Queue(ApiFields.Country, incomingCreditCard.Country);
        }

        private string GetExternalIp()
        {
            try
            {
                string externalIP;
                externalIP = (new WebClient()).DownloadString("http://checkip.dyndns.org/");
                externalIP = (new Regex(@"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}"))
                             .Matches(externalIP)[0].ToString();
                return externalIP;
            }
            catch { return null; }
        }

        /// <summary>
        /// Loader for use with form POSTS from web
        /// </summary>
        /// <param name="post"></param>
        public AuthorizationCaptureRequest(NameValueCollection post)
        {
            this.SetApiAction(RequestAction.AuthorizeAndCapture);
            this.Queue(ApiFields.CreditCardNumber, post[ApiFields.CreditCardNumber]);
            this.Queue(ApiFields.CreditCardExpiration, post[ApiFields.CreditCardExpiration]);
            this.Queue(ApiFields.Amount, post[ApiFields.Amount]);
        }

        protected virtual void SetQueue(string cardNumber, string expirationMonthAndYear, decimal amount, string description)
        {
            this.Queue(ApiFields.CreditCardNumber, cardNumber);
            this.Queue(ApiFields.CreditCardExpiration, expirationMonthAndYear);
            this.Queue(ApiFields.Amount, amount.ToString(CultureInfo.InvariantCulture));
            this.Queue(ApiFields.Description, description);
        }
    }
}
