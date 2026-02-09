namespace CleanArhictecture_2025.Domain.Employees
{
    public sealed record Address
    {
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Town { get; set; }
        public string? FullAdress { get; set; }
    }


}
// burada default! bu property kesin dolacak sen karışma , sen uyarı verme bana.
// burada newlediğim zaman fullname diye bir data getir ancak bu veritabanımda yer almasın. sadece kod tarafında oluşsun.
// decimal hassas ondalık
//addressteki recorda normalde () içine görünmez property şeklinde yazacaktık ama sonra bunların değerlerinin atanması gerektirdi o zaman, bunun yerine biz görünür property şeklinde yazdık 