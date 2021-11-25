﻿using Microsoft.Win32;
using NapierBankService.BussinesLayer;
using NapierBankService.DataLayer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NapierBankService
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {




        List<string> mentions = new List<string>();
        Dictionary<string, int> hashtags = new Dictionary<string, int>();
        List<AllMessages> output = new List<AllMessages>();
        List<AllMessages> input = new List<AllMessages>();
        List<string> List_QUAR = new List<string>();
        List<string> LIST_SIR = new List<string>();

        //AllMessages message = new AllMessages();


        SendTWEET tweet = new SendTWEET();
        SendSMS sms = new SendSMS();
        AllMessages allmsg = new AllMessages();
        List<AllMessages> msgs = new List<AllMessages>();
        List<string> incidentList = new List<string>();
        generate newsms = new generate();



        public MainWindow()
        {


            InitializeComponent();

            msgs = outputFile(msgs);

            emptyJson();

            foreach (var Hastaghs in hashtags)
            {
                hashtag_list.Items.Add(Hastaghs.Value);
            }


            if (msgs != null)
            {

                foreach (var Data in msgs)
                {
                    // msgs = Serializing(msgs);
                    data_listbox.Items.Add(Data.ID);
                }
            }

            if (mentions != null) {
                foreach (var Mention in mentions)
                {

                    mention_list.Items.Add(Mention);
                }

               
            }

           

                        
                    
                
            


        }


        static List<AllMessages> outputFile(List<AllMessages> msgs)
        {

            string fileName = @"../../../Json_data.json";

            if (File.Exists(fileName))
            {
                var ALLDATA = JsonConvert.DeserializeObject<List<AllMessages>>(File.ReadAllText(fileName));
                return ALLDATA;
            }
            return null;
        }

        private void emptyJson()
        {
            //call file name
            // check if is empty, and if is empty input a []

            List<AllMessages> msgs = new List<AllMessages>();

                string fileName = @"../../../Json_data.json";
                string fileNameTXT = @"../../../Json_data.txt";


            if (File.Exists(fileName))
            {
                var ALLDATA = JsonConvert.SerializeObject(File.ReadAllText(fileName));

                while (ALLDATA.Equals(""))
                {
                    File.WriteAllText(fileName, "[]");
                    File.WriteAllText(fileNameTXT, "[]");
                }
            }

            


        }


        private void send_btn_Click(object sender, RoutedEventArgs e)
        {

          
          List<AllMessages> msgs = new List<AllMessages>();


            string user = user_txtbox.Text;
            string message = body_txtbox.Text;
            string mail_subject = subject_txtbox.Text;
            string numbers = new String(user.ToCharArray().Where(c => Char.IsDigit(c)).ToArray()); //checks if the input is only digits.
            string SC1 = sc1_txtbox.Text;
            string SC2 = sc2_txtbox.Text;
            string SC3 = sc3_txtbox.Text;
            string DATEPICKER = date_sir.Text;

            MainWindow main = new MainWindow();


           






            if (!user.StartsWith("@") && !user.StartsWith("+") && mail_subject.Contains("SIR") && mail_subject.Contains("sir"))
            {
                
                    MessageBox.Show("Incorrect format! \n For Twitter, Sender has to start with '@', followed by the twitter name," +
                        "                              \n  For SMS, has to start with '+' followed by the country code and the phone number," +
                        "                              \n  For Standard email just the normall email, a subject and the message, " +
                        "                              \n  For SIGNIFICANT INCIDENT REPORTS, subject have to be SIR, fill up all the boxes, now try again! :)" + MessageBoxImage.Information);
                return;
                
            }

            //SENDING TWEETS AND ADDING IT TO THE JSON FILE.
            if (user.StartsWith("@"))
            {

                if (message.Length > 140 || message.Length == 0 || message == "" || user == "")
                {
                    MessageBox.Show("The boxes cant be empty and the message cant be more than 140 characters for a sms or twitter! :( ");
                    return;
                }


                // Checking for any abbreviation calling the class made, which is checking for any.
                ChoseAbvs abvs = new ChoseAbvs();
                string addabv = abvs.main(message);
                message = addabv;

                msgs.Add(new AllMessages()
                {

                    ID = newsms.Generator_Id("T"),

                    Message = newsms.Twitter_tweet(message, "T"),

                    Twitter_User = newsms.Twitter_username(user, "T")


                });

                MessageBox.Show("Tweet Sent!");



                if (message != null)
                {


                    // SEARCH THROUGH WORDS 
                    string sentence = message;
                    foreach (string word in (sentence).Split(' '))
                    {
                        // If there is a mention
                        if (word.StartsWith("@"))
                        {
                            if (!mentions.Contains(word))
                            {
                                mentions.Add(word);
                                mention_list.Items.Add(word);


                            }
                        }

                        // If there is a hashtag
                        if (word.StartsWith("#"))
                        {
                            if (hashtags.ContainsKey(word))
                            {
                                hashtags[word] += 1;
                            }
                            else
                            {
                                hashtags.Add(word, 1);
                                hashtag_list.Items.Add(word);
                            }
                        }
                    }
                }

                if (msgs != null)
                {
                    //data_listbox.Items.Clear();
                    foreach (var Data in msgs)
                    {

                        data_listbox.Items.Add(Data.ID);
                    }
                }


            }
           


            //SENDING SMS AND ADDING IT TO THE JSON LIST.
            if (user.StartsWith("+"))
            {

                // Checking for any abbreviation calling the class made, which is checking for any.
                ChoseAbvs abvs = new ChoseAbvs();
                string addabv = abvs.main(message);
                message = addabv;

                if ((user.Length >= 13) || (user.Length <= 6) || user == numbers)
                {
                    MessageBox.Show("Incorrect phone number format, have to be with the country code followed by the ccountry, i.e(+34745738295).");
                    return;

                }
                else
                {

                    msgs.Add(new AllMessages()
                    {
                        ID = newsms.Generator_Id("S"),

                        Message = newsms.SMS_type(message, "S"),

                        Sms_Phone = newsms.SMS_type_PHONE(user, "S")

                    });



                    MessageBox.Show("SMS Sent!");
                    if (msgs != null)
                    {
                        data_listbox.Items.Clear();
                        foreach (var Data in msgs)
                        {

                            data_listbox.Items.Add(Data.ID);
                        }
                    }


                }
            }











           


            //SENDING EMAILS (STANDARD EMAIL MESSAGES & SIGNIFICANT INCIDENT REPORTS.)
            if (user.Contains("@") && mail_subject != "" && !user.StartsWith("@") && !mail_subject.Contains("SIR"))
            {


                if (message.Length > 1029)
                {

                    MessageBox.Show("The message cant be longer than 1029 characters, try again!.");
                    return;
                }

                if (mail_subject == null)
                {
                    MessageBox.Show("The Subject cant be empty, for a significant incident report the subject must be 'SIR'");
                    return;
                }


               

                //  msgs = Serializing(msgs);



                msgs.Add(new AllMessages()
                {
                    ID = newsms.Generator_Id("E"),

                    Message = newsms.Email_message(message, "E"),

                    Email_subject = newsms.Email_subject(mail_subject, "E"),

                    Email_address = newsms.Email_address(user, "E"),              

                });

                MessageBox.Show("STANDARD EMAIL MESSAGE SENT!");

                


                // URLs 
                message = URL_QUARANTINED(message);


                // check nature of incident

                string sentence = message;






                
            }




            // check nature of incident

            Boolean found = false;

            List<string> incidentList = new List<string>();

            incidentList.Add("cash loss");
            incidentList.Add("bomb threat");
            incidentList.Add("staff attack");
            incidentList.Add("atmtheft");
            incidentList.Add("suspicious incident");
            incidentList.Add("customer attack");
            incidentList.Add("staff abuse");
            incidentList.Add("terrorism");
            incidentList.Add("theft");
            incidentList.Add("intelligence");
            incidentList.Add("raid");
            incidentList.Add("Cash loss");
            incidentList.Add("Bomb threat");
            incidentList.Add("Staff attack");
            incidentList.Add("Atm theft");
            incidentList.Add("Suspicious incident");
            incidentList.Add("Customer attack");
            incidentList.Add("Staff abuse");
            incidentList.Add("Terrorism");
            incidentList.Add("Theft");
            incidentList.Add("Intelligence");
            incidentList.Add("Raid");

          
           

            //Significant incident report check.
            if (mail_subject.Contains("SIR") || mail_subject.Contains("sir") || mail_subject.Contains("SIGNIFICANT INCIDENT REPORTS") || mail_subject.Contains("Sir")) 
            {

               

                if (SC1.Length > 2 || SC2.Length > 2 || SC3.Length > 2 )                  
                {
                    MessageBox.Show("The Sort Code can´t be longer than 2 characters, example (99-99-99), you need to enter 2 numbers on each text box, try again:)");
                    return;

                }

                
                if (!CheckDigits(SC1) || !CheckDigits(SC2) || !CheckDigits(SC3))
                {

                    MessageBox.Show("For sending an significant incident report you must fill up the sort code(only numbers accepted) boxes and enter a date of incident, and the subject must be 'SIR', 'sir', 'SIGNIFICANT INCIDENT REPORTS' or  'Sir' .");
                    return;
                }
                

                if (SC1.Length == 0 || SC2.Length == 0 || SC3.Length == 0 || DATEPICKER == null || SC1.Length < 2 || SC2.Length < 2|| SC3.Length < 2)
                {



                    MessageBox.Show("For sending an significant incident report you must fill up the sort code(only numbers accepted) boxes and enter a date of incident, and the subject must be 'SIR', 'sir', 'SIGNIFICANT INCIDENT REPORTS' or  'Sir' .");
                    main.Show();
                    Close();






                }
                else
                {

                    msgs.Add(new AllMessages()
                    {
                        ID = newsms.Generator_Id("E"),

                        Message = newsms.Email_message(message, "E"),

                        Email_subject = newsms.Email_subject(mail_subject, "E"),

                        Email_address = newsms.Email_address(user, "E"),

                        Sc1 = newsms.SC1(SC1, "E"),
                        Sc2 = newsms.SC2(SC2, "E"),
                        Sc3 = newsms.SC3(SC3, "E"),

                        Date = newsms.Datepicker(DATEPICKER, "E")


                    });


                    //Each time there is an inccident from the list on the message it will print all the data on the SIR list.
                    foreach (string incident in incidentList)
                    {

                        string letter = (((incident).Split(',')[0]).ToLower());

                        letter = Regex.Replace(letter, @"\s+", "");


                        LIST_SIR.Add(message);
                        list_SIR.Items.Add("Date: " + DATEPICKER + ", Sort Code: " + SC1 + "," + SC2 + "," + SC3 + ". Nature of Incident: " + incident);
                        found = true;

                    }
                    if (!found)
                        MessageBox.Show("Nature of incident not found.");


                   

                }




                

                


            }



           
          

     
           

            if (msgs != null)
            {
                data_listbox.Items.Clear();
                foreach (var Data in msgs)
                {

                    data_listbox.Items.Add(Data.ID);
                }
            }


            // Writing the inputs to the json file.
            string jsonFile3 = @"../../../Json_data.json";
            string txtFile3 = @"../../../Json_data.txt";

            string serialize = JsonConvert.SerializeObject(msgs.ToArray(), Formatting.Indented);
            string serializetxt = JsonConvert.SerializeObject(msgs.ToArray() + "\n");

            File.WriteAllText(jsonFile3, serialize);
            File.WriteAllText(txtFile3, serializetxt);

        }

        bool CheckDigits(string str)
        {
            foreach (char d in str)
            {
                if (d < '0' || d > '9')
                    return false;
            }

            return true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                if (fileDialog.ShowDialog() == true)
                {
                    string find = System.IO.Path.GetExtension(fileDialog.FileName);

                    if (find.Equals(".json"))
                    {
                        System.IO.StreamReader fileJson = new System.IO.StreamReader(fileDialog.FileName);

                        string lineJson = "";
                        string tempObj = "";



                        while ((lineJson = fileJson.ReadLine()) != null)
                        {
                            string[] objects = lineJson.Split('}');

                            Array.Resize(ref objects, objects.Length - 1);

                            foreach (string obj in objects)
                            {
                                if (obj == String.Empty)
                                    break;



                                string all = obj.Substring(obj.IndexOf("")); // id and Message                          
                                string id = all.Split(',')[0]; // id
                                tempObj = (obj + "}").Remove(0, 1); // THE JSON STRING                             



                                if (Regex.IsMatch(id, "S"))
                                {
                                    AllMessages data = JsonConvert.DeserializeObject<AllMessages>(tempObj);

                                    allmsg.ID = data.ID;
                                    allmsg.Message = data.Message;


                                    data_listbox.Items.Add(data.ID);


                                    input.Add(data);
                                    //ssg.sms_message.Insert(message_txtbox);

                                    // displaying information.
                                    header_txtblock.Text = data.ID;
                                    body_txtbox.Text = data.Message;
                                    user_txtbox.Text = data.Sms_Phone;
                                    subject_txtbox.Text = " ";
                                    sc1_txtbox.Text = " ";
                                    sc2_txtbox.Text = " ";
                                    sc3_txtbox.Text = " ";



                                }
                                else if (Regex.IsMatch(id, "T"))
                                {

                                    AllMessages data = JsonConvert.DeserializeObject<AllMessages>(tempObj);


                                    allmsg.ID = data.ID;
                                    allmsg.Message = data.Message;
                                    //  allmsg.twitter_username = twt.twitter_username;



                                    data_listbox.Items.Add(data.ID);


                                    input.Add(data);



                                    // displaying information.
                                    header_txtblock.Text = data.ID;
                                    body_txtbox.Text = data.Message;
                                    user_txtbox.Text = data.Twitter_User;
                                    subject_txtbox.Text = " ";
                                    sc1_txtbox.Text = " ";
                                    sc2_txtbox.Text = " ";
                                    sc3_txtbox.Text = " ";

                                }

                                else if (Regex.IsMatch(id, "E"))
                                {

                                    AllMessages data = JsonConvert.DeserializeObject<AllMessages>(tempObj);


                                    allmsg.ID = data.ID;
                                    allmsg.Message = data.Message;
                                    // mail.email_subject = mails.email_subject;
                                    //  mail.email_message = mails.email_message;


                                    data_listbox.Items.Add(data.ID);



                                    input.Add(data);

                                    // displaying information.
                                    header_txtblock.Text = data.ID;
                                    user_txtbox.Text = data.Email_address;
                                    subject_txtbox.Text = data.Email_subject;
                                    body_txtbox.Text = data.Message;

                                    if (subject_txtbox.Text.Contains("SIR") || subject_txtbox.Text.Contains("sir") || subject_txtbox.Text.Contains("SIGNIFICANT INCIDENT REPORTS") || subject_txtbox.Text.Contains("Sir"))
                                    {
                                        sc1_txtbox.Text = data.Sc1;
                                        sc2_txtbox.Text = data.Sc2;
                                        sc3_txtbox.Text = data.Sc3;

                                    }

                                }
                            }
                        }
                    }
                    else if (find.Equals(".txt"))
                    {
                        File.ReadAllText(fileDialog.FileName);

                        foreach (string line in File.ReadAllLines(fileDialog.FileName))
                        {

                            txt_list.Items.Add(line);

                        }
                    }



                }
                else
                {
                    MessageBox.Show("Your file is empty.");
                }
            }
            catch (Exception a)
            {
                MessageBox.Show(a.Message);
            }
        }

        private string URL_QUARANTINED(string phrase)
        {
            foreach (string word in phrase.Split(' '))
            {
                if (word.StartsWith("http:") || word.StartsWith("https:"))
                {
                    string f = phrase.Replace(word, "<URL Quarantined>");
                    phrase = f;
                    foreach (string word1 in (phrase).Split(' '))
                    {
                        if (!List_QUAR.Contains(word))
                        {
                            List_QUAR.Add(word);
                            url_list.Items.Add(word);
                        }
                    }
                }
            }

            return phrase;
        }

        private void printTWEET(AllMessages message)
        {



            try
            {
                tweet.ID = message.ID;
                tweet.Message = message.Message;
                tweet.Twitter_User = message.Twitter_User;



                // TEXT - max 140 chars
                int i = message.Message.IndexOf(" ") + 1;
                string str = message.Message.Substring(i);
                tweet.Message = str;


                body_txtbox.Text = message.Message;
                header_txtblock.Text = message.ID;
                user_txtbox.Text = message.Twitter_User;
                subject_txtbox.Text = " ";

                // Checking for any abbreviation calling the class made, which is checking for any.
                // shortAbs abvs = new shortAbs();
                // string newM = abvs.main(tweet.Message);
                // tweet.Message = newM;

                // SEARCH THROUGH WORDS 
                string sentence = tweet.Message;

                foreach (string word in (sentence).Split(' '))
                {
                    // If there is a mention
                    if (word.StartsWith("@"))
                    {
                        if (!mentions.Contains(word))
                        {
                            mentions.Add(word);
                            mention_list.Items.Add(word);
                        }
                    }

                    // If there is a hashtag
                    if (word.StartsWith("#"))
                    {
                        if (hashtags.ContainsKey(word))
                        {
                            hashtags[word] += 1;
                            hashtag_list.Items.Add(word);
                        }
                        else
                        {
                            hashtags.Add(word, 1);
                        }
                    }
                }

                //Output file
                output.Add(tweet);
                outputFile(output);

                // SHOW RESULTS
                header_txtblock.Text = tweet.ID;
                body_txtbox.Text = tweet.Message;
                user_txtbox.Text = tweet.Twitter_User;

                // TRENDING LIST
                hashtag_list.Items.Clear();
                var sortedDict = hashtags.OrderBy(x => x.Value);
                foreach (var item in sortedDict.OrderByDescending(key => key.Value))
                {
                    hashtag_list.Items.Add(item);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void printSMS(AllMessages message)
        {


           

            // SENDER - int
            try
            {
                sms.ID = message.ID;
                sms.Message = message.Message;
                sms.Sms_Phone = message.Sms_Phone.Split(' ')[1]; // second word (number)


                int i = sms.Message.IndexOf(" ") + 1;
                string str = sms.Message.Substring(i); // delete the first word
                int x = str.IndexOf(" ") + 1;
                string str2 = str.Substring(x); // delete the second word
                sms.Message = str2;

                // Checking for any abbreviation calling the class made, which is checking for any.
                ChoseAbvs abvs = new ChoseAbvs();
                string addabv = abvs.main(sms.Message);
                sms.Message = addabv;

                // OUTPUT TO FILE
                msgs.Add(sms);
                outputFile(msgs);


                body_txtbox.Text = message.Message;
                header_txtblock.Text = message.ID;
                user_txtbox.Text = message.Email_address;
                subject_txtbox.Text = message.Email_subject;

            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }



        }

        //Looks for the json id and prints it.
        private void ChoseItem(object sender, MouseButtonEventArgs e)
        {


            try
            {
                //reads the ID´S.
                string line = Convert.ToString(data_listbox.SelectedItem);

                // search for object with that id in data in
                foreach (AllMessages message in msgs)
                {



                    if (line.Equals(message.ID))
                    {
                        // depending on type, select process
                        if (message.ID[0].Equals('S'))
                        {
                            printSMS(message);


                            body_txtbox.Text = message.Message;
                            header_txtblock.Text = message.ID;
                            user_txtbox.Text = message.Sms_Phone;
                            subject_txtbox.Text = " ";
                            sc1_txtbox.Text = " ";
                            sc2_txtbox.Text = " ";
                            sc3_txtbox.Text = " ";
                            date_sir.Text = " ";




                            break;

                        }
                        else if (message.ID[0].Equals('E'))
                        {

                            body_txtbox.Text = message.Message;
                            header_txtblock.Text = message.ID;
                            user_txtbox.Text = message.Email_address;
                            subject_txtbox.Text = message.Email_subject;
                            sc1_txtbox.Text = message.Sc1;
                            sc2_txtbox.Text = message.Sc2;
                            sc3_txtbox.Text = message.Sc3;
                            date_sir.Text = message.Date;
                         


                            break;
                        }

                        else if (message.ID[0].Equals('T'))
                        {
                            printTWEET(message);
                            body_txtbox.Text = message.Message;
                            header_txtblock.Text = message.ID;
                            user_txtbox.Text = message.Twitter_User;
                            subject_txtbox.Text = "";
                            sc1_txtbox.Text = " ";
                            sc2_txtbox.Text = " ";
                            sc3_txtbox.Text = " ";
                            date_sir.Text = " ";

                            break;
                        }
                    }

                }
            }
            catch (Exception b)
            {
                MessageBox.Show(b.Message);
            }


        }

        private void TxtItem(object sender, MouseButtonEventArgs e)
        {
            try
            {
                //reads the ID´S.
                string line = Convert.ToString(txt_list.SelectedItem);

                // search for object with that id in data in
                foreach (AllMessages message in msgs)
                {



                    if (line.Equals(message.ID))
                    {
                        // depending on type, select process
                        if (message.ID[0].Equals('S'))
                        {
                            printSMS(message);


                            body_txtbox.Text = message.Message;
                            header_txtblock.Text = message.ID;
                            user_txtbox.Text = message.Sms_Phone;
                            subject_txtbox.Text = " ";
                            sc1_txtbox.Text = " ";
                            sc2_txtbox.Text = " ";
                            sc3_txtbox.Text = " ";
                            date_sir.Text = " ";




                            break;

                        }
                        else if (message.ID[0].Equals('E'))
                        {

                            body_txtbox.Text = message.Message;
                            header_txtblock.Text = message.ID;
                            user_txtbox.Text = message.Email_address;
                            subject_txtbox.Text = message.Email_subject;
                            sc1_txtbox.Text = message.Sc1;
                            sc2_txtbox.Text = message.Sc2;
                            sc3_txtbox.Text = message.Sc3;
                            date_sir.Text = message.Date;


                            break;
                        }

                        else if (message.ID[0].Equals('T'))
                        {
                            printTWEET(message);
                            body_txtbox.Text = message.Message;
                            header_txtblock.Text = message.ID;
                            user_txtbox.Text = message.Twitter_User;
                            subject_txtbox.Text = "";
                            sc1_txtbox.Text = " ";
                            sc2_txtbox.Text = " ";
                            sc3_txtbox.Text = " ";
                            date_sir.Text = " ";

                            break;
                        }
                    }

                }
            }
            catch (Exception b)
            {
                MessageBox.Show(b.Message);
            }



        }
    }
}

