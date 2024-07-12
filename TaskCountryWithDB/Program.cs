using System.Data.SqlClient;
using System.Data.SqlTypes;
using TaskCountryWithDB.Constants;
using TaskCountryWithDB.Constants.Messages;
using TaskCountryWithDB.Services;

namespace TaskCountryWithDB
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool exit = true;
            while (exit)
            {
                ShowMenu();
                BasicMessages.InputMessage("choice");
                string input = Console.ReadLine();
                bool IsSucceded = int.TryParse(input, out int choice);
                if (IsSucceded)
                {
                    switch((Choice)choice)
                    {
                        case Choice.AddCountry:
                            CountryService.AddCountry();
                            break;
                        case Choice.ShowCountries:
                            CountryService.ShowCountries();
                            break;
                        case Choice.ChangeCountryDetails:
                            CountryService.UpdateCountryDetailsa();
                            break;
                        case Choice.DeleteCountry:
                            CountryService.DeleteCountry();
                            break;
                        case Choice.GetDetailsOfCountry:
                            CountryService.GetDetailsOfCountry();
                            break;
                        case Choice.AddCity:
                            CityService.AddCity();
                            break;
                        case Choice.ShowAllCities:
                            CityService.ShowAllCities();
                            break;
                        case Choice.ShowAllCitiesOfCountry:
                            CityService.ShowAllCitiesOfCountry();
                            break;
                        case Choice.ChangeCityDetail:
                            CityService.UpdateDetailsOfCity();
                            break;
                        case Choice.DeleteCity:
                            CityService.DeleteCity();
                            break;
                        case Choice.ShowDetailsOfCity:
                            CityService.ShowDetailsOfCity();
                            break;
                        case Choice.Exit:
                            exit = false;
                            break;
                        default:
                            ErrorMessages.InputError("Operation");
                            break;
                    }
                }
                else
                {
                    ErrorMessages.InputError(input);
                }
            }
        }
        public static void ShowMenu()
        {
            Console.WriteLine("---Menu---");
            Console.WriteLine("0        Exit");
            Console.WriteLine("1        Add Country");
            Console.WriteLine("2        Show Countries");
            Console.WriteLine("3        Change Country Details");
            Console.WriteLine("4        Delete Country");
            Console.WriteLine("5        Get Details Of Country");
            Console.WriteLine("6        Add City");
            Console.WriteLine("7        Show All Cities");
            Console.WriteLine("8        Show All cities of Country");
            Console.WriteLine("9        Change City Detail");
            Console.WriteLine("10       Delete City");
            Console.WriteLine("11       Show Details Of city");
        }
    }
}
