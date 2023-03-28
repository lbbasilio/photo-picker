#pragma warning disable CS8618 // Suppress "Non-nullable field must contain a non-null value when exiting constructor." warning
namespace PhotoPicker.Entities
{
    public class User
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }
        public DateTime LastLogin { get; set; }
        public UserRole Role { get; set; }
    }

    public enum UserRole
    {
        User,
        Photographer
    }
}
#pragma warning restore CS8618