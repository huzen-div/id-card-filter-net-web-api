using BroadcastAPI.Entities.Users;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using BroadcastAPI.Models.Users;

namespace BroadcastAPI.Repositories
{
    public class UserDetailRepository
    {
        public List<Users> GetAll(SqlConnection dbCon)
        {
            const string sql = @"
                SELECT [id]
                ,[idCard]
                ,convert(varchar, [dateOfBirth], 23) dateOfBirth
                ,[address]
                FROM [dbo].[user]
            ";

            return dbCon.Query<Users>(sql).ToList();
        }
        public List<Users> GetByFilter(SqlConnection dbCon, UserFilterRequest model)
        {
            string sql = @"
                SELECT [id]
                ,[idCard]
                ,convert(varchar, [dateOfBirth], 23) dateOfBirth
                ,[address]
                FROM [dbo].[user]
                WHERE 1=1
            ";
            if (model.Id != null)
            {
                sql += "AND [id] = @id ";
            }
            if (model.IdCard != null)
            {
                sql += "AND [idCard] LIKE '%'+@idCard+'%' ";
            }
            if (model.DateOfBirth != null)
            {
                sql += "AND [dateOfBirth] = @dateOfBirth ";
            }

            return dbCon.Query<Users>(sql, model).ToList();
        }
        public Users GetByIdCard(SqlConnection dbCon, string idCard)
        {
            const string sql = @"
                SELECT [id]
                ,[idCard]
                ,[dateOfBirth]
                ,[address]
                FROM [dbo].[user]
                WHERE [idCard] = @idCard
            ";

            return dbCon.QuerySingleOrDefault<Users>(sql, new { idCard });
        }
        public int Create(SqlConnection dbCon, UserCreateRequest model)
        {
            int affectedRows = dbCon.Execute(@" 
                INSERT INTO [dbo].[user]
                (
                    [idCard]
                    ,[dateOfBirth]
                    ,[address]
                )
                VALUES
                (
                    @idCard
                    ,@dateOfBirth
                    ,@address
                )"
            , model);

            return affectedRows;
        }
        public int Update(SqlConnection dbCon, UserCreateRequest model)
        {
            int affectedRows = dbCon.Execute(@" 
                UPDATE [user]
                SET [idCard] = @idCard
                ,[dateOfBirth] = @dateOfBirth
                ,[address] = @address
                WHERE [id] = @id
            "
            , model);
            return affectedRows;
        }
    }
}
