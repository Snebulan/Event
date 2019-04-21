using System;
using System.Collections.Generic;
using System.Text;

namespace Event
{
    class InputManager
    {

        /// <summary>
        /// Handles inputs of strings
        /// </summary>
        /// <param name="prompt">Input prompt</param>
        /// <returns>input as string</returns>
        public static string InputString(string prompt)
        {
            string input = "";
            do
            {
                Console.WriteLine(prompt);
                Console.Write("Input: ");

                input = Console.ReadLine();
            } while (input.Length == 0 || input == " ");
            return input;
        }

        /// <summary>
        /// Handles inputs of bool
        /// </summary>
        /// <param name="prompt">Input prompt</param>
        /// <returns>input as a bool</returns>
        public static bool InputBool(string prompt)
        {
            bool validBool = true;
            bool responseBool = false;
            do
            {
                string input = InputString(prompt + " Enter only \"y\" or \"n\")");
                if (input == "y")
                {
                    responseBool = true;
                }
                else if (input == "n")
                {
                    responseBool = false;
                }
                else
                {
                    validBool = false;
                }
            } while (!validBool);
            return responseBool;

        }

        /// <summary>
        /// Handles inputs of int
        /// </summary>
        /// <param name="prompt">Input prompt</param>
        /// <returns>input as an int</returns>
        public static int InputInt(string prompt)
        {
            bool validInt = false;
            int inputInt = 0;
            do
            {
                string input = InputString(prompt);
                validInt = int.TryParse(input, out inputInt);
            } while (!validInt);
            return inputInt;
        }

        /// <summary>
        /// Handles inputs of datetime
        /// </summary>
        /// <param name="prompt">Input prompt</param>
        /// <returns>input as a datetime</returns>
        public static DateTime InputDate(string prompt)
        {
            bool validDateTime = false;
            DateTime dateTime = new DateTime();
            do
            {
                string input = InputString(prompt);
                try
                {
                    dateTime = DateTime.Parse(input);
                    validDateTime = true;
                }
                catch (Exception e)
                {

                    validDateTime = false;
                }

            } while (!validDateTime);

            return dateTime;
        }

    }
}
