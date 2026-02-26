using CleanArhictecture_2025.Domain.Abstractions;
using CleanArhictecture_2025.Domain.Employees;
using CleanArhictecture_2025.Domain.Users;
using GenericRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CleanArhictecture_2025.Infrastructure.Context;

internal class ApplicationDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>, IUnitOfWork
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }
    public DbSet<Employee> Employees { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        modelBuilder.Ignore<IdentityUserToken<Guid>>();
        modelBuilder.Ignore<IdentityUserClaim<Guid>>();
        modelBuilder.Ignore<IdentityRoleClaim<Guid>>();
        modelBuilder.Ignore<IdentityUserLogin<Guid>>();
        modelBuilder.Ignore<IdentityUserRole<Guid>>();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<Entity>();

        HttpContextAccessor httpContextAccessor = new();
        string userIdString = httpContextAccessor.HttpContext!.User.Claims.First(p => p.Type == "user-id").Value;
        Guid userId = Guid.Parse(userIdString);

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property(p => p.CreateAt)
                    .CurrentValue = DateTimeOffset.UtcNow;
                entry.Property(p => p.CreateUserId)
                    .CurrentValue = userId;
            }

            if (entry.State == EntityState.Modified)
            {
                if (entry.Property(p => p.IsDeleted).CurrentValue == true)
                {
                    entry.Property(p => p.DeleteAt).CurrentValue = DateTimeOffset.UtcNow;
                    entry.Property(p => p.DeleteUserId).CurrentValue = userId;
                }
                else
                {
                    entry.Property(p => p.UpdateAt)
                    .CurrentValue = DateTimeOffset.UtcNow;
                    entry.Property(p => p.UpdateUserId).CurrentValue = userId;
                }
            }

            if (entry.State == EntityState.Deleted)
            {
                throw new ArgumentException("Db'den direkt silme işlemi yapamazsınız.");
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

}

// sen internalsın çünkü seni başka katmanın bilmesine gerek yok 
// IUnitOfWork eklememizin sebebi: artık her repository için ayrı ayrı savechanges yapmayacağız, tek bir savechanges yaparak tüm işlemlerimizi kaydedebileceğiz. Bu sayede transaction yönetimi de yapmış oluyoruz.

// burada overrideonmodelcreating yapmamızın sebebi: domaindeki employee içeriisdenki personelinf ve address i nasıl veritabanına yazması gerektiği hakkında maplama yapmam gerekiyor. infrastructure içerisinde configurations dosyasında neden yaptığımızı detaylı olarak açıkladım

// burada saveChangesAsync override ediyoruz çünkü veritabanı işlemi yaptıktan sonra örnek ekleme güncelleme silme tam kaydederken otomatik createAt updateAt deleteAt gibi kolonların doldurulmasını sağlamak istiyorum. Bu sayede örn artık employee eklerken oraya createAt=utc.now yazmama gerek kalmıyor. Sonrasında entrystate işleminde eğer birisi delete işlemi yaparsa orada hata veriyoruz yapmasını engelliyoruz "hard delete". bunun yerine "soft delete" kullanıyoruz. IsDeleted=true ise bu kayıt aslında silinmiş oluyor yalnız veri tabanımızda kalmaya devam ediyor ismi sadece true. Yani ben bu veriyi yarın false yaparak geri getirebilirim demek oluyor.

// burada Ignore<IdentityUserToken>Token tablosu oluşturma
// Ignore<IdentityUserClaim>User claim tablosu oluşturma
// Ignore<IdentityRoleClaim>Role claim tablosu oluşturma
// Ignore<IdentityUserLogin>External login tablosu oluşturma
// Ignore<IdentityUserRole>User-Role ilişki tablosu db oluştururken tablo olarak ekeleme bunları

// şimdi burada savechange içerisine şunu eklenen güncellenen ve silinen işlemlerin hangi user tarafından gerçekleştirileceğini otomatik veri tabanına kaydetmek için bunu yaptık. Httpcontextaccessor eklememizin sebebi bu işlemlerin her zaman istek ile yapılacağı ve toke gerekli tabiki. Token üretirken içerisine claim olara ilgili id değerini verdik. Örn istek geldi. Httpcontextaccessor ile yakalandı ve claiminden hangi user eklemiş gördük ve veri tabanına kaydettik. Bu savechange metodu userManager işlemleri haric employee veya diğerleridir hepsinde çalışır. Userekleme ve user manager kullanımı savechange metodunu çalıştırmaz.