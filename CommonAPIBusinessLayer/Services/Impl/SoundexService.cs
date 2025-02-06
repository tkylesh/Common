using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommonAPIBusinessLayer.Services.Interface;

namespace CommonAPIBusinessLayer.Services.Impl
{
    public class SoundexService : ISoundexService
    {
        public bool NameComparison(string name1, string name2)
        {
            if (name1 == null || name2 == null)
                return false;

            var soundexName1 = NameCheck(name1, name1.Length);
            var soundexName2 = NameCheck(name2, name2.Length);

            if (soundexName1 == soundexName2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public string NameCheck(string name, int length)
        {
            // Value to return
            string value = "";
            // Size of the name to process
            int size = name.Length;
            // Make sure the name is at least two characters in length
            if (size > 1)
            {
                // Convert the name to all uppercase
                name = name.ToUpper();
                // Convert the name to character array for faster processing
                char[] chars = name.ToCharArray();
                // Buffer to build up with character codes
                StringBuilder buffer = new StringBuilder();
                buffer.Length = 0;
                // The current and previous character codes
                int prevCode = 0;
                int currCode = 0;
                // Append the first character to the buffer
                buffer.Append(chars[0]);
                // Loop through all the characters and convert them to the proper character code
                for (int i = 1; i < size; i++)
                {
                    switch (chars[i])
                    {
                        case 'A':
                            currCode = 0;
                            break;
                        case 'E':
                            currCode = 0;
                            break;
                        case 'I':
                            currCode = 0;
                            break;
                        case 'O':
                            currCode = 0;
                            break;
                        case 'U':
                            currCode = 0;
                            break;
                        case 'H':
                            currCode = 0;
                            break;
                        case 'W':
                            currCode = 0;
                            break;
                        case 'Y':
                            currCode = 0;
                            break;
                        case 'B':
                            currCode = 1;
                            break;
                        case 'F':
                            currCode = 1;
                            break;
                        case 'P':
                            currCode = 1;
                            break;
                        case 'V':
                            currCode = 1;
                            break;
                        case 'C':
                            currCode = 2;
                            break;
                        case 'G':
                            currCode = 2;
                            break;
                        case 'J':
                            currCode = 2;
                            break;
                        case 'K':
                            currCode = 2;
                            break;
                        case 'Q':
                            currCode = 2;
                            break;
                        case 'S':
                            currCode = 2;
                            break;
                        case 'X':
                            currCode = 2;
                            break;
                        case 'Z':
                            currCode = 2;
                            break;
                        case 'D':
                            currCode = 3;
                            break;
                        case 'T':
                            currCode = 3;
                            break;
                        case 'L':
                            currCode = 4;
                            break;
                        case 'M':
                            currCode = 5;
                            break;
                        case 'N':
                            currCode = 5;
                            break;
                        case 'R':
                            currCode = 6;
                            break;
                    }
                    // Check to see if the current code is the same as the last one
                    if (currCode != prevCode)
                    {
                        // Check to see if the current code is 0 (a vowel); do not process vowels
                        if (currCode != 0)
                            buffer.Append(currCode);
                    }
                    // Set the new previous character code
                    prevCode = currCode;
                    // If the buffer size meets the length limit, then exit the loop
                    if (buffer.Length == length)
                        break;
                }
                // Pad the buffer, if required
                size = buffer.Length;
                if (size < length)
                    buffer.Append('0', (length - size));
                // Set the value to return
                value = buffer.ToString();
            }
            // Return the value
            return value;
        }
    }
}
