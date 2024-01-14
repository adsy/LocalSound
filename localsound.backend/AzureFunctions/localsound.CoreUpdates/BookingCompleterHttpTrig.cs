using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using localsound.CoreUpdates.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace localsound.CoreUpdates
{
    public class BookingCompleterHttpTrig
    {
        private readonly LocalSoundDbContext _dbContext;

        public BookingCompleterHttpTrig(LocalSoundDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [FunctionName("BookingCompleterHttpTrig")]
        public async Task Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "booking-completer/http")] HttpRequest req,
            ILogger log)
        {
            var bookingCount = await _dbContext.ArtistBooking.CountAsync(x => x.BookingConfirmed == true && !x.BookingCompleted && x.BookingDate < DateTime.Now);

            var iteration = 0;
            while (iteration * 500 < bookingCount)
            {
                var bookings = await _dbContext.ArtistBooking.Where(x => x.BookingConfirmed == true && !x.BookingCompleted && x.BookingDate < DateTime.Now)
                    .Skip(iteration * 500)
                    .Take(500)
                    .ToListAsync();

                foreach (var booking in bookings)
                {
                    booking.BookingCompleted = true;
                }

                await _dbContext.SaveChangesAsync();

                iteration++;
            }
        }
    }
}
