using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace NapierBankService
{
    class AllMessages
    {
        private string regex = @"^(?<countryCode>[\+][1-9]{1}[0-9]{0,2})?(?<areaCode>0?[1-9]\d{0,4})(?<number>[1-9][\d]{5,12})(?<extension>x\d{0,4})?$";
        private string id;
        private string message;
        private string twitter_user;
        private string sms_phone;
        private string email_address;
        private string email_subject;
       
       
        public string ID
        {
            get { return id; }
            set
            {
                // if the id is 10 chars long (1 letter and 9 numbers) and the other 9 are numbers
                if((value.Length == 10) && (int.TryParse((value.Remove(0, 1)), out int k)))
                {
                    id = value.ToUpper();
                }
                else
                {
                    throw new ArgumentException("ID is not written in the correct format: 'S','E' or 'T' followed by 9 numeric characters");
                }
            }
        }
        public string Message
        {
            get { return message; }

            set
            {


                if ((value.Length > 0) && (value.Length < 141))
                    message = value;
                else

                    throw new ArgumentException("message between 0 and 140 characters, try again.");

            }
        }



        public string Twitter_User
        {
            get { return twitter_user; }

            set
            {
               
                    twitter_user = value;

                
            }
        }      

        public string Sms_phone
        {
            get { return sms_phone; }
            set
            {
                if (!Regex.IsMatch(value, regex)) throw new ArgumentException("Incorrect phone number");
                else sms_phone = value;
            }

        }

        public string Email_address
        {
            get { return email_address; }
            set
            {
               
                email_address = value;
            }
        }

        public string Email_subject
        {
            get { return email_subject; }
            set
            {
                if ((value.Length < 21) && (value.Length > 0))
                    email_subject = value;
                else
                    throw new Exception("Subject can be a max of 20 characters.");
            }
        }
       


    }
}
