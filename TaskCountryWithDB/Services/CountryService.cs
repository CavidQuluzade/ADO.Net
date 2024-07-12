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
    public static class CountryService
    {
        public static void AddCountry()
        {
            using (SqlConnection SqlConnection = new SqlConnection(SqlStringConnection.DEFAULT))
            {
                SqlConnection.Open();
            CountryName: BasicMessages.InputMessage("Country name");
                string countryname = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(countryname))
                {
                    var SqlCommand = new SqlCommand("SELECT * FROM Countries WHERE Name = @countryname", SqlConnection);
                    SqlCommand.Parameters.AddWithValue("@countryname", countryname);
                    int CountryId = Convert.ToInt32(SqlCommand.ExecuteScalar());
                    if (CountryId > 0)
                    {
                        ErrorMessages.Exists(countryname);
                        return;
                    }
                CountryArea: BasicMessages.InputMessage("Area size");
                    string countrysize = Console.ReadLine();
                    decimal CountryArea;
                    bool IsSucceded = decimal.TryParse(countrysize, out CountryArea);
                    if (IsSucceded)
                    {
                        SqlCommand = new SqlCommand("INSERT INTO Countries VALUES(@countryname, @countryarea)", SqlConnection);
                        SqlCommand.Parameters.AddWithValue("@countryname", countryname);
                        SqlCommand.Parameters.AddWithValue("@countryarea", CountryArea);
                        try
                        {
                            int ChangedRows = SqlCommand.ExecuteNonQuery();
                            if (ChangedRows > 0)
                            {
                                BasicMessages.SuccessMessage(countryname, "added");
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
                        ErrorMessages.InputError(countrysize);
                        goto CountryArea;
                    }
                }
                else
                {
                    ErrorMessages.InputError("countryname");
                    goto CountryName;
                }
            }
        }
        public static void ShowCountries()
        {
            using (SqlConnection SqlConnection = new SqlConnection(SqlStringConnection.DEFAULT))
            {
                SqlConnection.Open();
                SqlCommand Command = new SqlCommand("SELECT * FROM Countries", SqlConnection);
                using (var get = Command.ExecuteReader())
                {
                    while (get.Read())
                    {
                        string CountryName = Convert.ToString(get["Name"]);
                        decimal CountryArea = Convert.ToDecimal(get["Area"]);
                        BasicMessages.OutputMessageCountry("Country", CountryName, "Area", CountryArea);
                    }
                }
            }
        }
        public static void DeleteCountry()
        {
        CountryName: CountryService.ShowCountries();
            BasicMessages.InputMessage("country name");
            string countryname = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(countryname))
            {
                using (SqlConnection SqlConnection = new SqlConnection(SqlStringConnection.DEFAULT))
                {
                    SqlConnection.Open();
                    var SqlCommand = new SqlCommand("SELECT * FROM Countries WHERE Name = @countryname", SqlConnection);
                    SqlCommand.Parameters.AddWithValue("@countryname", countryname);
                    int CountryId = Convert.ToInt32(SqlCommand.ExecuteScalar());
                    if (CountryId > 0)
                    {
                        try
                        {
                            var SqlCommandCity = new SqlCommand("SELECT Count(Cities.Id) FROM Cities JOIN Countries ON CountryId = Countries.Id WHERE CountryId = @countryid", SqlConnection);
                            SqlCommandCity.Parameters.AddWithValue("@countryid", CountryId);
                            int CitiesCount = Convert.ToInt32(SqlCommandCity.ExecuteScalar());
                            if (CitiesCount > 0)
                            {
                                var Delete = new SqlCommand("DELETE FROM Cities WHERE CountryId = @CountryId", SqlConnection);
                                Delete.Parameters.AddWithValue("@CountryId", CountryId);
                                int deletedrowscity = Delete.ExecuteNonQuery();
                                if (deletedrowscity > 0)
                                {
                                    Delete = new SqlCommand("DELETE FROM Countries WHERE Name = @CountryName", SqlConnection);
                                    Delete.Parameters.AddWithValue("@CountryName", countryname);
                                    int deletedrows = Delete.ExecuteNonQuery();
                                    if (deletedrows > 0)
                                    {
                                        BasicMessages.SuccessMessage(countryname, "deleted");
                                    }
                                    else
                                    {
                                        ErrorMessages.Error();
                                    }
                                }
                                else
                                {
                                    ErrorMessages.Error();
                                }
                            }
                            else
                            {
                                var Delete = new SqlCommand("DELETE FROM Countries WHERE Name = @CountryName", SqlConnection);
                                Delete.Parameters.AddWithValue("@CountryName", countryname);
                                int deletedrows = Delete.ExecuteNonQuery();
                                if (deletedrows > 0)
                                {
                                    BasicMessages.SuccessMessage(countryname, "deleted");
                                }
                                else
                                {
                                    ErrorMessages.Error();
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
                        ErrorMessages.NotExists(countryname);
                        goto CountryName;
                    }
                }
            }
            else
            {
                ErrorMessages.InputError("countryname");
                goto CountryName;
            }
        }
        public static void GetDetailsOfCountry()
        {
            ShowCountries();
            BasicMessages.InputMessage("Country name");
            string countryname = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(countryname))
            {
                using (SqlConnection sqlConnection = new SqlConnection(SqlStringConnection.DEFAULT))
                {
                    sqlConnection.Open();
                    var SqlCommand = new SqlCommand("SELECT * FROM Countries WHERE Name = @Countryname", sqlConnection);
                    SqlCommand.Parameters.AddWithValue("@CountryName", countryname);
                    using (var get = SqlCommand.ExecuteReader())
                    {
                        while (get.Read())
                        {
                            int id = Convert.ToInt32(get["Id"]);
                            string CountryName = Convert.ToString(get["Name"]);
                            decimal CountryArea = Convert.ToDecimal(get["Area"]);
                            BasicMessages.OutputMessageCountryDetails("Id", id, "Country", CountryName, "Area", CountryArea);
                        }
                    }
                }
            }
            else
            {
                ErrorMessages.InputError("country name");
            }
        }
        public static void UpdateCountryDetailsa()
        {
            CountryName: ShowCountries();
            BasicMessages.InputMessage("country name");
            string countryname = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(countryname))
            {
                using (SqlConnection sqlConnection = new SqlConnection(SqlStringConnection.DEFAULT))
                {
                    try
                    {
                        sqlConnection.Open();
                        var SqlCommand = new SqlCommand("SELECT * FROM Countries WHERE Name = @countryname", sqlConnection);
                        SqlCommand.Parameters.AddWithValue("@countryname", countryname);
                        int countryid = Convert.ToInt32(SqlCommand.ExecuteScalar());
                        if (countryid > 0)
                        {
                            Answer1: BasicMessages.WhatChangeCountryMessage();
                            string answer = Console.ReadLine();
                            if (!string.IsNullOrWhiteSpace(answer))
                            {
                                if (answer == "name")
                                {
                                    Answer3:
                                    BasicMessages.InputMessage("new country name");
                                    string newcountryname = Console.ReadLine();
                                    if (!string.IsNullOrWhiteSpace(newcountryname))
                                    {
                                        SqlCommand = new SqlCommand("SELECT * FROM Countries WHERE Name = @countryname", sqlConnection);
                                        SqlCommand.Parameters.AddWithValue("@countryname", newcountryname);
                                        int newcountryid = Convert.ToInt32(SqlCommand.ExecuteScalar());
                                        if (newcountryid > 0)
                                        {
                                            ErrorMessages.Exists(newcountryname);
                                        Answer2: BasicMessages.ChangeMessage("country name");
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
                                            SqlCommand = new SqlCommand("UPDATE Countries SET Name = @countryname WHERE Id = @CountryId", sqlConnection);
                                            SqlCommand.Parameters.AddWithValue("@CountryId", countryid);
                                            SqlCommand.Parameters.AddWithValue("@countryname", newcountryname);
                                            int changedrows = SqlCommand.ExecuteNonQuery();
                                            if(changedrows > 0)
                                            {
                                                BasicMessages.SuccessMessage(newcountryname, "updated");
                                            }
                                            else
                                            {
                                                ErrorMessages.Error();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        ErrorMessages.InputError("country name");
                                        goto Answer3;
                                    }
                                }
                                else if (answer == "area")
                                {
                                    Area: BasicMessages.InputMessage("new country area");
                                    string input = Console.ReadLine();
                                    bool isSucceded = decimal.TryParse(input, out decimal newarea);
                                    if (isSucceded)
                                    {   
                                        var SqlCommandCity = new SqlCommand("SELECT Count(Cities.Id) FROM Cities JOIN Countries ON @CountryId = Countries.Id", sqlConnection);
                                        SqlCommandCity.Parameters.AddWithValue("@CountryId", countryid);
                                        int CitiesCount = Convert.ToInt32(SqlCommandCity.ExecuteScalar());
                                        SqlCommandCity = new SqlCommand("SELECT SUM(Cities.Area) FROM Cities JOIN Countries ON @CountryId = Countries.Id", sqlConnection);
                                        SqlCommandCity.Parameters.AddWithValue("@CountryId", countryid);
                                        decimal CitiesArea = Convert.ToDecimal(SqlCommandCity.ExecuteScalar());
                                        if (CitiesCount == 0 || CitiesArea < newarea)
                                        {
                                            SqlCommand = new SqlCommand("UPDATE Countries SET Area = @newarea WHERE Id = @CountryId", sqlConnection);
                                            SqlCommand.Parameters.AddWithValue("@CountryId", countryid);
                                            SqlCommand.Parameters.AddWithValue("@newarea", newarea);
                                            int changedrows = SqlCommand.ExecuteNonQuery();
                                            if (changedrows > 0) 
                                            {
                                                BasicMessages.SuccessMessage(countryname, "updated");
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
                                else if (answer == "both")
                                {
                                    Answer3:
                                    BasicMessages.InputMessage("new country name");
                                    string newcountryname = Console.ReadLine();
                                    if (!string.IsNullOrWhiteSpace(newcountryname))
                                    {
                                        SqlCommand = new SqlCommand("SELECT * FROM Countries WHERE Name = @countryname", sqlConnection);
                                        SqlCommand.Parameters.AddWithValue("@countryname", newcountryname);
                                        int newcountryid = Convert.ToInt32(SqlCommand.ExecuteScalar());
                                        if (newcountryid > 0)
                                        {
                                            ErrorMessages.Exists(newcountryname);
                                            Answer2: BasicMessages.ChangeMessage("country name");
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
                                            Area: BasicMessages.InputMessage("new country area");
                                            string input = Console.ReadLine();
                                            bool isSucceded = decimal.TryParse(input, out decimal newarea);
                                            if (isSucceded)
                                            {
                                                var SqlCommandCity = new SqlCommand("SELECT Count(Cities.Id) FROM Cities JOIN Countries ON @CountryId = Countries.Id", sqlConnection);
                                                SqlCommandCity.Parameters.AddWithValue("@CountryId", countryid);
                                                int CitiesCount = Convert.ToInt32(SqlCommandCity.ExecuteScalar());
                                                SqlCommandCity = new SqlCommand("SELECT SUM(Cities.Area) FROM Cities JOIN Countries ON @CountryId = Countries.Id", sqlConnection);
                                                SqlCommandCity.Parameters.AddWithValue("@CountryId", countryid);
                                                decimal CitiesArea = Convert.ToDecimal(SqlCommandCity.ExecuteScalar());
                                                if (CitiesCount == 0 || CitiesArea < newarea)
                                                {
                                                    SqlCommand = new SqlCommand("UPDATE Countries SET Name = @newname, Area = @newarea WHERE Id = @CountryId", sqlConnection);
                                                    SqlCommand.Parameters.AddWithValue("@CountryId", countryid);
                                                    SqlCommand.Parameters.AddWithValue("@newname", newcountryname);
                                                    SqlCommand.Parameters.AddWithValue("@newarea", newarea);
                                                    int changedrows = SqlCommand.ExecuteNonQuery();
                                                    if (changedrows > 0)
                                                    {
                                                        BasicMessages.SuccessMessage(countryname, "updated");
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
                                    }
                                    else
                                    {
                                        ErrorMessages.InputError("country name");
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
                            ErrorMessages.InputError(countryname);
                            goto CountryName;
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
