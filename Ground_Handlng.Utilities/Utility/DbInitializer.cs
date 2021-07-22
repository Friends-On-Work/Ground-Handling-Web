using Ground_Handlng.Abstractions.Utility;
using Ground_Handlng.DataObjects.Data.Context;
using Ground_Handlng.DataObjects.Models.Master;
using Ground_Handlng.DataObjects.Models.UserManagment.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ground_Handlng.Utilities.Utility
{
    public class DbInitializer : IDbInitializer
    {
        private readonly IServiceProvider serviceProvider;
        public IConfiguration Configuration { get; }

        public DbInitializer(IServiceProvider _serviceProvider, IConfiguration _configuration)
        {
            serviceProvider = _serviceProvider;
            Configuration = _configuration;
        }

        //Seed, Creat Admin role and one Admin users, and assign privileges
        public async void Initialize()
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                //create database schema if none exists
                var applicationDbContext = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                applicationDbContext.Database.EnsureCreated();

                if (!applicationDbContext.Users.Any())
                {
                    //Newly added
                    List<ApplicationPrivilege> privileges = new List<ApplicationPrivilege>()
                    {
                        new ApplicationPrivilege(){Id = Guid.NewGuid().ToString(), Action="Roles-Create", Description="Create Roles"},
                        new ApplicationPrivilege(){Id = Guid.NewGuid().ToString(), Action="Roles-Index", Description="List Roles"},
                        new ApplicationPrivilege(){Id = Guid.NewGuid().ToString(), Action="Roles-Edit", Description="Edit Roles"},
                        new ApplicationPrivilege(){Id = Guid.NewGuid().ToString(), Action="Roles-Delete", Description="Delete Roles"},
                        
                        new ApplicationPrivilege(){Id = Guid.NewGuid().ToString(), Action="Account-Index", Description="List Accounts"},
                        new ApplicationPrivilege(){Id = Guid.NewGuid().ToString(), Action="Account-Update", Description="Update Account"},
                        new ApplicationPrivilege(){Id = Guid.NewGuid().ToString(), Action="Account-Register", Description="Create Account"},

                        new ApplicationPrivilege(){Id = Guid.NewGuid().ToString(), Action="Privileges-Create", Description="Create privileges"},
                        new ApplicationPrivilege(){Id = Guid.NewGuid().ToString(), Action="Privileges-Edit", Description="Edit privileges"},
                        new ApplicationPrivilege(){Id = Guid.NewGuid().ToString(), Action="Privileges-Delete", Description="Delete privileges"},
                        new ApplicationPrivilege(){Id = Guid.NewGuid().ToString(), Action="Privileges-Index", Description="List privileges"},
                     };
                    List<PasswordStore> passwordStores = new List<PasswordStore>()
                    {
                         new PasswordStore(){ Password="abcd@1234"},
                         new PasswordStore(){ Password="Abcd@1234"},
                         new PasswordStore(){ Password="1234@Abcd"},
                         new PasswordStore(){ Password="abcd@5678"},
                         new PasswordStore(){ Password="abcd@abcd"},
                         new PasswordStore(){ Password="123456789@abcd"},
                         new PasswordStore(){ Password="abcd@12345"},
                         new PasswordStore(){ Password="Abcd@12345"},
                         new PasswordStore(){ Password="ABCD@12345"},
                         new PasswordStore(){ Password="aBCD@12345"},
                         new PasswordStore(){ Password="asdf@12345"},
                         new PasswordStore(){ Password="asdf@1234"},
                         new PasswordStore(){ Password="password"},
                         new PasswordStore(){ Password="123456"},
                         new PasswordStore(){ Password="12345678"},
                         new PasswordStore(){ Password="qwerty@1234"},
                         new PasswordStore(){ Password="qwerty"},
                         new PasswordStore(){ Password="admin"},
                    };
                    applicationDbContext.PasswordStore.AddRange(passwordStores);
                    applicationDbContext.ApplicationPrivileges.AddRange(privileges);

                    //end Newly added

                    //If there is already an Administrator role, abort
                    var _roleManager = serviceScope.ServiceProvider.GetService<RoleManager<ApplicationRole>>();

                    if (!(await _roleManager.RoleExistsAsync(Configuration.GetSection("UserSettings")["UserRole"])))
                    {
                        //Create the Administartor Role
                        var result = await _roleManager.CreateAsync(new ApplicationRole(Configuration.GetSection("UserSettings")["UserRole"], "Global Access"));

                        if (!result.Succeeded)
                            return;
                    }

                    foreach (ApplicationPrivilege ap in privileges)
                    {
                        applicationDbContext.ApplicationRolePrivileges.Add(
                            new ApplicationRolePrivilege
                            {
                                PrivilegeId = ap.Id,
                                RoleId = applicationDbContext.Roles.FirstOrDefault(r => r.Name == Configuration.GetSection("UserSettings")["UserRole"])?.Id
                            });
                    }

                    applicationDbContext.SaveChanges();

                    //Create the default Admin account and apply the Administrator role
                    var _userManager = serviceScope.ServiceProvider.GetService<UserManager<ApplicationUser>>();

                    ApplicationUser user = new ApplicationUser
                    {
                        UserName = Configuration.GetSection("UserSettings")["UserEmail"],
                        Email = Configuration.GetSection("UserSettings")["UserEmail"]
                    };

                    var success = await _userManager.CreateAsync(user, Configuration.GetSection("UserSettings")["UserPassword"]);

                    if (success.Succeeded)
                        await _userManager.AddToRoleAsync(await _userManager.FindByNameAsync(user.UserName), Configuration.GetSection("UserSettings")["UserRole"]);
                    //await etUserManager.AddUserToRole(user.Id, model.Role);

                }
            }
        }
    }

    //public class MenuGenerator
    //{
    //    private readonly IActionDescriptorCollectionProvider _provider;

    //    public MenuGenerator()
    //    {
    //    }

    //    public MenuGenerator(IActionDescriptorCollectionProvider provider)
    //    {
    //        _provider = provider;
    //    }

    //    public void RouteValue()
    //    {
    //        var routes = _provider.ActionDescriptors.Items.Select(x => new {
    //            Action = x.RouteValues["Action"],
    //            Controller = x.RouteValues["Controller"],
    //            x.AttributeRouteInfo.Name,
    //            x.AttributeRouteInfo.Template
    //        }).ToList();

    //        Console.Write("route Test");
    //    }
  
    //}
}
