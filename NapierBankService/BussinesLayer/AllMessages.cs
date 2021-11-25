using NapierBankService.DataLayer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace NapierBankService
{
    class AllMessages
    {
        private string id;
        private string message;
        private string twitter_user;
        private string sms_phone;
        private string email_address;
        private string email_subject;
        public string Sc1 { get; set; }
        public string Sc2 { get; set; }
        public string Sc3 { get; set; }
        public string Date { get; set; }


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
                // Checking for any abbreviation calling the class made, which is checking for any.
                ChoseAbvs abvs = new ChoseAbvs();
                string addabv = abvs.main(message);
                message = addabv;
                message = value;
            

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

        public string Sms_Phone
        {
            get { return sms_phone; }
            set
            {              
               sms_phone = value;
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
                    email_subject = value;
                
            }
        }
       


    }
}
