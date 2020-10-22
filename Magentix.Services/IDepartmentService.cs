using System.Collections.Generic;
using Magentix.Domain.Models.Tickets;

namespace Magentix.Services
{
    public interface IDepartmentService
    {
        Department GetDepartment(int id);

        Department GetDepartmentByName(string departmentName);

        int GetDepartmentIdByName(string departmentName);

        string GetDepartmentNameById(int departmentId);

        IEnumerable<Department> GetDepartments();

        void ResetCache();

        void UpdatePriceTag(string departmentName, string priceTag);
    }
}
