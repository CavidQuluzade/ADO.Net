using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskCountryWithDB.Constants.Messages
{
    public static class BasicMessages
    {
        public static void InputMessage(string message) => Console.WriteLine("Enter " + message);
        public static void SuccessMessage(string name, string type) => Console.WriteLine(name + " " + type);
        public static void OutputMessageCity(string message1, string result1, string message2, decimal result2) => Console.WriteLine(message1 + " - " + result1 + " | " + message2 + " - " + result2);
        public static void OutputMessageCountry(string message1, string result1, string message2, decimal result2) => Console.WriteLine(message1 + " - " + result1 + " | " + message2 + " - " + result2);
        public static void OutputMessageCityDetails(string message1, int result1, string message2, string result2, string message3, decimal result3, string message4, string result4) => Console.WriteLine(message1 + " - " + result1 + " | " + message2 + " - " + result2 + " | " + message3 + " - " + result3 + " | " + message4 + " - " + result4);
        public static void OutputMessageCountryDetails(string message1, int result1, string message2, string result2, string message3, decimal result3) => Console.WriteLine(message1 + " - " + result1 + " | " + message2 + " - " + result2 + " | " + message3 + " - " + result3);
        public static void ChangeMessage(string message1) => Console.WriteLine("Do you want change " + message1 + " (y/n)");
        public static void WhatChangeCountryMessage() => Console.WriteLine("What do you want to change (name/area/both)");
        public static void WhatChangeCityMessage() => Console.WriteLine("What do you want to change (name/area/country/all)");
    }
}
