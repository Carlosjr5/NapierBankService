using System.Collections.Generic;
using System.IO;

namespace NapierBankService.DataLayer
{
    /*
  * Author: Carlos Jimenez Rodriguez, 40452913
  * 
  * Description of class: On this class, the main purpuse is to read from a csv file, read 2 columns,
  *                                     the string "abb" which are the abbreviations, covers one column 
  *                                     and "def", the second column , meaning definition.
  *                                     so whenever in the message there is an input like abb, reads the definition and adds it next to it with <> signs.
  * 
  * Date last modified: 26/11/2021
  */

    class ChoseAbvs
    {

        public string main(string sentence)
        {
            //Create list for each columns on the file.
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
                //Reading the sentence from the message splitting, looking for a word on the list abb and its definition.
                foreach (string word in (sentence).Split(' '))
                {
                    foreach (string abr in abb)
                    {
                        if (word.Equals(abr))
                        {
                            // Find the definition
                            int index = abb.IndexOf(abr);
                            string all = def[index];

                            // Replace word for actual words with the signs <>.
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
