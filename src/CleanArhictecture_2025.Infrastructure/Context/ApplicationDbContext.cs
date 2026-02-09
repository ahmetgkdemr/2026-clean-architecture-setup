using CleanArhictecture_2025.Domain.Abstractions;
using CleanArhictecture_2025.Domain.Employees;
using GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace CleanArhictecture_2025.Infrastructure.Context;

internal class ApplicationDbContext : DbContext, IUnitOfWork
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }
    public DbSet<Employee> Employees { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<Entity>();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property(p => p.CreateAt)
                    .CurrentValue = DateTimeOffset.UtcNow;
            }

            if (entry.State == EntityState.Modified)
            {
                if (entry.Property(p => p.IsDeleted).CurrentValue == true)
                {
                    entry.Property(p => p.DeleteAt).CurrentValue = DateTimeOffset.UtcNow;
                }
                else
                {
                    entry.Property(p => p.UpdateAt)
                    .CurrentValue = DateTimeOffset.UtcNow;
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