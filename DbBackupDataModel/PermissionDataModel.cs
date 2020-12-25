using DbBackupEntities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbBackupDataModel
{
    public class PermissionDataModel
    {
        MySqlDatabase db = new MySqlDatabase();
        
        public List<Permission> GetPermission()
        {
            List<Permission> permission = new List<Permission>();
            DataSet ds = new DataSet();

            string sql = @"SELECT PermissionNameID,PermissionName,PermissionDisplayName 
                            FROM acc_permissionName";

            ds = db.GetDataSet(sql, CommandType.Text);
            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
            {
                permission.Add(new Permission
                {
                    PermissionId = Convert.ToInt32(ds.Tables[0].Rows[j]["PermissionNameID"]),
                    PermissionName = Convert.ToString(ds.Tables[0].Rows[j]["PermissionName"]),
                    PermissionDisplayName = Convert.ToString(ds.Tables[0].Rows[j]["PermissionDisplayName"])
                });
            }
            return permission;
        }

        public void AddPermission(Permission model)
        {
            DataSet ds = new DataSet();

            string sql = @"INSERT INTO acc_permissionName(PermissionName,PermissionDisplayName)
                           VALUES (@PermissionName,@PermissionDisplayName)";

            MySql.Data.MySqlClient.MySqlParameter[] param = new MySql.Data.MySqlClient.MySqlParameter[2];
            param[0] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "PermissionName", DbType = DbType.String, Value = model.PermissionName };
            param[1] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "PermissionDisplayName", DbType = DbType.String, Value = model.PermissionDisplayName };
            db.ExecuteNonQuery(sql, CommandType.Text, param);
        }

        public Permission GetPermission(int id)
        {
            Permission permission = new Permission();
            DataSet ds = new DataSet();

            string sql = @"SELECT PermissionNameID,PermissionName,PermissionDisplayName 
                            FROM acc_permissionName 
                            Where PermissionNameID=@PermissionNameID";

            MySql.Data.MySqlClient.MySqlParameter[] param = new MySql.Data.MySqlClient.MySqlParameter[1];
            param[0] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "PermissionNameID", DbType = DbType.UInt32, Value = id };
            ds = db.GetDataSet(sql, CommandType.Text, param);

            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
            {
                permission.PermissionId = Convert.ToInt32(ds.Tables[0].Rows[j]["PermissionNameID"]);
                permission.PermissionName = Convert.ToString(ds.Tables[0].Rows[j]["PermissionName"]);
                permission.PermissionDisplayName = Convert.ToString(ds.Tables[0].Rows[j]["PermissionDisplayName"]);
            }
            return permission;
        }

        public void UpdatePermission(int id, Permission model)
        {
            string sql = @"UPDATE acc_permissionName SET 
                            PermissionName=@PermissionName,
                            PermissionDisplayName=@PermissionDisplayName
                            Where PermissionNameID=@PermissionNameID";

            MySql.Data.MySqlClient.MySqlParameter[] param = new MySql.Data.MySqlClient.MySqlParameter[3];
            param[0] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "PermissionName", DbType = DbType.String, Value = model.PermissionName };
            param[1] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "PermissionDisplayName", DbType = DbType.String, Value = model.PermissionDisplayName };
            param[2] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "PermissionNameID", DbType = DbType.Int32, Value = id };
            db.ExecuteNonQuery(sql, CommandType.Text, param);

        }

        public void DeletePermission(int id)
        {
            string sql = @"DELETE FROM acc_permissionName 
                            Where PermissionNameID=@PermissionNameID";

            MySql.Data.MySqlClient.MySqlParameter[] param = new MySql.Data.MySqlClient.MySqlParameter[1];
            param[0] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "PermissionNameID", DbType = DbType.Int32, Value = id };
            db.ExecuteNonQuery(sql, CommandType.Text, param);
        }

        public List<Permission> GetPermissionByGroup(int id)
        {
            List<Permission> permission = new List<Permission>();
            DataSet ds = new DataSet();

            string sql = @"SELECT pname.PermissionNameID,pname.PermissionName,pname.PermissionDisplayName,per.HasAccess 
                            FROM acc_permission per
                            join acc_group grp
                            on per.groupid  = grp.GroupID
                            join acc_permissionName pname
                            on per.permissionNameId = pname.PermissionNameID
                            where grp.groupid=@groupid";
            
            MySql.Data.MySqlClient.MySqlParameter[] param = new MySql.Data.MySqlClient.MySqlParameter[1];
            param[0] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "groupid", DbType = DbType.Int32, Value = id };
            ds = db.GetDataSet(sql, CommandType.Text,param);

            for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
            {
                permission.Add(new Permission
                {
                    PermissionId = Convert.ToInt32(ds.Tables[0].Rows[j]["PermissionNameID"]),
                    PermissionName = Convert.ToString(ds.Tables[0].Rows[j]["PermissionName"]),
                    PermissionDisplayName = Convert.ToString(ds.Tables[0].Rows[j]["PermissionDisplayName"]),
                    HasAccess = Convert.ToString(ds.Tables[0].Rows[j]["HasAccess"])
                });
            }
            return permission;
        }

        public void SyncGroupPermission()
        {
            string sql = @"updateGroupPermission";
            db.ExecuteNonQuery(sql, CommandType.StoredProcedure);
        }

        public void UpdatePermissionByGroup(int id, List<CheckBoxModel> list)
        {
            //update
            string sql = @"UPDATE acc_permission set HasAccess='N'
                            WHERE groupid=@groupid";

            MySql.Data.MySqlClient.MySqlParameter[] param = new MySql.Data.MySqlClient.MySqlParameter[1];
            param[0] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "groupid", DbType = DbType.Int32, Value = id };
            db.ExecuteNonQuery(sql, CommandType.Text, param);

            //set
            var permissionId = list.Where(a=>a.IsChecked == true).Select(x=>x.Value).ToArray();


            string ids= string.Join(",", permissionId);
            string perIds = "";

            for(int i = 0; i < permissionId.Count(); i++)
            {
                if (string.IsNullOrWhiteSpace(perIds))
                {
                    perIds = "@id" + (i + 1).ToString();
                }
                else
                {
                    perIds = perIds + ",@id" + (i + 1).ToString();
                }
            }

            sql = @"UPDATE acc_permission set HasAccess='Y' 
                        WHERE groupid=@groupid
                        AND PermissionNameID in ("+perIds+")";

            MySql.Data.MySqlClient.MySqlParameter[] param1 = new MySql.Data.MySqlClient.MySqlParameter[permissionId.Count()+1];
            param1[0] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "groupid", DbType = DbType.Int32, Value = id };

            for (int i = 0; i < permissionId.Count(); i++)
            {
                string perid = "id" + (i + 1);
                param1[i + 1] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = perid, DbType = DbType.Int32, Value = permissionId[i] };
            }

           // param1[1] = new MySql.Data.MySqlClient.MySqlParameter { ParameterName = "permissionId", DbType = DbType.String, Value = ids };

            db.ExecuteNonQuery(sql, CommandType.Text, param1);
        }
    }
}
