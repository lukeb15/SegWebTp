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
        private IMapper<ForumThread, TableThreadDTO> ThreadMap { set; get; }
        public UserMapper()
        {
            ThreadMap = new TableThreadMapper();
        }

        public UserDTO MapTo(User dbModel)
        {
            UserDTO dto = new UserDTO();
            dto.Id = dbModel.Id;
            dto.UserName = dbModel.UserName;
            dto.Email = dbModel.Email;
            dto.ProfilePictureLink = dbModel.ProfilePictureLink;
            dto.Threads = dbModel.Threads.ToList()
                            .Select(t => ThreadMap.MapTo(t)).ToList();
            dto.Points = dto.Threads.Sum(utp => utp.Points);

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