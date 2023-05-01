using localsound.backend.Domain.Enum;

namespace localsound.backend.Domain.Model.Interfaces
{
    public interface IAppUserDto
    {
        public CustomerType CustomerType { get; set; }
        //public bool EmailConfirmed { get; set; }
    }
}
