using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace project123.Models
{
    public class Product

    {

        public string Name { get; set; }
        public double Price { get; set; }

        public Guid ID { get; set; }

        public string Description { get; set; }

        [NotMapped] public ImageSource QRCode { get; set; }


    }
}
