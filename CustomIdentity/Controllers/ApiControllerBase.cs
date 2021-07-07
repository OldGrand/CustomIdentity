using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace CustomIdentity.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public abstract class ApiControllerBase : ControllerBase
    {
        protected readonly IMapper Mapper;

        protected ApiControllerBase(IMapper mapper)
        {
            Mapper = mapper;
        }
    }
}
