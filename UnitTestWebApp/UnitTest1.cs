using Microsoft.VisualStudio.TestTools.UnitTesting;
using DAL.Entities;
using DAL;
using BusinessLayer;

namespace UnitTestWebApp
{
      

        [TestClass]
        public class UnitTest1
        {
             IAccountDAL accountdaltest = new AccountDALTest();

            #region DepositAndWithdrawTest() Passed
            [TestMethod]
            public void DepositAndWithdrawTest()
            {

                AccountBL accountbl = new AccountBL(accountdaltest);

                

                decimal expectedCheckingBalance = 2000m;
                decimal expectedBusinessBalance = 4001m;
                decimal expectedLoanBalance = 0;
                decimal expectedTermdepositBalance = 0;

                //Checking Deposit Test
                //===========================================================================================
                IAccount checkingAccountInfo = new CheckingAccount()
                {
                    Accountno = accountbl.GenerateAccountno(),
                    Balance = accountbl.GetBalance(),
                    CustomerId = 1,
                    AccountStatus = true

                };

                checkingAccountInfo.AccountType = accountbl.GetAcountType(checkingAccountInfo);
                checkingAccountInfo.Interestrate = accountbl.GetIntrestrate(checkingAccountInfo);

                accountbl.OpenAccount(checkingAccountInfo);


                accountbl.Deposit(checkingAccountInfo, 1000m); //good test
                accountbl.Deposit(checkingAccountInfo, 2000m); //good test
                accountbl.Deposit(checkingAccountInfo, 1.0m); //good test

                accountbl.Deposit(checkingAccountInfo, -1000m); //bad test
                accountbl.Deposit(checkingAccountInfo, -1m); //bad test
                accountbl.Deposit(checkingAccountInfo, -1.0m); //bad test
                accountbl.Deposit(checkingAccountInfo, 0m); //bad test

                //after deposit: 3001



                accountbl.Withdraw(checkingAccountInfo, 900m); //good test
                accountbl.Withdraw(checkingAccountInfo, 100m); //good test
                accountbl.Withdraw(checkingAccountInfo, 1.0m); //good test

                accountbl.Withdraw(checkingAccountInfo, -1000m); //bad test
                accountbl.Withdraw(checkingAccountInfo, -1m); //bad test
                accountbl.Withdraw(checkingAccountInfo, -1.0m); //bad test
                accountbl.Withdraw(checkingAccountInfo, 0m); //bad test



                Assert.AreEqual<decimal>(expectedCheckingBalance, checkingAccountInfo.Balance);//output: 2000


                IAccount businessAccountInfo = new BusinessAccount()
                {
                    Accountno = accountbl.GenerateAccountno(),
                    Balance = accountbl.GetBalance(),
                    CustomerId = 1,
                    AccountStatus = true,
                };


                businessAccountInfo.AccountType = accountbl.GetAcountType(businessAccountInfo);
                businessAccountInfo.Interestrate = accountbl.GetIntrestrate(businessAccountInfo);

                accountbl.OpenAccount(businessAccountInfo);

                accountbl.Deposit(businessAccountInfo, 2000m); //good test
                accountbl.Deposit(businessAccountInfo, 2000m); //good test
                accountbl.Deposit(businessAccountInfo, 1.0m); //good test

                accountbl.Deposit(businessAccountInfo, -1000m); //bad test
                accountbl.Deposit(businessAccountInfo, -1m); //bad test
                accountbl.Deposit(businessAccountInfo, -1.0m); //bad test
                accountbl.Deposit(businessAccountInfo, 0m); //bad test



                Assert.AreEqual<decimal>(expectedBusinessBalance, businessAccountInfo.Balance);//output 3000

                IAccount loanAccountInfo = new Loan(0)
                {
                    Accountno = accountbl.GenerateAccountno(),
                    Balance = accountbl.GetBalance(),
                    CustomerId = 1,
                    AccountStatus = true


                };


                loanAccountInfo.Interestrate = accountbl.GetIntrestrate(loanAccountInfo);

                accountbl.OpenAccount(loanAccountInfo);

                accountbl.Deposit(loanAccountInfo, 2000m); //good test
                accountbl.Deposit(loanAccountInfo, 2000m); //good test
                accountbl.Deposit(loanAccountInfo, 1.0m); //good test

                accountbl.Deposit(loanAccountInfo, -1000m); //bad test
                accountbl.Deposit(loanAccountInfo, -1m); //bad test
                accountbl.Deposit(loanAccountInfo, -1.0m); //bad test
                accountbl.Deposit(loanAccountInfo, 0m); //bad test


                Assert.AreEqual<decimal>(expectedLoanBalance, loanAccountInfo.Balance);

                IAccount termdepositAccountInfo = new TermDeposit(0)
                {
                    Accountno = accountbl.GenerateAccountno(),
                    Balance = accountbl.GetBalance(),
                    CustomerId = 1,
                    AccountStatus = true


                };


                accountbl.OpenAccount(termdepositAccountInfo);

                accountbl.Deposit(termdepositAccountInfo, 2000m); //good test
                accountbl.Deposit(termdepositAccountInfo, 2000m); //good test
                accountbl.Deposit(termdepositAccountInfo, 1.0m); //good test

                accountbl.Deposit(termdepositAccountInfo, -1000m); //bad test
                accountbl.Deposit(termdepositAccountInfo, -1m); //bad test
                accountbl.Deposit(termdepositAccountInfo, -1.0m); //bad test
                accountbl.Deposit(termdepositAccountInfo, 0m); //bad test


                Assert.AreEqual<decimal>(expectedTermdepositBalance, termdepositAccountInfo.Balance);
            }
            #endregion


