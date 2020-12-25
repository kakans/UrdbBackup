using DbBackupEntities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbBackupDataModel
{
    public class GroupDataModel
    {
        MySqlDatabase db = new MySqlDatabase();
        
        public List<UserGroup> GetGroups()
        {
            List<UserGroup> groups = new List<UserGroup>();
            DataSet ds = new DataSet();

            string sql = @"SELECT GroupID, GroupName, Active FROM acc_group";

            ds = db.GetDataSet(sql, CommandType.Text);
            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
            {
                groups.Add(new UserGroup
                {
                    GroupId = Convert.ToInt32(ds.Tables[0].Rows[j]["GroupID"]),
                    GroupName = Convert.ToString(ds.Tables[0].Rows[j]["GroupName"]),
                    Active = Convert.ToBoolean(ds.Tables[0].Rows[j]["Active"])
                });
            }
            return groups;
        }

        public void AddGroup(UserGroup model)
        {
            DataSet ds = new DataSet();

            string sql = @"INSERT INTO acc_group(GroupName,Active)
                           VALUES (@GroupName,@Active)";

            MySql.Data.MySqlClient.MySqlParameter[] param = new MySql.Data.MySqlClient.MySqlParameter[2];
            param[0] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "GroupName", DbType = DbType.String, Value = model.GroupName };
            param[1] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "Active", DbType = DbType.Int32, Value = model.Active };
            db.ExecuteNonQuery(sql, CommandType.Text, param);
        }

        public UserGroup GetGroup(int id)
        {
            UserGroup group = new UserGroup();
            DataSet ds = new DataSet();

            string sql = @"SELECT GroupID, GroupName, Active 
                            FROM acc_group
                            WHERE GroupID=@GroupID";

            MySql.Data.MySqlClient.MySqlParameter[] param = new MySql.Data.MySqlClient.MySqlParameter[1];
            param[0] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "GroupID", DbType = DbType.UInt32, Value = id };
            ds = db.GetDataSet(sql, CommandType.Text, param);

            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
            {
                group.GroupId = Convert.ToInt32(ds.Tables[0].Rows[j]["GroupID"]);
                group.GroupName = Convert.ToString(ds.Tables[0].Rows[j]["GroupName"]);
                group.Active = Convert.ToBoolean(ds.Tables[0].Rows[j]["Active"]);
            }
            return group;
        }

        public void UpdateGroup(int id, UserGroup model)
        {
            string sql = @"UPDATE acc_group SET 
                            GroupName=@GroupName,
                            Active=@Active
                            Where GroupID=@GroupID";

            MySql.Data.MySqlClient.MySqlParameter[] param = new MySql.Data.MySqlClient.MySqlParameter[3];
            param[0] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "GroupName", DbType = DbType.String, Value = model.GroupName };
            param[1] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "Active", DbType = DbType.Boolean, Value = model.Active };
            param[2] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "GroupID", DbType = DbType.Int32, Value = id };
            db.ExecuteNonQuery(sql, CommandType.Text, param);
        }

        public void DeleteGroup(int id)
        {
            string sql = @"DELETE FROM acc_group 
                            Where GroupID=@GroupID";

            MySql.Data.MySqlClient.MySqlParameter[] param = new MySql.Data.MySqlClient.MySqlParameter[1];
            param[0] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "GroupID", DbType = DbType.Int32, Value = id };
            db.ExecuteNonQuery(sql, CommandType.Text, param);
        }
    }
}
