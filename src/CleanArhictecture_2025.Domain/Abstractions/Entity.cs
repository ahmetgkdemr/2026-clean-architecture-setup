using CleanArhictecture_2025.Domain.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArhictecture_2025.Domain.Abstractions
{
    public abstract class Entity
    {
        public Entity()
        {
            Id = Guid.CreateVersion7();
        }
        public Guid Id { get; set; }

        #region Autdit Log
        public bool IsActive { get; set; } = true;
        public DateTimeOffset CreateAt { get; set; }
        public Guid CreateUserId { get; set; } = default!;
        public string CreateUserName => GetCreateUserName();
        public DateTimeOffset? UpdateAt { get; set; }
        public Guid? UpdateUserId { get; set; }
        public string? UpdateUserName => GetUpdateUserName();
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeleteAt { get; set; }
        public Guid? DeleteUserId { get; set; }

        private string GetCreateUserName()
        {
            HttpContextAccessor httpContextAccessor = new();
            var userManager = httpContextAccessor
                .HttpContext
                .RequestServices
                .GetRequiredService<UserManager<AppUser>>();

            AppUser appUser = userManager.Users.First(p => p.Id == CreateUserId);
            return appUser.FullName + " (" + appUser.Email + ")";
        }
        private string? GetUpdateUserName()
        {
            if (UpdateUserId is null)
                return null;

            HttpContextAccessor httpContextAccessor = new();
            var userManager = httpContextAccessor
                .HttpContext
                .RequestServices
                .GetRequiredService<UserManager<AppUser>>();

            AppUser appUser = userManager.Users.First(p => p.Id == UpdateUserId);
            return appUser.FullName + " (" + appUser.Email + ")";

        }

        #endregion

    }
}


// burada neden datetimeoffset kullanma sebebimiz farklı bölgelerde kullanılan zaman dilimlerinin bilgisini de saklamak için kullanıyoruz.

// önceden new Guid(); kullanılıyordu artık Guid.CreateVersion7() kullanılıyor. Bu yeni yaklaşımın farkı artık sıralanabilir olmakta ürettiğimi guidler.

// burada yazdigimiz getcreateusername bizim getqueryhandlerda bir işlemin kim tarafından yapıldığını göstermek için kullanıyoruz. Ben bunun işlemini getallqueryhandler da yaptığım için burada sadece getqueryhandler için bir metod oluşturuyoruz gibi düşünülebilir.