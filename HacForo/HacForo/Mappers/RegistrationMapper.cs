using HacForo.Models;
using HacForo.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HacForo.Mappers
{
    public class RegistrationMapper : IMapper<User, RegistrationDTO>
    {
        public RegistrationDTO MapTo(User dbModel)
        {
            throw new InvalidOperationException("You can't access to the user data");
        }

        public User MapTo(RegistrationDTO dto)
        {
            using (var db = new Models.HacForoContainer())
            {
                var crypto = new SimpleCrypto.PBKDF2();
                var encrypPass = crypto.Compute(dto.Password);
                var dbModel = db.UserSet.Create();
                dbModel.Email = dto.Email;
                dbModel.Password = encrypPass;
                dbModel.PasswordSalt = crypto.Salt;
                dbModel.FirstName = dto.FirstName;
                dbModel.LastName = dto.LastName;
                dbModel.UserName = dto.UserName;
                dbModel.CreationDate = DateTime.Now;

                return dbModel;
            }
        }
    }
}