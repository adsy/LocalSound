using System;
using System.Linq;
using System.Threading.Tasks;
using localsound.CoreUpdates.Persistence;
using Microsoft.Azure.WebJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace localsound.CoreUpdates
{
    public class BookingCompleterTimerTrig
    {
        private readonly LocalSoundDbContext _dbContext;

        public BookingCompleterTimerTrig(LocalSoundDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [FunctionName("BookingCompleter")]
        public async Task Run([TimerTrigger("0 10 0 * * *")]TimerInfo myTimer, ILogger log)
        {
            var bookingCount = await _dbContext.ArtistBooking.CountAsync(x => x.BookingConfirmed == true && x.BookingDate.Date <= DateTime.Now.Date);
            
            var iteration = 0;
            while (iteration*500 < bookingCount)
            {
                var bookings = await _dbContext.ArtistBooking.Where(x => x.BookingConfirmed == true && x.BookingDate.Date <= DateTime.Now.Date)
                    .Skip(iteration * 500)
                    .Take(500)
                    .ToListAsync();

                foreach(var booking in bookings)
                {
                    booking.BookingCompleted = true;
                }

                await _dbContext.SaveChangesAsync();

                iteration++;
            }
        }
    }
}
