using LibraryApp_API.Data;
using LibraryApp_API.Models;
using LibrarySystemWPF.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Status = LibraryApp_API.Models.Status;

namespace LibraryApp_API.Services
{
    public class UserService
    {
        private readonly LibraryDbContext _context;

        public UserService(LibraryDbContext context)
        {
            _context = context;
        }

        public List<User> GetAllUsers()
        {
            return _context.Clients
                .Include(c => c.GenderRef)
                .Include(c => c.StatusRef)
                .Include(c => c.TypeRef)
                .Include(c => c.LanguageRef)
                .Select(c => new User
                {
                    ClientID = c.ClientID,
                    ClientName = c.ClientName,
                    LanguageID = c.LanguageID,
                    Name = c.Name,
                    BirthDate = c.BirthDate,
                    Age = c.Age,
                    Gender = c.GenderID,
                    Status = c.StatusID,
                    Type = c.TypeID,
                    Password = c.Password,
                    BooksQuota = c.BooksQuota,
                    BorrowDuration = c.BorrowDuration,
                    GenderDesc = c.GenderRef.GenderDesc,
                    StatusDesc = c.StatusRef.StatusDesc,
                    TypeDesc = c.TypeRef.TypeDesc,
                    LanguageDesc = c.LanguageRef.LanguageDesc
                }).ToList();
        }

        public bool SaveUser(User user)
        {
            if (user.ClientID == 0)
            {
                var newClient = new Client
                {
                    ClientName = user.ClientName,
                    LanguageID = user.LanguageID,
                    Name = user.Name,
                    BirthDate = user.BirthDate,
                    Age = user.Age,
                    GenderID = user.Gender,
                    StatusID = user.Status,
                    TypeID = user.Type,
                    Password = user.Password,
                    BooksQuota = user.BooksQuota,
                    BorrowDuration = user.BorrowDuration
                };
                _context.Clients.Add(newClient);
            }
            else
            {
                var existingClient = _context.Clients.Find(user.ClientID);
                if (existingClient == null) return false;

                existingClient.ClientName = user.ClientName;
                existingClient.LanguageID = user.LanguageID;
                existingClient.Name = user.Name;
                existingClient.BirthDate = user.BirthDate;
                existingClient.Age = user.Age;
                existingClient.GenderID = user.Gender;
                existingClient.StatusID = user.Status;
                existingClient.TypeID = user.Type;
                existingClient.BooksQuota = user.BooksQuota;
                existingClient.BorrowDuration = user.BorrowDuration;

                if (!string.IsNullOrEmpty(user.Password))
                    existingClient.Password = user.Password;
            }
            return _context.SaveChanges() > 0;
        }

        public bool DeleteUser(int id)
        {
            var client = _context.Clients.Find(id);
            if (client == null) return false;

            _context.Clients.Remove(client);
            return _context.SaveChanges() > 0;
        }

        public User GetUserByUsername(string username)
        {
            var client = _context.Clients
                .Include(c => c.GenderRef)
                .Include(c => c.StatusRef)
                .Include(c => c.TypeRef)
                .Include(c => c.LanguageRef)
                .FirstOrDefault(c => c.ClientName == username);

            if (client == null) return null;

            return new User
            {
                ClientID = client.ClientID,
                ClientName = client.ClientName,
                LanguageID = client.LanguageID,
                Name = client.Name,
                BirthDate = client.BirthDate,
                Age = client.Age,
                Gender = client.GenderID,
                Status = client.StatusID,
                Type = client.TypeID,
                Password = client.Password,
                BooksQuota = client.BooksQuota,
                BorrowDuration = client.BorrowDuration,
                GenderDesc = client.GenderRef?.GenderDesc,
                StatusDesc = client.StatusRef?.StatusDesc,
                TypeDesc = client.TypeRef?.TypeDesc,
                LanguageDesc = client.LanguageRef?.LanguageDesc
            };
        }

        public List<Gender> GetGenders()
        {
            return _context.Genders.ToList();
        }

        public List<Status> GetStatusList()
        {
            return _context.Statuses.ToList();
        }

        public List<Models.Type> GetUserTypes()
        {
            return _context.Types.ToList();
        }

        public List<Language> GetLanguages()
        {
            return _context.Languages.ToList();
        }
    }
}