            #region TransferTest() Passed
            [TestMethod]
            public void TransferTest()
            {
                AccountBL accountbl = new AccountBL();


                decimal expectedCheckingBalance = 1890;
                decimal expectedBusinessBalance = -920;
                decimal expectedLoanBalance = 0;
                decimal expectedTermdepositBalance = 0;
                decimal expectedCheckingBalance2 = 10;
                decimal expectedBusinessBalance2 = 20;
                decimal expectedLoanBalance2 = 0;
                decimal expectedTermdepositBalance2 = 0;


                //Checking Deposit Test
                //===========================================================================================
                IAccount checkingAccountInfo = new CheckingAccount()
                {
                    Accountno = accountbl.GenerateAccountno(),
                    Balance = accountbl.GetBalance(),
                    CustomerId = 1,
                    AccountStatus = true

                };

                checkingAccountInfo.AccountType = accountbl.GetAcountType(checkingAccountInfo);
                checkingAccountInfo.Interestrate = accountbl.GetIntrestrate(checkingAccountInfo);

                accountbl.OpenAccount(checkingAccountInfo);

                //=============================================second checkingAccount=========================

                IAccount checkingAccountInfo2 = new CheckingAccount()
                {
                    Accountno = accountbl.GenerateAccountno(),
                    Balance = accountbl.GetBalance(),
                    CustomerId = 1,
                    AccountStatus = true

                };

                checkingAccountInfo2.AccountType = accountbl.GetAcountType(checkingAccountInfo2);
                checkingAccountInfo2.Interestrate = accountbl.GetIntrestrate(checkingAccountInfo2);

                accountbl.OpenAccount(checkingAccountInfo2);




                //=============================================================================================


                IAccount businessAccountInfo = new BusinessAccount()
                {
                    Accountno = accountbl.GenerateAccountno(),
                    Balance = accountbl.GetBalance(),
                    CustomerId = 1,
                    AccountStatus = true,


                };

                businessAccountInfo.AccountType = accountbl.GetAcountType(businessAccountInfo);
                businessAccountInfo.Interestrate = accountbl.GetIntrestrate(businessAccountInfo);

                accountbl.OpenAccount(businessAccountInfo);


                IAccount businessAccountInfo2 = new BusinessAccount()
                {
                    Accountno = accountbl.GenerateAccountno(),
                    Balance = accountbl.GetBalance(),
                    CustomerId = 1,
                    AccountStatus = true,


                };

                businessAccountInfo2.AccountType = accountbl.GetAcountType(businessAccountInfo2);
                businessAccountInfo2.Interestrate = accountbl.GetIntrestrate(businessAccountInfo2);

                accountbl.OpenAccount(businessAccountInfo2);




                IAccount loanAccountInfo = new Loan(0)
                {
                    Accountno = accountbl.GenerateAccountno(),
                    Balance = accountbl.GetBalance(),
                    CustomerId = 1,
                    AccountStatus = true


                };


                loanAccountInfo.Interestrate = accountbl.GetIntrestrate(loanAccountInfo);


                accountbl.OpenAccount(loanAccountInfo);

                IAccount loanAccountInfo2 = new Loan(0)
                {
                    Accountno = accountbl.GenerateAccountno(),
                    Balance = accountbl.GetBalance(),
                    CustomerId = 1,
                    AccountStatus = true


                };


                loanAccountInfo.Interestrate = accountbl.GetIntrestrate(loanAccountInfo2);


                accountbl.OpenAccount(loanAccountInfo2);




                IAccount termdepositAccountInfo = new TermDeposit(0)
                {
                    Accountno = accountbl.GenerateAccountno(),
                    Balance = accountbl.GetBalance(),
                    CustomerId = 1,
                    AccountStatus = true


                };


                accountbl.OpenAccount(termdepositAccountInfo);

                IAccount termdepositAccountInfo2 = new TermDeposit(0)
                {
                    Accountno = accountbl.GenerateAccountno(),
                    Balance = accountbl.GetBalance(),
                    CustomerId = 1,
                    AccountStatus = true


                };


                accountbl.OpenAccount(termdepositAccountInfo2);



                accountbl.Deposit(checkingAccountInfo, 1000);


                //1.from checking to business
                accountbl.Transfer(checkingAccountInfo, businessAccountInfo, 100);
                accountbl.Transfer(checkingAccountInfo, businessAccountInfo, -200);
                accountbl.Transfer(checkingAccountInfo, businessAccountInfo, 0);
                accountbl.Transfer(checkingAccountInfo, businessAccountInfo, -99999);
                // checking:900,business: 100

                //2.from checking to loan
                accountbl.Transfer(checkingAccountInfo, loanAccountInfo, 100);
                accountbl.Transfer(checkingAccountInfo, loanAccountInfo, -200);
                accountbl.Transfer(checkingAccountInfo, loanAccountInfo, 0);
                accountbl.Transfer(checkingAccountInfo, loanAccountInfo, -99999);
                // checking:900,business: 0

                //3.from checking to term deposit
                accountbl.Transfer(checkingAccountInfo, termdepositAccountInfo, 100);
                accountbl.Transfer(checkingAccountInfo, termdepositAccountInfo, -200);
                accountbl.Transfer(checkingAccountInfo, termdepositAccountInfo, 0);
                accountbl.Transfer(checkingAccountInfo, termdepositAccountInfo, -99999);
                // checking:900,term deposit: 0



                //4.from business to checking
                accountbl.Transfer(businessAccountInfo, checkingAccountInfo, 1000);
                accountbl.Transfer(businessAccountInfo, checkingAccountInfo, -200);
                accountbl.Transfer(businessAccountInfo, checkingAccountInfo, 0);
                //business: -900 , checking:1900

                //5.from business to loan
                accountbl.Transfer(businessAccountInfo, loanAccountInfo, 1000);
                accountbl.Transfer(businessAccountInfo, loanAccountInfo, -200);
                accountbl.Transfer(businessAccountInfo, loanAccountInfo, 0);
                accountbl.Transfer(businessAccountInfo, loanAccountInfo, -99999);
                // business:-900,loan: 0

                //6.from business to termdeposit
                accountbl.Transfer(businessAccountInfo, termdepositAccountInfo, 1000);
                accountbl.Transfer(businessAccountInfo, termdepositAccountInfo, -200);
                accountbl.Transfer(businessAccountInfo, termdepositAccountInfo, 0);
                accountbl.Transfer(businessAccountInfo, termdepositAccountInfo, -99999);
                // business:-900,term deposit 0

                //7.from loan to termdeposit
                accountbl.Transfer(loanAccountInfo, termdepositAccountInfo, 1000);
                accountbl.Transfer(loanAccountInfo, termdepositAccountInfo, -200);
                accountbl.Transfer(loanAccountInfo, termdepositAccountInfo, 0);
                accountbl.Transfer(loanAccountInfo, termdepositAccountInfo, -99999);
                // loan: 0, termdeposit: 0

                //8.from termdeposit to loan
                accountbl.Transfer(termdepositAccountInfo, loanAccountInfo, 1000);
                accountbl.Transfer(termdepositAccountInfo, loanAccountInfo, -200);
                accountbl.Transfer(termdepositAccountInfo, loanAccountInfo, 0);
                accountbl.Transfer(termdepositAccountInfo, loanAccountInfo, -99999);
                // term deposit: 0 loan: 0

                //9.from termdeposit to business
                accountbl.Transfer(termdepositAccountInfo, businessAccountInfo, 1000);
                accountbl.Transfer(termdepositAccountInfo, businessAccountInfo, -200);
                accountbl.Transfer(termdepositAccountInfo, businessAccountInfo, 0);
                accountbl.Transfer(termdepositAccountInfo, businessAccountInfo, -99999);
                // term deposit: 0 business: -900

                //10.from termdeposit to checking
                accountbl.Transfer(termdepositAccountInfo, checkingAccountInfo, 1000);
                accountbl.Transfer(termdepositAccountInfo, checkingAccountInfo, -200);
                accountbl.Transfer(termdepositAccountInfo, checkingAccountInfo, 0);
                accountbl.Transfer(termdepositAccountInfo, checkingAccountInfo, -99999);
                // term deposit: 0 checking: 1900

                //11.from loan to business
                accountbl.Transfer(loanAccountInfo, businessAccountInfo, 1000);
                accountbl.Transfer(loanAccountInfo, businessAccountInfo, -200);
                accountbl.Transfer(loanAccountInfo, businessAccountInfo, 0);
                accountbl.Transfer(loanAccountInfo, businessAccountInfo, -99999);
                // loan: 0, business: -900

                //12.from loan to checking
                accountbl.Transfer(loanAccountInfo, checkingAccountInfo, 1000);
                accountbl.Transfer(loanAccountInfo, checkingAccountInfo, -200);
                accountbl.Transfer(loanAccountInfo, checkingAccountInfo, 0);
                accountbl.Transfer(loanAccountInfo, checkingAccountInfo, -99999);
                // loan: 0, checking: 1900

                //13.from checking to checking
                accountbl.Transfer(checkingAccountInfo, checkingAccountInfo2, 10);
                accountbl.Transfer(checkingAccountInfo, checkingAccountInfo2, -200);
                accountbl.Transfer(checkingAccountInfo, checkingAccountInfo2, 0);
                accountbl.Transfer(checkingAccountInfo, checkingAccountInfo2, -99999);
                //checking1:1890 checking2:10

                //14.from business to business
                accountbl.Transfer(businessAccountInfo, businessAccountInfo2, 20);
                accountbl.Transfer(businessAccountInfo, businessAccountInfo2, -200);
                accountbl.Transfer(businessAccountInfo, businessAccountInfo2, 0);
                accountbl.Transfer(businessAccountInfo, businessAccountInfo2, -99999);
                //business1: -910 business2:20

                //15.from loan to loan
                accountbl.Transfer(loanAccountInfo, loanAccountInfo2, 10);
                accountbl.Transfer(loanAccountInfo, loanAccountInfo2, -200);
                accountbl.Transfer(loanAccountInfo, loanAccountInfo2, 0);
                accountbl.Transfer(loanAccountInfo, loanAccountInfo2, -99999);
                //loan1: 0, loan2: 0

                //16.from termdeposit to termdeposit
                accountbl.Transfer(termdepositAccountInfo, termdepositAccountInfo2, 10);
                accountbl.Transfer(termdepositAccountInfo, termdepositAccountInfo2, -200);
                accountbl.Transfer(termdepositAccountInfo, termdepositAccountInfo2, 0);
                accountbl.Transfer(termdepositAccountInfo, termdepositAccountInfo2, -99999);
                //termdeposit1: 0, to termdeposit2: 0





                Assert.AreEqual(expectedCheckingBalance, checkingAccountInfo.Balance);
                Assert.AreEqual(expectedCheckingBalance2, checkingAccountInfo2.Balance);

                Assert.AreEqual(expectedBusinessBalance, businessAccountInfo.Balance);
                Assert.AreEqual(expectedBusinessBalance2, businessAccountInfo2.Balance);

                Assert.AreEqual(expectedLoanBalance, loanAccountInfo.Balance);
                Assert.AreEqual(expectedLoanBalance2, loanAccountInfo2.Balance);

                Assert.AreEqual(expectedTermdepositBalance, termdepositAccountInfo.Balance);
                Assert.AreEqual(expectedTermdepositBalance2, termdepositAccountInfo2.Balance);





            }
            #endregion


