
namespace Application.DTOS
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

       public void ConvertToDTO(User user)
        {
            if (user == null)
                throw new NullReferenceException();

            Id = user.Id;
            Name = user.Name;
            Email = user.Email;
        }
        public void ConvertFromDTO(User user)
        {
            if (user == null)
                throw new NullReferenceException();

             user.Id =  Id;
             user.Name = Name;
             user.Email = Email;
        }
        public User CreateTaskFromDTO()
        {
            User user = new User();
            user.Id = Id;
            user.Name = Name;
            user.Email = Email;
            return user;
        }
    }
}
