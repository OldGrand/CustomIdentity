using System.Threading.Tasks;
using CustomIdentity.BLL.Models.Permissions;

namespace CustomIdentity.BLL.Services.Interfaces
{
    public interface IClaimService
    {
        Task AddOrUpdateUserClaimsAsync(UserClaimAssociativesUpdateModel model);
        Task CreateClaimAsync(UserClaimCreateModel model);
    }
}