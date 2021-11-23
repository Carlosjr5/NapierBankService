using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NapierBankService.DataLayer
{
    class ChoseAbvs
    {

        public string main(string sentence)
        {
            List<string> abb = new List<string>();
            List<string> def = new List<string>();

            // ABREVIATIONS - read file
            using (var reader = new StreamReader(@"../../../textwords.csv"))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line.Split(',');
                    abb.Add(values[0]);
                    def.Add(values[1]);
                }
            }

            try
            {
                foreach (string word in (sentence).Split(' '))
                {
                    foreach (string abr in abb)
                    {
                        if (word.Equals(abr))
                        {
                            // Find the definition
                            int index = abb.IndexOf(abr);
                            string all = def[index];

                            // Replace word for actual words
                            string words = word + " <" + all + ">";

                            int index2 = sentence.IndexOf(word);

                            char wordAfter;
                            string wordAfter2;

                            try // if it's not the last word
                            {
                                wordAfter = sentence[index2 + 1 + word.Length];

                                wordAfter2 = wordAfter + "";

                                if (wordAfter2.Contains("<"))
                                {
                                    break;
                                }
                                else
                                {
                                    string newM = (sentence).Replace(word, words);
                                    sentence = newM;
                                }
                            }
                            catch // if it is the last word
                            {
                                string newM = (sentence).Replace(word, words);
                                sentence = newM;
                            }
                        }
                    }
                }
                return (sentence);
            }
            catch
            {
                return sentence;
            }

        }
    }
}
