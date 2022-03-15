using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Models
{
    public class OrderHeader
    {
        public int OrderHeaderId { get; set; }
        public string ApplicationuserId { get; set; }
        [ValidateNever]
        public ApplicationUser Applicationuser { get; set; }
        public DateTime DateOfOrder { get; set; }
        public DateTime DateOfShipping { get; set; }
        public double OrderTotal { get; set; }
        public string? OrderStatus { get; set; }
        public string? PaymentStatus { get; set; }
        public string? TrackingNumber { get; set; }
        public string? Carrior { get; set; }
        public string? SessionId { get; set; }
        public string? PaymentIntentId { get; set; }
        public DateTime DateOfPayment { get; set; }
        public DateTime DueDate { get; set; }
        [Required]
        public string Phon { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string PostalCode { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
