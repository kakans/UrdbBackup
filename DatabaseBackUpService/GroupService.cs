using DbBackupDataModel;
using DbBackupEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseBackUpService
{
    public class GroupService
    {
        GroupDataModel dataModel = new GroupDataModel();


        public List<UserGroup> GetGroups()
        {
            return dataModel.GetGroups();
        }

        public void AddGroup(UserGroup model)
        {
            dataModel.AddGroup(model);
        }

        public UserGroup GetGroup(int id)
        {
            return dataModel.GetGroup(id);
        }

        public void UpdateGroup(int id, UserGroup model)
        {
            dataModel.UpdateGroup(id, model);
        }

        public void DeleteGroup(int id)
        {
            dataModel.DeleteGroup(id);
        }
    }
}
