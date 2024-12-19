using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Domain.ApplicationServices.Users
{
    public interface IUserService
    {
        #region Commons
        Task<AuthorizedResponseModel> Login(LoginViewModel model);
        Task<UserProfileModel> GetUserProfile(string userName);
        Task<bool> InitializeUserAdminAsync();
        Task<ResponseResult> UpdateUserInfor(UpdateUserInfoViewModel model,UserProfileModel currentUser);
        Task<ResponseResult> ChangePassword(ChangePasswordViewModel model,UserProfileModel currentUser);
        #endregion


        #region Customers
            Task<ResponseResult> RegisterCustomer(RegisterUserViewModel model);
        #endregion

        #region SystemUser
        Task<ResponseResult> RegisterSystemUser(RegisterUserViewModel model);
        Task<ResponseResult> AssignRoles(AssignRolesViewModel model);
        Task<ResponseResult> RemoveRoles(RemoveRolesViewModel model);   
        Task<ResponseResult> AssignPermissions(AssignPermissionsViewModel model);
        Task<PageResult<UserViewModel>> GetUsers(UserSearchQuery query);
        Task<PageResult<RoleViewModel>> GetRoles(RoleSearchQuery query);
        Task<RoleViewModel> GetRoleDetail(Guid roleId);


        #endregion

    }
}
