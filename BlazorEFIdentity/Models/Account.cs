using System;
using BlazorEFIdentity.Data;

namespace BlazorEFIdentity.Models;

public class Account
{
    public int Id { get; set; }
    public string AccountNumber { get; set; }
    public string AccountName { get; set; }
    public decimal Balance { get; set; }
    public string AccountType { get; set; }
    public bool IsActive { get; set; }
    public string CardNumber { get; set; }
    public string ApplicationUserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; }
    public List<Transaction> Transactions { get; set; }
}
