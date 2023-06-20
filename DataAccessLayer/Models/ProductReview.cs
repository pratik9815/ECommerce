using DataAccessLayer.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class ProductReview
    {
        public Guid Id { get; set; }    
        public string CustomerId { get; set; }
        public string Message { get; set; }
        //One product can have many review
        public Product Product { get; set; }        
        public Guid ProductId { get; set; }
        public Rating Rating { get; set; }  


    }
}
