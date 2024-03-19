using Library.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Repos
{
    public class TransactionRepo : ITransactionRepo
    {
        //Admin Methods

        LibraryDBContext cnt = new LibraryDBContext();

        public async Task DeleteTransaction(int transactionId)
        {
            try
            {
                Transaction transaction = await GetTransactionById(transactionId);
                cnt.Transactions.Remove(transaction);
                await cnt.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new LibraryException(ex.Message);
            }
        }

        public async Task<List<Transaction>> GetAllTransactions()
        {
            List<Transaction> transactions = await cnt.Transactions.Include(tran=>tran.Book).Include(tran=>tran.Borrower).ToListAsync();
            return transactions;
        }

        public async Task<List<Transaction>> GetBorrowTransactions()
        {
            try
            {
                List<Transaction> transactions = await (from t in cnt.Transactions.Include(tran => tran.Book).Include(tran => tran.Borrower) where t.TransactionType == "Borrow" select t).ToListAsync();
                return transactions;
            }
            catch
            {
                throw new LibraryException("No such Transactions Exists.");
            }
        }

        public async Task<List<Transaction>> GetReturnTransactions()
        {
            try
            {
                List<Transaction> transactions = await (from t in cnt.Transactions.Include(tran => tran.Book).Include(tran => tran.Borrower) where t.TransactionType == "Return" select t).ToListAsync();
                return transactions;
            }
            catch
            {
                throw new LibraryException("No such Transactions Exists.");
            }
        }

        public async Task<Transaction> GetTransactionById(int transactionId)
        {
            try
            {
                Transaction transaction = await (from t in cnt.Transactions.Include(tran => tran.Book).Include(tran => tran.Borrower) where t.TransactionId == transactionId select t).FirstAsync();
                return transaction;
            }
            catch
            {
                throw new LibraryException("No such Transaction Id Exists.");
            }
        }

        public async Task<List<Transaction>> GetTransactionsBetweenDates(DateTime fromDate, DateTime toDate)
        {
            try
            {
                List<Transaction> transactions = await (from t in cnt.Transactions.Include(tran => tran.Book).Include(tran => tran.Borrower) where t.TransactionDate >= fromDate && t.TransactionDate <= toDate.AddDays(1) select t).ToListAsync();
                return transactions;
            }
            catch
            {
                throw new LibraryException("No such Transactions Exists.");
            }
        }

        public async Task<List<Transaction>> GetTransactionsByBookId(int bookId)
        {
            try
            {
                List<Transaction> transactions = await (from t in cnt.Transactions.Include(tran => tran.Book).Include(tran => tran.Borrower) where t.BookId == bookId select t).ToListAsync();
                return transactions;
            }
            catch
            {
                throw new LibraryException("No such Book Id Exists.");
            }
        }

        public async Task<List<Transaction>> GetTransactionsByBorrowerId(int borrowerId)
        {
            try
            {
                List<Transaction> transactions = await(from t in cnt.Transactions.Include(tran => tran.Book).Include(tran => tran.Borrower) where t.BorrowerId == borrowerId select t).ToListAsync();
                return transactions;
            }
            catch
            {
                throw new LibraryException("No such Borrower Id Exists.");
            }
        }

        public async Task UpdateTransaction(int transactionId, Transaction transaction)
        {
            try
            {
                Transaction transactionToEdit = await GetTransactionById(transactionId);
                transactionToEdit.DueDate = transaction.DueDate;
                
                await cnt.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                throw new LibraryException(ex.Message);
            }
        }



        //User Methods
        public async Task<List<Transaction>> GetAllTransactionsOfUser(int borrowerId)
        {
            try
            {
                List<Transaction> transactions = await (from t in cnt.Transactions.Include(tran => tran.Book).Include(tran => tran.Borrower) where t.BorrowerId == borrowerId select t).ToListAsync();
                return transactions;
            }
            catch
            {
                throw new LibraryException("No such Borrower Id Exists.");
            }
        }

        public async Task<List<Transaction>> GetBorrowTransactionsOfUser(int borrowerId)
        {
            try
            {
                List<Transaction> transactions = await (from t in cnt.Transactions.Include(tran => tran.Book).Include(tran => tran.Borrower) where t.BorrowerId == borrowerId && t.TransactionType == "Borrow" select t).ToListAsync();
                return transactions;
            }
            catch
            {
                throw new LibraryException("No such Borrower Id Exists.");
            }
        }

        public async Task<List<Transaction>> GetReturnTransactionsOfUser(int borrowerId)
        {
            try
            {
                List<Transaction> transactions = await (from t in cnt.Transactions.Include(tran => tran.Book).Include(tran => tran.Borrower) where t.BorrowerId == borrowerId && t.TransactionType == "Return" select t).ToListAsync();
                return transactions;
            }
            catch
            {
                throw new LibraryException("No such Borrower Id Exists.");
            }
        }

        public async Task<List<Transaction>> GetTransactionsBetweenDatesOfUser(int borrowerId, DateTime fromDate, DateTime toDate)
        {
            try
            {
                List<Transaction> transactions = await (from t in cnt.Transactions.Include(tran => tran.Book).Include(tran => tran.Borrower) where t.BorrowerId == borrowerId && t.TransactionDate >= fromDate && t.TransactionDate <= toDate select t).ToListAsync();
                return transactions;
            }
            catch
            {
                throw new LibraryException("No such Borrower Id Exists.");
            }
        }

        public async Task<List<Transaction>> GetTransactionsByBookIdOfUser(int borrowerId, int bookId)
        {
            try
            {
                List<Transaction> transactions = await (from t in cnt.Transactions.Include(tran => tran.Book).Include(tran => tran.Borrower) where t.BorrowerId == borrowerId && t.BookId == bookId select t).ToListAsync();
                return transactions;
            }
            catch
            {
                throw new LibraryException("No such Borrower Id or Book Id Exists.");
            }
        }

        public async Task<List<Transaction>> GetTransactionsByDueDateOfUser(int borrowerId, DateTime dueDate)
        {
            try
            {
                List<Transaction> transactions = await (from t in cnt.Transactions.Include(tran => tran.Book).Include(tran => tran.Borrower) where t.BorrowerId == borrowerId && t.DueDate == dueDate select t).ToListAsync();
                return transactions;
            }
            catch
            {
                throw new LibraryException("No such Borrower Id or Due Date Exists.");
            }
        }

        public async Task<List<Transaction>> GetActiveTransactions()
        {
            try
            {
                List<Transaction> transactions = await (from t in cnt.Transactions.Include(tran => tran.Book).Include(tran => tran.Borrower)
                                                        where (t.TransactionType == "Borrow" &&
                                                       !cnt.Transactions.Any(returnTransaction => returnTransaction.TransactionType == "Return"
                                                       && returnTransaction.BookId == t.BookId && returnTransaction.BorrowerId == t.BorrowerId 
                                                       && returnTransaction.TransactionDate >= t.TransactionDate)) select t).ToListAsync();
                return transactions;
            }
            catch(Exception ex)
            {
                throw new LibraryException(ex.Message);
            }
        }
    }
}