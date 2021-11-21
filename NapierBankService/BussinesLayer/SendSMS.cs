using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace NapierBankService
{
    class SendSMS : AllMessages
    {
       
        private string sms_phone;       
       private string regex = @"^(?<countryCode>[\+][1-9]{1}[0-9]{0,2})?(?<areaCode>0?[1-9]\d{0,4})(?<number>[1-9][\d]{5,12})(?<extension>x\d{0,4})?$";

       

        public string Sms_phone
        {
            get { return sms_phone; }
            set {if (!Regex.IsMatch(value, regex))throw new ArgumentException("Incorrect phone number");
                 else   sms_phone = value;}

        }

       

    }
}

