using CleanArhictecture_2025.Domain.Abstractions;
using CleanArhictecture_2025.Domain.Employees;
using CleanArhictecture_2025.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArhictecture_2025.Application.Employees;

public sealed record EmployeeGetAllQuery() : IRequest<IQueryable<EmployeeGetAllQueryResponse>>;

public sealed class EmployeeGetAllQueryResponse : EntityDto
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public DateOnly BirthOfDate { get; set; }
    public decimal Salary { get; set; }
    public string TCNo { get; set; } = default!;
}

internal sealed class EmployeeGetAllQueryHandler(
    IEmployeeRepository employeeRepository,
    UserManager<AppUser> userManager) : IRequestHandler<EmployeeGetAllQuery, IQueryable<EmployeeGetAllQueryResponse>>
{
    public Task<IQueryable<EmployeeGetAllQueryResponse>> Handle(EmployeeGetAllQuery request, CancellationToken cancellationToken)
    {
        var response = (from employee in employeeRepository.GetAll()
                        join create_user in userManager.Users.AsQueryable() on employee.CreateUserId equals create_user.Id
                        join update_user in userManager.Users.AsQueryable() on employee.UpdateUserId equals update_user.Id into update_user
                        from update_users in update_user.DefaultIfEmpty()
                        select new EmployeeGetAllQueryResponse
                        {
                            FirstName = employee.FirstName,
                            LastName = employee.LastName,
                            BirthOfDate = employee.BirthOfDate,
                            CreateAt = employee.CreateAt,
                            Id = employee.Id,
                            Salary = employee.Salary,
                            TCNo = employee.PersonelInformation.TCNo,
                            DeleteAt = employee.DeleteAt,
                            IsDeleted = employee.IsDeleted,
                            UpdateAt = employee.UpdateAt,
                            CreateUserId = employee.CreateUserId,
                            CreateUserName = create_user.FullName + " (" + create_user.Email + ")",
                            UpdateUserId = employee.UpdateUserId,
                            UpdateUserName = employee.UpdateUserId == null ? null : update_users.FullName + " (" + update_users.Email + ")",
                        });

        return Task.FromResult(response);
    }
}



//listeyi tamamen çekmeyeceğiz IQueryable olarak döndüreceğiz listeyi tamamen çekmeyeceğiz, bizim OData yapısından güç alacağız. OData yapısı dönen IQueryablı sorgularken geniş sorgular atabilmemizi ve bunu sorgu esnasında yapabilmemizi sağlıyor. Güvenlik açığı olabilecek metodlarla yapmıyoruz ama personel listesi gibi yapılarda bunu yapabiliriz

//biz create commandda result olarak döndürmüştük cqrs de, burada ise response oluşturucaz sebebi ise : value object ve smart enum gibi yapılar OData da çekilirken sıkıntı çıkabiliyor

// mediatr patern zorunluluğu async yapmak task yapıyoruz o yüzden

// burada select içinde neden mapleme işlemi yapamadık çünkü biz veritabanında yapılıyor o işlem memoryde değil bu yüzden 300 tane property olsa hepsini tek tek yazmamız lazım