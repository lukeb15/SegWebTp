using HacForo.Models;
using HacForo.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HacForo.Mappers
{
    public class TableThreadMapper : IMapper<ForumThread, TableThreadDTO>
    {
        public IMapper<User, TableUserDTO> UserMap { get; private set; }

        public TableThreadMapper()
        {
            UserMap = new TableUserMapper();
        }        

        public TableThreadDTO MapTo(ForumThread dbModel)
        {
            TableThreadDTO dto = new TableThreadDTO();

            dto.Id = dbModel.Id;
            dto.Title = dbModel.Title;
            dto.CreationDate = dbModel.CreationDate.ToLongDateString();
            dto.User = UserMap.MapTo(dbModel.User);
            dto.ImageLink = dbModel.ImageLink;
            dto.Points = dbModel.UserThreadPoints.Sum(utp => utp.Points);

            return dto;
        }

        public ForumThread MapTo(TableThreadDTO dto)
        {
            ForumThread modelDb;
            if(dto.Id != 0)
            { 
                using (var db = new Models.HacForoContainer())
                {
                      modelDb = db.ForumThreadSet.Find(dto.Id);
                }
            }
            else
            {
                modelDb = new ForumThread();
                modelDb.Title = dto.Title;
                modelDb.UserId = dto.User.Id;
                modelDb.CreationDate = DateTime.Now;
                modelDb.ImageLink = dto.ImageLink;
            }

            return modelDb;
        }
    }
}