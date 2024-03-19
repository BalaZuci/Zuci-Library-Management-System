using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Repos
{
    public interface ITransactionRepo
    {
        //Admin Methods
        Task<List<Transaction>> GetAllTransactions();
        Task<List<Transaction>> GetBorrowTransactions();
        Task<List<Transaction>> GetReturnTransactions();
        Task<List<Transaction>> GetActiveTransactions();
        Task<List<Transaction>> GetTransactionsByBookId(int bookId);
        Task<List<Transaction>> GetTransactionsByBorrowerId(int borrowerId);
        Task<List<Transaction>> GetTransactionsBetweenDates(DateTime fromDate, DateTime toDate);
        Task<Transaction> GetTransactionById(int transactionId);

        Task UpdateTransaction(int transactionId, Transaction transaction);
        Task DeleteTransaction(int transactionId);

        //User Methods
        Task<List<Transaction>> GetAllTransactionsOfUser(int borrowerId);
        Task<List<Transaction>> GetBorrowTransactionsOfUser(int borrowerId);
        Task<List<Transaction>> GetReturnTransactionsOfUser(int borrowerId);
        Task<List<Transaction>> GetTransactionsByBookIdOfUser(int borrowerId, int bookId);
        Task<List<Transaction>> GetTransactionsBetweenDatesOfUser(int borrowerId, DateTime fromDate, DateTime toDate);
    }
}
