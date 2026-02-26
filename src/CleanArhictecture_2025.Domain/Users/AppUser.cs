using Microsoft.AspNetCore.Identity;

namespace CleanArhictecture_2025.Domain.Users;

public sealed class AppUser : IdentityUser<Guid>
{
    public AppUser()
    {
        Id = Guid.CreateVersion7();
    }
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string FullName => $"{FirstName} {LastName}"; //computed property sadece okuma

    #region Autdit Log
    public DateTimeOffset CreateAt { get; set; }
    public Guid CreateUserId { get; set; } = default!;
    public DateTimeOffset? UpdateAt { get; set; }
    public Guid? UpdateUserId { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeleteAt { get; set; }
    public Guid? DeleteUserId { get; set; }
    #endregion
}


// burada diretk identityuser id sinin nasıl tutulacağını belirtebiliyorum guid. Genelde string kullanılır 

// şimdi normalde bu klasöre bir de User için bir repository yazmamız gerekirdi ancak UserManager bizim tüm bu işlemlerimizi kendisi hallediyor ben yazmayacağım.

// şimdi biz identityde ekledikten sonra burada bir şey eksik o da entity classını inherit ediyorduk normalde burada identityi inherit ettik bu yüzden bizim Auth bilgilerimiz eksik(creatat updateat deleteat gibi) 

// şimdi ben entity classımı da interih edeimiyorum 2 class inherit yok bu yüzden manuel audit log bilgilerimi de ekliyorum buraya