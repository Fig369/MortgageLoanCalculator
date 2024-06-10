namespace MortgageLoanCalculator.Models
{
    public class LoanModel
    {
        public decimal Payment { get; set; }
        public decimal Rate { get; set; }
        public decimal LoanAmount { get; set; }
        public decimal TotalInterest { get; set; }
        public decimal TotalCost { get; set; }
        public int Term { get; set; }
        public string? Message { get; set; }

        public List<LoanPaymentModel> Payments { get; set; } = new List<LoanPaymentModel>();
    }

    public class LoanPaymentModel
    {
        public int Month { get; set; }
        public decimal Payment { get; set; }
        public decimal MonthlyPrincipal { get; set; }
        public decimal MonthlyInterest { get; set; }
        public decimal TotalInterest { get; set; }
        public decimal Balance { get; set; }
    }
}
