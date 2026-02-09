namespace CleanArhictecture_2025.Domain.Employees
{
    public sealed record PersonelInformation
    {
        public string TCNo { get; set; } = default!;
        public string? Email { get; set; }
        public string? Phone1 { get; set; }
        public string? Phone2 { get; set; }
    }


}
// burada default! bu property kesin dolacak sen karışma , sen uyarı verme bana.
// burada newlediğim zaman fullname diye bir data getir ancak bu veritabanımda yer almasın. sadece kod tarafında oluşsun.
// decimal hassas ondalık
//addressteki recorda normalde () içine görünmez property şeklinde yazacaktık ama sonra bunların değerlerinin atanması gerektirdi o zaman, bunun yerine biz görünür property şeklinde yazdık 