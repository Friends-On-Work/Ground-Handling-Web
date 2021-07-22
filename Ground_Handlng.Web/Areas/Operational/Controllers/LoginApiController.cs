using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Ground_Handlng.Abstractions.Identity;
using Ground_Handlng.DataObjects;
using Ground_Handlng.DataObjects.Data.Context;
using Ground_Handlng.DataObjects.Models.Others;
using Ground_Handlng.DataObjects.Models.RequestObject;
using Ground_Handlng.DataObjects.Models.UserManagment;
using Ground_Handlng.DataObjects.Models.UserManagment.Identity;
using Ground_Handlng.DataObjects.RequestObject;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Ground_Handlng.Web.Areas.Operational.Controllers
{
    [Area("Operational")]
    [DisplayName("Operational")]
    [Route("api/LoginApi")]
    public class LoginApiController : Controller
    {
        public SignInManager<ApplicationUser> SignInManager { get; private set; }
        public UserManager<ApplicationUser> UserManager { get; private set; }
        public ApplicationDbContext context;
        readonly IETRoleManager etRoleManager;
        IHttpContextAccessor _accessor;
        readonly IETUserManager etUserManager;
        AccessLog accessLog;
        public LoginApiController(ApplicationDbContext _context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IETRoleManager _roleManager, IETUserManager _userManager, IHttpContextAccessor accessor)
        {
            SignInManager = signInManager;
            context = _context;
            etRoleManager = _roleManager;
            etUserManager = _userManager;
            UserManager = userManager;
            _accessor = accessor;
        }

        // GET: api/<controller>
        [HttpGet]
        [Route("Login")]
        public async Task<LoginResponseObject> Login(string Username, string Password)
        {
            //Append missed ZEROS
            string userName = Username.Trim();
            string appendableDigit = "";
            for (int i = 0; i < (8 - userName.Length); i++)
                appendableDigit += "0";

            Username = appendableDigit + Username.Trim();
            var response = new LoginResponseObject();
            var result = await SignInManager.PasswordSignInAsync(Username, Password, false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                var user = context.Users.Where(u => u.UserName == Username).ToList().FirstOrDefault();
                var roles = await etUserManager.GetUserRolesAsync(user.Id);
                response.FullName = user.FullName;
                response.Address = user.Address;
                response.Username = user.UserName;
                response.Password = Password;
                response.Position = user.Position;
                response.EmployeeId = user.UserName;
                response.Email = user.Email;
                response.Status = OperationStatus.SUCCESS;
                response.Message = "User Successfuly Login";
                response.StatusAPI = "SUCCESS";
                response.MessageAPI = "SUCCESS";
                return response;

            }
            return new LoginResponseObject()
            {
                Message = "User Not Found",
                Status =  OperationStatus.ERROR
            };
            
        }

        // GET api/<controller>/5
        [HttpGet]
        [Route("ResetPassword")]
        public async Task<OperationResult> ResetPassword(string Username,string OldPassword, string NewPassword)
        {
            var responseOperationResult = new OperationResult();
            //Append missed ZEROS
            string userName = Username.Trim();

            string appendableDigit = "";
            for (int i = 0; i < (8 - userName.Length); i++)
                appendableDigit += "0";

            Username = appendableDigit + Username.Trim();
            //
            var weakPasswords = context.PasswordStore.Select(con => con.Password).ToList();
            if (weakPasswords.Contains(NewPassword))
            {
                responseOperationResult.StatusAPI = "OK";
                responseOperationResult.Message = "Password should be stronger";
                responseOperationResult.Status = OperationStatus.Ok;
                responseOperationResult.MessageAPI= "Password should be stronger";
                return responseOperationResult;
            }
            else
            {
                var passwordStrength = PasswordCheck.GetPasswordStrength(NewPassword);
                if (passwordStrength == PasswordStrength.VeryStrong || passwordStrength == PasswordStrength.Strong)
                {
                    var user = await UserManager.FindByNameAsync(Username);

                    if (user == null)
                    {
                        // Don't reveal that the user does not exist
                        responseOperationResult.StatusAPI = "OK";
                        responseOperationResult.Message = "User Name Does Not Exist";
                        responseOperationResult.Status = OperationStatus.Ok;
                        responseOperationResult.MessageAPI = "User Name Not Found";
                        return responseOperationResult;
                    }
                    var Code = await UserManager.GeneratePasswordResetTokenAsync(user);

                    var result = await UserManager.ResetPasswordAsync(user, Code, NewPassword);
                    if (result.Succeeded)
                    {
                        //accessLog.Save(this.ControllerContext.RouteData.Values["action"].ToString(), User.Identity.Name, "Password Reset for Username = " + user.UserName, _accessor.HttpContext.Connection.LocalIpAddress.ToString(), this.ControllerContext.RouteData.Values["controller"].ToString());

                        responseOperationResult.StatusAPI = "SUCCESS";
                        responseOperationResult.Message = "Password Successfuly Reseted";
                        responseOperationResult.Status = OperationStatus.SUCCESS;
                        responseOperationResult.MessageAPI = "Password Reset Success";
                        return responseOperationResult;
                    }
                    responseOperationResult.Message = "Reset Password Has Encounter Error";
                    responseOperationResult.StatusAPI = result.Succeeded.ToString();
                    responseOperationResult.Status = OperationStatus.ERROR;
                    responseOperationResult.MessageAPI = "Reset Password Has Encounter Error";
                    return responseOperationResult;
                }
                else
                {
                    responseOperationResult.StatusAPI = "OK";
                    responseOperationResult.Message = "Password should be stronger";
                    responseOperationResult.Status = OperationStatus.Ok;
                    responseOperationResult.MessageAPI = "Password should be stronger";
                    return responseOperationResult;
                }

            }
        }

        [HttpGet]
        [Route("ChangePassword")]
        public async Task<OperationResult> ChangePassword(string Username, string OldPassword, string NewPassword)
        {
            var responseOperationResult = new OperationResult();
            //Append missed ZEROS
            string userName = Username.Trim();

            string appendableDigit = "";
            for (int i = 0; i < (8 - userName.Length); i++)
                appendableDigit += "0";

            Username = appendableDigit + Username.Trim();
            //
            var weakPasswords = context.PasswordStore.Select(con => con.Password).ToList();
            if (weakPasswords.Contains(NewPassword))
            {
                responseOperationResult.StatusAPI = "OK";
                responseOperationResult.Message = "Password should be stronger";
                responseOperationResult.Status = OperationStatus.Ok;
                responseOperationResult.MessageAPI = "Password should be stronger";
                return responseOperationResult;
            }
            else
            {
                var passwordStrength = PasswordCheck.GetPasswordStrength(NewPassword);
                if (passwordStrength == PasswordStrength.VeryStrong || passwordStrength == PasswordStrength.Strong)
                {
                    var user = await UserManager.FindByNameAsync(Username);

                    if (user == null)
                    {
                        // Don't reveal that the user does not exist
                        responseOperationResult.StatusAPI = "OK";
                        responseOperationResult.Message = "User Name Does Not Exist";
                        responseOperationResult.Status = OperationStatus.Ok;
                        responseOperationResult.MessageAPI = "User Name Not Found";
                        return responseOperationResult;
                    }
                    var Code = await UserManager.GeneratePasswordResetTokenAsync(user);

                    var result = await UserManager.ChangePasswordAsync(user, OldPassword, NewPassword);
                    if (result.Succeeded)
                    {
                        //accessLog.Save(this.ControllerContext.RouteData.Values["action"].ToString(), User.Identity.Name, "Password Reset for Username = " + user.UserName, _accessor.HttpContext.Connection.LocalIpAddress.ToString(), this.ControllerContext.RouteData.Values["controller"].ToString());

                        responseOperationResult.StatusAPI = "SUCCESS";
                        responseOperationResult.Message = "Password Successfuly Reseted";
                        responseOperationResult.Status = OperationStatus.SUCCESS;
                        responseOperationResult.MessageAPI = "Password Reset Success";
                        return responseOperationResult;
                    }
                    responseOperationResult.Message = result.Errors.FirstOrDefault().Description;
                    responseOperationResult.StatusAPI = result.Succeeded.ToString();
                    responseOperationResult.Status = OperationStatus.ERROR;
                    responseOperationResult.MessageAPI = "Incorrect Password";
                    return responseOperationResult;
                }
                else
                {
                    responseOperationResult.StatusAPI = "OK";
                    responseOperationResult.Message = "Password should be stronger";
                    responseOperationResult.Status = OperationStatus.ERROR;
                    responseOperationResult.MessageAPI = "Password should be stronger";
                    return responseOperationResult;
                }

            }
        }
        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
