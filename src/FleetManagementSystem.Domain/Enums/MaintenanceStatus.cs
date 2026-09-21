namespace FleetManagementSystem.Domain.Enums;

public enum MaintenanceStatus
{
    Scheduled = 1,    // مجدولة
    InProgress = 2,   // قيد التنفيذ
    Completed = 3,    // مكتملة
    Cancelled = 4     // ملغية
}
