using ERP.DAL.DB.Entities;

namespace ERP.API.Models.Expense
{
    public class ExpensePostVM
    {
        public DateTime ExpenseDate { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public int PaymentModeId { get; set; }
        public int ExpenseTypeId { get; set; }
    }
}
