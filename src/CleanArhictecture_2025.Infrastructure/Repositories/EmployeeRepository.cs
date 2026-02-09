using CleanArhictecture_2025.Domain.Employees;
using CleanArhictecture_2025.Infrastructure.Context;
using GenericRepository;

namespace CleanArhictecture_2025.Infrastructure.Repositories;
internal sealed class EmployeeRepository : Repository<Employee, ApplicationDbContext>, IEmployeeRepository
{
    public EmployeeRepository(ApplicationDbContext context) : base(context)
    {

    }
}

// Repository ve servis arasındaki fark şudur: Biz repositoryde ana işlemleri yap db de kaydet sil getir,biz servislerde ise bir çok farklı işlem yaparız sonra içine repositorydeki işlemleri de dahil ederiz