using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskCountryWithDB.Constants.Messages
{
    public static class ErrorMessages
    {
        public static void InputError(string message) => Console.WriteLine(message + " is invalid. Use correct symbols");
        public static void Error() => Console.WriteLine("Error happened.");
        public static void Exists(string message) => Console.WriteLine(message + "exists. Use another name");
        public static void NotExists(string message) => Console.WriteLine(message + "doesn't exists. Use correct name");
        public static void AreaError() => Console.WriteLine("City area is bigger than country area");
    }
}
