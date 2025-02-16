using System;

namespace BlazorEFIdentity.Models;

public class Transaction
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public decimal Balance { get; set; }
    public string Currency { get; set; }
    public bool IsReserved { get; set; }
    public string Message { get; set; }
    public decimal Amount { get; set; }
    public int AccountId { get; set; }
    public Account Account { get; set; }
}
