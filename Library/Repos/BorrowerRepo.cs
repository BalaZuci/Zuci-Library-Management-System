using Library.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Repos
{
    public class BorrowerRepo : IBorrowerRepo
    {
        LibraryDBContext cnt = new LibraryDBContext();
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

        public async Task<List<Borrower>> GetAllBorrowers()
        {
            List<Borrower> borrowers = await cnt.Borrowers.ToListAsync();
            return borrowers;
        }

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