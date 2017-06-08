using HacForo.Models;
using HacForo.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HacForo.Mappers
{
    public class CommentMapper : IMapper<Comment, CommentDTO>
    {
        public IMapper<User, TableUserDTO> UserMap { get; private set; }

        public CommentMapper()
        {
            UserMap = new TableUserMapper();
        }

        public CommentDTO MapTo(Comment dbModel)
        {
            CommentDTO dto = new CommentDTO();
            dto.Id = dbModel.Id;
            dto.Message = dbModel.Message;
            dto.User = UserMap.MapTo(dbModel.User);
            dto.ThreadId = dbModel.ThreadId;
            dto.CreationDate = dbModel.CreationDate.ToLongTimeString();

            return dto;
        }

        public Comment MapTo(CommentDTO dto)
        {
            Comment dbModel = new Comment();
            dbModel.ThreadId = dto.ThreadId;
            dbModel.Message = dto.Message;
            dbModel.UserId = dto.User.Id;
            dbModel.CreationDate = DateTime.Now;

            return dbModel;
        }
    }
}