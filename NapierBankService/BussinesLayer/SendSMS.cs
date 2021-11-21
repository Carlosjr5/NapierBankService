using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace NapierBankService
{
    class SendSMS : AllMessages
    {
       
        private string sms_phone;       

       

        public string Sms_phone
        {
            get { return sms_phone; }
            set { sms_phone = value;}

        }

       

    }
}

