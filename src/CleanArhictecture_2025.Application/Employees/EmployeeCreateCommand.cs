using CleanArhictecture_2025.Domain.Employees;
using FluentValidation;
using GenericRepository;
using Mapster;
using MediatR;
using TS.Result;

namespace CleanArhictecture_2025.Application.Employees;

public sealed record EmployeeCreateCommand(
    string FirstName,
    string LastName,
    DateOnly BirthOfDate,
    decimal Salary,
    PersonelInformation PersonelInformation,
    Address? Address,
    bool IsActive) : IRequest<Result<string>>;

public sealed class EmployeeCreateCommandValidator : AbstractValidator<EmployeeCreateCommand>
{
    public EmployeeCreateCommandValidator()
    {
        RuleFor(x => x.FirstName).MinimumLength(3).WithMessage("Ad en az 3 karakter olmalıdır.");
        RuleFor(x => x.LastName).MinimumLength(3).WithMessage("Soyad en az 3 karakter olmalıdır.");
        RuleFor(x => x.PersonelInformation.TCNo)
            .MinimumLength(11).WithMessage("Geçerli bir TC numarası giriniz.")
            .MaximumLength(11).WithMessage("Geçerli bir TC numarası giriniz.");
    }
}

internal sealed class EmployeeCreateCommandHandler(
    IEmployeeRepository employeeRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<EmployeeCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(EmployeeCreateCommand request, CancellationToken cancellationToken)
    {
        if (request.PersonelInformation is null)
            return Result<string>.Failure("Personel bilgileri boş olamaz.");

        var isEmployeeExists = await employeeRepository.AnyAsync(p => p.PersonelInformation.TCNo == request.PersonelInformation.TCNo, cancellationToken);

        if (isEmployeeExists)
        {
            return Result<string>.Failure("Bu TC numarası daha önce kaydedilmiştir.");
        }

        Employee employee = request.Adapt<Employee>();//oto requestten gelenleri employee dönüştür
        employeeRepository.Add(employee);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return "Personel kaydı başarıyla tamamlandı.";
    }
}



//internal aynı projede kullanmanı sağlıyor. 
//otomatik nesne dönüştürmek için Mapster kütüphanesini kullanıyoruz. kopya.Adapt<nereye> yapılıyor
//kodu yaz -> refactor yap, tekrar kodu yaz -> refactor yap BEST PRACTICE
//BEST PRACTICE: Handler internal olmalı, valiadasyon public yoksa çalışmaz, requestin diğer katmanlar tarafından erişebilinmesi lazım public olmalı,handler başka bir katmandan erişilemez
