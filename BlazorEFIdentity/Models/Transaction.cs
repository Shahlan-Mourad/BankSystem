using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorEFIdentity.Models;

public class Transaction
{
    [Key]
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public decimal Balance { get; set; }
    public string Currency { get; set; } = "SEK";
    public bool IsReserved { get; set; }
    public string? Message { get; set; }
    public decimal Amount { get; set; }
    public int AccountId { get; set; }
    [ForeignKey("AccountId")]
    public Account Account { get; set; } = new Account();
}
