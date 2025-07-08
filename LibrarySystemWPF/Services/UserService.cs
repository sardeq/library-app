using LibrarySystemWPF.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace LibrarySystemWPF.Services
{
    public class UserService
    {
        private readonly DatabaseService _db = new DatabaseService();

        public List<User> GetAllUsers()
        {
            string query = @"SELECT c.*, g.GenderDesc, s.StatusDesc, t.TypeDesc, l.LanguageDesc
                             FROM Clients c
                             LEFT JOIN Gender g ON c.Gender = g.GenderID
                             LEFT JOIN Status s ON c.Status = s.StatusID
                             LEFT JOIN Types t ON c.Type = t.TypeID
                             LEFT JOIN Languages l ON c.LanguageID = l.LanguageID";

            var dt = _db.GetData(query);
            return dt.AsEnumerable().Select(row => new User
            {
                ClientID = (int)row["ClientID"],
                ClientName = row["ClientName"].ToString(),
                LanguageID = row["LanguageID"] as int? ?? 0,
                Name = row["Name"].ToString(),
                BirthDate = row["BirthDate"] as DateTime? ?? DateTime.MinValue,
                Age = row["Age"] as int? ?? 0,
                Gender = row["Gender"] as int? ?? 0,
                Status = row["Status"] as int? ?? 0,
                Type = row["Type"] as int? ?? 0,
                Password = row["Password"].ToString(),
                BooksQuota = row["BooksQuota"] as int? ?? 0,
                BorrowDuration = row["BorrowDuration"] as int? ?? 0,
                GenderDesc = row["GenderDesc"].ToString(),
                StatusDesc = row["StatusDesc"].ToString(),
                TypeDesc = row["TypeDesc"].ToString(),
                LanguageDesc = row["LanguageDesc"].ToString()
            }).ToList();
        }

        public bool SaveUser(User user)
        {
            string query;
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@ClientName", (object)user.ClientName ?? DBNull.Value),
                new SqlParameter("@LanguageID", user.LanguageID),
                new SqlParameter("@Name", (object)user.Name ?? DBNull.Value),
                new SqlParameter("@BirthDate", user.BirthDate != DateTime.MinValue ? (object)user.BirthDate : DBNull.Value),
                new SqlParameter("@Age", user.Age),
                new SqlParameter("@Gender", user.Gender),
                new SqlParameter("@Status", user.Status),
                new SqlParameter("@Type", user.Type),
                new SqlParameter("@BooksQuota", user.BooksQuota),
                new SqlParameter("@BorrowDuration", user.BorrowDuration)
            };

            if (user.ClientID == 0) // New user
            {
                query = @"INSERT INTO Clients (ClientName, LanguageID, Name, BirthDate, Age, Gender, 
                          Status, Type, Password, BooksQuota, BorrowDuration)
                          VALUES (@ClientName, @LanguageID, @Name, @BirthDate, @Age, @Gender, 
                          @Status, @Type, @Password, @BooksQuota, @BorrowDuration)";
                parameters.Add(new SqlParameter("@Password", (object)user.Password ?? DBNull.Value));
            }
            else // Update existing user
            {
                query = @"UPDATE Clients SET 
                          ClientName = @ClientName,
                          LanguageID = @LanguageID,
                          Name = @Name,
                          BirthDate = @BirthDate,
                          Age = @Age,
                          Gender = @Gender,
                          Status = @Status,
                          Type = @Type,
                          BooksQuota = @BooksQuota,
                          BorrowDuration = @BorrowDuration";

                if (!string.IsNullOrEmpty(user.Password))
                {
                    query += ", Password = @Password";
                    parameters.Add(new SqlParameter("@Password", user.Password));
                }

                query += " WHERE ClientID = @ClientID";
                parameters.Add(new SqlParameter("@ClientID", user.ClientID));
            }

            return _db.ExecuteNonQuery(query, parameters.ToArray());
        }

        public bool DeleteUser(int clientId)
        {
            string query = "DELETE FROM Clients WHERE ClientID = @ClientID";
            SqlParameter[] parameters = { new SqlParameter("@ClientID", clientId) };
            return _db.ExecuteNonQuery(query, parameters);
        }

        public DataTable GetGenders()
        {
            return _db.GetData("SELECT GenderID, GenderDesc FROM Gender");
        }

        public DataTable GetStatusList()
        {
            return _db.GetData("SELECT StatusID, StatusDesc FROM Status");
        }

        public DataTable GetUserTypes()
        {
            return _db.GetData("SELECT TypeID, TypeDesc FROM Types");
        }

        public DataTable GetLanguages()
        {
            return _db.GetData("SELECT LanguageID, LanguageDesc FROM Languages");
        }
    }
}