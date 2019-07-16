using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Monivault.AppModels;
using Monivault.TransactionLogs.Dto;
using Microsoft.EntityFrameworkCore;
using System.IO;
using OfficeOpenXml;
using System.Globalization;

namespace Monivault.TransactionLogs
{
    public class TransactionLogAppService : MonivaultAppServiceBase, ITransactionLogAppService
    {
        private readonly IRepository<TransactionLog, long> _transactionRepository;
        private readonly IRepository<AccountHolder> _accountHolderRepository;
        private readonly string TransactionTempFolderName = "TransactionHistory_Temp";

        public TransactionLogAppService(
                IRepository<TransactionLog, long> transactionRepository,
                IRepository<AccountHolder> accountHolderRepository
            )
        {
            _transactionRepository = transactionRepository;
            _accountHolderRepository = accountHolderRepository;
        }
        
        public async Task<List<RecentTransactionDto>> GetRecentTransactions(LimitedResultRequestDto requestDto)
        {
            var user = await GetCurrentUserAsync();
            var accountHolder = _accountHolderRepository.Single(p => p.UserId == user.Id);

            var transactionLogs = _transactionRepository.GetAllList(p => p.AccountHolderId == accountHolder.Id).OrderByDescending(p => p.CreationTime)
                                    .Take(requestDto.MaxResultCount).ToList();
            return ObjectMapper.Map<List<RecentTransactionDto>>(transactionLogs);
        }

        public List<ProfileViewTransactionDto> GetTransactionsForProfileView(string accountHolderKey)
        {
            var ahKey = Guid.Parse(accountHolderKey);
            var accountHolder = _accountHolderRepository.Query(qm => qm.Where(p => p.AccountHolderKey == ahKey).Include(p => p.User)
                                                            .Single(p => p.AccountHolderKey == ahKey));

            var transactionLogs = _transactionRepository.GetAllList(p => p.AccountHolderId == accountHolder.Id).OrderByDescending(p => p.CreationTime);

            return ObjectMapper.Map<List<ProfileViewTransactionDto>>(transactionLogs);
        }

        public (decimal totalCredit, decimal totalDebit) GetTotalCreditAndDebit()
        {
            var totalCredit = 0m;
            var totalDebit = 0m;

            var creditTransactions = _transactionRepository.GetAllList(p => p.CreationTime.Year == DateTime.UtcNow.Year && p.TransactionType == TransactionLog.TransactionTypes.Credit);
            foreach (var creditTransaction in creditTransactions)
            {
                totalCredit += creditTransaction.Amount;
            }

            var debitTransactions = _transactionRepository.GetAllList(p => p.CreationTime.Year == DateTime.UtcNow.Year && p.TransactionType == TransactionLog.TransactionTypes.Debit);

            foreach (var debitTransaction in debitTransactions)
            {
                totalDebit += debitTransaction.Amount;
            }

            return (totalCredit, totalDebit);
        }

        public void LogTransaction()
        {
            throw new System.NotImplementedException();
        }

        public async Task<FileInfo> CreateRecentTransactionsHistory()
        {
            var currentUser = await GetCurrentUserAsync();
            var accountHolder = _accountHolderRepository.FirstOrDefault(p => p.UserId == currentUser.Id);

            FileInfo outputFile = null;

            if(accountHolder != null)
            {
                var transactionLogs = _transactionRepository.GetAllList(p => p.AccountHolderId == accountHolder.Id);

                var worksheetRow = 2;
                var templateFileName = "TransactionHistory_Template.xlsx";

                var templateFile = Path.Combine("wwwroot" + Path.DirectorySeparatorChar + TransactionTempFolderName, templateFileName);

                var currentTransactionFileName = templateFileName.Replace("Template", DateTime.UtcNow.ToFileTimeUtc().ToString());
                var currentTransactionFile = Path.Combine("wwwroot" + Path.DirectorySeparatorChar +  TransactionTempFolderName, currentTransactionFileName);

                var currentTransactionFileInfo = new FileInfo(currentTransactionFile);

                File.Copy(templateFile, currentTransactionFile);

                using (ExcelPackage excelFile = new ExcelPackage(currentTransactionFileInfo))
                {
                    var worksheet = excelFile.Workbook.Worksheets["Transactions"];

                    foreach (var transactionLog in transactionLogs)
                    {
                        worksheet.Cells[worksheetRow, 1].Value = transactionLog.Amount.ToString("C2", new CultureInfo("ig-NG"));
                        worksheet.Cells[worksheetRow, 2].Value = transactionLog.TransactionType;
                        worksheet.Cells[worksheetRow, 3].Value = transactionLog.Description;
                        worksheet.Cells[worksheetRow++, 4].Value = transactionLog.CreationTime.ToShortDateString();
                    }

                    excelFile.Save();
                    outputFile = excelFile.File;
                }

            }

            return outputFile;
        }
    }
}