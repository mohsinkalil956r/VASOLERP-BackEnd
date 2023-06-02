using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP.DAL.DB.Entities
{
    public class Expense : IBaseEntity
    {
        public int Id { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime ExpenseDate { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }

        public int PaymentModeId { get; set; }
        public int ExpenseTypeId { get; set; }

        public ExpenseType ExpenseType { get; set; }
        public PaymentMode PaymentMode { get; set; }
    }
}
