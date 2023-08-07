namespace ERP.API.Models.ExpenseGetReponse
{
    public class ExpenseGetResponseVM
    {
        public int Id { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public int ExpenseTypeId { get; set; }
        public int PaymentModeId { get; set; }
    }
}
