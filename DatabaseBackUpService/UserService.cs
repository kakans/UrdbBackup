using DbBackupDataModel;
using DbBackupEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseBackUpService
{
    public class UserService
    {
        UserDataModel dataModel = new UserDataModel();

        public LoginEntity ValidateUser(LoginEntity user)
        {
            return dataModel.ValidateUser(user);
        }
        public List<UserPermission> GetUserPermission(int groupId)
        {
            return dataModel.GetUserPermission(groupId);
        }
        public List<UserEntity> GetAllUsers()
        {
            return dataModel.GetAllUsers();
        }

        public List<ListItem> GetUserGroups()
        {
            return dataModel.GetUserGroups();
        }

        public void AddUser(UserEntity model)
        {
            dataModel.AddUser(model);
        }

        public UserEntity GetUser(int id)
        {
            return dataModel.GetUser(id);
        }

        public void UpdateUser(int id, UserEntity model)
        {
            dataModel.UpdateUser(id, model);
        }
    }
}
