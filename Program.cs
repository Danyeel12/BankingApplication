using Microsoft.VisualBasic;
using System.Reflection.Metadata.Ecma335;
using System;
using Newtonsoft.Json;
using System.Security.Principal;
using System.Threading.Channels;

namespace Banking_Application
{
   
    class Person
    {
        //Fields
        private string _Password;
        public readonly string SIN;

        //Properties
        public bool IsAuthenticated   { get; private set; }
        public string Name { get; private set; }
        //constructor
        public Person(string name, string sin)
            => (Name, SIN, _Password) = (name, sin, sin.Substring(0, 3));

        //Methods
        public void Login(string password)
        {

            if (password != _Password)
            {
                throw new AccountException("Invalid password.");
            }
            IsAuthenticated = true;
        }
        public void Logout()
        {
            IsAuthenticated = false;
        }
        public override string ToString()
            => $"{Name} {(IsAuthenticated ? "is authenticated" : "is not authenticated")}";

    }
    struct Transaction
    {
        //properties
        public string AccountNumber { get; }
        public double Amount { get; }
        public double EndBalance { get; }
        public Person Originator { get; }
        public DateTime Time { get; }

        //methods
        public Transaction(string accountNumber, double amount, double endBalance, Person person, DateTime time)
            => (AccountNumber, Amount, EndBalance, Originator, Time) = (accountNumber, amount, endBalance, person, time);

        public override string ToString()
        {
            string type = Amount > 0 ? "Deposit" : "Withdraw";
            return $"{Time.ToShortTimeString()} {Originator.Name,-10} {type,10} {Math.Abs(Amount),12:C2} {EndBalance,12:C2}"; //include withdraw or deposit in the output
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            //testing the visa account
            Console.WriteLine("\nAll acounts:");
            Bank.PrintAccounts();
            Console.WriteLine("\nAll Users:");
            Bank.PrintPersons();
            Person p0, p1, p2, p3, p4, p5, p6, p7, p8, p9, p10;
            p0 = Bank.GetPerson("Janani");
            p1 = Bank.GetPerson("Ilia");
            p2 = Bank.GetPerson("Tom");
            p3 = Bank.GetPerson("Syed");
            p4 = Bank.GetPerson("Arben");
            p5 = Bank.GetPerson("Patrick");
            p6 = Bank.GetPerson("Yin");
            p7 = Bank.GetPerson("Hao");
            p8 = Bank.GetPerson("Jake");
            p9 = Bank.GetPerson("Joanne");
            p10 = Bank.GetPerson("Nicoletta");
            p0.Login("123"); p1.Login("234");
            p2.Login("345"); p3.Login("456");
            p4.Login("567"); p5.Login("678");
            p6.Login("789"); p7.Login("890");
            p10.Login("234"); p8.Login("901");
            //a visa account
            VisaAccount a = Bank.GetAccount("VS-100000") as VisaAccount;
            a.DoPayment(1500, p0);
            a.DoPurchase(200, p1);
            a.DoPurchase(25, p2);
            a.DoPurchase(15, p0);
            a.DoPurchase(39, p1);
            a.DoPayment(400, p0);
            Console.WriteLine(a);
            a = Bank.GetAccount("VS-100001") as VisaAccount;
            a.DoPayment(500, p0);
            a.DoPurchase(25, p3);
            a.DoPurchase(20, p4);
            a.DoPurchase(15, p5);
            Console.WriteLine(a);

            //SAVINGS ACCOUNT

            SavingsAccount b = (SavingsAccount)Bank.GetAccount("SV-100002");
            
            b.Withdraw(300, p6);
            b.Withdraw(32.90, p6);
            b.Withdraw(50, p7);
            b.Withdraw(111.11, p8);
            Console.WriteLine(b);

            b = (SavingsAccount)Bank.GetAccount("SV-100003");
            b.Deposit(300, p3);     //ok even though p3 is not a holder
            b.Deposit(32.90, p2);
            b.Deposit(50, p5);
            b.Withdraw(111.11, p10);
            Console.WriteLine(b);



            //a checking account
            CheckingAccount c = (CheckingAccount)Bank.GetAccount("CK-100004");
            c.Deposit(33.33, p7);
            c.Deposit(40.44, p7);
            c.Withdraw(150, p2);
            c.Withdraw(200, p4);
            c.Withdraw(645, p6);
            c.Withdraw(350, p6);
            Console.WriteLine(c);

            c = (CheckingAccount)Bank.GetAccount("CK-100005");
            c.Deposit(33.33, p8);
            c.Deposit(40.44, p7);
            c.Withdraw(450, p10);
            c.Withdraw(500, p8);
            c.Withdraw(645, p10);
            c.Withdraw(850, p10);
            Console.WriteLine(c);

            a = Bank.GetAccount("VS-100006") as VisaAccount;
            a.DoPayment(700, p0);
            a.DoPurchase(20, p3);
            a.DoPurchase(10, p1);
            a.DoPurchase(15, p1);
            Console.WriteLine(a);

            b = (SavingsAccount)Bank.GetAccount("SV-100007");
            b.Deposit(300, p3); //ok even though p3 is not a holder
            b.Deposit(32.90, p2);
            b.Deposit(50, p5);
            b.Withdraw(111.11, p7);
            Console.WriteLine(b);

            Console.WriteLine("\n\nExceptions:");


            //The following will cause exception
            try
            {
                p8.Login("911");//incorrect password
            }
            catch (AccountException e) { Console.WriteLine(e.Message); }

            try
            {
                p3.Logout();
                a.DoPurchase(12.5, p3);     //exception user is not logged in
            }
            catch (AccountException e) { Console.WriteLine(e.Message); }

            try
            {
                a.DoPurchase(12.5, p0); //user is not associated with this account
            }
            catch (AccountException e) { Console.WriteLine(e.Message); }

            try
            {

                a.DoPurchase(5825, p4); //credit limit exceeded
            }
            catch (AccountException e) { Console.WriteLine(e.Message); }


            try
            {
                c.Withdraw(1500, p5); //no overdraft
            }
            catch (AccountException e) { Console.WriteLine(e.Message); }

            try
            {
                Bank.GetAccount("CK-100018"); //account does not exist
            }
            catch (AccountException e) { Console.WriteLine(e.Message); }

            try
            {
                Bank.GetPerson("Trudeau"); //user does not exist
            }
            catch (AccountException e) { Console.WriteLine(e.Message); }

            foreach (Account account in Bank.accounts)
            {
                Console.WriteLine("\nBefore PrepareMonthlyReport()");
                Console.WriteLine(account);
                Console.WriteLine("\nAfter PrepareMonthlyReport()");
                account.PrepareMonthlyReport(); //all transactions are cleared, balance changes
                Console.WriteLine(account);

                //Bank.SaveAccounts("Accounts.txt");

            }
        }

    }
}
