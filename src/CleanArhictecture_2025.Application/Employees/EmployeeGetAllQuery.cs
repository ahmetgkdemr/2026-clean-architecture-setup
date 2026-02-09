using CleanArhictecture_2025.Domain.Abstractions;
using CleanArhictecture_2025.Domain.Employees;
using MediatR;
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
    IEmployeeRepository employeeRepository) : IRequestHandler<EmployeeGetAllQuery, IQueryable<EmployeeGetAllQueryResponse>>
{
    public Task<IQueryable<EmployeeGetAllQueryResponse>> Handle(EmployeeGetAllQuery request, CancellationToken cancellationToken)
    {
        var response = employeeRepository.GetAll()
            .Select(e => new EmployeeGetAllQueryResponse
            {
                FirstName = e.FirstName,
                LastName = e.LastName,
                BirthOfDate = e.BirthOfDate,
                CreateAt = e.CreateAt,
                Id = e.Id,
                Salary = e.Salary,
                TCNo = e.PersonelInformation.TCNo,
                DeleteAt = e.DeleteAt,
                IsDeleted = e.IsDeleted,
                UpdateAt = e.UpdateAt
            }).AsQueryable();
        return Task.FromResult(response);
    }
}



//listeyi tamamen çekmeyeceğiz IQueryable olarak döndüreceğiz listeyi tamamen çekmeyeceğiz, bizim OData yapısından güç alacağız. OData yapısı dönen IQueryablı sorgularken geniş sorgular atabilmemizi ve bunu sorgu esnasında yapabilmemizi sağlıyor. Güvenlik açığı olabilecek metodlarla yapmıyoruz ama personel listesi gibi yapılarda bunu yapabiliriz

//biz create commandda result olarak döndürmüştük cqrs de, burada ise response oluşturucaz sebebi ise : value object ve smart enum gibi yapılar OData da çekilirken sıkıntı çıkabiliyor

// mediatr patern zorunluluğu async yapmak task yapıyoruz o yüzden

// burada select içinde neden mapleme işlemi yapamadık çünkü biz veritabanında yapılıyor o işlem memoryde değil bu yüzden 300 tane property olsa hepsini tek tek yazmamız lazım