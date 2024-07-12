using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskCountryWithDB.Constants.Messages;
using TaskCountryWithDB.Database;

namespace TaskCountryWithDB.Services
{
    public static class CityService
    {
        public static void AddCity()
        {
            using (SqlConnection SqlConnection = new SqlConnection(SqlStringConnection.DEFAULT))
            {
                SqlConnection.Open();
            CityName: BasicMessages.InputMessage("City name");
                string cityname = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(cityname))
                {
                CityArea: BasicMessages.InputMessage("City Area size");
                    string citysize = Console.ReadLine();
                    decimal CityArea;
                    bool IsSucceded = decimal.TryParse(citysize, out CityArea);
                    if (IsSucceded)
                    {
                        try
                        {
                            CountryService.ShowCountries();
                            BasicMessages.InputMessage("name of country");
                            string countryname = Console.ReadLine();
                            var SqlCommandCountry = new SqlCommand("SELECT * FROM Countries WHERE Name = @countryname", SqlConnection);
                            SqlCommandCountry.Parameters.AddWithValue("@countryname", countryname);
                            int CountryId = Convert.ToInt32(SqlCommandCountry.ExecuteScalar());
                            if (CountryId <= 0)
                            {
                                ErrorMessages.NotExists("Country " + countryname);
                            }
                            else
                            {
                                SqlCommandCountry = new SqlCommand("SELECT Area FROM Countries WHERE Name = @countryname", SqlConnection);
                                SqlCommandCountry.Parameters.AddWithValue("@countryname", countryname);
                                decimal CountryArea = Convert.ToDecimal(SqlCommandCountry.ExecuteScalar());
                                var SqlCommandCity = new SqlCommand("SELECT Count(Cities.Id) FROM Cities  WHERE CountryId = Countries.Id WHERE CountryId = @countryid", SqlConnection);
                                SqlCommandCity.Parameters.AddWithValue("@countryid", CountryId);
                                int CitiesCount = Convert.ToInt32(SqlCommandCity.ExecuteScalar());
                                if (CitiesCount == 0 && CityArea <= CountryArea)
                                {
                                    SqlCommandCity = new SqlCommand("INSERT INTO Cities VALUES(@cityname, @cityarea, @countryId)", SqlConnection);
                                    SqlCommandCity.Parameters.AddWithValue("@cityname", cityname);
                                    SqlCommandCity.Parameters.AddWithValue("@cityarea", CityArea);
                                    SqlCommandCity.Parameters.AddWithValue("@countryId", CountryId);
                                    try
                                    {
                                        int ChangedRows = SqlCommandCity.ExecuteNonQuery();
                                        if (ChangedRows > 0)
                                        {
                                            BasicMessages.SuccessMessage(cityname, "added");
                                        }
                                        else
                                        {
                                            ErrorMessages.Error();
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        ErrorMessages.Error();
                                    }
                                }
                                else if (CitiesCount > 0)
                                {
                                    SqlCommandCity = new SqlCommand("SELECT SUM(Cities.Area) FROM Cities JOIN Countries ON @CountryId = Countries.Id", SqlConnection);
                                    SqlCommandCity.Parameters.AddWithValue("@CountryId", CountryId);
                                    decimal CitiesArea = Convert.ToDecimal(SqlCommandCity.ExecuteScalar());
                                    if (CountryArea >= CitiesArea)
                                    {
                                        SqlCommandCity = new SqlCommand("INSERT INTO Cities VALUES(@cityname, @cityarea, @countryId)", SqlConnection);
                                        SqlCommandCity.Parameters.AddWithValue("@cityname", cityname);
                                        SqlCommandCity.Parameters.AddWithValue("@cityarea", CityArea);
                                        SqlCommandCity.Parameters.AddWithValue("@countryId", CountryId);
                                        try
                                        {
                                            int ChangedRows = SqlCommandCity.ExecuteNonQuery();
                                            if (ChangedRows > 0)
                                            {
                                                BasicMessages.SuccessMessage(cityname, "added");
                                            }
                                            else
                                            {
                                                ErrorMessages.Error();
                                            }
                                        }
                                        catch (Exception)
                                        {
                                            ErrorMessages.Error();
                                        }
                                    }
                                    else
                                    {
                                        ErrorMessages.AreaError();
                                        goto CityArea;
                                    }
                                }
                            }
                        }
                        catch (Exception)
                        {
                            ErrorMessages.Error();
                        }
                    }
                    else
                    {
                        ErrorMessages.InputError(citysize);
                        goto CityArea;
                    }
                }
                else
                {
                    ErrorMessages.InputError(cityname);
                    goto CityName;
                }
            }
        }
        public static void ShowAllCities()
        {
            using (SqlConnection SqlConnection = new SqlConnection(SqlStringConnection.DEFAULT))
            {
                SqlConnection.Open();
                var Command = new SqlCommand("SELECT Cities.Name, Cities.Area FROM Cities", SqlConnection);
                using (var get = Command.ExecuteReader())
                {
                    while (get.Read())
                    {
                        string CityName = Convert.ToString(get["Name"]);
                        decimal CityArea = Convert.ToDecimal(get["Area"]);
                        BasicMessages.OutputMessageCity("City", CityName, "Area", CityArea);
                    }
                }
            }
        }
        public static void ShowAllCitiesOfCountry()
        {
            using (SqlConnection SqlConnection = new SqlConnection(SqlStringConnection.DEFAULT))
            {
                SqlConnection.Open();
                CountryService.ShowCountries();
                BasicMessages.InputMessage("country name");
                string countryname = Console.ReadLine();
                var SqlCommand = new SqlCommand("SELECT Id FROM Countries WHERE Name = @countryname", SqlConnection);
                SqlCommand.Parameters.AddWithValue("@countryname", countryname);
                int CountryId = Convert.ToInt32(SqlCommand.ExecuteScalar());
                if (CountryId <= 0)
                {
                    ErrorMessages.NotExists("Country " + countryname);
                }
                else
                {
                    SqlCommand = new SqlCommand("SELECT Cities.Name, Cities.Area FROM Cities WHERE Cities.CountryId = @CountryId", SqlConnection);
                    SqlCommand.Parameters.AddWithValue("@CountryId", CountryId);
                    using (var get = SqlCommand.ExecuteReader())
                    {
                        while (get.Read())
                        {
                            string CityName = Convert.ToString(get["Name"]);
                            decimal CityArea = Convert.ToDecimal(get["Area"]);
                            BasicMessages.OutputMessageCity("City", CityName, "Area", CityArea);
                        }
                    }
                }
            }
        }
        public static void DeleteCity()
        {
        CountryName:
            CityService.ShowAllCities();
            BasicMessages.InputMessage("city name");
            string cityname = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(cityname))
            {
                using (SqlConnection SqlConnection = new SqlConnection(SqlStringConnection.DEFAULT))
                {
                    SqlConnection.Open();
                    var SqlCommand = new SqlCommand("SELECT * FROM Cities WHERE Name = @cityname", SqlConnection);
                    SqlCommand.Parameters.AddWithValue("@cityname", cityname);
                    try
                    {
                        int CityId = Convert.ToInt32(SqlCommand.ExecuteScalar());

                        if (CityId > 0)
                        {
                            SqlCommand Delete = new SqlCommand("DELETE FROM Cities WHERE Id = @CityId", SqlConnection);
                            Delete.Parameters.AddWithValue("@CityId", CityId);
                            int affectedrows = Delete.ExecuteNonQuery();
                            if (affectedrows > 0)
                            {
                                BasicMessages.SuccessMessage(cityname, "deleted");
                                return;
                            }
                        }
                        else
                        {
                            ErrorMessages.NotExists(cityname);
                        }
                    }
                    catch (Exception)
                    {
                        ErrorMessages.Error();
                    }
                }

            }
            else
            {
                ErrorMessages.InputError("cityname");
                goto CountryName;
            }
        }
        public static void ShowDetailsOfCity()
        {
            ShowAllCities();
            BasicMessages.InputMessage("City name");
            string cityname = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(cityname))
            {
                using (SqlConnection sqlConnection = new SqlConnection(SqlStringConnection.DEFAULT))
                {
                    sqlConnection.Open();
                    var SqlCommand1 = new SqlCommand("SELECT CountryId FROM Cities WHERE Name = @CityName", sqlConnection);
                    SqlCommand1.Parameters.AddWithValue("@CityName", cityname);
                    int countryid = Convert.ToInt32(SqlCommand1.ExecuteScalar());
                    if (countryid > 0)
                    {
                        var SqlCommand2 = new SqlCommand("SELECT * FROM Countries JOIN Cities ON @CountryId = Countries.Id", sqlConnection);
                        SqlCommand2.Parameters.AddWithValue("@CountryId", countryid);
                        string countryname = default;
                        using (var get = SqlCommand2.ExecuteReader())
                        {
                            while (get.Read())
                            {
                                countryname = Convert.ToString(get["Name"]);
                            }
                        }
                        var SqlCommand3 = new SqlCommand("SELECT * FROM Cities WHERE Name = @CityName", sqlConnection);
                        SqlCommand3.Parameters.AddWithValue("@CityName", cityname);

                        using (var get1 = SqlCommand3.ExecuteReader())
                        {
                            while (get1.Read())
                            {
                                int id = Convert.ToInt32(get1["Id"]);
                                string CityName = Convert.ToString(get1["Name"]);
                                decimal CityArea = Convert.ToDecimal(get1["Area"]);

                                BasicMessages.OutputMessageCityDetails("Id", id, "City", CityName, "Area", CityArea, "Country name", countryname);
                            }
                        }

                    }
                    else
                    {
                        ErrorMessages.NotExists(cityname);
                    }
                }
            }
            else
            {
                ErrorMessages.InputError("cityname");
            }
        }
        public static void UpdateDetailsOfCity()
        {
            CityName: ShowAllCities();
            BasicMessages.InputMessage("city name");
            string cityname = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(cityname))
            {
                using (SqlConnection sqlConnection = new SqlConnection(SqlStringConnection.DEFAULT))
                {
                    try
                    {
                        sqlConnection.Open();
                        var SqlCommand = new SqlCommand("SELECT * FROM Cities WHERE Name = @cityname", sqlConnection);
                        SqlCommand.Parameters.AddWithValue("@cityname", cityname);
                        int cityid = Convert.ToInt32(SqlCommand.ExecuteScalar());
                        SqlCommand = new SqlCommand("SELECT CountryId FROM Cities WHERE @CityId = Id", sqlConnection);
                        SqlCommand.Parameters.AddWithValue("@CityId", cityid);
                        int countryid = Convert.ToInt32(SqlCommand.ExecuteScalar());
                        if (cityid > 0)
                        {
                        Answer1: BasicMessages.WhatChangeCityMessage();
                            string answer = Console.ReadLine();
                            if (!string.IsNullOrWhiteSpace(answer))
                            {
                                if (answer == "name")
                                {
                                Answer3:
                                    BasicMessages.InputMessage("new city name");
                                    string newcityname = Console.ReadLine();
                                    if (!string.IsNullOrWhiteSpace(newcityname))
                                    {
                                        SqlCommand = new SqlCommand("SELECT * FROM Cities WHERE Name = @cityname", sqlConnection);
                                        SqlCommand.Parameters.AddWithValue("@cityname", newcityname);
                                        int newcityid = Convert.ToInt32(SqlCommand.ExecuteScalar());
                                        if (newcityid > 0)
                                        {
                                            ErrorMessages.Exists(newcityname);
                                        Answer2: BasicMessages.ChangeMessage("city name");
                                            answer = Console.ReadLine();
                                            if (!string.IsNullOrWhiteSpace(answer))
                                            {
                                                if (answer == "y")
                                                {
                                                    goto Answer3;
                                                }
                                                else if (answer == "n")
                                                {
                                                    goto Answer1;
                                                }
                                                else
                                                {
                                                    goto Answer2;
                                                }
                                            }
                                            else
                                            {
                                                ErrorMessages.InputError("answer");
                                                goto Answer2;
                                            }
                                        }
                                        else
                                        {
                                            SqlCommand = new SqlCommand("UPDATE Cities SET Name = @cityname WHERE Id = @CityId", sqlConnection);
                                            SqlCommand.Parameters.AddWithValue("@CityId", cityid);
                                            SqlCommand.Parameters.AddWithValue("@cityname", newcityname);
                                            int changedrows = SqlCommand.ExecuteNonQuery();
                                            if (changedrows > 0)
                                            {
                                                BasicMessages.SuccessMessage(newcityname, "updated");
                                            }
                                            else
                                            {
                                                ErrorMessages.Error();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        ErrorMessages.InputError("city name");
                                        goto Answer3;
                                    }
                                }
                                else if (answer == "area")
                                {
                                Area: BasicMessages.InputMessage("new city area");
                                    string input = Console.ReadLine();
                                    bool isSucceded = decimal.TryParse(input, out decimal newarea);
                                    if (isSucceded)
                                    {
                                        var SqlCommandCity = new SqlCommand("SELECT Count(Cities.Id) FROM Cities JOIN Countries ON CountryId = Countries.Id WHERE CountryId = @countryid", sqlConnection);
                                        SqlCommandCity.Parameters.AddWithValue("@countryid", countryid);
                                        int CitiesCount = Convert.ToInt32(SqlCommandCity.ExecuteScalar());
                                        SqlCommandCity = new SqlCommand("SELECT SUM(Cities.Area) FROM Cities JOIN Countries ON CountryId = Countries.Id WHERE CountryId = @CountryId", sqlConnection);
                                        SqlCommandCity.Parameters.AddWithValue("@CountryId", countryid);
                                        decimal CitiesArea = Convert.ToDecimal(SqlCommandCity.ExecuteScalar());
                                        SqlCommandCity = new SqlCommand("SELECT Area FROM Cities WHERE Id = @CityId", sqlConnection);
                                        SqlCommandCity.Parameters.AddWithValue("@CityId", cityid);
                                        decimal CityArea = Convert.ToDecimal(SqlCommandCity.ExecuteScalar());
                                        SqlCommand = new SqlCommand("SELECT Area FROM Countries WHERE Id = @CountryId", sqlConnection);
                                        SqlCommand.Parameters.AddWithValue("@CountryId", countryid);
                                        decimal CountryArea = Convert.ToInt32(SqlCommand.ExecuteScalar());
                                        if (CitiesCount == 0 && newarea <= CountryArea)
                                        {
                                            SqlCommand = new SqlCommand("UPDATE Cities SET Area = @newarea WHERE Id = @CityId", sqlConnection);
                                            SqlCommand.Parameters.AddWithValue("@CityId", cityid);
                                            SqlCommand.Parameters.AddWithValue("@newarea", newarea);
                                            int changedrows = SqlCommand.ExecuteNonQuery();
                                            if (changedrows > 0)
                                            {
                                                BasicMessages.SuccessMessage(cityname, "updated");
                                            }
                                            else
                                            {
                                                ErrorMessages.Error();
                                            }
                                        }
                                        else if (CitiesArea - CityArea + newarea <= CountryArea)
                                        {
                                            SqlCommand = new SqlCommand("UPDATE Cities SET Area = @newarea WHERE Id = @CityId", sqlConnection);
                                            SqlCommand.Parameters.AddWithValue("@CityId", cityid);
                                            SqlCommand.Parameters.AddWithValue("@newarea", newarea);
                                            int changedrows = SqlCommand.ExecuteNonQuery();
                                            if (changedrows > 0)
                                            {
                                                BasicMessages.SuccessMessage(cityname, "updated");
                                            }
                                            else
                                            {
                                                ErrorMessages.Error();
                                            }
                                        }
                                        else
                                        {
                                            ErrorMessages.AreaError();
                                            goto Area;
                                        }
                                    }
                                    else
                                    {
                                        ErrorMessages.InputError("area");
                                        goto Area;
                                    }
                                }
                                else if (answer == "country")
                                {
                                Answer3:
                                    CountryService.ShowCountries();
                                    BasicMessages.InputMessage("select country name if you don;t have country (a - add country)");
                                    string countryname = Console.ReadLine();
                                    if (!string.IsNullOrWhiteSpace(countryname))
                                    {
                                        if (countryname == "a")
                                        {
                                            CountryService.AddCountry();
                                            goto Answer3;
                                        }
                                        else
                                        {
                                            SqlCommand = new SqlCommand("SELECT * FROM Countries WHERE Name = @countryname", sqlConnection);
                                            SqlCommand.Parameters.AddWithValue("@countryname", countryname);
                                            int newcountryid = Convert.ToInt32(SqlCommand.ExecuteScalar());
                                            if (newcountryid > 0)
                                            {
                                                SqlCommand = new SqlCommand("UPDATE Cities SET CountryId = @countryid WHERE Id = @cityid", sqlConnection);
                                                SqlCommand.Parameters.AddWithValue("@countryid", newcountryid);
                                                SqlCommand.Parameters.AddWithValue("@cityid", cityid);
                                                int changedrows = SqlCommand.ExecuteNonQuery();
                                                if (changedrows > 0) { BasicMessages.SuccessMessage(cityname, "updated"); }
                                                else { ErrorMessages.Error(); }
                                            }
                                            else
                                            {
                                                ErrorMessages.NotExists(countryname);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        ErrorMessages.InputError("country name");
                                        goto Answer3;
                                    }
                                }
                                else if (answer == "all")
                                {
                                Answer3:
                                    BasicMessages.InputMessage("new city name");
                                    string newcityname = Console.ReadLine();
                                    if (!string.IsNullOrWhiteSpace(newcityname))
                                    {
                                        SqlCommand = new SqlCommand("SELECT * FROM Cities WHERE Name = @cityname", sqlConnection);
                                        SqlCommand.Parameters.AddWithValue("@cityname", newcityname);
                                        int newcityid = Convert.ToInt32(SqlCommand.ExecuteScalar());
                                        if (newcityid > 0)
                                        {
                                            ErrorMessages.Exists(newcityname);
                                        Answer2: BasicMessages.ChangeMessage("city name");
                                            answer = Console.ReadLine();
                                            if (!string.IsNullOrWhiteSpace(answer))
                                            {
                                                if (answer == "y")
                                                {
                                                    goto Answer3;
                                                }
                                                else if (answer == "n")
                                                {
                                                    goto Answer1;
                                                }
                                                else
                                                {
                                                    goto Answer2;
                                                }
                                            }
                                            else
                                            {
                                                ErrorMessages.InputError("answer");
                                                goto Answer2;
                                            }
                                        }
                                        else
                                        {                                 
                                            Answer4:
                                            CountryService.ShowCountries();
                                            BasicMessages.InputMessage("select country name if you don;t have country (a - add country)");
                                                string countryname = Console.ReadLine();
                                            if (!string.IsNullOrWhiteSpace(countryname))
                                            {
                                                if (countryname == "a")
                                                {
                                                    CountryService.AddCountry();
                                                    goto Answer4;
                                                }
                                                else
                                                {
                                                    SqlCommand = new SqlCommand("SELECT * FROM Countries WHERE Name = @countryname", sqlConnection);
                                                    SqlCommand.Parameters.AddWithValue("@countryname", countryname);
                                                    int newcountryid = Convert.ToInt32(SqlCommand.ExecuteScalar());
                                                    if (newcountryid > 0)
                                                    {
                                                        var SqlCommandCity = new SqlCommand("UPDATE Cities SET CountryId = @countryid WHERE Id = @cityid", sqlConnection);
                                                        SqlCommandCity.Parameters.AddWithValue("@countryid", newcountryid);
                                                        SqlCommandCity.Parameters.AddWithValue("@cityid", cityid);
                                                        int changedrows = SqlCommandCity.ExecuteNonQuery();
                                                        if (changedrows > 0) { BasicMessages.SuccessMessage(cityname, "updated"); }
                                                        else { ErrorMessages.Error(); }


                                                    Area: BasicMessages.InputMessage("new city area");
                                                        string input = Console.ReadLine();
                                                        bool isSucceded = decimal.TryParse(input, out decimal newarea);
                                                        if (isSucceded)
                                                        {
                                                            SqlCommandCity = new SqlCommand("SELECT Area FROM Cities WHERE Id = @CityId", sqlConnection);
                                                            SqlCommandCity.Parameters.AddWithValue("@CityId", cityid);
                                                            decimal CityArea = Convert.ToDecimal(SqlCommandCity.ExecuteScalar());
                                                            SqlCommand = new SqlCommand("SELECT Count(Cities.Id) FROM Cities JOIN Countries ON CountryId = Countries.Id WHERE CountryId = @countryid", sqlConnection);
                                                            SqlCommand.Parameters.AddWithValue("@countryid", newcountryid);
                                                            int CitiesCount = Convert.ToInt32(SqlCommand.ExecuteScalar());
                                                            SqlCommand = new SqlCommand("SELECT SUM(Cities.Area) FROM Cities JOIN Countries ON CountryId = Countries.Id WHERE CountryId = @CountryId", sqlConnection);
                                                            SqlCommand.Parameters.AddWithValue("@CountryId", newcountryid);
                                                            decimal CitiesArea = Convert.ToDecimal(SqlCommand.ExecuteScalar());
                                                            var SqlCommandCountry = new SqlCommand("SELECT Area FROM Countries WHERE Id = @CountryId", sqlConnection);
                                                            SqlCommandCountry.Parameters.AddWithValue("@CountryId", newcountryid);
                                                            decimal CountryArea = Convert.ToInt32(SqlCommandCountry.ExecuteScalar());
                                                            if (CitiesCount == 0 && newarea <= CountryArea)
                                                            {
                                                                SqlCommand = new SqlCommand("UPDATE Cities SET Name = @newname, Area = @newarea WHERE Id = @CityId", sqlConnection);
                                                                SqlCommand.Parameters.AddWithValue("@newname", newcityname);
                                                                SqlCommand.Parameters.AddWithValue("@CityId", cityid);
                                                                SqlCommand.Parameters.AddWithValue("@newarea", newarea);
                                                                changedrows = SqlCommand.ExecuteNonQuery();
                                                                if (changedrows > 0)
                                                                {
                                                                    BasicMessages.SuccessMessage(cityname, "updated");
                                                                }
                                                                else
                                                                {
                                                                    ErrorMessages.Error();
                                                                }
                                                            }
                                                            else if (CitiesArea - CityArea + newarea <= CountryArea)
                                                            {
                                                                SqlCommand = new SqlCommand("UPDATE Cities SET Name = @newname, Area = @newarea WHERE Id = @CityId", sqlConnection);
                                                                SqlCommand.Parameters.AddWithValue("@newname", newcityname); 
                                                                SqlCommand.Parameters.AddWithValue("@CityId", cityid);
                                                                SqlCommand.Parameters.AddWithValue("@newarea", newarea);
                                                                changedrows = SqlCommand.ExecuteNonQuery();
                                                                if (changedrows > 0)
                                                                {
                                                                    BasicMessages.SuccessMessage(cityname, "updated");
                                                                }
                                                                else
                                                                {
                                                                    ErrorMessages.Error();
                                                                }
                                                            }
                                                            else
                                                            {
                                                                ErrorMessages.AreaError();
                                                                goto Area;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            ErrorMessages.InputError("area");
                                                            goto Area;
                                                        }


                                                    }
                                                    else
                                                    {
                                                        ErrorMessages.NotExists(countryname);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                ErrorMessages.InputError("country name");
                                                goto Answer3;
                                            }
                                            

                                        }
                                    }
                                    else
                                    {
                                        ErrorMessages.InputError("city name");
                                        goto Answer3;
                                    }
                                }
                                else
                                {
                                    ErrorMessages.InputError("answer");
                                    goto Answer1;
                                }
                            }
                            else
                            {
                                ErrorMessages.InputError("answer");
                                goto Answer1;
                            }
                        }
                        else
                        {
                            ErrorMessages.InputError(cityname);
                            goto CityName;
                        }
                    }
                    catch (Exception)
                    {
                        ErrorMessages.Error();
                    }
                }
            }
        }
    }
}
