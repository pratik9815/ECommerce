﻿using DataAccessLayer.Common;
using DataAccessLayer.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Customer : AuditableEntity
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }

        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public Gender Gender { get; set; }
        public DateTime DOB { get; set; } 

        //for order relationship
        public ICollection<Order> Orders { get; set; }

    }
}
