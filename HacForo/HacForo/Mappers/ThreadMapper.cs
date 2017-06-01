using HacForo.Models;
using HacForo.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HacForo.Mappers
{
    public class ThreadMapper : IMapper<ForumThread, ThreadDTO>
    {
        public IMapper<User, UserDTO> UserMap { get; private set; }

        public ThreadMapper()
        {
            UserMap = new UserMapper();
        }        

        public ThreadDTO MapTo(ForumThread dbModel)
        {
            ThreadDTO dto = new ThreadDTO();

            dto.Id = dbModel.Id;
            dto.Title = dbModel.Title;
            dto.CreationDate = dbModel.CreationDate.ToLongTimeString();
            dto.User = UserMap.MapTo(dbModel.User);

            return dto;
        }

        public ForumThread MapTo(ThreadDTO dto)
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
                modelDb.Description = dto.Description;
                modelDb.UserId = dto.User.Id;
                modelDb.User = UserMap.MapTo(dto.User);
                modelDb.CreationDate = DateTime.Now;
            }

            return modelDb;
        }
    }
}