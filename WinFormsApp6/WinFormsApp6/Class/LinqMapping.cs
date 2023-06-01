using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace WinFormsApp6
{
    [Table(Name = "Users")]
    internal class Users
    {
        [Column(Name = "UserID", DbType = "Int", IsPrimaryKey = true)]
        public int UserId
        { get; set; }
        [Column(Name = "Username", DbType = "nvarchar(255)")]
        public string UserName
        { get; set; }
        [Column(Name = "UserPassword", DbType = "nvarchar(255)")]
        public string Password
        { get; set; }
        [Column(Name = "UserGroupID", DbType = "varchar(10)")]
        public string UserGroupID
        { get; set; }
    }
    [Table(Name = "UserGroup")]
    internal class UserGroup
    {
        [Column(Name = "GroupID", DbType = "varchar(10)", IsPrimaryKey = true)]
        public string GroupID { get; set; }
        [Column(Name = "GroupName", DbType = "nvarchar(255)")]
        public string GroupName { get; set; }
        [Column(Name = "UserForm", DbType = "nvarchar(100)")]
        public string UserForm { get; set; }
        [Column(Name = "ConnectString", DbType = "nvarchar(255)")]

        public string ConnectString { get; set; }   
    }
    [Table (Name = "Menu")]
    internal class Menu
    {
        [Column(Name = "MenuName", DbType = "nvarchar(255)", IsPrimaryKey = true)]
        public string MenuName { get; set; }
        [Column(Name = "MenuDescription", DbType = "nvarchar(255)")]
        public string MenuDescription { get; set; } 
    }
    [Table (Name = "UserMenu")]
    internal class UserMenu
    {
        [Column(Name = "GroupID", DbType = "varchar(10)")]
        public string GroupID { get; set; }
        [Column(Name = "MenuName", DbType = "nvarchar(255)")]
        public string MenuName { get; set; }
    }
}
