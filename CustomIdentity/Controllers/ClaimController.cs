using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CustomIdentity.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CustomIdentity.Controllers
{
    public class ClaimController : ApiControllerBase
    {
        private readonly IClaimService _claimService;

        public ClaimController(IMapper mapper,
            IClaimService claimService) : base(mapper)
        {
            _claimService = claimService;
        }

        public IActionResult GetAllClaims()
        {
        }
    }
}
