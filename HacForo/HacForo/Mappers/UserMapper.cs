using HacForo.Models;
using HacForo.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HacForo.Mappers
{
    public class UserMapper : IMapper<User, UserDTO>
    {
        public UserDTO MapTo(User dbModel)
        {
            UserDTO dto = new UserDTO();
            dto.Id = dbModel.Id;
            dto.UserName = dbModel.UserName;

            return dto;
        }

        public User MapTo(UserDTO dto)
        {
            using (var db = new Models.HacForoContainer())
            {
                return db.UserSet.Find(dto.Id);
            }
        }
    }
}