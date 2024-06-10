using Microsoft.AspNetCore.Mvc;
using MortageLoanCalculator.Models;
using MortgageLoanCalculator.Models;
using System.Diagnostics;


namespace MortgageLoanCalculator.Controllers
{
    public class HomeController : Controller
    {

        [HttpPost]
        public IActionResult Calculator(LoanModel loan)
        {
            // Calculate the loan payments...
            var loanHelper = new LoanHelper();
            LoanModel newLoan = loanHelper.GetPayments(loan);

            // Return a JSON object with the results
            return Json(new
            {
                payment = newLoan.Payment,
                totalPrincipal = newLoan.LoanAmount,
                totalInterest = newLoan.TotalInterest,
                totalCost = newLoan.TotalCost
                // Add more properties here as needed
            });
        }

        [HttpGet]
        public IActionResult Calculator()
        {
            // Define preset values
            LoanModel loan = new()
            {
                Rate = 7.5m,
                LoanAmount = 300000m,
                Term = 30
            };

            return View(loan);
        }

        public IActionResult Index()
        {
            var model = new LoanModel();
            // Initialize the model properties here
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    public class LoanHelper
    {
        public LoanModel GetPayments(LoanModel loan)
        {
            loan.Payment = Math.Round(CalcPayment(loan.LoanAmount, loan.Rate, loan.Term), 2);

            var balance = loan.LoanAmount;
            var totalInterest = 0.0m;
            var monthlyRate = CalcMonthlyRate(loan.Rate);

            for (int month = 1; month <= loan.Term; month++)
            {
                var monthlyInterest = Math.Round(balance * monthlyRate, 2);
                totalInterest += monthlyInterest;
                var monthlyPrincipal = Math.Round(loan.Payment - monthlyInterest, 2);
                balance = Math.Round(balance - monthlyPrincipal, 2);

                LoanPaymentModel loanPayment = new LoanPaymentModel
                {
                    Month = month,
                    Payment = Math.Round(loan.Payment, 2),
                    MonthlyPrincipal = monthlyPrincipal,
                    MonthlyInterest = monthlyInterest,
                    TotalInterest = Math.Round(totalInterest, 2),
                    Balance = balance
                };

                loan.Payments.Add(loanPayment);
            }

            loan.TotalInterest = Math.Round(totalInterest, 2);
            loan.TotalCost = Math.Round(loan.LoanAmount + totalInterest, 2);

            return loan;
        }


        private decimal CalcPayment(decimal loanAmount, decimal rate, int term)
        {
            var monthlyRate = CalcMonthlyRate(rate);
            return loanAmount * monthlyRate / (decimal)Math.Pow(1 + (double)monthlyRate, -term);
        }

        private decimal CalcMonthlyRate(decimal annualRate)
        {
            return annualRate / 12 / 100;
        }
    }
}
