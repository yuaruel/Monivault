using Abp.Application.Services;
using Abp.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Monivault.AccountHolders.Dto;
using Monivault.AppModels;
using Monivault.Banks.Dto;
using Monivault.Exceptions;
using Monivault.Users;
using Monivault.Users.Dto;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Monivault.AccountHolders
{
    [RemoteService(false)]
    public class AccountHolderAppService : MonivaultAppServiceBase, IAccountHolderAppService
    {
        private readonly IRepository<AccountHolder> _accountHolderRepository;
        private readonly IRepository<Bank> _bankRepository;
        private readonly IRepository<SavingsInterest, long> _savingInterestRepository;
        private readonly IUserAppService _userAppService;

        public AccountHolderAppService(
                IRepository<AccountHolder> accountHolderRepository,
                IRepository<Bank> bankRepository,
                IRepository<SavingsInterest, long> savingInterestRepository,
                IUserAppService userAppService
            )
        {
            _accountHolderRepository = accountHolderRepository;
            _bankRepository = bankRepository;
            _savingInterestRepository = savingInterestRepository;
            _userAppService = userAppService;
        }

        public BalanceDto GetAccountHolderBalance()
        {
            var accountHolder = _accountHolderRepository.Single(p => p.UserId == AbpSession.UserId);

            var balanceDto = new BalanceDto
            {
                AvailableBalance = accountHolder.AvailableBalance,
                LedgerBalance = accountHolder.LedgerBalance
            };

            return balanceDto;
        }

        public async Task<AccountHolderDto> GetAccountHolderDetail()
        {
            var user = await GetCurrentUserAsync();
            var accountHolder = _accountHolderRepository.FirstOrDefault(p => p.UserId == user.Id);

            if (accountHolder == null)
            {
                return null;
            }

            var accountHolderDto = ObjectMapper.Map<AccountHolderDto>(accountHolder);
            if (accountHolder.BankId.HasValue)
            {
                var bank = _bankRepository.Single(p => p.Id == accountHolder.BankId);
                var bankDto = ObjectMapper.Map<BankDto>(bank);

                accountHolderDto.Bank = bankDto;
            }

            return accountHolderDto;
        }

        public AccountHolderDto GetAccountHolderDetailByUserId(long userId)
        {
            var accountHolder = _accountHolderRepository.FirstOrDefault(p => p.UserId == userId);

            if (accountHolder == null)
            {
                return null;
            }

            var accountHolderDto = ObjectMapper.Map<AccountHolderDto>(accountHolder);
            if (accountHolder.BankId.HasValue)
            {
                var bank = _bankRepository.Single(p => p.Id == accountHolder.BankId);
                var bankDto = ObjectMapper.Map<BankDto>(bank);

                accountHolderDto.Bank = bankDto;
            }

            return accountHolderDto;
        }

        public List<AccountHolderListDto> GetAccountHolderList()
        {
            var accountHolders = _accountHolderRepository.Query(qm => qm.Include(ac => ac.User)).ToList();
            var accountHolderList = new List<AccountHolderListDto>();

            foreach(var accountHolder in accountHolders)
            {
                var user = accountHolder.User;

                var accountHolderListDto = new AccountHolderListDto
                {
                    AccountHolderKey = accountHolder.AccountHolderKey,
                    AccountIdentity = accountHolder.AccountIdentity,
                    FirstName = user.Name,
                    LastName = user.Surname,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.RealEmailAddress
                };

                accountHolderList.Add(accountHolderListDto);
            }

            return accountHolderList;
        }

        public decimal GetInterestAccrued()
        {
            var accruedInterest = 0.0m;
            try
            {
                var accountHolder = _accountHolderRepository.Single(p => p.UserId == AbpSession.UserId);
                var savingsInterest = _savingInterestRepository.FirstOrDefault(p => p.Status == SavingsInterest.StatusTypes.Running && p.AccountHolderId == accountHolder.Id);

                if(savingsInterest != null)
                {
                    accruedInterest = savingsInterest.InterestAccrued;
                }
            }
            catch(InvalidOperationException ioExc)
            {
                Logger.Error($"Interest accrued exception: {ioExc.StackTrace}");
            }

            return accruedInterest;
        }

        public async Task<decimal> GetInterestReceivedForCurrentYear()
        {
            var interestRecieved = 0.0m;

            try
            {
                var user = await GetCurrentUserAsync();
                var accountHolder = _accountHolderRepository.Single(p => p.UserId == user.Id);
                var savingsInterest = _savingInterestRepository.FirstOrDefault(p => p.Status == SavingsInterest.StatusTypes.Completed && p.AccountHolderId == accountHolder.Id);

                if (savingsInterest != null)
                {
                    interestRecieved = savingsInterest.InterestAccrued;
                }
            }
            catch(InvalidOperationException ioExc)
            {
                Logger.Error($"Interest accrued exception: {ioExc.StackTrace}");
            }

            return interestRecieved;
        }

        public int GetTotalNumberOfAccountHolders()
        {
            return _accountHolderRepository.Count();
        }

        /// <summary>
        /// Checks if the account holder available balance is more than the amount required for the transaction.
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public async Task<bool> IsAvailableBalanceEnough(decimal amount)
        {
            var user = await GetCurrentUserAsync();
            var accountHolder = await _accountHolderRepository.SingleAsync(p => p.UserId == user.Id);

            if (accountHolder.AvailableBalance <= amount) throw new InsufficientBalanceException();

            return true;
        }

        public async Task UpdateBankDetails(string bankKey, string accountNumber, string accountName)
        {
            var bankGuidKey = Guid.Parse(bankKey);
            var bank = _bankRepository.Single(p => p.BankKey == bankGuidKey);

            var user = await GetCurrentUserAsync();
            var accountHolder = _accountHolderRepository.Single(p => p.UserId == user.Id);

            accountHolder.Bank = bank;
            accountHolder.BankAccountNumber = accountNumber;
            accountHolder.BankAccountName = accountName;
        }

        public async Task UploadAccountHolders(IFormFile uploadFile)
        {
            using (var fileStream = new MemoryStream())
            {
                await uploadFile.CopyToAsync(fileStream).ConfigureAwait(false);

                using (var package = new ExcelPackage(fileStream))
                {
                    var worksheet = package.Workbook.Worksheets[0];

                    var rowCount = worksheet.Dimension?.Rows;
                    var cellCount = worksheet.Dimension?.Columns;

                    for(int row = 2; row <= rowCount.Value; row++)
                    {
                        try
                        {
                            var userDto = new CreateUserDto
                            {
                                Name = worksheet.Cells[row, 1].Value.ToString(),
                                Surname = worksheet.Cells[row, 2].Value.ToString(),
                                UserName = worksheet.Cells[row, 3].Value.ToString(),
                                PhoneNumber = worksheet.Cells[row, 4].Value.ToString(),
                                EmailAddress = worksheet.Cells[row, 5]?.Value.ToString(),
                                Password = Authorization.Users.User.CreateRandomPassword()
                            };

                            var user = await _userAppService.Create(userDto);

                            var accountHolder = new AccountHolder
                            {
                                UserId = user.Id,
                                AvailableBalance = 0
                            };

                            _accountHolderRepository.Insert(accountHolder);
                        }
                        catch(Exception exc)
                        {
                            Logger.Error($"Error: {exc.StackTrace}");
                        }
                    }
                }
            }
        }
    }
}