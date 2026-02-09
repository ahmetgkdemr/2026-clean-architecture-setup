namespace CleanArhictecture_2025.Domain.Abstractions
{
    public abstract class Entity
    {
        public Entity()
        {
            Id = new Guid();
        }
        public Guid Id { get; set; }
        public DateTimeOffset CreateAt { get; set; }
        public DateTimeOffset? UpdateAt { get; set; }
        public bool IsDeleted { get; set; }
        public DateTimeOffset? DeleteAt { get; set; }
    }
}


// burada neden datetimeoffset kullanma sebebimiz farklı bölgelerde kullanılan zaman dilimlerinin bilgisini de saklamak için kullanıyoruz.