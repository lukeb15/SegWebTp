using HacForo.Models;
using HacForo.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HacForo.Mappers
{
    public class TableUserMapper : IMapper<User, TableUserDTO>
    {
        public TableUserMapper()
        {        
        }
       
        public TableUserDTO MapTo(User dbModel)
        {
            TableUserDTO dto = new TableUserDTO();
            dto.Id = dbModel.Id;
            dto.UserName = dbModel.UserName;
            dto.ProfilePictureLink = dbModel.ProfilePictureLink;
           
            return dto;
        }

        public User MapTo(TableUserDTO dto)
        {
            using (var db = new Models.HacForoContainer())
            {
                return db.UserSet.Find(dto.Id);
            }
        }
    }
}