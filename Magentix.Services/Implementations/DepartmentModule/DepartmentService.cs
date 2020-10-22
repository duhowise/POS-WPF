using Magentix.Domain.Models.Tickets;
using Magentix.Infrastructure.Data;
using Magentix.Persistance;
using Magentix.Persistance.Data;
using Magentix.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Magentix.Services.Implementations.DepartmentModule
{
    [Export(typeof(IDepartmentService))]
    public class DepartmentService : IDepartmentService
    {
        private readonly ICacheDao _cacheDao;

        private IEnumerable<Department> _departments;

        public IEnumerable<Department> Departments
        {
            get
            {
                IEnumerable<Department> departments = this._departments;
                if (departments == null)
                {
                    IEnumerable<Department> departments1 = this._cacheDao.GetDepartments();
                    IEnumerable<Department> departments2 = departments1;
                    this._departments = departments1;
                    departments = departments2;
                }
                return departments;
            }
        }

        [ImportingConstructor]
        public DepartmentService(ICacheDao cacheDao)
        {
            this._cacheDao = cacheDao;
        }

        public Department GetDepartment(int id)
        {
            if (id == 0 || this.Departments.All<Department>((Department x) => x.Id != id))
            {
                return null;
            }
            return this.Departments.First<Department>((Department x) => x.Id == id);
        }

        public Department GetDepartmentByName(string departmentName)
        {
            return this.Departments.FirstOrDefault<Department>((Department x) => x.Name == departmentName);
        }

        public int GetDepartmentIdByName(string departmentName)
        {
            Department department = this.Departments.FirstOrDefault<Department>((Department x) => x.Name == departmentName);
            if (department == null)
            {
                return 0;
            }
            return department.Id;
        }

        public string GetDepartmentNameById(int departmentId)
        {
            Department department = this.Departments.FirstOrDefault<Department>((Department x) => x.Id == departmentId);
            if (department == null)
            {
                return "";
            }
            return department.Name;
        }

        public IEnumerable<Department> GetDepartments()
        {
            return this.Departments;
        }

        public void ResetCache()
        {
            this._departments = null;
        }

        public void UpdatePriceTag(string departmentName, string priceTag)
        {
            using (IWorkspace workspace = WorkspaceFactory.Create())
            {
                Department department = workspace.Single<Department>((Department y) => y.Name == departmentName, new Expression<Func<Department, object>>[0]);
                if (department != null)
                {
                    department.PriceTag = priceTag;
                    workspace.CommitChanges();
                }
            }
        }
    }
}
