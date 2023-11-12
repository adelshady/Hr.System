using Hr.Application.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hr.Application.Common.Global
{
    public static class Permission
    {
        public static List<string> GeneratePermissionList(string module)
        {
            return new List<string>()
            {
                $"Permission.{module}.Create",
                $"Permission.{module}.View",
                $"Permission.{module}.Edit",
                $"Permission.{module}.Delete"
            };
        }
        public static List<string> GenerateAllPermissions()
        {
            var allPermissions = new List<string>();
            var modules = Enum.GetValues(typeof(Modules));
            foreach (var module in modules)
            {
                allPermissions.AddRange(GeneratePermissionList(module.ToString()));
            }
            return allPermissions;
        }


        //constants to call them in the controller authrization like this [Authorize(Permissions.Permission.View)]

        public static class Employee
        {
            public const string Create = "Permission.Employee.Create";
            public const string View = "Permission.Employee.View";
            public const string Edit = "Permission.Employee.Edit";
            public const string Delete = "Permission.Employee.Delete";
        }

        public static class GeneralSetting
        {
            public const string Create = "Permission.GeneralSetting.Create";
            public const string View = "Permission.GeneralSetting.View";
            public const string Edit = "Permission.GeneralSetting.Edit";
            public const string Delete = "Permission.GeneralSetting.Delete";
        }

        public static class Department
        {
            public const string Create = "Permission.Department.Create";
            public const string View = "Permission.Department.View";
            public const string Edit = "Permission.Department.Edit";
            public const string Delete = "Permission.Department.Delete";
        }

        public static class Salary
        {
            public const string Create = "Permission.Salary.Create";
            public const string View = "Permission.Salary.View";
            public const string Edit = "Permission.Salary.Edit";
            public const string Delete = "Permission.Salary.Delete";
        }

        public static class Attendance
        {
            public const string Create = "Permission.Attendance.Create";
            public const string View = "Permission.Attendance.View";
            public const string Edit = "Permission.Attendance.Edit";
            public const string Delete = "Permission.Attendance.Delete";
        }

        public static class Permissions
        {
            public const string Create = "Permission.Permission.Create";
            public const string View = "Permission.Permission.View";
            public const string Edit = "Permission.Permission.Edit";
            public const string Delete = "Permission.Permission.Delete";
        }
    }
}
