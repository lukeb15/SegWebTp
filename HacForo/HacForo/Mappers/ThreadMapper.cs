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
        public IMapper<User, TableUserDTO> TableUserMap { get; private set; }
        public IMapper<Comment, CommentDTO> CommentMap { get; private set; }

        public ThreadMapper() 
        {
            TableUserMap = new TableUserMapper();
            CommentMap = new CommentMapper();
        }        

        public ThreadDTO MapTo(ForumThread dbModel)
        {
            ThreadDTO dto = new ThreadDTO();

            dto.Id = dbModel.Id;
            dto.Title = dbModel.Title;
            dto.CreationDate = dbModel.CreationDate.ToLongTimeString();
            dto.User = TableUserMap.MapTo(dbModel.User);
            dto.Description = dbModel.Description;
            dto.ImageLink = dbModel.ImageLink;
            dto.Points = dbModel.UserThreadPoints.Sum(utp => utp.Points);
            dto.UserCanPoint = false;
            dto.Comments = dbModel.Comments.Select(c => CommentMap.MapTo(c)).ToList();

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
                modelDb.CreationDate = DateTime.Now;
                modelDb.ImageLink = dto.ImageLink;
            }

            return modelDb;
        }
    }
}