

using ERP.API.Models.ExpenseTypeGetResponse;
using ERP.API.Models.PaymentModeGetResponse;

namespace ERP.API.Models.ExpenseGetReponse

{
    public class ExpenseGetResponseVM 
    {
        public int Id { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public ExpenseTypeGetResponseVM ExpenseType { get; set; }
        public PaymentModeGetResponseVM PaymentMode { get; set; }
    }
}