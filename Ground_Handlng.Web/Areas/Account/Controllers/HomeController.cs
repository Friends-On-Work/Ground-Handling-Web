using Ground_Handlng.DataObjects.Data.Context;
using Ground_Handlng.DataObjects.Models.UserManagment.Identity;
using Ground_Handlng.DataObjects.ViewModel.Identity;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Internal;
using System.Data;
using Microsoft.EntityFrameworkCore.Internal;
using Ground_Handlng.DataObjects.Models.Master;
using System.Threading.Tasks;
using Ground_Handlng.DataObjects.Models.Others;

namespace Ground_Handlng.Web.Controllers
{
    [Area("Account")]
    [DisplayName("Master Data")]
    public class HomeController : Controller
    {
        private ApplicationDbContext dbContext;
        private IApiVersionDescriptionProvider provider;
        public  IActionDescriptorCollectionProvider _actionDescriptorCollectionProvider;
        public Menus menus;
        [NonAction]
        public void privilageCreate()
        {
            var controller = "";
            var action = "";
            string[] strList = { "Controller", "Controller" };
            string[] stringSpareted = { };
            var privilageBuilder = "";
            Assembly asm = Assembly.GetExecutingAssembly();
            var controlleractionlist = asm.GetTypes().Where(type => typeof(Controller).IsAssignableFrom(type)) //filter controllers
                .SelectMany(type => type.GetMethods())
                .Where(method => method.IsPublic && !method.IsDefined(typeof(NonActionAttribute)));
            var model = new ApplicationPrivilege();
            var privilages = dbContext.ApplicationPrivileges.Select(con => con.Action).ToList();
            //The following will extract controllers, actions, attributes and return types:
            var controlleractionlistforAll = asm.GetTypes()
        .Where(type => typeof(Controller).IsAssignableFrom(type))
        .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
        .Where(method => !method.IsDefined(typeof(NonActionAttribute)))
        .Where(m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any())
        .Select(x => new { Controller = x.DeclaringType.Name, Action = x.Name, ReturnType = x.ReturnType.Name, Attributes = String.Join(",", x.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", ""))) })
        .OrderBy(x => x.Controller).ThenBy(x => x.Action).ToList();
            foreach (var controllerAndAction in controlleractionlistforAll)
            {
                controller = controllerAndAction.Controller;
                action = controllerAndAction.Action;
                stringSpareted = controller.Split(strList, 2, StringSplitOptions.RemoveEmptyEntries);
                privilageBuilder = stringSpareted[0].ToString() + "-" + action.ToString();
                if (!privilages.Contains(privilageBuilder))
                {
                    model.Id = Guid.NewGuid().ToString();
                    model.Action = privilageBuilder;
                    model.Description = privilageBuilder;
                    dbContext.ApplicationPrivileges.Add(model);
                    dbContext.SaveChanges();
                    privilages.Add(privilageBuilder);
                    privilages.ToList();
                }

            }
        }
        public HomeController(ApplicationDbContext _dbContext, IActionDescriptorCollectionProvider actionDescriptorCollectionProvider)
        {
            dbContext = _dbContext;
            _actionDescriptorCollectionProvider = actionDescriptorCollectionProvider;
            menus = new Menus(_dbContext);
        }
        [NonAction]
        public void OperationalBuilder(List<IGrouping<string, MenuConstractor>> masterDataConstract, List<Menus> menus, Menus menuToCreate)
        {
            foreach (var group in masterDataConstract)
            {
                foreach (var Action in group)
                {
                    if (Action.Category == "Operational")
                    {
                        if (menus.Count == 0)
                        {
                            menuToCreate = new Menus(dbContext);
                            //create the Root
                            menuToCreate.Name = "ROOT";
                            menuToCreate.Icon = "";
                            menuToCreate.ParentId = null;
                            menuToCreate.Url = $"";
                            var response = menuToCreate.Save() as DatabaseOperationResponse;
                            if (response.Status == OperationStatus.SUCCESS)
                                menus.Add(menuToCreate);
                        }
                        else
                        {
                            var selectRoot = menus.Where(con => con.Name == "ROOT" && con.ParentId == null).FirstOrDefault();
                            if (!menus.Select(con => con.Name).Contains("Operational"))
                            {
                                menuToCreate = new Menus(dbContext);
                                //create the Root
                                menuToCreate.Name = "Operational";
                                menuToCreate.Icon = "";
                                menuToCreate.ParentId = selectRoot.MenuId;
                                menuToCreate.Url = $"";
                                var response = menuToCreate.Save() as DatabaseOperationResponse;
                                if (response.Status == OperationStatus.SUCCESS)
                                    menus.Add(menuToCreate);
                            }
                            else
                                menuToCreate = menus.Where(con => con.Name == "Operational").FirstOrDefault();

                        }
                        var selectRootFOr = menus.Where(con => con.Name == "ROOT" && con.ParentId == null).FirstOrDefault();
                        if (!menus.Select(con => con.Name).Contains("Operational"))
                        {
                            menuToCreate = new Menus(dbContext);
                            //create the Root
                            menuToCreate.Name = "Operational";
                            menuToCreate.Icon = "";
                            menuToCreate.ParentId = selectRootFOr.MenuId;
                            menuToCreate.Url = $"";
                            var response = menuToCreate.Save() as DatabaseOperationResponse;
                            if (response.Status == OperationStatus.SUCCESS)
                                menus.Add(menuToCreate);
                        }
                        else
                            menuToCreate = menus.Where(con => con.Name == "Operational").FirstOrDefault();

                        if (Action.Action == "Index")
                        {
                            createMenuForControllerInAccountMangment(Action, menus, menuToCreate, menuToCreate.Name);
                        }
                        else
                        {
                            createMenuForControllerInAccountMangment(Action, menus, menuToCreate, menuToCreate.Name);
                        }

                    }
                }

            }
        }
        [NonAction]
        public void MasterDataBuilder(List<IGrouping<string, MenuConstractor>> masterDataConstract, List<Menus> menus, Menus menuToCreate)
        {
            foreach (var group in masterDataConstract)
            {
                foreach (var Action in group)
                {
                    if (Action.Category == "MasterData")
                    {
                        if (menus.Count == 0)
                        {
                            menuToCreate = new Menus(dbContext);
                            //create the Root
                            menuToCreate.Name = "ROOT";
                            menuToCreate.Icon = "";
                            menuToCreate.ParentId = null;
                            menuToCreate.Url = $"";
                            var response = menuToCreate.Save() as DatabaseOperationResponse;
                            if (response.Status == OperationStatus.SUCCESS)
                            {
                                TempData["SuccessAlertMessage"] = response.Message;
                                menus.Add(menuToCreate);
                            }
                            else
                                TempData["FailureAlertMessage"] = response.Message;
                        }
                        else
                        {
                            var selectRoot = menus.Where(con => con.Name == "ROOT" && con.ParentId == null).FirstOrDefault();
                            if (!menus.Select(con => con.Name).Contains("Master Data"))
                            {
                                menuToCreate = new Menus(dbContext);
                                //create the Root
                                menuToCreate.Name = "Master Data";
                                menuToCreate.Icon = "";
                                menuToCreate.ParentId = selectRoot.MenuId;
                                menuToCreate.Url = $"";
                                var response = menuToCreate.Save() as DatabaseOperationResponse;
                                if (response.Status == OperationStatus.SUCCESS)
                                {
                                    TempData["SuccessAlertMessage"] = response.Message;
                                    menus.Add(menuToCreate);
                                }
                                else
                                    TempData["FailureAlertMessage"] = response.Message;
                            }
                            else
                                menuToCreate = menus.Where(con => con.Name == "Master Data").FirstOrDefault();

                        }
                        var selectRootFOr = menus.Where(con => con.Name == "ROOT" && con.ParentId == null).FirstOrDefault();
                        if (!menus.Select(con => con.Name).Contains("Master Data"))
                        {
                            menuToCreate = new Menus(dbContext);
                            //create the Root
                            menuToCreate.Name = "Master Data";
                            menuToCreate.Icon = "";
                            menuToCreate.ParentId = selectRootFOr.MenuId;
                            menuToCreate.Url = $"";
                            var response = menuToCreate.Save() as DatabaseOperationResponse;
                            if (response.Status == OperationStatus.SUCCESS)
                                menus.Add(menuToCreate);
                        }
                        else
                            menuToCreate = menus.Where(con => con.Name == "Master Data").FirstOrDefault();

                        if (Action.Action == "Index")
                        {
                            createMenuForControllerInAccountMangment(Action, menus, menuToCreate, menuToCreate.Name);
                        }

                    }
                }

            }
        }
        [NonAction]
        public void AccountMangmentBuilder(List<IGrouping<string,MenuConstractor>> accountConstractor,List<Menus> menus, Menus menuToCreate)
        {
            foreach (var group in accountConstractor)
            {
                foreach (var Action in group)
                {
                    if (Action.Category == "AccountMangement")
                    {
                        if (menus.Count == 0)
                        {
                            menuToCreate = new Menus(dbContext);
                            //create the Root
                            menuToCreate.Name = "ROOT";
                            menuToCreate.Icon = "";
                            menuToCreate.ParentId = null;
                            menuToCreate.Url = $"";
                            var response = menuToCreate.Save() as DatabaseOperationResponse;
                            if (response.Status == OperationStatus.SUCCESS)
                                menus.Add(menuToCreate);
                        }
                        else
                        {
                            var selectRoot = menus.Where(con => con.Name == "ROOT" && con.ParentId == null).FirstOrDefault();
                            if (!menus.Select(con => con.Name).Contains("Account Mangement"))
                            {
                                menuToCreate = new Menus(dbContext);
                                //create the Root
                                menuToCreate.Name = "Account Mangement";
                                menuToCreate.Icon = "";
                                menuToCreate.ParentId = selectRoot.MenuId;
                                menuToCreate.Url = $"";
                                var response = menuToCreate.Save() as DatabaseOperationResponse;
                                if (response.Status == OperationStatus.SUCCESS)
                                    menus.Add(menuToCreate);

                            }
                            else
                                menuToCreate = menus.Where(con => con.Name == "Account Mangement").FirstOrDefault();

                        }
                        var selectRootFOr = menus.Where(con => con.Name == "ROOT" && con.ParentId == null).FirstOrDefault();
                        if (!menus.Select(con => con.Name).Contains("Account Mangement"))
                        {
                            menuToCreate = new Menus(dbContext);
                            //create the Root
                            menuToCreate.Name = "Account Mangement";
                            menuToCreate.Icon = "";
                            menuToCreate.ParentId = selectRootFOr.MenuId;
                            menuToCreate.Url = $"";
                            var response = menuToCreate.Save() as DatabaseOperationResponse;
                            if (response.Status == OperationStatus.SUCCESS)
                                menus.Add(menuToCreate);
                        }
                        else
                            menuToCreate = menus.Where(con => con.Name == "Account Mangement").FirstOrDefault();

                        if (Action.Action == "Index" || Action.Action == "ChangePassword" || Action.Action == "ResetPassword")
                        {
                            createMenuForControllerInAccountMangment(Action, menus, menuToCreate, menuToCreate.Name);
                        }

                    }
                }

            }
        }
        [NonAction]
        public void createMenuForControllerInAccountMangment(MenuConstractor accountConstractor, List<Menus> menus, Menus menuToCreate,string name)
        {
            string[] strList = { "Controller", "Controller" };
            string[] stringSpareted = { };
            var controllerName = "";
            stringSpareted = accountConstractor.Controller.Split(strList, 2, StringSplitOptions.RemoveEmptyEntries);
            controllerName = stringSpareted[0].ToString();
            var selectParent = menus.Where(con => con.Name == name).FirstOrDefault();
            if (!menus.Select(con => con.Name).Contains(controllerName))
            {
                menuToCreate = new Menus(dbContext);
                //create the Root
                menuToCreate.Name = controllerName;
                menuToCreate.Icon = "";
                menuToCreate.ParentId = selectParent.MenuId;
                menuToCreate.Url = $"";
                var response = menuToCreate.Save() as DatabaseOperationResponse;
                if (response.Status == OperationStatus.SUCCESS)
                    menus.Add(menuToCreate);
            }
            else
                menuToCreate=menus.Where(con => con.Name == controllerName).FirstOrDefault();
               if (accountConstractor.Action == "ChangePassword")
                CreateActionIndexForController(accountConstractor, menus, menuToCreate, "Account");
               else if (accountConstractor.Action == "ResetPassword")
                CreateActionIndexForController(accountConstractor, menus, menuToCreate, "Account");
               else
                CreateActionIndexForController(accountConstractor,menus,menuToCreate, controllerName);
        }
        [NonAction]
        public void CreateActionIndexForController(MenuConstractor accountConstractor, List<Menus> menus, Menus menuToCreate, string name)
        {
           
            string[] strList = { "Controller", "Controller" };
            string[] stringSpareted = { };
            var privilageBuilder = "";
            var selectParent = menus.Where(con => con.Name == name).FirstOrDefault();
            var controllerName = "";
            stringSpareted = accountConstractor.Controller.Split(strList, 2, StringSplitOptions.RemoveEmptyEntries);
            controllerName = stringSpareted[0].ToString();
            privilageBuilder = stringSpareted[0].ToString() + "-" + accountConstractor.Action.ToString();
            if (!menus.Select(con => con.Name).Contains(controllerName+"s"))
            {
                menuToCreate = new Menus(dbContext);
                if (accountConstractor.Action == "Index")
                    menuToCreate.Name = controllerName + "s";
                else
                    menuToCreate.Name = accountConstractor.Action;
                //create the Root
                //menuToCreate.Name = controllerName + "s";
                menuToCreate.Icon = "fa fa-user";
                menuToCreate.ParentId = selectParent.MenuId;
                menuToCreate.Url = $"/{accountConstractor.Category}/{controllerName}/{accountConstractor.Action}";
                menuToCreate.Privilages = privilageBuilder;
                var response = menuToCreate.Save() as DatabaseOperationResponse;
                if (response.Status == OperationStatus.SUCCESS)
                    menus.Add(menuToCreate);
            }
            else if(!menus.Select(con => con.Name).Contains(accountConstractor.Action) || !menus.Select(con=>con.Privilages).Contains(privilageBuilder))
            {
                var menu = menus.Where(con => con.ParentId == selectParent.MenuId).ToList();
                var url = $"/{accountConstractor.Category}/{controllerName}/{accountConstractor.Action}";
                if(accountConstractor.Category != "AccountMangement")
                {
                    if (!menu.Any(c => c.Url == url) && !menu.Any(c=>c.Privilages == privilageBuilder))
                    {
                        menuToCreate = new Menus(dbContext);
                        if (accountConstractor.Action == "Index")
                            menuToCreate.Name = controllerName + "s";
                        else
                            menuToCreate.Name = accountConstractor.Action;
                        //create the Root
                        //menuToCreate.Name = controllerName + "s";
                        menuToCreate.Icon = "fa fa-user";
                        menuToCreate.ParentId = selectParent.MenuId;
                        menuToCreate.Url = $"/{accountConstractor.Category}/{controllerName}/{accountConstractor.Action}";
                        menuToCreate.Privilages = privilageBuilder;
                        var response = menuToCreate.Save() as DatabaseOperationResponse;
                        if (response.Status == OperationStatus.SUCCESS)
                            menus.Add(menuToCreate);
                    }
                }
            }
           
        }
        [DisplayName("Master Data")]
        //[Route("[Action]")]
        public async Task<IActionResult> Index()
        {
            //PrivilageBuillder privilageBuild = new PrivilageBuillder(dbContext);
            privilageCreate();
            var permissionChartRoleUsers = new List<PermissionChartRoleUserviewModel>();
            var userId = User.Identity.Name;
            var users = dbContext.UserRoles.ToList();
            var roles = dbContext.Roles.ToList();
            var previleges = dbContext.ApplicationPrivileges.ToList();

            foreach (var role in roles)
            {
                if (role != null)
                {
                    permissionChartRoleUsers.Add(new PermissionChartRoleUserviewModel
                    {
                        RoleName = role.Name,
                        Description = role.Description,
                        NumberOfUsers = users.Where(u => u.RoleId == role.Id).ToList().Count
                    });
                }
            }
            string[] strList = { "Controller", "Controller" };
            string[] stringSpareted = { };
            Assembly asm = Assembly.GetExecutingAssembly();
            //var ControllerDisplayName = string.Empty;
            //var ActionsDisplayName = string.Empty;
            //var ControllerAttributes = ControllerContext.ActionDescriptor.ControllerTypeInfo.GetCustomAttributes(typeof(DisplayNameAttribute), true);
            //var AreaAttribute = ControllerContext.ActionDescriptor.ControllerName;
            //if (ControllerAttributes.Length > 0)
            //  {
            //    ControllerDisplayName = ((DisplayNameAttribute)ControllerAttributes[0]).DisplayName;
            //  }
            var model = new ApplicationPrivilege();
            var privilages = dbContext.ApplicationPrivileges.Select(con => con.Action).ToList();
            string[] CustomeAtr = { };
            //The following will extract controllers, actions, attributes and return types:
            var controlleractionlistforAll = asm.GetTypes().Where(type => typeof(Controller).IsAssignableFrom(type)).SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                .Where(method => !method.IsDefined(typeof(NonActionAttribute))).Where(m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any())
                .Select(x => new {  Areas = x.DeclaringType.Namespace.Split('.').Reverse().Skip(1).First(), Controller = x.DeclaringType.Name, Action = x.Name, ReturnType = x.ReturnType.Name, Attributes = String.Join(",", x.GetCustomAttributes<DisplayNameAttribute>().Select(a => a.DisplayName)) })
                .OrderBy(x => x.Controller).ThenBy(x => x.Action).ToList();

            //var allActionOfMasterData = asm.GetTypes().Where(type => typeof(Controller).IsAssignableFrom(type)).SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
            //     .Where(method => !method.IsDefined(typeof(NonActionAttribute))).Where(m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any())
            //     .Select(x => new { Controller = x.DeclaringType.Name, Action = x.Name, Attributes = String.Join(",", x.GetCustomAttributes<DisplayNameAttribute>().Select(a => a.DisplayName)) }).ToList();

            //var testControllerGet = asm.GetTypes().Where(type => typeof(Controller).IsAssignableFrom(type)).SelectMany(type => type.GetCustomAttributes())
            //    .Select(x => new { NameToDisplay = x.IsDefaultAttribute(), Typeid = x.TypeId }).ToList();

            //var listOfController = asm.GetTypes().Where(type => typeof(Controller).IsAssignableFrom(type)).SelectMany(type => type.GetCustomAttributes<DisplayNameAttribute>())
            //    .Select(x => new { NameToDisplay = x.DisplayName, Typeid = x.TypeId  }).ToList();

            //var listOfControllerWithAreaAndDisplay = asm.GetTypes().Where(type => typeof(Controller).IsAssignableFrom(type)).SelectMany(type => type.GetCustomAttributes<AreaAttribute>())
            //    .Select(x => new { NameToDisplay = x.RouteValue, Typeid = x.TypeId, otherValue = x.RouteKey }).ToList();

            //var listOfControllerArea = asm.GetTypes().Where(type => typeof(Controller).IsAssignableFrom(type)).ToList();
            var thisType = GetType();
            Type t;
            //saving Areas 
            var areaName = string.Empty;
            var areaFullName = string.Empty;
            string[] areaSplit = { };
            var areaList = new List<string>();
            var menuToCreate = new Menus(dbContext);
            var menu = await menus.GetList();

            var listOfControllerConstractor = new List<MenuConstractor>();
            var listOfOrderByCatagory = new List<MenuConstractor>();
            var listOfOrderByController = new List<MenuConstractor>();
            var menuConstractor = new MenuConstractor();
            foreach (var ActionInArea in controlleractionlistforAll)
            {
                 menuConstractor = new MenuConstractor();
                menuConstractor.Area = ActionInArea.Areas;
                menuConstractor.Category = ActionInArea.Attributes;
                menuConstractor.Controller = ActionInArea.Controller;
                menuConstractor.Action = ActionInArea.Action;
                if(menuConstractor.Area != null)
                    listOfControllerConstractor.Add(menuConstractor);
            }
            var AreaGrouping = listOfControllerConstractor.GroupBy(con => con.Area).ToList();
            foreach(var group in AreaGrouping)
            {
                foreach(var catagory in group)
                {
                     menuConstractor = new MenuConstractor();
                    menuConstractor = catagory;                  
                    listOfOrderByCatagory.Add(menuConstractor);
                }
            }
            var CatagoryGrouping = listOfOrderByCatagory.GroupBy(con => con.Category).ToList();
            foreach(var group in CatagoryGrouping)
            {
                foreach(var controllerCatagory in group)
                {
                     menuConstractor = new MenuConstractor();
                    menuConstractor = controllerCatagory;
                    listOfOrderByController.Add(menuConstractor);
                }
            }
            var controllerGrouping = listOfOrderByController.GroupBy(con => con.Controller).ToList();
            //Save Action On Respective Area and Respective Root
            AccountMangmentBuilder(controllerGrouping, menu, menuToCreate);
            MasterDataBuilder(controllerGrouping, menu, menuToCreate);
            OperationalBuilder(controllerGrouping, menu, menuToCreate);
            menu = await menus.GetList();
            var parentMenuList = new List<Menus>();
            var menuStart = new Menus();
            var controllerAndActionName = new Dictionary<string, string>();
           

            ViewBag.PermissionChartRoleUsers = permissionChartRoleUsers;

            ViewBag.NumberOfUsers = users.Count;
            ViewBag.NumberOfRoles = roles.Count;
            ViewBag.NumberOfPrevileges = previleges.Count;
            //ViewBag.NumberOfBusinessModels = previleges.Where;

            return View();
        }

        //[Route("[action]")]
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        [HttpGet]
        public IActionResult LandingPage()
        {
            return View();
        }

        [Route("[action]")]
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        //[Route("[action]")]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //[Route("error/404")]
        public IActionResult Error404()
        {
            return View();
        }

        [Route("error/{code:int}")]
        public IActionResult Error(int code)
        {
            // handle different codes or just return the default error view
            return View();
        }
    }
}
