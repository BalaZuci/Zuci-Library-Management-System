using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Models
{
    /// <summary>
    /// This a custom Exception Class, where Class Library Exceptions are thrown by this class Exception.
    /// </summary>
    public class LibraryException : Exception
    {
        public LibraryException(string msg) : base(msg) { }
    }
}
