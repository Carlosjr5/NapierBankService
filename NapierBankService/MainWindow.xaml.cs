using Microsoft.Win32;
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




        List<string> quarantineList = new List<string>();
        Dictionary<string, string> SIR = new Dictionary<string, string>();
        List<string> mentions = new List<string>();
        Dictionary<string, int> hashtags = new Dictionary<string, int>();
        List<AllMessages> output = new List<AllMessages>();
        List<AllMessages> input = new List<AllMessages>();
        //AllMessages message = new AllMessages();



       
        List<SendSMS> sms = new List<SendSMS>();

        List<SendEMAIL> mail = new List<SendEMAIL>();

        List<SendTWEET> twt = new List<SendTWEET>();

        ChoseAbvs newAbvs = new ChoseAbvs();
        AllMessages allmsg = new AllMessages();



        List<AllMessages> msgs = new List<AllMessages>();





        public MainWindow()
        {
            List<AllMessages> msgs = new List<AllMessages>();

            InitializeComponent();

              msgs = outputFile(msgs);


           

            
            if (msgs != null)
            {

                foreach (var Data in msgs)
                {
                   // msgs = Serializing(msgs);
                    data_listbox.Items.Add(Data.ID);
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


        static List<AllMessages> Serializing(List<AllMessages> msgs)
        {



            // Writing the inputs to the json file.
            string jsonFile3 = @"../../../Json_data.json";
            string txtFile3 = @"../../../Json_data.txt";

            string serialize = JsonConvert.SerializeObject(msgs, Formatting.Indented);
            string serializetxt = JsonConvert.SerializeObject(msgs.ToArray());

            File.WriteAllText(jsonFile3, serialize);
            File.WriteAllText(txtFile3, serializetxt + Environment.NewLine);
            MessageBox.Show("Message Sent!");

            return msgs;


        }
        

    
        
      




        private void send_btn_Click(object sender, RoutedEventArgs e)
        {

            string user = user_txtbox.Text;
            string message = body_txtbox.Text;
            string mail_subject = subject_txtbox.Text;
            string numbers = new String(user.ToCharArray().Where(c => Char.IsDigit(c)).ToArray()); //checks if the input is only digits.         

            //   string type_message = ngenerator.SMS_type(sms_message, "S");
            //  string type_phone = ngenerator.SMS_type_PHONE(sms_phone_number, "S");

           

            generate newsms = new generate();



          
               


                if (message == "" || user == "")
                {
                    MessageBox.Show("Empty boxes, try again!.");
                }


                if (message.Length > 140 || message.Length == 0)
                {
                    MessageBox.Show("The message cant be more than 140 characters! :( ");
                }



                if (user.StartsWith("@"))
                {



                    msgs.Add(new AllMessages()
                    {

                        ID = newsms.Generator_Id("T"),

                        Message = newsms.Twitter_tweet(message, "T"),

                        Twitter_User = newsms.Twitter_username(user, "T")


                    });
                               


                }else if (!user.StartsWith("@")){

                      MessageBox.Show("Twitter user must start with '@'  and Phone numbers with '+' :), If you are sending and email DONT WORRY!, GREETINGS :)");

                }



                if (user.StartsWith("+") && user.Length <= 13 && user.Length >= 5)
                {


                    if ((user.Length >= 13) && (user.Length <= 6))
                    {
                        MessageBox.Show("Incorrect phone number format, have to be with the country code followed by the ccountry, i.e(+34745738295).");

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


                    }
                }



                if (user.Contains("@") && mail_subject != "")
                {

                 msgs = Serializing(msgs);
                    msgs.Add(new AllMessages()
                    {
                        ID = newsms.Generator_Id("E"),

                        Message = newsms.Email_message(message, "E"),

                        Email_subject = newsms.Email_subject(mail_subject, "E"),

                        Email_address = newsms.Email_address(user, "E")

                    });
                                 



                }
                else if (message.Length > 1029)
                {

                    MessageBox.Show("Empty box, try again!.");
                }


            if (msgs != null)
            {
               // data_listbox.Items.Clear();
                foreach (var Data in msgs)
                {
                   
                    data_listbox.Items.Add(Data.ID);
                }
            }

            
            // Writing the inputs to the json file.
            string jsonFile3 = @"../../../Json_data.json";
            string txtFile3 = @"../../../Json_data.txt";

            string serialize = JsonConvert.SerializeObject(msgs.ToArray(), Formatting.Indented);
            string serializetxt = JsonConvert.SerializeObject(msgs.ToArray());

            File.WriteAllText(jsonFile3, serialize);
            File.WriteAllText(txtFile3, serializetxt + Environment.NewLine);
            MessageBox.Show("Message Sent!");


            




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


                body_txtbox.Text = message.Message;
                header_txtblock.Text = message.ID;
                user_txtbox.Text = message.Twitter_User;
                subject_txtbox.Text = " ";

                // SEARCH THROUGH WORDS 
                string sentence = tweet.Message;
                foreach (string word in (sentence).Split(' '))
                {
                    // If there is a mention
                    if (message.Message.StartsWith("@"))
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

                // Checking for any abbreviation calling the class made, which is checking for any.
                // shortAbs abvs = new shortAbs();
                // string newM = abvs.main(tweet.Message);
                // tweet.Message = newM;



                //Output file
            //    output.Add(tweet);
              //  MainWindow save = new MainWindow();
               // save.outputFile(output);

                // SHOW RESULTS
                header_txtblock.Text =  tweet.ID;
                body_txtbox.Text =  tweet.Message;
                user_txtbox.Text =  tweet.Twitter_User;

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
                        letter = Regex.Replace(letter, @"\s+", "");

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
             //   output.Add(email);
               // MainWindow save = new MainWindow();
               // save.outputFile(output);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

        }


        private void printSMS(AllMessages message)
        {

            List<AllMessages> msgs = new List<AllMessages>();

            AllMessages sms = new AllMessages();

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
              //  shortAbs abvs = new shortAbs();
               // string addabv = abvs.main(sms.Message);
               // sms.Message = addabv;

                // OUTPUT TO FILE
                msgs.Add(sms);
                MainWindow save = new MainWindow();
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
            List<AllMessages> msgs = new List<AllMessages>();


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
                            
                            // ABBREVIATIONS
                            string newM = newAbvs.main(message.Message);
                            message.Message = newM;


                            break;

                        }
                        else if (message.ID[0].Equals('E'))
                        {
                            printEMAIL(message);
                            
                            body_txtbox.Text = message.Message;
                            header_txtblock.Text = message.ID;
                            user_txtbox.Text = message.Email_address;
                            subject_txtbox.Text = message.Email_subject;
                            
                            break;
                        }

                        else if (message.ID[0].Equals('T'))
                        {
                            printTWEET(message);
                            body_txtbox.Text = message.Message;
                            header_txtblock.Text = message.ID;
                            user_txtbox.Text = message.Twitter_User;
                            subject_txtbox.Text = "";

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
            AllMessages allmsg = new AllMessages();
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
                        if (message.ID[0].Equals('S'))
                        {
                            printSMS(message);
                            // ABBREVIATIONS
                            string newM = newAbvs.main(allmsg.Message);
                            allmsg.Message = newM;
                            body_txtbox.Text = message.Message;
                            header_txtblock.Text = message.ID;
                            user_txtbox.Text = message.Sms_Phone;


                            break;
                        }
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
 

