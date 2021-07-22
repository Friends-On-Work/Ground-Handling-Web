using Ground_Handlng.DataObjects.Data.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Ground_Handlng.DataObjects.Models.UserManagment.Identity
{
    /// <summary>
    /// System user to be authorized
    /// </summary>
    public class GroundServicePlanningUser
    {
        ApplicationDbContext applicationDbContext;
        private readonly IServiceProvider serviceProvider;
        public string Username { get; set; }

        private List<ApplicationUserRole> Roles = new List<ApplicationUserRole>();

        public GroundServicePlanningUser(IServiceProvider _serviceProvider)
        {
            serviceProvider = _serviceProvider;
        }

        public GroundServicePlanningUser(string _username, IServiceProvider serviceProvider)
        {
            var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();

            applicationDbContext = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
            applicationDbContext.Database.EnsureCreated();

            Username = _username;
            GetUserRolesPrivileges();
        }

        /// <summary>
        /// Gets user privileges using its role
        /// </summary>
        private void GetUserRolesPrivileges()
        {
            //get user
            ApplicationUser user = applicationDbContext.Users.Include(ur => ur.UserRoles).Where(u => u.UserName == this.Username).FirstOrDefault();
             
            if (user != null)
            {
                var userRoles = applicationDbContext.UserRoles.Where(ur => ur.UserId == user.Id).ToList();

                foreach (var role in userRoles)
                {
                    this.Roles.Add(new ApplicationUserRole { RoleId = role.RoleId });
                }
            }
        }

        /// <summary>
        /// Checks whether a user has a given privilege
        /// </summary>
        /// <param name="requiredPrivilege">Privilege to be checked</param>
        /// <returns></returns>
      
        public bool HasPrivilege(string requiredPrivilege)
        {
            bool found = false;
            List<ApplicationRolePrivilege> rolePrivilegelist = applicationDbContext.ApplicationRolePrivileges.ToList();
            var privileges = applicationDbContext.ApplicationPrivileges.ToList();

            foreach (ApplicationUserRole userRole in this.Roles)
            {
                List<ApplicationRolePrivilege> rolePrivilege = rolePrivilegelist.Where(r => r.RoleId == userRole.RoleId).ToList();
                foreach (var privilege in rolePrivilege)
                {
                    found = privileges.Where(p => p.Action == requiredPrivilege && privilege.PrivilegeId == p.Id).ToList().Count > 0;
                    if (found)
                        break;
                }
                if (found)
                    break;
            }
            return found;
        }
    }

    public class GroundServicePlanningAuthorizationFilter : Attribute, IAsyncAuthorizationFilter 
    {
        //private readonly IHttpContextAccessor _httpContextAccessor;

        //public GroundServicePlanningAuthorizationFilter(HttpContextAccessor httpContextAccessor)
        //{
        //    _httpContextAccessor = httpContextAccessor;
        //}

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            await Task.Delay(1);

            if (context != null && context?.ActionDescriptor is ControllerActionDescriptor descriptor)
            {
                //var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;


                /*Create permission string based on the requested controller 
                 name and action name in the format 'controllername-action'*/
                string requiredPrivilege = String.Format("{0}-{1}", descriptor.ControllerName, descriptor.ActionName);

                /*Create an instance of our custom user authorisation object passing requesting
                  user's 'Windows Username' into constructor*/
                GroundServicePlanningUser requestingUser = new GroundServicePlanningUser(context.HttpContext.User.Identity.Name, context.HttpContext.RequestServices);

                //Check if the requesting user has the permission to run the controller's action
                if (!requestingUser.HasPrivilege(requiredPrivilege))
                {
                    /*User doesn't have the required permission and is not a SysAdmin, return our 
                      custom '401 Unauthorized' access error. Since we are setting 
                      filterContext.Result to contain an ActionResult page, the controller's 
                      action will not be run.

                      The custom '401 Unauthorized' access error will be returned to the 
                      browser in response to the initial request.*/
                    context.Result = new RedirectToRouteResult(new RouteValueDictionary {
                                                { "action", "Unauthorized" },
                                                { "controller", "Account" }, { "area" , "Account" } });
                }

                /*If the user has the permission to run the controller's action, then 
                  filterContext.Result will be uninitialized and executing the controller's 
                  action is dependant on whether filterContext.Result is uninitialized.*/
            }
            else
            {
                //{context.HttpContext.Request.Scheme}://{context.HttpContext.Request.Host}
                // UrlHolder.url = $"{context.HttpContext.Request.Scheme}://{context.HttpContext.Request.Host}{context.HttpContext.Request.Path}";
                context.Result = new RedirectToRouteResult(new RouteValueDictionary {
                                                { "action", "LogOff" },
                                                { "controller", "Account" } , { "area" , "Account" } });
            }
            //var hasClaim = context.HttpContext.User.Claims.Any(c => c.Type == _claim.Type && c.Value == _claim.Value);
            //if (!hasClaim)
            //{
            //    context.ActionDescriptor.Result = new ForbidResult();
            //}
        }
    }


    //public class GroundServicePlanningAuthorizationFilter : Attribute//, IAuthorizationFilter 
    //{
    //    public void OnAuthorization(AuthorizationFilterContext context)
    //    {
    //        if (context != null && context?.ActionDescriptor is ControllerActionDescriptor descriptor)
    //        {
    //            /*Create permission string based on the requested controller 
    //             name and action name in the format 'controllername-action'*/
    //            string requiredPrivilege = String.Format("{0}-{1}", descriptor.ControllerName, descriptor.ActionName);



    //            /*Create an instance of our custom user authorisation object passing requesting
    //              user's 'Windows Username' into constructor*/
    //            AdminLteTemplateUser requestingUser = new AdminLteTemplateUser(context.HttpContext.User.Identity.Name, context.HttpContext.RequestServices);



    //            //Check if the requesting user has the permission to run the controller's action
    //            if (string.IsNullOrEmpty(context.HttpContext.User.Identity.Name))
    //            {
    //                context.Result = new RedirectToRouteResult(new RouteValueDictionary {
    //                                            { "action", "Login" },
    //                                            { "controller", "Account" },
    //                                            { "returnUrl", $"{context.HttpContext.Request.Path}{context.HttpContext.Request.QueryString}" } });
    //            }
    //            else if (!requestingUser.HasPrivilege(requiredPrivilege))
    //            {
    //                /*User doesn't have the required permission and is not a SysAdmin, return our 
    //                  custom '401 Unauthorized' access error. Since we are setting 
    //                  filterContext.Result to contain an ActionResult page, the controller's 
    //                  action will not be run.

    //                  The custom '401 Unauthorized' access error will be returned to the 
    //                  browser in response to the initial request.*/
    //                context.Result = new RedirectToRouteResult(new RouteValueDictionary {
    //                                            { "action", "Unauthorized" },
    //                                            { "controller", "Account" },
    //                 { "area", "" } });
    //            }
    //            /*If the user has the permission to run the controller's action, then 
    //              filterContext.Result will be uninitialized and executing the controller's 
    //              action is dependant on whether filterContext.Result is uninitialized.*/
    //        }

    //        //var hasClaim = context.HttpContext.User.Claims.Any(c => c.Type == _claim.Type && c.Value == _claim.Value);
    //        //if (!hasClaim)
    //        //{
    //        //    context.ActionDescriptor.Result = new ForbidResult();
    //        //}
    //    }
    //}

}
