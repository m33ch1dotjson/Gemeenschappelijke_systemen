using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Comparers
{
    /// <summary>
    /// Compares two Employee objects based on their FullName property.
    /// Allows sorting in ascending (A-Z) or descending (Z-A) order.
    /// </summary>
    public class EmployeeComparer : IComparer<Employee>
    {
        private readonly bool _descending;

        /// <summary>
        /// Constructor to define the sort direction.
        /// </summary>
        /// <param name="descending">Set to true for Z-A sorting, false for A-Z.</param>
        public EmployeeComparer(bool descending = false)
        {
            _descending = descending;
        }

        /// <summary>
        /// Compares two employees by their full names.
        /// </summary>
        public int Compare(Employee x, Employee y)
        {
            if (x == null || y == null)
                return 0; // basic null-safety

            return _descending
                ? string.Compare(y.GetFullName(), x.GetFullName())
                : string.Compare(x.GetFullName(), y.GetFullName());
        }
    }
}
