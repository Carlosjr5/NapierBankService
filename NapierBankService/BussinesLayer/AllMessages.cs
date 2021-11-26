using NapierBankService.DataLayer;

namespace NapierBankService
{
    /*
    * Author: Carlos Jimenez Rodriguez, 40452913
    * Description of class: In this class I am saving the inputs for later, save it on lists, and print it back.
    * Date last modified: 26/11/2021
    */
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
                //ID 9 numbers starting with the letter or the message S,E OR T.
                if ((value.Length == 10) && (int.TryParse((value.Remove(0, 1)), out int k)))
                {
                    id = value.ToUpper();
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