            #region ClosedAccountDepositAndWithdrawTest() Passed
            [TestMethod]
            public void ClosedAccountDepositAndWithdrawTest()
            {

                AccountBL accountbl = new AccountBL();

                decimal expectedCheckingBalance = 0;
                decimal expectedBusinessBalance = 0;
                decimal expectedLoanBalance = 0;
                decimal expectedTermdepositBalance = 0;
                bool expectedStatus = false;

                IAccount checkingAccountInfo = new CheckingAccount()
                {
                    Accountno = accountbl.GenerateAccountno(),
                    Balance = accountbl.GetBalance(),
                    CustomerId = 1,
                    AccountStatus = true

                };

                checkingAccountInfo.AccountType = accountbl.GetAcountType(checkingAccountInfo);
                checkingAccountInfo.Interestrate = accountbl.GetIntrestrate(checkingAccountInfo);

                accountbl.OpenAccount(checkingAccountInfo);


                accountbl.CloseAccount(checkingAccountInfo);



                accountbl.Deposit(checkingAccountInfo, 1000m); //good test
                accountbl.Deposit(checkingAccountInfo, 2000m); //good test
                accountbl.Deposit(checkingAccountInfo, 1.0m); //good test

                accountbl.Deposit(checkingAccountInfo, -1000m); //bad test
                accountbl.Deposit(checkingAccountInfo, -1m); //bad test
                accountbl.Deposit(checkingAccountInfo, -1.0m); //bad test
                accountbl.Deposit(checkingAccountInfo, 0m); //bad test



                accountbl.Withdraw(checkingAccountInfo, 900m); //good test
                accountbl.Withdraw(checkingAccountInfo, 100m); //good test
                accountbl.Withdraw(checkingAccountInfo, 1.0m); //good test

                accountbl.Withdraw(checkingAccountInfo, -1000m); //bad test
                accountbl.Withdraw(checkingAccountInfo, -1m); //bad test
                accountbl.Withdraw(checkingAccountInfo, -1.0m); //bad test
                accountbl.Withdraw(checkingAccountInfo, 0m); //bad test

                Assert.AreEqual(expectedStatus, checkingAccountInfo.AccountStatus);
                Assert.AreEqual(expectedCheckingBalance, checkingAccountInfo.Balance);


                IAccount businessAccountInfo = new BusinessAccount()
                {
                    Accountno = accountbl.GenerateAccountno(),
                    Balance = accountbl.GetBalance(),
                    CustomerId = 1,
                    AccountStatus = true,
                };


                businessAccountInfo.AccountType = accountbl.GetAcountType(businessAccountInfo);
                businessAccountInfo.Interestrate = accountbl.GetIntrestrate(businessAccountInfo);

                accountbl.OpenAccount(businessAccountInfo);
                accountbl.CloseAccount(businessAccountInfo);



                accountbl.Deposit(businessAccountInfo, 2000m); //good test
                accountbl.Deposit(businessAccountInfo, 2000m); //good test
                accountbl.Deposit(businessAccountInfo, 1.0m); //good test

                accountbl.Deposit(businessAccountInfo, -1000m); //bad test
                accountbl.Deposit(businessAccountInfo, -1m); //bad test
                accountbl.Deposit(businessAccountInfo, -1.0m); //bad test
                accountbl.Deposit(businessAccountInfo, 0m); //bad test


                Assert.AreEqual(expectedStatus, businessAccountInfo.AccountStatus);
                Assert.AreEqual(expectedBusinessBalance, businessAccountInfo.Balance);//output 3000

                IAccount loanAccountInfo = new Loan(0)
                {
                    Accountno = accountbl.GenerateAccountno(),
                    Balance = accountbl.GetBalance(),
                    CustomerId = 1,
                    AccountStatus = true


                };


                loanAccountInfo.Interestrate = accountbl.GetIntrestrate(loanAccountInfo);

                accountbl.OpenAccount(loanAccountInfo);
                accountbl.CloseAccount(loanAccountInfo);

                accountbl.Deposit(loanAccountInfo, 2000m); //good test
                accountbl.Deposit(loanAccountInfo, 2000m); //good test
                accountbl.Deposit(loanAccountInfo, 1.0m); //good test

                accountbl.Deposit(loanAccountInfo, -1000m); //bad test
                accountbl.Deposit(loanAccountInfo, -1m); //bad test
                accountbl.Deposit(loanAccountInfo, -1.0m); //bad test
                accountbl.Deposit(loanAccountInfo, 0m); //bad test

                Assert.AreEqual(expectedStatus, loanAccountInfo.AccountStatus);
                Assert.AreEqual(expectedLoanBalance, loanAccountInfo.Balance);

                IAccount termdepositAccountInfo = new TermDeposit(0)
                {
                    Accountno = accountbl.GenerateAccountno(),
                    Balance = accountbl.GetBalance(),
                    CustomerId = 1,
                    AccountStatus = true


                };


                accountbl.OpenAccount(termdepositAccountInfo);
                accountbl.CloseAccount(termdepositAccountInfo);

                accountbl.Deposit(termdepositAccountInfo, 2000m); //good test
                accountbl.Deposit(termdepositAccountInfo, 2000m); //good test
                accountbl.Deposit(termdepositAccountInfo, 1.0m); //good test

                accountbl.Deposit(termdepositAccountInfo, -1000m); //bad test
                accountbl.Deposit(termdepositAccountInfo, -1m); //bad test
                accountbl.Deposit(termdepositAccountInfo, -1.0m); //bad test
                accountbl.Deposit(termdepositAccountInfo, 0m); //bad test

                Assert.AreEqual(expectedStatus, termdepositAccountInfo.AccountStatus);
                Assert.AreEqual(expectedTermdepositBalance, termdepositAccountInfo.Balance);


            }
        }
        #endregion
    }
}
