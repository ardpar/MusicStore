﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MainMusicStore.Models.DbModels
{
    public class OrderHeader
    {
        [Key]
        public int Id { get; set; }

        public string ApplicationUserId { get; set; }

        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        public DateTime ShippingDate { get; set; }
        [Required]
        public double OrderTotal { get; set; }

        public string TrackingNumber { get; set; }
        public string Carrier { get; set; }
        public string OrderStatus { get; set; }

        public string PaymentStatus { get; set; }
        public DateTime PaymentDueDate { get; set; }
        public string TransectionId { get; set; }
        public string PhoneNumber { get; set; }
        public string StreetAdress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Name { get; set; }
    }
}
