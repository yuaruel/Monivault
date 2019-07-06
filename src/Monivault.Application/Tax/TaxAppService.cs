using Abp.Domain.Repositories;
using Monivault.AppModels;
using Monivault.Tax.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Monivault.Tax
{
    public class TaxAppService : MonivaultAppServiceBase, ITaxAppService
    {
        private readonly IRepository<TaxProfile, long> _taxProfileRepository;
        private readonly IRepository<AccountHolder> _accountHolderRepository;
        private readonly IRepository<TaxType> _taxTypeRepository;
        private readonly IRepository<TaxPayment, long> _taxPaymentRepository;

        public TaxAppService(
                IRepository<TaxProfile, long> taxProfileRepository,
                IRepository<AccountHolder> accountHolderRepository,
                IRepository<TaxType> taxTypeRepository,
                IRepository<TaxPayment, long> taxPaymentRepository
            )
        {
            _taxProfileRepository = taxProfileRepository;
            _accountHolderRepository = accountHolderRepository;
            _taxTypeRepository = taxTypeRepository;
            _taxPaymentRepository = taxPaymentRepository;
        }

        public async Task<TaxProfileDto> GetTaxProfile()
        {
            var taxProfileDto = new TaxProfileDto();

            var currentUser = await GetCurrentUserAsync();
            try
            {
                var accountHolder = _accountHolderRepository.Single(p => p.UserId == currentUser.Id);

                //Set default full name for tax profile, if the account holder has not updated their profile yet.
                taxProfileDto.FullName = $"{currentUser.Name} {currentUser.Surname}";

                var taxProfile = _taxProfileRepository.Single(p => p.AccountHolderId == accountHolder.Id);

                taxProfileDto.Tin = taxProfile.Tin;
                taxProfileDto.TaxProfileKey = taxProfile.TaxProfileKey.ToString();
                taxProfileDto.ReconcilliationPvNumber = taxProfile.ReconcilliationPvNumber;
                taxProfileDto.FullName = taxProfile.FullName;
                taxProfileDto.Email = taxProfile.Email;
                taxProfileDto.PhoneNumber = taxProfile.PhoneNumber;
            }
            catch(Exception exc)
            {
                Logger.Error(exc.StackTrace);
            }

            return taxProfileDto;

        }

        public List<TaxTypeDto> GetTaxTypes()
        {
            var taxTypes = _taxTypeRepository.GetAllList();

            var taxTypeDtoList = ObjectMapper.Map<List<TaxTypeDto>>(taxTypes);

            return taxTypeDtoList;
        }

        public async Task MakePayment(MakePaymentInput input)
        {
            var currentUser = await GetCurrentUserAsync();
            var accountHolder = _accountHolderRepository.Single(p => p.UserId == currentUser.Id);
            var taxProfile = _taxProfileRepository.Single(p => p.AccountHolderId == accountHolder.Id);

            var taxType = _taxTypeRepository.Single(p => p.Id == input.TaxType);

            var taxPayment = new TaxPayment
            {
                Tin = input.TaxIdentificationNumber,
                FullName = input.FullName,
                ReconcilliationPvNumber = taxProfile.ReconcilliationPvNumber,
                EmailAddress = taxProfile.Email,
                TaxPeriod = input.TaxPeriod,
                TaxTypeId = input.TaxType,
                PhoneNumber = taxProfile.PhoneNumber,
                AccountHolderId = accountHolder.Id,
                Amount = input.Amount
            };

            _taxPaymentRepository.Insert(taxPayment);
        }

        public async Task UpdateTaxProfile(UpdateTaxProfileInput input)
        {
            var currentUser = await GetCurrentUserAsync();

            var accountHolder = _accountHolderRepository.Single(p => p.UserId == currentUser.Id);

            var taxPofile = new TaxProfile
            {
                AccountHolderId = accountHolder.Id,
                Tin = input.Tin,
                FullName = input.FullName,
                ReconcilliationPvNumber = input.ReconcilliationPvNumber,
                PhoneNumber = input.PhoneNumber,
                Email = input.Email
            };

            _taxProfileRepository.Insert(taxPofile);
        }

        
    }
}
