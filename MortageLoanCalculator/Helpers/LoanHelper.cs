using MortgageLoanCalculator.Models;

namespace MortgageLoanCalculator.Helpers
{
    public class LoanHelper
    {
        // This method calculates the payment schedule for a loan
        public LoanModel GetPayments(LoanModel loan)
        {
            // Calculate the monthly payment
            loan.Payment = CalcPayment(loan.LoanAmount, loan.Rate, loan.Term);

            // Initialize the balance and total interest
            var balance = loan.LoanAmount;
            var totalInterest = 0.0m;

            // Calculate the monthly interest rate
            var monthlyRate = CalcMonthlyRate(loan.Rate);

            // Loop over each month of the loan term
            for (int month = 1; month <= loan.Term; month++)
            {
                // Calculate the interest for this month
                var monthlyInterest = CalcMonthlyInterest(balance, monthlyRate);

                // Add the interest to the total interest
                totalInterest += monthlyInterest;

                // Calculate the principal for this month
                var monthlyPrincipal = loan.Payment - monthlyInterest;

                // Subtract the principal from the balance
                balance -= monthlyPrincipal;

                // Create a new loan payment for this month
                LoanPaymentModel loanPayment = new LoanPaymentModel
                {
                    Month = month,
                    Payment = loan.Payment,
                    MonthlyPrincipal = monthlyPrincipal,
                    MonthlyInterest = monthlyInterest,
                    TotalInterest = totalInterest,
                    Balance = balance
                };

                // Add the loan payment to the loan's payment schedule
                loan.Payments.Add(loanPayment);
            }

            // Set the total interest and total cost of the loan
            loan.TotalInterest = totalInterest;
            loan.TotalCost = loan.LoanAmount + totalInterest;

            // Return the loan with its payment schedule
            return loan;
        }

        // This method calculates the monthly payment for a loan
        private decimal CalcPayment(decimal loanAmount, decimal rate, int term)
        {
            // Calculate the monthly interest rate
            var monthlyRate = CalcMonthlyRate(rate);

            // Calculate the payment
            return loanAmount * monthlyRate / (decimal)Math.Pow(1 + (double)monthlyRate, -term);
        }

        // This method calculates the monthly interest rate
        private decimal CalcMonthlyRate(decimal annualRate)
        {
            // Convert the annual interest rate to a monthly interest rate
            return annualRate / 12 / 100;
        }

        // This method calculates the interest for a month
        private decimal CalcMonthlyInterest(decimal balance, decimal monthlyRate)
        {
            // Calculate the interest based on the balance and the monthly interest rate
            return balance * monthlyRate;
        }
    }
}
