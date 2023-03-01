using System;
using Hotel.DataBase;
using Hotel.Interfaces;
using Hotel.Context;
using Hotel.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hotel.Controllers
{
    [ApiController]
    [Route("PromocodeAction")]
	public class PromocodeAction
	{
		public MainInterface _context;

		public PromocodeAction(MainInterface context)
		{
			_context = context;
		}
        [HttpPost("AddPromocode")]
        public void AddPromocode(PromocodeModel args)
        {
            var user = _context.users.FirstOrDefault(x => x.UserId == args.UserId);

            if(user.Role==Role.Admin && user.Online == true)
            {
                _context.promocodes.Add(new Promocode
                {
                    Sum = args.Sum,
                    PromocodeName = args.NamePromocode
                });

                Console.WriteLine("Successful!");
            }
            else
            {
                Console.WriteLine("Op's! You're not an admin or not online!");
            }
        }
        [HttpPost("UsePromocode")]
        public int UsePromocode() {

            int count = 0;

            int SumPromocode = 0;

            while (count < 5)
            {
                Console.WriteLine("Enter your promocode: ");

                string promocode = Console.ReadLine();

                var promo = _context.promocodes.FirstOrDefault(x => x.PromocodeName == promocode);

                if (promo != null)
                {
                    SumPromocode = promo.Sum;

                    Console.WriteLine("Successful!");

                    return SumPromocode;
                }

                count++;
            }
            Console.WriteLine("Op's! You used all 5 attempts!");

            return 0;

        }
        [HttpGet("ShowPromocode")]
        public void ShowPromocode()
        {
            foreach (Promocode promocode in _context.promocodes)
            {
                Console.WriteLine("Promocode: {0} Sum: {1}", promocode.PromocodeName, promocode.Sum);

            }
        }
    }
}

