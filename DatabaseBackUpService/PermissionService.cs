using DbBackupDataModel;
using DbBackupEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseBackUpService
{
    public class PermissionService
    {
        PermissionDataModel dataModel = new PermissionDataModel();

        public void SyncGroupPermission()
        {
            dataModel.SyncGroupPermission();
        }
        public List<Permission> GetPermission()
        {
            return dataModel.GetPermission();
        }

        public void AddPermission(Permission model)
        {
            dataModel.AddPermission(model);
        }

        public Permission GetPermission(int id)
        {
            return dataModel.GetPermission(id);
        }

        public List<Permission>GetPermissionByGroup(int id)
        {
            return dataModel.GetPermissionByGroup(id);
        }

        public void UpdatePermission(int id, Permission model)
        {
            dataModel.UpdatePermission(id, model);
        }

        public void DeletePermission(int id)
        {
            dataModel.DeletePermission(id);
        }

        public void UpdatePermissionByGroup(int id, List<CheckBoxModel> list)
        {
            dataModel.UpdatePermissionByGroup(id, list);
        }
    }
}
