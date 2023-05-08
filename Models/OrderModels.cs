using System;
namespace Hotel.Models
{
	public class OrderModels
	{
         public Guid OrderId { get; set; }

         public Guid UserId { get; set; }

         public int TotalPrice { get; set; }

         public string OrderTime { get; set; }

         public string Promocode { get; set; }
    }
}
