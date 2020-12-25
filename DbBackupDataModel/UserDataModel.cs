using DbBackupEntities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbBackupDataModel
{
    public class UserDataModel
    {
        MySqlDatabase db = new MySqlDatabase();

        /// <summary>
        /// This function will validate the user details and return the user object.
        /// </summary>
        /// <param name="user">User Entity</param>
        /// <returns> return user object </returns>
        public LoginEntity ValidateUser(LoginEntity user)
        {
            LoginEntity userData = new LoginEntity();
            DataSet ds = new DataSet();

            string sql = @"SELECT UserId,FirstName,LastName,GroupId 
                            FROM User 
                            Where UserName=@UserName 
                            and Password=@Password 
                            and Active=1";

            MySql.Data.MySqlClient.MySqlParameter[] param = new MySql.Data.MySqlClient.MySqlParameter[2];
            param[0] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "UserName", DbType = DbType.String, Size = 45, Value = user.UserName };
            param[1] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "Password", DbType = DbType.String, Size = 45, Value = user.Password };


            ds = db.GetDataSet(sql, CommandType.Text, param);
            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
            {
                userData.UserId = Convert.ToInt32(ds.Tables[0].Rows[j]["UserId"]);
                //userData.FirstName = Convert.ToString(ds.Tables[0].Rows[j]["FirstName"]);
                //userData.LastName = Convert.ToString(ds.Tables[0].Rows[j]["LastName"]);
                userData.Group = new UserGroup { GroupId = Convert.ToInt32(ds.Tables[0].Rows[j]["GroupId"]) };
            }
            return userData;
        }

        public List<UserPermission> GetUserPermission(int groupId)
        {
            List<UserPermission> userPermission = new List<UserPermission>();
            UserEntity userData = new UserEntity();
            DataSet ds = new DataSet();

            string sql = @"SELECT accpn.PermissionName, HasAccess FROM acc_permission accp 
                            JOIN acc_group accpg ON accp.GroupID = accpg.GroupID 
                            JOIN acc_permissionName accpn ON accp.PermissionNameID = accpn.PermissionNameID 
                            WHERE accpg.GroupID = @groupid and accp.Active = 1";

            MySql.Data.MySqlClient.MySqlParameter[] param = new MySql.Data.MySqlClient.MySqlParameter[1];
            param[0] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "groupid", DbType = DbType.Int32, Value = groupId };

            ds = db.GetDataSet(sql, CommandType.Text, param);
            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
            {
                userPermission.Add(new UserPermission
                {
                    PermissionName = Convert.ToString(ds.Tables[0].Rows[j]["PermissionName"]),
                    HasAccess = Convert.ToString(ds.Tables[0].Rows[j]["HasAccess"])
                });
            }
            return userPermission;
        }

        public List<UserEntity> GetAllUsers()
        {
            List<UserEntity> users = new List<UserEntity>();
            DataSet ds = new DataSet();

            string sql = @"SELECT usr.UserId, usr.UserName,usr.Password, usr.FirstName, usr.LastName,accg.GroupName, usr.Active
                            FROM User usr 
                            join acc_group accg on usr.GroupId = accg.GroupId";

            ds = db.GetDataSet(sql, CommandType.Text);
            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
            {
                users.Add(new UserEntity
                {
                    UserId = Convert.ToInt32(ds.Tables[0].Rows[j]["UserId"]),
                    UserName = Convert.ToString(ds.Tables[0].Rows[j]["UserName"]),
                    FirstName = Convert.ToString(ds.Tables[0].Rows[j]["FirstName"]),
                    LastName = Convert.ToString(ds.Tables[0].Rows[j]["LastName"]),
                    Active = Convert.ToBoolean(ds.Tables[0].Rows[j]["Active"]),
                    Group = new UserGroup { GroupName = Convert.ToString(ds.Tables[0].Rows[j]["GroupName"]) }
                });
            }
            return users;
        }

        public List<ListItem> GetUserGroups()
        {
            List<ListItem> lstGroups = new List<ListItem>();
            DataSet ds = new DataSet();
            string sql = string.Format("SELECT GroupID,GroupName FROM acc_group where active = 1");
            ds = db.GetDataSet(sql, CommandType.Text);
            lstGroups.Add(new ListItem { Text = "Select Group", Value = "" });
            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
            {
                lstGroups.Add(new ListItem
                {
                    Value = Convert.ToString(ds.Tables[0].Rows[j]["GroupID"]),
                    Text = Convert.ToString(ds.Tables[0].Rows[j]["GroupName"])
                });
            }
            return lstGroups;
        }

        public void AddUser(UserEntity model)
        {
            DataSet ds = new DataSet();

            string sql = @"INSERT INTO User(UserName,Password,FirstName,LastName,GroupId,Active)
                           VALUES (@UserName,@Password,@FirstName,@LastName,@GroupId,@Active)";

            MySql.Data.MySqlClient.MySqlParameter[] param = new MySql.Data.MySqlClient.MySqlParameter[6];
            param[0] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "UserName", DbType = DbType.String, Value = model.UserName };
            param[1] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "Password", DbType = DbType.String, Value = model.Password };
            param[2] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "FirstName", DbType = DbType.String, Value = model.FirstName };
            param[3] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "LastName", DbType = DbType.String, Value = model.LastName };
            param[4] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "GroupId", DbType = DbType.Int32, Value = model.SelectedGroupId };
            param[5] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "Active", DbType = DbType.Int32, Value = model.Active };
            db.ExecuteNonQuery(sql, CommandType.Text, param);
        }

        public UserEntity GetUser(int id)
        {
            UserEntity userData = new UserEntity();
            DataSet ds = new DataSet();

            string sql = @"SELECT UserId,UserName,Password,FirstName,LastName,GroupId,Active 
                            FROM User Where UserId=@UserId";

            MySql.Data.MySqlClient.MySqlParameter[] param = new MySql.Data.MySqlClient.MySqlParameter[1];
            param[0] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "UserId", DbType = DbType.UInt32, Value = id };
            ds = db.GetDataSet(sql, CommandType.Text, param);

            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
            {
                userData.UserId = Convert.ToInt32(ds.Tables[0].Rows[j]["UserId"]);
                userData.UserName = Convert.ToString(ds.Tables[0].Rows[j]["UserName"]);
                userData.Password = Convert.ToString(ds.Tables[0].Rows[j]["Password"]);
                userData.FirstName = Convert.ToString(ds.Tables[0].Rows[j]["FirstName"]);
                userData.LastName = Convert.ToString(ds.Tables[0].Rows[j]["LastName"]);
                userData.SelectedGroupId = Convert.ToInt32(ds.Tables[0].Rows[j]["GroupId"]);
                userData.Active = Convert.ToBoolean(ds.Tables[0].Rows[j]["Active"]);
            }
            return userData;
        }

        public void UpdateUser(int id, UserEntity model)
        {
            string sql = @"UPDATE User SET 
                            UserName=@UserName,
                            Password=@Password,
                            FirstName=@FirstName,
                            LastName=@lastName,
                            GroupId=@GroupId,
                            Active=@Active 
                            Where UserId=@UserId";

            MySql.Data.MySqlClient.MySqlParameter[] param = new MySql.Data.MySqlClient.MySqlParameter[7];
            param[0] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "UserName", DbType = DbType.String, Value = model.UserName };
            param[1] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "Password", DbType = DbType.String, Value = model.Password };
            param[2] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "FirstName", DbType = DbType.String, Value = model.FirstName };
            param[3] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "LastName", DbType = DbType.String, Value = model.LastName };
            param[4] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "GroupId", DbType = DbType.Int32, Value = model.SelectedGroupId };
            param[5] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "Active", DbType = DbType.Int32, Value = model.Active };
            param[6] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "UserId", DbType = DbType.UInt32, Value = id };

            db.ExecuteNonQuery(sql, CommandType.Text, param);

        }
    }
}
