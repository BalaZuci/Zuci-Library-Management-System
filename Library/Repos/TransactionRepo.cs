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
    /// <summary>
    /// implements the ITransactionRepo interface
    /// implements all the operations can be done on the transaction table
    /// </summary>
    public class TransactionRepo : ITransactionRepo
    {
        //Admin Methods

        LibraryDBContext cnt = new LibraryDBContext();
        /// <summary>
        /// deletes the transaction for database, an admin mehtod
        /// </summary>
        /// <param name="transactionId">specifies the transaction</param>
        /// <returns>void</returns>
        /// <exception cref="LibraryException">throws exception if something went wrong</exception>
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
        /// <summary>
        /// for getting all the transaction done in the library, this is a admin method
        /// </summary>
        /// <returns>list of transactions</returns>
        public async Task<List<Transaction>> GetAllTransactions()
        {
            List<Transaction> transactions = await cnt.Transactions.Include(tran=>tran.Book).Include(tran=>tran.Borrower).ToListAsync();
            return transactions;
        }
        /// <summary>
        /// for getting all the borrow transaction done in the library, this is a admin method
        /// </summary>
        /// <returns>list of transactions</returns>
        /// <exception cref="LibraryException">throws exception if something went wrong</exception>
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
        /// <summary>
        /// for getting the return transactions done in the library, this is also a admin method
        /// </summary>
        /// <returns>List of transactions</returns>
        /// <exception cref="LibraryException">throws exception if something went wrong</exception>
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
        /// <summary>
        /// for getting particular transaction based on the Id
        /// </summary>
        /// <param name="transactionId">specifies the transaction to view</param>
        /// <returns>transaction object</returns>
        /// <exception cref="LibraryException">throws exception if something went wrong</exception>
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
        /// <summary>
        /// for getting the transactions happened between dates
        /// </summary>
        /// <param name="fromDate">from which date</param>
        /// <param name="toDate">to which date</param>
        /// <returns>List of transactions</returns>
        /// <exception cref="LibraryException">throws exception if something went wrong</exception>
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
        /// <summary>
        /// for getting the transactions done on particular book
        /// </summary>
        /// <param name="bookId">specifies the book</param>
        /// <returns>list of transactions</returns>
        /// <exception cref="LibraryException">throws exception if something went wrong</exception>
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
        /// <summary>
        /// for getting the transactions done by particular borrower, used by both admin and user
        /// </summary>
        /// <param name="borrowerId">specifies the borrower</param>
        /// <returns>List of transactions</returns>
        /// <exception cref="LibraryException">throws exception if something went wrong</exception>
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
        /// <summary>
        /// for updaiing the Due Date of active transactions, only done by admin
        /// </summary>
        /// <param name="transactionId">specifies the transaction to be updated</param>
        /// <param name="transaction">Contains the updated details of the transaction</param>
        /// <returns>void</returns>
        /// <exception cref="LibraryException">throws exception if something went wrong</exception>
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
        /// <summary>
        /// this is the admin method, for finding the Active transactions(which are borrowed not returned)
        /// </summary>
        /// <returns>List of transactions</returns>
        /// <exception cref="LibraryException">throws exception if something went wrong</exception>
        public async Task<List<Transaction>> GetActiveTransactions()
        {
            try
            {
                List<Transaction> transactions = await (from t in cnt.Transactions.Include(tran => tran.Book).Include(tran => tran.Borrower)
                                                        where (t.TransactionType == "Borrow" &&
                                                       !cnt.Transactions.Any(returnTransaction => returnTransaction.TransactionType == "Return"
                                                       && returnTransaction.BookId == t.BookId && returnTransaction.BorrowerId == t.BorrowerId
                                                       && returnTransaction.TransactionDate >= t.TransactionDate))
                                                        select t).ToListAsync();
                return transactions;
            }
            catch (Exception ex)
            {
                throw new LibraryException(ex.Message);
            }
        }

        //User Methods

        /// <summary>
        /// this user method to get there own transactions
        /// </summary>
        /// <param name="borrowerId">specifies the user who is logged in</param>
        /// <returns>list of transactions</returns>
        /// <exception cref="LibraryException">throws exception if something went wrong</exception>
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
        /// <summary>
        /// this user method to get there own borrow transactions
        /// </summary>
        /// <param name="borrowerId">specifies the user who is logged in</param>
        /// <returns>list of transactions</returns>
        /// <exception cref="LibraryException">throws exception if something went wrong</exception>
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
        /// <summary>
        /// this user method to get there own return transactions
        /// </summary>
        /// <param name="borrowerId">specifies the user who is logged in</param>
        /// <returns>list of transactions</returns>
        /// <exception cref="LibraryException">throws exception if something went wrong</exception>
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

        /// <summary>
        /// For getting own transactions done between fromdate and todate
        /// </summary>
        /// <param name="borrowerId">specifies the user</param>
        /// <param name="fromDate">from date</param>
        /// <param name="toDate">to Date</param>
        /// <returns>List of transactions</returns>
        /// <exception cref="LibraryException">throws exception if something went wrong with borrower id</exception>
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
        /// <summary>
        /// for getting transactions done on particular book by logged in user
        /// </summary>
        /// <param name="borrowerId">specifies the borrower</param>
        /// <param name="bookId">specifies the book</param>
        /// <returns>List of transactions</returns>
        /// <exception cref="LibraryException">throws exception if something went wrong</exception>
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
    }
}