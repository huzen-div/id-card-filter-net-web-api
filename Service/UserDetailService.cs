using BroadcastAPI.Entities.Users;
using BroadcastAPI.Models.Users;
using BroadcastAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace BroadcastAPI.Service
{
    public class UserDetailService
    {
        private readonly ILogger _logger;
        public UserDetailService(ILogger logger)
        {
            _logger = logger;
        }

        public List<Users> GetAll(SqlConnection dbCon)
        {
            _logger.LogInformation("Service: GetAll");
            UserDetailRepository userDetailRepo = new();
            List<Users> Users = userDetailRepo.GetAll(dbCon);
            if (Users == null)
            {
                throw new Exception("Users is not exist.");
            }

            return Users;
        }

        public List<Users> GetByFilter(SqlConnection dbCon, UserFilterRequest model)
        {
            _logger.LogInformation("Service: GetByFilter");
            UserDetailRepository userDetailRepo = new();
            List<Users> User = userDetailRepo.GetByFilter(dbCon, model);
            if (User == null)
            {
                throw new Exception("User is not exist.");
            }

            return User;
        }

        public int Create(SqlConnection dbCon, UserCreateRequest model)
        {
            _logger.LogInformation("Service: Create");
            UserDetailRepository userDetailRepo = new();

            Users Users = userDetailRepo.GetByIdCard(dbCon, model.IdCard);
            if (Users == null)
            {
                int affectedRows = userDetailRepo.Create(dbCon, model);
                if (affectedRows != 1)
                {
                    throw new Exception("เกิดข้อผิดพลาด กรุณาทำรายการใหม่อีกครั้ง");
                }
                return affectedRows;
            }
            else
            {
                throw new Exception("มีเลขบัตรนี้อยู่แล้ว กรุณาทำรายการใหม่อีกครั้ง");
            }
        }
        public int Update(SqlConnection dbCon, UserCreateRequest model)
        {
            _logger.LogInformation("Service: Update");
            UserDetailRepository userDetailRepo = new();

            Users Users = userDetailRepo.GetByIdCard(dbCon, model.IdCard);
            if (Users != null && Users.Id == model.Id)
            {
                int affectedRows = userDetailRepo.Update(dbCon, model);
                if (affectedRows != 1)
                {
                    throw new Exception("เกิดข้อผิดพลาด กรุณาทำรายการใหม่อีกครั้ง");
                }
                return affectedRows;
            }
            else if (Users == null)
            {
                int affectedRows = userDetailRepo.Update(dbCon, model);
                if (affectedRows != 1)
                {
                    throw new Exception("เกิดข้อผิดพลาด กรุณาทำรายการใหม่อีกครั้ง");
                }
                return affectedRows;
            }
            else
            {
                throw new Exception("มีเลขบัตรนี้อยู่แล้ว กรุณาทำรายการใหม่อีกครั้ง");
            }
        }
    }
}
