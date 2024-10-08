﻿using Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Shared.RequestFeatures;

namespace Repository;

public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
{
    public EmployeeRepository(RepositoryContext repositoryContext) :
        base(repositoryContext)
    {
    }

    public async Task<IEnumerable<Employee>> GetEmployeesAsync(Guid companyId, EmployeeParameters employeeParameters,
        bool trackChanges) => await 
            FindByCondition(e => e.CompanyId == companyId, trackChanges)
                .OrderBy(e => e.Name)
                .Skip((employeeParameters.PageNumber - 1) * employeeParameters.PageSize)
                .Take(employeeParameters.PageSize)
                .ToListAsync();

    public async Task<Employee> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges) => await 
        FindByCondition(e => e.CompanyId.Equals(companyId) && e.Id.Equals(id), trackChanges)
            .SingleOrDefaultAsync();

    public void CreateEmployeeForCompany(Guid companyId, Employee employee)
    {
        employee.CompanyId = companyId;
        Create(employee);
    }

    public void DeleteEmployee(Employee employee) => Delete(employee);

}