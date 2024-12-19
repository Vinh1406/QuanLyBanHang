using Demo.Domain;
using Demo.Domain.ApplicationServices.Users;
using Demo.Domain.Entity;
using Demo.Domain.Exceptions;
using Demo.Domain.InfrastructureServices;
using Demo.Domain.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Demo.Application.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IGenericRepository<Permission, Guid> _permissionsRepository;
        private readonly IGenericRepository<RolePermission, Guid> _rolesPermissionRepository;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public UserService(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IGenericRepository<Permission, Guid> permissionsRepository, IGenericRepository<RolePermission, Guid> rolesPermissionRepository, IJwtTokenService jwtTokenService, IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _permissionsRepository = permissionsRepository;
            _rolesPermissionRepository = rolesPermissionRepository;
            _jwtTokenService = jwtTokenService;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        #region commom
        public async Task<AuthorizedResponseModel> Login(LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                throw new UserException.UserNotFoundException();
            }
            var checkPassword = await _userManager.CheckPasswordAsync(user, model.Password);
            if (checkPassword == null)
            {
                throw new UserException.PasswordNotCorrectException();
            }

            var claims = new List<Claim>
            {
                new ("UserName",user.UserName ),
                new (ClaimTypes.Email,user.Email )

            };
            var response = new AuthorizedResponseModel()
            {
                JwtToken = _jwtTokenService.GenerateAccessToken(claims)
            };
            return response;
        }

        public async Task<UserProfileModel> GetUserProfile(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            if (user == null)
            {
                throw new UserException.UserNotFoundException();
            }
            var result = new UserProfileModel()
            {
                UserId = user.Id,
                UserName = userName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
            };
            if (!user.IsSystemUser)
            {
                return result;
            }
            var userRoles = await _userManager.GetRolesAsync(user);
            if (userRoles == null || !userRoles.Any())
            {
                return result;
            }
            var roles = _roleManager.Roles;
            var permissions = _permissionsRepository.FindAll();
            var rolePermissions = _rolesPermissionRepository.FindAll();
            var userPermission = from r in roles
                                 join p in rolePermissions on r.Id equals p.RoleId
                                 select new { p.PermissionCode, r.Name };
            var filerPermisstions = userPermission.Where(s => userRoles.Contains(s.Name)).Select(x => x.PermissionCode).ToList();
            result.Permissions = filerPermisstions.ToList().Distinct().ToList();

            return result;
        }

        public async Task<ResponseResult> UpdateUserInfor(UpdateUserInfoViewModel model, UserProfileModel currentUser)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(s => s.Id == currentUser.UserId);
            if (user == null)
            {
                throw new UserException.UserNotFoundException();
            }

            user.PhoneNumber = model.PhoneNumber;
            user.UserName = model.UserName;
            user.Email = model.Email;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return ResponseResult.Success("update user profile success");
            }
            else
            {
                var errors = JsonConvert.SerializeObject(result.Errors);
                throw new UserException.HandleUserException(errors);
            }
        }
        public async Task<ResponseResult> ChangePassword(ChangePasswordViewModel model, UserProfileModel currentUser)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(s => s.Id == currentUser.UserId);
            if (user == null)
            {
                throw new UserException.UserNotFoundException();
            }
            var changePassword = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (changePassword.Succeeded)
            {
                return ResponseResult.Success("change password success");
            }
            else
            {
                var errors = JsonConvert.SerializeObject(changePassword.Errors);
                throw new UserException.HandleUserException(errors);
            }

        }
        #endregion

        #region Customers

        public async Task<ResponseResult> RegisterCustomer(RegisterUserViewModel model)
        {
            var user = new AppUser()
            {
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                IsSystemUser = false,
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return ResponseResult.Success("create register customer success");
            }
            else
            {
                var errors = JsonConvert.SerializeObject(result.Errors);
                throw new UserException.HandleUserException(errors);
            }
        }

        #endregion

        #region System_Users
        public async Task<ResponseResult> RegisterSystemUser(RegisterUserViewModel model)
        {
            var user = new AppUser()
            {
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                IsSystemUser = true,
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return ResponseResult.Success();
            }
            else
            {
                var errors = JsonConvert.SerializeObject(result.Errors);
                throw new UserException.HandleUserException(errors);
            }
        }



        public async Task<ResponseResult> AssignRoles(AssignRolesViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                throw new UserException.UserNotFoundException();
            }
            var result = await _userManager.AddToRolesAsync(user, model.RolesNames);
            if (result.Succeeded)
            {
                return ResponseResult.Success("asign role to user success");
            }
            else
            {
                var errors = JsonConvert.SerializeObject(result.Errors);
                throw new UserException.HandleUserException(errors);
            }
        }


        public async Task<ResponseResult> RemoveRoles(RemoveRolesViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                throw new UserException.UserNotFoundException();
            }
            var result = await _userManager.RemoveFromRolesAsync(user, model.RoleNames);
            if (result.Succeeded)
            {
                return ResponseResult.Success("asign role to user success");
            }
            else
            {
                var errors = JsonConvert.SerializeObject(result.Errors);
                throw new UserException.HandleUserException(errors);
            }
        }
        public async Task<ResponseResult> AssignPermissions(AssignPermissionsViewModel model)
        {
            var permissions = model.Permissions.Where(s => s.IsInRole).Select(s => new RolePermission()
            {
                Id = Guid.NewGuid(),
                RoleId = model.RoleId,
                PermissionCode = s.PermissionCode,
            }).ToList();
            var currentPermissions = await _rolesPermissionRepository.FindAll(s => s.RoleId == model.RoleId).ToListAsync();
            try
            {
                await _unitOfWork.BeginTransactionAsync();
                _rolesPermissionRepository.RemoveMultiple(currentPermissions);
                _rolesPermissionRepository.AddRange(permissions);
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitAsync();
                return ResponseResult.Success();
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                throw new UserException.HandleUserException("Something went wrong");
            }
        }

        public async Task<PageResult<UserViewModel>> GetUsers(UserSearchQuery query)
        {
            var result = new PageResult<UserViewModel>() { CurrentPage = query.PageIndex };
            var user = _userManager.Users.Where(s => s.IsSystemUser == query.IsSystemUser);
            if (!string.IsNullOrEmpty(query.Keyword))
            {
                user = user.Where(x => x.UserName.Contains(query.Keyword) || x.Email.Contains(query.Keyword) || x.PhoneNumber.Contains(query.Keyword));
            }
            result.TotalCount = await user.CountAsync();
            result.Data = await user.Select(x => new UserViewModel
            {
                UserId = x.Id,
                UserName = x.UserName,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber
            }).ToListAsync();
            return result;
        }
        public async Task<PageResult<RoleViewModel>> GetRoles(RoleSearchQuery query)
        {
            var result = new PageResult<RoleViewModel>() { CurrentPage = query.PageIndex };
            var role = _roleManager.Roles;
            if (!string.IsNullOrEmpty(query.Keyword))
            {
                role = role.Where(x => x.Name.Contains(query.Keyword));
            }
            result.TotalCount = await role.CountAsync();
            result.Data = await role.Select(x => new RoleViewModel
            {
                RoleId = x.Id,
                RoleName = x.Name,
            }).ToListAsync();
            return result;
        }
        public async Task<RoleViewModel> GetRoleDetail(Guid roleId)
        {
            var roles = _roleManager.Roles;
            var permissionList = await _permissionsRepository.FindAll().ToListAsync();
            var rolePermission = _rolesPermissionRepository.FindAll();

            var role = roles.FirstOrDefault(x => x.Id == roleId);
            if (role == null)
            {
                throw new UserException.RoleNotFoundException();
            }
            var permissionCodeIsInRole = await rolePermission.Where(x => x.RoleId == roleId).Select(x => x.PermissionCode).ToListAsync();
            var permission = permissionList.Select(x => new PermissionViewModel
            {
                PermissionCode = x.Code,
                PermissionName = x.Name,
                ParentPermissionCode = x.ParentCode,
                IsInRole = permissionCodeIsInRole.Contains(x.Code),
            }).ToList();

            var userInRole = (await _userManager.GetUsersInRoleAsync(role.Name)).Select(x => new UserViewModel
            {
                UserId = x.Id,
                UserName = x.UserName,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber

            }).ToList();

            var result = new RoleViewModel
            {
                RoleId = roleId,
                RoleName = role.Name,
                Permissions = permission,
                UsersInRole = userInRole,
            };
            return result;

        }

        #endregion

        #region
        public async Task<bool> InitializeUserAdminAsync()
        {
            var userAdmin = _configuration.GetSection("UserAdmin");
            if (userAdmin != null)
            {
                var user = await _userManager.FindByNameAsync(userAdmin["UserName"]);
                if (user == null)
                {
                    var createUser = new AppUser()
                    {
                        UserName = userAdmin["UserName"],
                        Email = userAdmin["Email"],
                        IsSystemUser = true
                    };

                    var createUserResult = await _userManager.CreateAsync(createUser, userAdmin["Password"]);
                    if (!createUserResult.Succeeded)
                    {
                        return false;
                    }
                    var adminRole = new AppRole() { Name = userAdmin["Role"] };
                    var createRoleResult = await _roleManager.CreateAsync(adminRole);
                    if (!createRoleResult.Succeeded)
                    {
                        return false;
                    }
                    var assignRoleResullt = await _userManager.AddToRoleAsync(createUser, adminRole.Name);
                    var listPermission = new List<Permission>()
                    {
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Pemissions.ROLE_PERMISSION,
                            Name = CommonConstants.Pemissions.ROLE_PERMISSION,
                            Index = 1
                        },
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Pemissions.VIEW_ROLE_PERMISSION,
                            Name = CommonConstants.Pemissions.VIEW_ROLE_PERMISSION,
                            ParentCode = CommonConstants.Pemissions.ROLE_PERMISSION,
                            Index = 2,
                        },
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Pemissions.ADD_ROLE_PERMISSION,
                            Name = CommonConstants.Pemissions.ADD_ROLE_PERMISSION,
                            ParentCode = CommonConstants.Pemissions.ROLE_PERMISSION,
                            Index = 3,
                        },
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Pemissions.UPDATE_ROLE_PERMISSION,
                            Name = CommonConstants.Pemissions.UPDATE_ROLE_PERMISSION,
                            ParentCode = CommonConstants.Pemissions.ROLE_PERMISSION,
                            Index = 4,
                        },
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Pemissions.DELETE_ROLE_PERMISSION,
                            Name = CommonConstants.Pemissions.DELETE_ROLE_PERMISSION,
                            ParentCode = CommonConstants.Pemissions.ROLE_PERMISSION,
                            Index = 5,
                        },

                        /**/

                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Pemissions.USER_PERMISSION,
                            Name = CommonConstants.Pemissions.USER_PERMISSION,
                            Index = 1
                        },
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Pemissions.VIEW_USER_PERMISSION,
                            Name = CommonConstants.Pemissions.VIEW_USER_PERMISSION,
                            ParentCode = CommonConstants.Pemissions.USER_PERMISSION,
                            Index = 2,
                        },
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Pemissions.ADD_USER_PERMISSION,
                            Name = CommonConstants.Pemissions.ADD_USER_PERMISSION,
                            ParentCode = CommonConstants.Pemissions.USER_PERMISSION,
                            Index = 3,
                        },
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Pemissions.UPDATE_USER_PERMISSION,
                            Name = CommonConstants.Pemissions.UPDATE_USER_PERMISSION,
                            ParentCode = CommonConstants.Pemissions.USER_PERMISSION,
                            Index = 4,
                        },
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Pemissions.DELETE_USER_PERMISSION,
                            Name = CommonConstants.Pemissions.DELETE_USER_PERMISSION,
                            ParentCode = CommonConstants.Pemissions.USER_PERMISSION,
                            Index = 5,
                        },



                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Pemissions.VARIANT_PERMISSION,
                            Name = CommonConstants.Pemissions.VARIANT_PERMISSION,
                            Index = 1
                        },
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Pemissions.VIEW_VARIANT_PERMISSION,
                            Name = CommonConstants.Pemissions.VIEW_VARIANT_PERMISSION,
                            ParentCode = CommonConstants.Pemissions.VARIANT_PERMISSION,
                            Index = 2,
                        },
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Pemissions.ADD_VARIANT_PERMISSION,
                            Name = CommonConstants.Pemissions.ADD_VARIANT_PERMISSION,
                            ParentCode = CommonConstants.Pemissions.VARIANT_PERMISSION,
                            Index = 3,
                        },
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Pemissions.UPDATE_VARIANT_PERMISSION,
                            Name = CommonConstants.Pemissions.UPDATE_VARIANT_PERMISSION,
                            ParentCode = CommonConstants.Pemissions.VARIANT_PERMISSION,
                            Index = 4,
                        },
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Pemissions.DELETE_VARIANT_PERMISSION,
                            Name = CommonConstants.Pemissions.DELETE_VARIANT_PERMISSION,
                            ParentCode = CommonConstants.Pemissions.VARIANT_PERMISSION,
                            Index = 5,
                        },




                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Pemissions.PRODUCT_PERMISSION,
                            Name = CommonConstants.Pemissions.PRODUCT_PERMISSION,
                            Index = 1
                        },
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Pemissions.VIEW_PRODUCT_PERMISSION,
                            Name = CommonConstants.Pemissions.VIEW_PRODUCT_PERMISSION,
                            ParentCode = CommonConstants.Pemissions.PRODUCT_PERMISSION,
                            Index = 2,
                        },
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Pemissions.ADD_PRODUCT_PERMISSION,
                            Name = CommonConstants.Pemissions.ADD_PRODUCT_PERMISSION,
                            ParentCode = CommonConstants.Pemissions.PRODUCT_PERMISSION,
                            Index = 3,
                        },
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Pemissions.UPDATE_PRODUCT_PERMISSION,
                            Name = CommonConstants.Pemissions.UPDATE_PRODUCT_PERMISSION,
                            ParentCode = CommonConstants.Pemissions.PRODUCT_PERMISSION,
                            Index = 4,
                        },
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Pemissions.DELETE_PRODUCT_PERMISSION,
                            Name = CommonConstants.Pemissions.DELETE_PRODUCT_PERMISSION,
                            ParentCode = CommonConstants.Pemissions.PRODUCT_PERMISSION,
                            Index = 5,
                        },



                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Pemissions.CATEGORY_PERMISSION,
                            Name = CommonConstants.Pemissions.CATEGORY_PERMISSION,
                            Index = 1
                        },
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Pemissions.VIEW_CATEGORY_PERMISSION,
                            Name = CommonConstants.Pemissions.VIEW_CATEGORY_PERMISSION,
                            ParentCode = CommonConstants.Pemissions.CATEGORY_PERMISSION,
                            Index = 2,
                        },
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Pemissions.ADD_CATEGORY_PERMISSION,
                            Name = CommonConstants.Pemissions.ADD_CATEGORY_PERMISSION,
                            ParentCode = CommonConstants.Pemissions.CATEGORY_PERMISSION,
                            Index = 3,
                        },
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Pemissions.UPDATE_CATEGORY_PERMISSION,
                            Name = CommonConstants.Pemissions.UPDATE_CATEGORY_PERMISSION,
                            ParentCode = CommonConstants.Pemissions.CATEGORY_PERMISSION,
                            Index = 4,
                        },
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Pemissions.DELETE_CATEGORY_PERMISSION,
                            Name = CommonConstants.Pemissions.DELETE_CATEGORY_PERMISSION,
                            ParentCode = CommonConstants.Pemissions.CATEGORY_PERMISSION,
                            Index = 5,
                        },



                        //
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Pemissions.ORDER_PERMISSION,
                            Name = CommonConstants.Pemissions.ORDER_PERMISSION,
                            Index = 1
                        },
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Pemissions.VIEW_ORDER_PERMISSION,
                            Name = CommonConstants.Pemissions.VIEW_ORDER_PERMISSION,
                            ParentCode = CommonConstants.Pemissions.ORDER_PERMISSION,
                            Index = 2,
                        },
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Pemissions.ADD_ORDER_PERMISSION,
                            Name = CommonConstants.Pemissions.ADD_ORDER_PERMISSION,
                            ParentCode = CommonConstants.Pemissions.ORDER_PERMISSION,
                            Index = 3,
                        },
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Pemissions.UPDATE_ORDER_PERMISSION,
                            Name = CommonConstants.Pemissions.UPDATE_ORDER_PERMISSION,
                            ParentCode = CommonConstants.Pemissions.ORDER_PERMISSION,
                            Index = 4,
                        },
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Pemissions.DELETE_ORDER_PERMISSION,
                            Name = CommonConstants.Pemissions.DELETE_ORDER_PERMISSION,
                            ParentCode = CommonConstants.Pemissions.ORDER_PERMISSION,
                            Index = 5,
                        },




                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Pemissions.IMAGE_PERMISSION,
                            Name = CommonConstants.Pemissions.IMAGE_PERMISSION,
                            Index = 1
                        },
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Pemissions.VIEW_IMAGE_PERMISSION,
                            Name = CommonConstants.Pemissions.VIEW_IMAGE_PERMISSION,
                            ParentCode = CommonConstants.Pemissions.IMAGE_PERMISSION,
                            Index = 2,
                        },
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Pemissions.ADD_IMAGE_PERMISSION,
                            Name = CommonConstants.Pemissions.ADD_IMAGE_PERMISSION,
                            ParentCode = CommonConstants.Pemissions.IMAGE_PERMISSION,
                            Index = 3,
                        },
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Pemissions.UPDATE_IMAGE_PERMISSION,
                            Name = CommonConstants.Pemissions.UPDATE_IMAGE_PERMISSION,
                            ParentCode = CommonConstants.Pemissions.IMAGE_PERMISSION,
                            Index = 4,
                        },
                        new Permission()
                        {
                            Id = Guid.NewGuid(),
                            Code = CommonConstants.Pemissions.DELETE_IMAGE_PERMISSION,
                            Name = CommonConstants.Pemissions.DELETE_IMAGE_PERMISSION,
                            ParentCode = CommonConstants.Pemissions.IMAGE_PERMISSION,
                            Index = 5,
                        },
                    };

                    var rolesPermission=listPermission.Select(x=> new RolePermission
                    {
                        Id=x.Id,
                        RoleId=adminRole.Id,
                        PermissionCode=x.Code,
                    }).ToList();
                    bool assignPermissionResult = true;
                    try
                    {
                        await _unitOfWork.BeginTransactionAsync();
                        _permissionsRepository.AddRange(listPermission);
                        _rolesPermissionRepository.AddRange(rolesPermission);
                        await _unitOfWork.SaveChangesAsync();
                        await _unitOfWork.CommitAsync();
                    }
                    catch (Exception ex)
                    {
                        await _unitOfWork.RollbackAsync();
                        assignPermissionResult = false;
                    }

                    if (!assignPermissionResult)
                    {
                        return false;
                    }
                    return true;
                }
            }
            else
            {
                return false;
            }
            return false;
        }
        #endregion



    }
}

