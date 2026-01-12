using DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient; // Thư viện SQL Server

namespace DAL
{
    public class NguoiDung_DAL
    {
        
        Connect db = new Connect();

        
        public DataTable GetAllUsers()
        {
            
            string sql = @"
                SELECT 
                    u.UserID_N01, 
                    u.Username_N01, 
                    u.Email_N01, 
                    u.Password_N01, 
                    u.Phone_N01, 
                    u.Address_N01, 
                    u.Status_N01,
                     
                    r.RoleName_N01
                FROM User_N01 u
                LEFT JOIN Roles_N01 r ON u.RoleID_N01 = r.RoleID_N01
                ORDER BY u.UserID_N01 DESC";

            return db.LoadData(sql);
        }

  
        public DataTable GetRoles()
        {
            return db.LoadData("SELECT RoleID_N01, RoleName_N01 FROM Roles_N01");
        }

      
        public void InsertUser(NguoiDung_DTO u)
        {
            
            string sql = @"INSERT INTO User_N01(UserID_N01, Username_N01, Email_N01, Password_N01, Phone_N01, Address_N01, RoleID_N01, Status_N01) 
                           VALUES(@ID, @User, @Email, @Pass, @Phone, @Addr, @Role, @Status)";

            SqlParameter[] para = {
                new SqlParameter("@ID", u.UserId), // DTO phải là String
                new SqlParameter("@User", u.Username),
                new SqlParameter("@Email", u.Email),
                new SqlParameter("@Pass", u.Password),
                new SqlParameter("@Phone", u.Phone),
                new SqlParameter("@Addr", u.Address),
                new SqlParameter("@Role", u.RoleId),
                new SqlParameter("@Status", u.Status)
            };

            db.Execute(sql, para);
        }

        
        public void UpdateUser(NguoiDung_DTO u)
        {
            string sql = @"UPDATE User_N01 
                           SET Username_N01 = @User,
                               Email_N01 = @Email, 
                               Phone_N01 = @Phone, 
                               Address_N01 = @Addr, 
                               RoleID_N01 = @Role, 
                               Status_N01 = @Status
                           WHERE UserID_N01 = @ID";

            SqlParameter[] para = {
                new SqlParameter("@ID", u.UserId), 
                new SqlParameter("@User", u.Username),
                new SqlParameter("@Email", u.Email),
                new SqlParameter("@Phone", u.Phone),
                new SqlParameter("@Addr", u.Address),
                new SqlParameter("@Role", u.RoleId),
                new SqlParameter("@Status", u.Status)
            };

            db.Execute(sql, para);
        }

      
        public void DeleteUser(string id)
        {
            string sql = "DELETE FROM User_N01 WHERE UserID_N01 = @ID";

            SqlParameter[] para = {
                new SqlParameter("@ID", id)
            };

            db.Execute(sql, para);
        }
    }
}