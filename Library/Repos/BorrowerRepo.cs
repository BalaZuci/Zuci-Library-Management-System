using Library.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Repos
{
    /// <summary>
    /// implements the Interface IBorrowerRepo
    /// this class contains all the functionalifies related to Borrower table in the database
    /// </summary>
    public class BorrowerRepo : IBorrowerRepo
    {
        LibraryDBContext cnt = new LibraryDBContext();
        /// <summary>
        /// Deletes the Borrower
        /// </summary>
        /// <param name="borrowerId">specifies the Borrower</param>
        /// <returns>void</returns>
        /// <exception cref="LibraryException">throws exception if borrower doesn't exists or any other thing went wrong.</exception>
        public async Task DeleteBorrower(int borrowerId)
        {
            try
            {
                Borrower borrower = await GetBorrowerById(borrowerId);
                cnt.Borrowers.Remove(borrower);
                await cnt.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new LibraryException(ex.Message);
            }
        }
        /// <summary>
        /// for getting all the borrowers present in the borrower table, this a admin method
        /// </summary>
        /// <returns>List of Borrowers</returns>
        public async Task<List<Borrower>> GetAllBorrowers()
        {
            List<Borrower> borrowers = await cnt.Borrowers.ToListAsync();
            return borrowers;
        }
        /// <summary>
        /// for getting borrowers of same type(admin or user)
        /// </summary>
        /// <param name="borrowerType">Admin or user</param>
        /// <returns>list of borrowers</returns>
        /// <exception cref="LibraryException">throws exception if some thing went wrong</exception>
        public async Task<List<Borrower>> GetBorrowerByBorrowerType(string borrowerType)
        {
            try
            {
                List<Borrower> borrower = await(from b in cnt.Borrowers where b.BorrowerType == borrowerType select b).ToListAsync();
                return borrower;
            }
            catch
            {
                throw new LibraryException("No such Borrower Type exists.");
            }
        }
        /// <summary>
        /// for getting the borrower by email, which is different for every one.
        /// used for login and register pages
        /// </summary>
        /// <param name="email">specifies the borrower through email</param>
        /// <returns>Borrower object</returns>
        /// <exception cref="LibraryException">throws exception if something went wrong</exception>
        public async Task<Borrower> GetBorrowerByEmail(string email)
        {
            try
            {
                Borrower borrower = await(from b in cnt.Borrowers where b.Email == email select b).FirstAsync();
                return borrower;
            }
            catch
            {
                throw new LibraryException("No such Email exists.");
            }
        }
        /// <summary>
        /// Getting the borrower by Id
        /// </summary>
        /// <param name="borrowerId"></param>
        /// <returns>Borrower Object which contains all the details</returns>
        /// <exception cref="LibraryException">throws exception if borrower not found</exception>
        public async Task<Borrower> GetBorrowerById(int borrowerId)
        {
            try
            {
                Borrower borrower = await (from b in cnt.Borrowers where b.BorrowerId == borrowerId select b).FirstAsync();
                return borrower;
            }
            catch
            {
                throw new LibraryException("No such Borrower Id exists.");
            }
        }
        /// <summary>
        /// for inserting new borrower into database
        /// </summary>
        /// <param name="borrower">borrower to be inserted</param>
        /// <returns>void</returns>
        /// <exception cref="LibraryException">throws exception if something went wrong</exception>
        public async Task InsertBorrower(Borrower borrower)
        {
            try
            {
                await cnt.Borrowers.AddAsync(borrower);
                await cnt.SaveChangesAsync();

                Borrower borrower1 = await GetBorrowerById(borrower.BorrowerId);
                if(borrower1.CreatedBy == 0)
                    borrower1.CreatedBy = borrower1.BorrowerId;
                
                borrower1.CreatedOn = DateTime.Now;
                borrower1.LastUpdatedOn = DateTime.Now;

                await cnt.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new LibraryException(ex.Message);
            }
        }
        /// <summary>
        /// for updating the borrower details in the database, this done by user
        /// </summary>
        /// <param name="borrowerId">specifies the borrower details need to be updated</param>
        /// <param name="borrower">contains updated details of the borrower</param>
        /// <returns>void</returns>
        /// <exception cref="LibraryException">throws exception if something went wrong</exception>
        public async Task UpdateBorrower(int borrowerId, Borrower borrower)
        {
            try
            {
                Borrower borrowerToEdit = await GetBorrowerById(borrowerId);

                borrowerToEdit.Name = borrower.Name;
                borrowerToEdit.Department = borrower.Department;
                borrowerToEdit.Email = borrower.Email;
                borrowerToEdit.Password = borrower.Password;
                borrower.BorrowerType = "User";

                borrowerToEdit.LastUpdatedOn = DateTime.Now;

                await cnt.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new LibraryException(ex.Message);
            }
        }
        /// <summary>
        /// to update details of borrower by admin
        /// </summary>
        /// <param name="borrowerId">specifies the borrower details to be updated</param>
        /// <param name="borrower">contains the updated details of the Borrower</param>
        /// <returns>void</returns>
        /// <exception cref="LibraryException">throws exception if something went wrong</exception>
        public async Task UpdateBorrowerByAdmin(int borrowerId, Borrower borrower)
        {
            try
            {
                Borrower borrowerToEdit = await GetBorrowerById(borrowerId);

                borrowerToEdit.Name = borrower.Name;
                borrowerToEdit.Department = borrower.Department;
                borrowerToEdit.BorrowerType = borrower.BorrowerType;
                borrowerToEdit.Email = borrower.Email;

                borrowerToEdit.LastUpdatedOn = DateTime.Now;

                await cnt.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new LibraryException(ex.Message);
            }
        }
    }
}