using CleanArhictecture_2025.Domain.Abstractions;

namespace CleanArhictecture_2025.Domain.Employees
{
    public sealed class Employee : Entity
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string FullName => string.Join(" ", FirstName, LastName);
        public DateOnly BirthOfDate { get; set; }
        public decimal Salary { get; set; }
        public PersonelInformation PersonelInformation { get; set; } = default!;
        public Address Address { get; set; } = default!;
    }


}
// burada default! bu property kesin dolacak sen karışma , sen uyarı verme bana.
// burada default! kullandık çünkü bu propertylerin kesinlikle dolması lazım. eğer ben doldurmazsam hata verir
// burada newlediğim zaman fullname diye bir data getir ancak bu veritabanımda yer almasın. sadece kod tarafında oluşsun.
// decimal hassas ondalık
// addressteki recorda normalde () içine görünmez property şeklinde yazacaktık ama sonra bunların değerlerinin atanması gerektirdi o zaman, bunun yerine biz görünür property şeklinde yazdık 
