using System;
using System.Collections.Generic;
using System.Text;

namespace Event
{
    class InputManager
    {

        private static string InputString(string prompt)
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

        private static bool InputBool(string prompt)
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

        private static int InputInt(string prompt)
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

        private static DateTime InputDate(string prompt)
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
