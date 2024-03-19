using Library.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Repos
{
    public interface IBorrowerRepo
    {
        Task<Borrower> GetBorrowerById(int borrowerId);
        Task<Borrower> GetBorrowerByEmail(string email);
        Task InsertBorrower(Borrower borrower);
        Task UpdateBorrower(int borrowerId, Borrower borrower);
        Task UpdateBorrowerByAdmin(int borrowerId, Borrower borrower);
        Task<List<Borrower>> GetAllBorrowers();
        Task<List<Borrower>> GetBorrowerByBorrowerType(string borrowerType);
        Task DeleteBorrower(int borrowerId);

    }
}
