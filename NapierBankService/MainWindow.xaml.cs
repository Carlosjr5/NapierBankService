using Microsoft.Win32;
using NapierBankService.BussinesLayer;
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
        public MainWindow()
        {
            InitializeComponent();
        }



        List<string> quarantineList = new List<string>();
        Dictionary<string, string> SIR = new Dictionary<string, string>();
        List<string> mentions = new List<string>();
        Dictionary<string, int> hashtags = new Dictionary<string, int>();
        List<AllMessages> output = new List<AllMessages>();
        List<AllMessages> input = new List<AllMessages>();




        private void outputFile(List<AllMessages> message)
        {
            using (StreamWriter file = File.CreateText(@"../../../Json_data.json"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, message);
            }
        }

        //Looks for the json id and prints it.
        private void ChoseItem(object sender, MouseButtonEventArgs e)
        {
            try
            {
                // read id
                string line = Convert.ToString(data_listbox.SelectedItem);

                // search for object with that id in data in
                foreach (AllMessages message in input)
                {

                   

                    if (line.Equals(message.ID))
                    {
                        // depending on type, select process
                        if (message.ID[0].Equals('S')) {
                            printSMS(message);
                            body_txtbox.Text = message.Message;
                            header_txtblock.Text = message.ID;
                            user_txtbox.Text = message.Sms_Phone;
                            break;
                        }

                        else if ((message.ID)[0].Equals('E')) { printEMAIL(message); break; }
                        else if ((message.ID)[0].Equals('T')) { printTWEET(message); break; }
                    }
                   
                }
            }
            catch (Exception b)
            {
                MessageBox.Show(b.Message);
            }


        }




        private void send_btn_Click(object sender, RoutedEventArgs e)
        {
            string user = user_txtbox.Text;
            string message = body_txtbox.Text;
           string mail_subject = subject_txtbox.Text;
            // string numbers = new String(user.ToCharArray().Where(c => Char.IsDigit(c)).ToArray()); //checks if the input is only digits.


            //   string type_message = ngenerator.SMS_type(sms_message, "S");
            //  string type_phone = ngenerator.SMS_type_PHONE(sms_phone_number, "S");

            generate newsms = new generate();

            if (message == "" || user == "")
            {
                MessageBox.Show("Empty boxes, try again!.");
            }
            else if (message.Length > 140)
            {
                MessageBox.Show("The message cant be more than 140 characters! :( ");
            }
            else if (user.StartsWith("@"))
            {


                List<SendTWEET> twt = new List<SendTWEET>();
                twt.Add(new SendTWEET()
                {
                    ID = newsms.Generator_Id("T"),

                    Message = newsms.Twitter_tweet(message, "T"),

                    Twitter_User = newsms.Twitter_username(user, "T")


                });

             

                // Writing the inputs to the json file.
                string file_json = JsonConvert.SerializeObject(twt.ToArray());
                File.AppendAllText(@"../../../Json_data.json", file_json + Environment.NewLine);
                File.AppendAllText(@"../../../Json_data.txt", file_json + Environment.NewLine);
                MessageBox.Show("Message: " + message + ", Twitter Account:" + user + ", Message sended!");


            }
            else if(user.StartsWith("+"))
            {
               
                List<SendSMS> smss = new List<SendSMS>();


                smss.Add(new SendSMS()
                {
                    ID = newsms.Generator_Id("S"),

                    Message = newsms.SMS_type(message, "S"),

                    Sms_Phone = newsms.SMS_type_PHONE(user, "S")

                });

              

                // Writing the inputs to the json file.
                string json_sms = JsonConvert.SerializeObject(smss.ToArray());
                File.AppendAllText(@"../../../Json_data.json", json_sms + Environment.NewLine);
                File.AppendAllText(@"../../../Json_data.txt", json_sms + Environment.NewLine);
                MessageBox.Show("Message: "+ message + ", Phone Number:" + user + ", Message sended!");

             


            }
            else if (mail_subject != null)
            {
                List<SendEMAIL> smss = new List<SendEMAIL>();
                smss.Add(new SendEMAIL()
                {
                    ID = newsms.Generator_Id("E"),

                    Message = newsms.Email_message(message, "E"),

                   Email_subject = newsms.Email_subject(mail_subject, "E"),

                    Email_address = newsms.Email_address(user, "E")

                });

                // Writing the inputs to the json file.
                string json_sms = JsonConvert.SerializeObject(smss.ToArray());
                File.AppendAllText(@"../../../Json_data.json", json_sms + Environment.NewLine);
                File.AppendAllText(@"../../../Json_data.txt", json_sms + Environment.NewLine);
                MessageBox.Show("Message sended!");
                MessageBox.Show( "Sender:" + user + ", Message: " + message +  ", Subject" + mail_subject + ", Message sended!");

            }

          
            MainWindow main = new MainWindow();
            main.Show();
            Close();




        }



        private void printTWEET(AllMessages message)
        {
            try
            {
                SendTWEET tweet = new SendTWEET();
                tweet.ID = message.ID;
                tweet.Message = message.Message;
                tweet.Twitter_User = message.Twitter_User;



                // TEXT - max 140 chars
                int i = message.Message.IndexOf(" ") + 1;
                string str = message.Message.Substring(i);
                tweet.Message = str;

                // SEARCH THROUGH WORDS 
                string sentence = tweet.Message;

                foreach (string word in (sentence).Split(' '))
                {
                    // If there is a mention
                    if (message.Twitter_User.StartsWith("@"))
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
                        }
                    }
                }

                    // Checking for any abbreviation calling the class made, which is checking for any.
                   // shortAbs abvs = new shortAbs();
                   // string newM = abvs.main(tweet.Message);
                   // tweet.Message = newM;



                //Output file
                output.Add(tweet);
                MainWindow save = new MainWindow();
                save.outputFile(output);

                // SHOW RESULTS
                header_txtblock.Text = "ID: " + tweet.ID;
                body_txtbox.Text = "Sender: " + tweet.Message;
                user_txtbox.Text = "Text: " + tweet.Twitter_User;

                // TRENDING LIST
                //      trendList.Items.Clear();
                //      var sortedDict = hashtags.OrderBy(x => x.Value);
                //    foreach (var item in sortedDict.OrderByDescending(key => key.Value))
                //  {
                //     trendList.Items.Add(item);
                //   }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void printEMAIL(AllMessages message)
        {
            SendEMAIL email = new SendEMAIL();

            List<string> incidentList = new List<string>();
           
            incidentList.Add("cashloss");
            incidentList.Add("bombthreat");
            incidentList.Add("staffattack");
            incidentList.Add("atmtheft");
            incidentList.Add("suspiciousincident");            
            incidentList.Add("customerattack");
            incidentList.Add("staffabuse");           
            incidentList.Add("terrorism");
            incidentList.Add("theft");
            incidentList.Add("intelligence");
            incidentList.Add("raid");

            //string sentence = "maria@gmail.com ,SIRhello, 99-99-99 ,Theft, Hi";
            string pharse = message.Message;

            try
            {
                email.ID = message.ID; // ID
                email.Message = message.Message; // BODY 
                email.Email_address = pharse.Split(',')[0]; //SENDER
                email.Email_subject = pharse.Split(',')[1]; // SUBJECT

                // SIGNIFICANT INCIDENT REPORT
                if ((email.Email_subject).Contains("SIR"))
                {
                    email.Message =
                        pharse.Split(',')[2] + ", " +
                        pharse.Split(',')[3] + ", " +
                        pharse.Split(',')[4];

                    Boolean found = false;

                    // check nature of incident
                    foreach (string incident in incidentList)
                    {
                        string letter = (((email.Message).Split(',')[1]).ToLower());
                        letter= Regex.Replace(letter, @"\s+", "");

                        if (letter.Equals(incident))
                        {
                            SIR.Add((email.Message).Split(',')[0], (email.Message).Split(',')[1]);
                           // sirList.Items.Add((email.Message).Split(',')[0] + ", " + (email.Message).Split(',')[1]);
                            found = true;
                        }
                    }

                    if (!found)
                        MessageBox.Show("Nature of incident not found.");

                }
                // STANDARD EMAIL MESSAGE
                else
                {
                    //string sentence = "maria@gmail.com 12345678901234567890 hello this is the text";

                    // TEXT - max of 1028 chars
                    email.Message = (email.Message).Split(',')[2];
                }

                // URLs 
                //email.Message = url_search(email.Message);

                // SHOW INFO
                user_txtbox.Text = "Sender: " + email.Email_address;
                subject_txtbox.Text = "Subject: " + email.Email_subject;
                body_txtbox.Text = "Text: " + email.Message;

                // SAVE IN JSON FILE
                output.Add(email);
                MainWindow save = new MainWindow();
                save.outputFile(output);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }


        private void printSMS(AllMessages message)
        {

            SendSMS sms = new SendSMS();

            // SENDER - int
            try
            {
                sms.ID = message.ID;
                sms.Message = message.Message;
                sms.Sms_phone = message.Sms_Phone.Split(' ')[1]; // second word (number)


                int i = sms.Message.IndexOf(" ") + 1;
                string str = sms.Message.Substring(i); // delete the first word
                int x = str.IndexOf(" ") + 1;
                string str2 = str.Substring(x); // delete the second word
                sms.Message = str2;

                // Checking for any abbreviation calling the class made, which is checking for any.
              //  shortAbs abvs = new shortAbs();
               // string addabv = abvs.main(sms.Message);
               // sms.Message = addabv;

                // OUTPUT TO FILE
                output.Add(sms);
                MainWindow save = new MainWindow();
                save.outputFile(output);

                // SHOW RESULTS
                header_txtblock.Text = "ID: " + message.ID;
                body_txtbox.Text = "Message: " + message.Message;
                user_txtbox.Text = "Text: " + message.Sms_Phone;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

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



                                // string filename = @"../../../sms.json";

                                //tempObj = File.ReadAllText(path: filename);
                                // sms data = JsonConvert.DeserializeObject<sms>(jsontext);
                                AllMessages allmsg = new AllMessages();

                                if (Regex.IsMatch(id, "S"))
                                {

                                    SendSMS smss = JsonConvert.DeserializeObject<SendSMS>(tempObj);

                                    allmsg.ID = smss.ID;
                                    allmsg.Message = smss.Message;

                                    //   allmsg.sms_phone_number = smss.sms_phone_number;

                                    data_listbox.Items.Add(smss.ID);
                                    input.Add(smss);
                                    //ssg.sms_message.Insert(message_txtbox);


                                    // displaying information.
                                    header_txtblock.Text = smss.ID;
                                    body_txtbox.Text = smss.Message;
                                    user_txtbox.Text = smss.Sms_phone;



                                }
                                else if (Regex.IsMatch(id, "T"))
                                {

                                
                                    SendTWEET twt = JsonConvert.DeserializeObject<SendTWEET>(tempObj);

                                  

                                    allmsg.ID = twt.ID;
                                    allmsg.Message = twt.Message;
                                    //  allmsg.twitter_username = twt.twitter_username;


                                    data_listbox.Items.Add(twt.ID);
                                    input.Add(twt);

                                    // displaying information.
                                    header_txtblock.Text = twt.ID;
                                    body_txtbox.Text = twt.Message;
                                    user_txtbox.Text = twt.Twitter_User;
                                }

                                else if (Regex.IsMatch(id, "E"))
                                {

                                    SendEMAIL mails = JsonConvert.DeserializeObject<SendEMAIL>(tempObj);


                                  


                                    allmsg.ID = mails.ID;
                                    allmsg.Message = mails.Message;
                                    // mail.email_subject = mails.email_subject;
                                    //  mail.email_message = mails.email_message;

                                    data_listbox.Items.Add(mails.ID);
                                    input.Add(mails);

                                    // displaying information.
                                         header_txtblock.Text = mails.ID;
                                       user_txtbox.Text = mails.Email_address;
                                      subject_txtbox.Text = mails.Email_subject;
                                      body_txtbox.Text = mails.Message;

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

        private void TxtItem(object sender, MouseButtonEventArgs e)
        {
            try
            {
                // read id
                string line = Convert.ToString(txt_list.SelectedItem);

                // search for object with that id in data in
                foreach (AllMessages message in input)
                {
                    if (line.Equals(message.ID))
                    {
                        // depending on type, select process
                        if (message.ID[0].Equals('S')) { 
                            printSMS(message);
                            MessageBox.Show(message.ID + message.Message + message.Sms_Phone);
                            break; }
                        else if (message.ID[0].Equals('E')) { printEMAIL(message); break; }
                        else if (message.ID[0].Equals('T')) { printTWEET(message); break; }
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

