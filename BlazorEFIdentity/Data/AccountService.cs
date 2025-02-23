using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorEFIdentity.Data;
using BlazorEFIdentity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BlazorEFIdentity.Services
{
    public class BankAccountService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<BankAccountService> _logger;

        public BankAccountService(ApplicationDbContext context, ILogger<BankAccountService> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<List<Account>> GetAccountsAsync()
        {
            try
            {
                return await _context.Accounts.Include(a => a.Transactions).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching accounts");
                throw;
            }
        }
        public async Task<Account?> GetAccountByIdAsync(int accountId)
        {
            try
            {
                return await _context.Accounts
                    .Include(a => a.Transactions)
                    .FirstOrDefaultAsync(a => a.Id == accountId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching account by ID: {AccountId}", accountId);
                throw;
            }
        }
        public async Task CreateAccountAsync(string accountName, string accountType, string accountNumber, string applicationUserId)
        {
            var account = new Account
            {
                AccountName = accountName,
                AccountType = accountType,
                AccountNumber = accountNumber,
                ApplicationUserId = applicationUserId // Tilldela användar-ID
            };

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
        }
        // Metod för att lägga till pengar på ett konto (endast för admin)
        public async Task<bool> DepositToAccountAsync(int accountId, decimal amount)
        {
            Console.WriteLine($"DepositToAccountAsync anropas med AccountId: {accountId}, Amount: {amount}");

            var account = await _context.Accounts.FindAsync(accountId);
            if (account == null)
            {
                Console.WriteLine("Kontot kunde inte hittas.");
                return false; // Kontot finns inte
            }

            Console.WriteLine($"Saldo före insättning: {account.Balance}");
            account.Balance += amount; // Uppdatera saldo
            Console.WriteLine($"Saldo efter insättning: {account.Balance}");

            await _context.SaveChangesAsync();
            Console.WriteLine($"Pengarna har satts in på konto {accountId}. Nytt saldo: {account.Balance}");
            return true;
        }
        public async Task AddTransactionAsync(int accountId, Transaction transaction)
        {
            var account = await GetAccountByIdAsync(accountId);
            if (account == null)
            {
                _logger.LogWarning("Attempt to add transaction to non-existent account: {AccountId}", accountId);
                throw new InvalidOperationException("Account does not exist.");
            }

            try
            {
                account.Transactions.Add(transaction);
                account.Balance += transaction.Amount;
                await _context.SaveChangesAsync();
                _logger.LogInformation("Added transaction to account: {AccountNumber}", account.AccountNumber);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding transaction to account: {AccountNumber}", account.AccountNumber);
                throw;
            }
        }
        public async Task<bool> TransferFundsAsync(int fromAccountId, int toAccountId, decimal amount, string message)
        {
            var fromAccount = await GetAccountByIdAsync(fromAccountId);
            var toAccount = await GetAccountByIdAsync(toAccountId);

            if (fromAccount == null || toAccount == null)
            {
                _logger.LogWarning("Attempt to transfer funds between non-existent accounts: {FromAccountId}, {ToAccountId}", fromAccountId, toAccountId);
                throw new InvalidOperationException("One or both accounts do not exist.");
            }

            if (fromAccount.Balance < amount)
            {
                _logger.LogWarning("Insufficient funds for transfer from account: {FromAccountNumber}", fromAccount.AccountNumber);
                return false;
            }

            try
            {
                await AddTransactionAsync(fromAccountId, new Transaction
                {
                    Date = DateTime.Now,
                    Amount = -amount,
                    Message = message,
                    AccountId = fromAccountId,
                    Account = fromAccount
                });

                await AddTransactionAsync(toAccountId, new Transaction
                {
                    Date = DateTime.Now,
                    Amount = amount,
                    Message = message,
                    AccountId = toAccountId,
                    Account = toAccount
                });

                _logger.LogInformation("Transferred {Amount} from account {FromAccountNumber} to {ToAccountNumber}", amount, fromAccount.AccountNumber, toAccount.AccountNumber);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error transferring funds from account {FromAccountNumber} to {ToAccountNumber}", fromAccount.AccountNumber, toAccount.AccountNumber);
                throw;
            }
        }
    }
}
