using CleanArhictecture_2025.Domain.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArhictecture_2025.Infrastructure.Configurations;
internal sealed class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.OwnsOne(p => p.PersonelInformation, builder =>
        {
            builder.Property(i => i.TCNo).HasColumnName("TCNO");
            builder.Property(i => i.Phone1).HasColumnName("Phone1");
            builder.Property(i => i.Phone2).HasColumnName("Phone2");
            builder.Property(i => i.Email).HasColumnName("Email");
        });

        builder.OwnsOne(p => p.Address, builder =>
        {
            builder.Property(i => i.Country).HasColumnName("Country");
            builder.Property(i => i.City).HasColumnName("City");
            builder.Property(i => i.Town).HasColumnName("Town");
            builder.Property(i => i.FullAdress).HasColumnName("FullAdress");
        });


        builder.Property(p => p.Salary).HasPrecision(18, 2);// burada money için böyle yapıyoruz
    }
}



// burada bunu yapmamızı sebebi şu: biz domain içerisinde employee sınıfı içerisinde propertylerimizi yazarken içerisine karışık durmaması adına PersonelInformation adında bir recort(valueobject) oluşturduk ve onun içerisinde personel bilgilerini tuttuk. Ama veritabanında bu şekilde durmaz, o yüzden burada mapleme yaparak veritabanında nasıl duracağını söylüyoruz. ownsone dediğimizde de bu bir owned entitydir yani sahip olunan bir varlıktır, tek başına var olmaz, employee ile birlikte var olur diyoruz. builder.Property(i => i.TCNo).HasColumnName("TCNO"); burada da veritabanında kolon adının nasıl olacağını söylüyoruz.

// eğer bu configuration işlemini yapmasaydın orm bu bilgileri ayrı bir tablo olarak yazmaya çalışır ve sonunda hata alırız budaki mapleme bu yüzden işe yarıyor.

// yani özü şu aslında biz bir class yazdık employee adında sonra başka bir classı da burada property olarak gösterdik , ama biz bu classın ayrı bir tablo olarak değil de employee tablosunun içerisinde kolonlar olarak durmasını istiyoruz, o yüzden burada mapleme yaparak bunu sağlıyoruz.