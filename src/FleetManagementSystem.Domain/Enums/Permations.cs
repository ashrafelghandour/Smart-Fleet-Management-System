namespace FleetManagementSystem.Domain.Enums;

public static class Permissions
{
    // Trip Permissions
    public const string TripsView = "Trips.View";
    public const string TripsCreate = "Trips.Create";
    public const string TripsEdit = "Trips.Edit";
    public const string TripsDelete = "Trips.Delete";
    public const string TripsComplete = "Trips.Complete";
    public const string TripsCancel = "Trips.Cancel";

    // Driver Permissions
    public const string DriversView = "Drivers.View";
    public const string DriversCreate = "Drivers.Create";
    public const string DriversEdit = "Drivers.Edit";
    public const string DriversDelete = "Drivers.Delete";
    public const string DriversManageStatus = "Drivers.ManageStatus";

    // Vehicle Permissions
    public const string VehiclesView = "Vehicles.View";
    public const string VehiclesCreate = "Vehicles.Create";
    public const string VehiclesEdit = "Vehicles.Edit";
    public const string VehiclesDelete = "Vehicles.Delete";
    public const string VehiclesManageStatus = "Vehicles.ManageStatus";

    // User Permissions
    public const string UsersView = "Users.View";
    public const string UsersCreate = "Users.Create";
    public const string UsersEdit = "Users.Edit";
    public const string UsersDelete = "Users.Delete";
    public const string UsersManageRoles = "Users.ManageRoles";

    // Report Permissions
    public const string ReportsView = "Reports.View";
    public const string ReportsGenerate = "Reports.Generate";
    public const string ReportsExport = "Reports.Export";
}

public static class Roles
{
    public const string Admin = "Admin";
    public const string Manager = "Manager";
    public const string Dispatcher = "Dispatcher";
    public const string Driver = "Driver";
    public const string Viewer = "Viewer";

    public static readonly Dictionary<string, string[]> RolePermissions = new()
    {
        // Admin - كل الصلاحيا
        [Admin] = new[]
        {
            Permissions.TripsView, Permissions.TripsCreate, Permissions.TripsEdit,
            Permissions.TripsDelete, Permissions.TripsComplete, Permissions.TripsCancel,
            Permissions.DriversView, Permissions.DriversCreate, Permissions.DriversEdit,
            Permissions.DriversDelete, Permissions.DriversManageStatus,
            Permissions.VehiclesView, Permissions.VehiclesCreate, Permissions.VehiclesEdit,
            Permissions.VehiclesDelete, Permissions.VehiclesManageStatus,
            Permissions.UsersView, Permissions.UsersCreate, Permissions.UsersEdit,
            Permissions.UsersDelete, Permissions.UsersManageRoles,
            Permissions.ReportsView, Permissions.ReportsGenerate, Permissions.ReportsExport
        },

        // Manager - كل شيء ما عدا حذف المستخدمين وإدارة الأدوار
        [Manager] = new[]
        {
            Permissions.TripsView, Permissions.TripsCreate, Permissions.TripsEdit,
            Permissions.TripsComplete, Permissions.TripsCancel,
            Permissions.DriversView, Permissions.DriversCreate, Permissions.DriversEdit,
            Permissions.DriversManageStatus,
            Permissions.VehiclesView, Permissions.VehiclesCreate, Permissions.VehiclesEdit,
            Permissions.VehiclesManageStatus,
            Permissions.UsersView,
            Permissions.ReportsView, Permissions.ReportsGenerate, Permissions.ReportsExport
        },

        // Dispatcher - إدارة الرحلات فقط
        [Dispatcher] = new[]
        {
            Permissions.TripsView, Permissions.TripsCreate, Permissions.TripsEdit,
            Permissions.TripsComplete, Permissions.TripsCancel,
            Permissions.DriversView,
            Permissions.VehiclesView,
            Permissions.ReportsView
        },

        // Driver - عرض رحلاته فقط
        [Driver] = new[]
        {
            Permissions.TripsView
        },

        // Viewer - مشاهدة فقط
        [Viewer] = new[]
        {
            Permissions.TripsView,
            Permissions.DriversView,
            Permissions.VehiclesView,
            Permissions.ReportsView
        }
    };

}