using System;

namespace AuthorizeNet.Utility
{
    public class GatewayHelpers
    {
        public enum GatewayProxyServerStatus
        {
            Normal = 0,
            Retrieved = 1,
            NoBackup = 2,
            Unknown = 3,
            DbError = 4,
            MissingOrgName = 5,
            InvalidRequest = 6,
            InvalidProvider = 7,
        }
    }

    public static class FinancialHelpers
    {
        public static decimal DEFAULT_AUTH_AMOUNT = 1.5m;

        public static CreditCard GetCreditCardInfo(CreditCardMetadata incomingCreditCard)
        {
            var creditCard = new CreditCard
            {
                CreditCardId = incomingCreditCard.CreditCardId,
                AccountId = incomingCreditCard.AccountId,
                ContactId = incomingCreditCard.ContactId,
                Type = incomingCreditCard.Type ?? null,
                Name = string.IsNullOrEmpty(incomingCreditCard.Name) ? null : incomingCreditCard.Name,
                Number = string.IsNullOrEmpty(incomingCreditCard.Number) ? null : incomingCreditCard.Number,
                CardholderName = string.IsNullOrEmpty(incomingCreditCard.CardholderName) ? null : incomingCreditCard.CardholderName,
                ExpirationMonth = incomingCreditCard.ExpirationMonth ?? null,
                ExpirationYear = string.IsNullOrEmpty(incomingCreditCard.ExpirationYear) ? null : incomingCreditCard.ExpirationYear,
                Address = string.IsNullOrEmpty(incomingCreditCard.Address) ? null : incomingCreditCard.Address,
                City = string.IsNullOrEmpty(incomingCreditCard.City) ? null : incomingCreditCard.City,
                StateOrProvince = string.IsNullOrEmpty(incomingCreditCard.StateOrProvince) ? null : incomingCreditCard.StateOrProvince,
                PostalCode = string.IsNullOrEmpty(incomingCreditCard.PostalCode) ? null : incomingCreditCard.PostalCode,
                StatusCode = incomingCreditCard.StatusCode ?? null,
                TransactionCode = incomingCreditCard.TransactionCode,
                AuthCode = incomingCreditCard.AuthCode,
                ResultCode = incomingCreditCard.ResultCode,
                ProfileCode = incomingCreditCard.ProfileCode,
                ProfilePaymentCode = incomingCreditCard.ProfilePaymentCode,
                ResultMessage = incomingCreditCard.ResultMessage,
            };
            if (!string.IsNullOrEmpty(incomingCreditCard.ExpirationDate))
            {
                if (incomingCreditCard.ExpirationDate.Length > 4 && incomingCreditCard.ExpirationDate.Contains("/"))
                {
                    if (incomingCreditCard.ExpirationDate.Length == 5)
                        incomingCreditCard.ExpirationDate = incomingCreditCard.ExpirationDate.Replace("/", "");
                    else if (incomingCreditCard.ExpirationDate.Length == 7)
                        incomingCreditCard.ExpirationDate = incomingCreditCard.ExpirationDate.Substring(0, 2)
                                                            + incomingCreditCard.ExpirationDate.Substring(5, 2);
                }

                if (incomingCreditCard.ExpirationDate.Length == 4)
                {
                    CreditCard.Enums.ExpirationMonth month =
                        GetExpirtationMonthFromNumber(incomingCreditCard.ExpirationDate.Substring(0, 2));
                    creditCard.ExpirationMonth = month;
                    creditCard.ExpirationYear = incomingCreditCard.ExpirationDate.Substring(2, 2);
                }
            }

            return creditCard;
        }

