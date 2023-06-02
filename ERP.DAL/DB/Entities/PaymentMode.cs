namespace ERP.DAL.DB.Entities
{
    public class PaymentMode : IBaseEntity
    {
        public int Id { get; set; }
        public bool IsActive { get; set; } = true;
        public string Name { get; set; }

        public List<Expense> Expenses { get; set; } = new();

    }
}