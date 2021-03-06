﻿using Sindikat.Identity.Application.Dtos;
using Sindikat.Identity.Application.Interfaces;
using Sindikat.Identity.Application.Validators;
using Sindikat.Identity.Common.Exceptions;
using Sindikat.Identity.Domain.Entities;
using System.Threading.Tasks;

namespace Sindikat.Identity.Application.Services
{
    public class ClaimValidatorService : BaseValidatorService, IClaimValidatorService
    {
        private readonly IBaseRepository<Claim> _repo;

        public ClaimValidatorService(IBaseRepository<Claim> repo)
        {
            _repo = repo;
        }

        public async Task ValidateBeforeSave(ClaimForSaveDto claimForSave)
        {
            var validator = new ClaimForSaveDtoValidator();

            CheckValidationResults(validator.Validate(claimForSave));

            var existingClaim = await _repo.FirstOrDefaultAsync(x => x.Name == claimForSave.Name);

            if (existingClaim != null)
                ThrowValidationError("Claim", $"Claim {claimForSave.Name} already exists!");
        }

        public async Task ValidateBeforeUpdate(int claimId, ClaimForSaveDto claimForSave)
        {
            var validator = new ClaimForSaveDtoValidator();

            CheckValidationResults(validator.Validate(claimForSave));

            var existingClaim = await _repo.FindAsync(claimId);

            if (existingClaim == null)
                throw new NotFoundException();

            var claimWithSameName = await _repo.FirstOrDefaultAsync(x => x.Name == claimForSave.Name);

            if (claimWithSameName != null && claimWithSameName.Id != existingClaim.Id)
                ThrowValidationError("Claim", $"Claim {claimWithSameName.Name} already exists!");
        }

        public async Task ValidateBeforeDelete(int claimId)
        {
            var claim = await _repo.FindAsync(claimId);

            if (claim == null)
                throw new NotFoundException();
        }

    }
}
