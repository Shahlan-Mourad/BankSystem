using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using BlazorEFIdentity.Data;

namespace BlazorEFIdentity.Models;

public class Account
{
    [Key]
    public int Id { get; set; }
    public string AccountNumber { get; set; } = string.Empty;
    public string AccountName { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public string? AccountType { get; set; }
    public bool IsActive { get; set; } = true;
    public string CardNumber { get; set; } = string.Empty;
    public string ApplicationUserId { get; set; } = string.Empty;
    [ForeignKey("ApplicationUserId ")]
    public ApplicationUser? ApplicationUser { get; set; }
     public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>(); // Initiera med en tom lista
}