        public static CreditCard.Enums.ExpirationMonth GetExpirtationMonthFromNumber(string number)
        {
            CreditCard.Enums.ExpirationMonth month = new CreditCard.Enums.ExpirationMonth();
            switch (number)
            {
                case "01":
                    month = CreditCard.Enums.ExpirationMonth.January;
                    break;

                case "02":
                    month = CreditCard.Enums.ExpirationMonth.February;
                    break;

                case "03":
                    month = CreditCard.Enums.ExpirationMonth.March;
                    break;

                case "04":
                    month = CreditCard.Enums.ExpirationMonth.April;
                    break;

                case "05":
                    month = CreditCard.Enums.ExpirationMonth.May;
                    break;

                case "06":
                    month = CreditCard.Enums.ExpirationMonth.June;
                    break;

                case "07":
                    month = CreditCard.Enums.ExpirationMonth.July;
                    break;

                case "08":
                    month = CreditCard.Enums.ExpirationMonth.August;
                    break;

                case "09":
                    month = CreditCard.Enums.ExpirationMonth.September;
                    break;

                case "10":
                    month = CreditCard.Enums.ExpirationMonth.October;
                    break;

                case "11":
                    month = CreditCard.Enums.ExpirationMonth.November;
                    break;

                case "12":
                    month = CreditCard.Enums.ExpirationMonth.December;
                    break;

            }

            return month;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cardNumber"></param>
        /// <returns></returns>
        public static string GetLastFourDigit(string cardNumber)
        {
            if (cardNumber != string.Empty && cardNumber.Length > 11)
            {
                cardNumber = cardNumber.Substring(cardNumber.Length - 4, 4);
                return cardNumber;
            }
            return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullname"></param>
        /// <returns></returns>
        public static string GetFirstName(string fullname)
        {
            string[] array = fullname.Split(' ');
            if (array.Length > 0)
            {
                return array[0];
            }
            else
                return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fullname"></param>
        /// <returns></returns>
        public static string GetLastName(string fullname)
        {
            string[] array = fullname.Split(' ');
            if (array.Length == 2)
            {
                return array[1];
            }
            else if (array.Length == 3)
            {
                return array[2];
            }
            else
            {
                return string.Empty;
            }
        }

        public static int GetMonthValue(CreditCardMetadata incomingCreditCard)
        {
            int expMonth = -1;
            if (incomingCreditCard.ExpirationMonth != null)
            {
                expMonth = incomingCreditCard.ExpirationMonth != null ? (int)incomingCreditCard.ExpirationMonth : -1;
            }

            if (expMonth != -1)
            {
                return expMonth;
            }
            else
            {
                if (incomingCreditCard.ExpirationDate.Length > 4 && incomingCreditCard.ExpirationDate.Contains("/"))
                {
                    DateTime date;
                    if (DateTime.TryParse(incomingCreditCard.ExpirationDate, out date)) ;
                    {
                        return date.Month;
                    }
                }

                if (incomingCreditCard.ExpirationDate.Length == 4 && IsNumber(incomingCreditCard.ExpirationDate))
                {
                    var month = incomingCreditCard.ExpirationDate.Substring(0, 2);
                    return Int32.Parse(month);
                }
            }

            return expMonth;
        }

        public static int GetYearValue(CreditCardMetadata incomingCreditCard)
        {
            if (incomingCreditCard.ExpirationYear != null)
            {
                int expYear;
                if (Int32.TryParse(incomingCreditCard.ExpirationYear, out expYear))
                {
                    if (expYear < 100)
                        return expYear;
                    else
                    {
                        return GetTwoDigitYearFromFourDigit(expYear);
                    }
                }
            }
            else
            {
                if (incomingCreditCard.ExpirationDate.Length > 4 && incomingCreditCard.ExpirationDate.Contains("/"))
                {
                    DateTime date;
                    if (DateTime.TryParse(incomingCreditCard.ExpirationDate, out date)) ;
                    {
                        return GetTwoDigitYearFromFourDigit(date.Year);
                    }
                }
                if (incomingCreditCard.ExpirationDate.Length == 4 && IsNumber(incomingCreditCard.ExpirationDate))
                {
                    var year = incomingCreditCard.ExpirationDate.Substring(2, 2);
                    return Int32.Parse(year);
                }
            }

            return 0;
        }

        private static int GetTwoDigitYearFromFourDigit(int year)
        {
            var yearString = year.ToString();
            if (yearString != string.Empty && yearString.Length > 3)
                yearString = yearString.Substring(yearString.Length - 2, 2);
            return Int32.Parse(yearString);
        }

        private static bool IsNumber(string numberString)
        {
            int number;
            if (Int32.TryParse(numberString, out number))
                return true;
            return false;
        }
    }
}
