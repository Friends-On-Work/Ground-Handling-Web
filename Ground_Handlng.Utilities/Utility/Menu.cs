using Ground_Handlng.DataObjects.Data.Context;
using Ground_Handlng.DataObjects.Models.Master;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Ground_Handlng.Utilities.Utility
{
    public class Menu
    {
        private ApplicationDbContext _db;
        public Menu(ApplicationDbContext db)
        {
            _db = db;
        }
        public string MenuBuilder(string User)
        {
            var user = _db.Users.Where(u => u.UserName == User).ToList().FirstOrDefault();
            List<Menus> menus = _db.Menus.ToList();
            List<Menus> menuList = _db.Menus.ToList();
            List<string> PermissionList = (from p in _db.ApplicationPrivileges
                                           join rp in _db.ApplicationRolePrivileges on
                                           p.Id
                                               equals
                                               rp.PrivilegeId
                                           join r in _db.Roles on rp.RoleId
                                           equals r.Id
                                           join ur in _db.UserRoles on r.Id
                                           equals ur.RoleId
                                           where ur.UserId == user.Id
                                           select p.Action).Distinct().ToList();

            DataSet ds = new DataSet();
            ds = ToDataSet(menus);
            DataTable table = ds.Tables[0];
            DataRow[] parentMenus = table.Select("ParentId is null");
            var sb = new StringBuilder();
           return MenuBasedOnPrivilage(parentMenus, table, sb, PermissionList, menuList);
        }
        private string GenerateUL(DataRow[] menu, DataTable table, StringBuilder sb)
        {
            if (menu.Length > 0)
            {
                foreach (DataRow dr in menu)
                {
                    string url = dr["Url"].ToString();
                    string menuText = dr["Name"].ToString();
                    string icon = dr["Icon"].ToString();

                    if (url != "")
                    {
                        string line = String.Format(@"<li class=""nav-item""><a href=""{0}"" class=""nav-link""><i class=""{2}""></i> <span>{1}</span></a></li>", url, menuText, icon);
                        sb.Append(line);
                    }

                    string pid = dr["MenuId"].ToString();
                    string parentId = dr["ParentId"].ToString();

                    DataRow[] subMenu = table.Select(String.Format("ParentId = '{0}'", pid));
                    if (subMenu.Length > 0 && !pid.Equals(parentId))
                    {
                        string line = String.Format(@"<li class=""nav-item has-treeview""><a href=""#"" class=""nav-link active""><i class=""{0}""></i> <p>{1}<i class=""right fa fa-angle-left""></i></p></a><ul class=""nav nav-treeview"">", icon, menuText);
                        var subMenuBuilder = new StringBuilder();
                        sb.AppendLine(line);
                        sb.Append(GenerateUL(subMenu, table, subMenuBuilder));
                        sb.Append("</ul></li>");
                    }
                }
            }
            return sb.ToString();
        }
        public string MenuToBeConstracted(List<Menus> menus, DataRow[] menu, DataTable table, StringBuilder sb)
        {
            foreach (var dr in menus)
            {
                string url = dr.Url.ToString();
                string menuText = dr.Name.ToString();
                string icon = dr.Icon.ToString();

                string pid = dr.MenuId.ToString();
                string parentId = dr.ParentId.ToString();

                DataRow[] subMenu = table.Select(String.Format("ParentId = '{0}'", pid));
                if (subMenu.Length > 0 && !pid.Equals(parentId))
                {
                    string line = String.Format(@"<li class=""nav-item has-treeview""><a href=""#"" class=""nav-link active""><i class=""{0}""></i> <p>{1}<i class=""right fa fa-angle-left""></i></p></a><ul class=""nav nav-treeview"">", icon, menuText);
                    var subMenuBuilder = new StringBuilder();
                    sb.AppendLine(line);
                    sb.Append(GenerateUL(subMenu, table, subMenuBuilder));
                    sb.Append("</ul></li>");
                }
            }
            return sb.ToString();
        }
        public List<Menus> LoopMenus(List<Menus> menus, Menus menuWithPrivilage)
        {
            var menuConstracted = new List<Menus>();
            if (menuWithPrivilage != null)
            {
                menuConstracted.Add(menuWithPrivilage);
                foreach (var menu in menus)
                {
                    menuWithPrivilage = menus.Where(con => con.MenuId == menuWithPrivilage.ParentId).FirstOrDefault();
                    if (menuWithPrivilage.ParentId != null)
                    {
                        menuConstracted.Add(menuWithPrivilage);
                    }
                    else
                        return menuConstracted;
                }
            }
            return menuConstracted;
        }
        public string MenuBasedOnPrivilage(DataRow[] menu, DataTable table, StringBuilder sb, List<string> privilages, List<Menus> menus)
        {
            var menuStartPrivilage = new Menus();
            var orderMenu = new Menus();
            var getFirstMenu = new Menus();
            var afterLoopingMenu = new List<Menus>();
            var allMenuConstract = new List<Menus>();
            var rootMenu = menus.Where(con => con.ParentId == null && con.Name == "ROOT").FirstOrDefault();
            //menus = menus.Where(con => con.ParentId != null).ToList();
            if (menu.Length > 0)
            {
                foreach (var privilage in privilages)
                {
                    menuStartPrivilage = menus.Where(con => con.Privilages == privilage).FirstOrDefault();
                    if (menuStartPrivilage != null)
                    {
                        afterLoopingMenu = LoopMenus(menus, menuStartPrivilage);
                        if (afterLoopingMenu.Count > 0)
                            foreach (var menuIn in afterLoopingMenu)
                                allMenuConstract.Add(menuIn);
                    }
                }
                if (allMenuConstract.Count > 0)
                {
                    orderMenu = allMenuConstract.Where(con => con.ParentId == rootMenu.MenuId).FirstOrDefault();
                    allMenuConstract = allMenuConstract.OrderBy(con => con.ParentId == rootMenu.MenuId).Distinct().ToList();
                    getFirstMenu = allMenuConstract.Last();
                    DataSet ds = new DataSet();
                    ds = ToDataSet(allMenuConstract);
                    table = ds.Tables[0];
                    DataRow[] parentMenus = table.Select(String.Format("ParentId = '{0}'", getFirstMenu.ParentId));
                    //var sb = new StringBuilder();
                    //MenuBasedOnPrivilage(parentMenus, table, sb, privilages, menuList);
                    string menuString = GenerateUL(parentMenus, table, sb);
                    //sb.Append(menuString);
                    //HttpContext.Session.SetString("menuString", menuString);
                }
            }
            return sb.ToString();
        }
        public DataSet ToDataSet<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            DataSet ds = new DataSet();
            ds.Tables.Add(dataTable);
            return ds;
        }
    }
}
