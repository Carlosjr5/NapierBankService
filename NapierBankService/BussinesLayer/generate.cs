using System;

namespace NapierBankService
{
    /*
 * Author: Carlos Jimenez Rodriguez, 40452913
 * 
 * Description of class: The purpose of this class is to generate a ramdom id, and save the phone, email address, twitter usernames,
 * and each with their letter to save the message as the input of the Sender/Username.
 * 
 * Date last modified: 26/11/2021
 */
    class generate
    {
        //New ID each time there is a message
        public string Generator_Id(string type)
        {
            var random = new Random();
            string n = string.Empty;
            for (int i = 0; i < 9; i++)
            {
                n = String.Concat(n, random.Next(10).ToString());
            }
            string name = type + n;

            return name;
        }



        //Saving the Twitter user name with the letter.
        public string Twitter_username(string writter, string letter)
        {
            if (letter == "T")
            {
                string twitter = writter;
                return twitter;
            }
            else
            {
                return "Incorrect format.";
            }

        }




        //Twitter message with letter.
        public string Twitter_tweet(string message, string letter)
        {
            if (letter == "T")
            {
                string tweet = message;
                return tweet;
            }
            else
            {
                return "Incorrect format.";
            }

        }

        //SMS message with letter to save it as a sms
        public string SMS_type(string message, string letter)
        {
            if (letter == "S")
            {
                string SMS = message;
                return SMS;
            }
            else
            {
                return "Incorrect format.";
            }

        }

        //Phone number with the letter to save it as a sms
        public string SMS_type_PHONE(string phone_number, string letter)
        {
            if (letter == "S")
            {
                string SMS_phone = phone_number;
                return SMS_phone;

            }
            else
            {
                return "Incorrect format.";
            }

        }

        //Email address of the user and Letter to save it as a email
        public string Email_address(string email_address, string letter)
        {
            if (letter == "E")
            {
                string address = email_address;
                return address;

            }
            else
            {
                return "Incorrect format.";
            }

        }

        public string Email_subject(string email_subject, string letter)
        {
            if (letter == "E")
            {
                string subject = email_subject;
                return subject;

            }
            else
            {
                return "Incorrect format.";
            }

        }

        public string Email_message(string email_message, string letter)
        {
            if (letter == "E")
            {
                string message = email_message;
                return message;

            }
            else
            {
                return "Incorrect format.";
            }

        }

        public string Sir_email(string sir_message, string letter)
        {
            if (letter == "E")
            {
                string sir = sir_message;
                return sir;
            }
            else
            {
                return "Incorrect format.";
            }

        }

        public string Sir_email_address(string sir_address, string letter)
        {
            if (letter == "E")
            {
                string sir = sir_address;
                return sir;
            }
            else
            {
                return "Incorrect format.";
            }



        }

        public string Datepicker(string date, string letter)
        {
            if (letter == "E")
            {
                string sir = date;
                return sir;
            }
            else
            {
                return "Incorrect format.";
            }
        }

        public string SC1(string sc1, string letter)
        {
            if (letter == "E")
            {
                string sir = sc1;
                return sir;
            }
            else
            {
                return "Incorrect format.";
            }
        }

        public string SC2(string sc2, string letter)
        {
            if (letter == "E")
            {
                string sir = sc2;
                return sir;
            }
            else
            {
                return "Incorrect format.";
            }
        }

        public string SC3(string sc3, string letter)
        {
            if (letter == "E")
            {
                string sir = sc3;
                return sir;
            }
            else
            {
                return "Incorrect format.";
            }
        }




    }
}

