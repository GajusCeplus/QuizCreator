using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizCreator
{
    class Caesar
    {
        private static char Cipher(char ch, int key)
        {
            
            string polskieznaki = "ąćęłńóśźżĄĆĘŁŃÓŚŹŻ";
            if (!char.IsLetter(ch) || polskieznaki.Contains(ch))
                return ch;

            char offset = char.IsUpper(ch) ? 'A' : 'a';
            return (char)((((ch + key) - offset) % 26) + offset);
          
        }

        public static string Encipher(string input, int key)
        {
            string output = string.Empty;

            foreach (char ch in input)
                output += Cipher(ch, key);

            return output;
        }

        public static string Decipher(string input, int key)
        {
            return Encipher(input, 26 - key);
        }
    }
}
