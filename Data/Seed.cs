using System.Collections.Generic;
using System.Linq;
using DatingApp.API.Models;
using Newtonsoft.Json;

namespace DatingApp.API.Data
{
    public class Seed
    {
        public static void SeedUser ( DataContext context) {
             if(!context.Users.Any()) {
                 var UserData =System.IO.File.ReadAllText("Data/UserSeedData.json");
                var users= JsonConvert.DeserializeObject <List <User >> (UserData);
                foreach (var user in users) {
                  byte [] passwordHash , passwordSalt ;
                  CreatePasswordHash("password", out passwordHash,out passwordSalt);
                  user.PasswordHash=passwordHash;
                  user.PasswordSalt=passwordSalt;
                  user.UserName=user.UserName.ToLower();
                  context.Users.Add(user);

                }
                context.SaveChanges();
             } 
        }

        private static void CreatePasswordHash(string v, out byte[] passwordHash, out byte[] passwordSalt)
        {
             using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(v));
            }
        }
    }
}