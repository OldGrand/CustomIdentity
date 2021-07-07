using CustomIdentity.DAL.Entities;

namespace CustomIdentity.BLL.Services.Interfaces
{
    public interface ITokenGeneratorService
    {
        public string CreateJwtToken(User userModel);
    }
}