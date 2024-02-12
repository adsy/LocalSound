using localsound.backend.Domain.Enum;
using localsound.backend.Domain.Model.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace localsound.backend.Persistence.DbContext
{
    public class Seed
    {
        public static async Task SeedData(LocalSoundDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole<Guid>> roleManager)
        {
            //if (!roleManager.Roles.Any())
            //{
            //    await roleManager.CreateAsync(new IdentityRole<Guid>(CustomerTypeEnum.NonArtist.ToString()));
            //    await roleManager.CreateAsync(new IdentityRole<Guid>(CustomerTypeEnum.Artist.ToString()));
            //}

            if (!(await context.AccountImageType.AnyAsync())){
                await context.AccountImageType.AddAsync(new AccountImageType
                {
                    AccountImageTypeId = AccountImageTypeEnum.ProfileImage,
                    AccountImageTypeName = "ProfileImage"
                });
                await context.AccountImageType.AddAsync(new AccountImageType
                {
                    AccountImageTypeId = AccountImageTypeEnum.CoverImage,
                    AccountImageTypeName = "CoverImage"
                });

                await context.SaveChangesAsync();
            }

            if (!(await context.Genres.AnyAsync()))
            {
                await context.Genres.AddRangeAsync(new List<Genre>
                {
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Blues" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Jazz" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Rock" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Classic Rock" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Alternative Rock" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Punk Rock" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Hard Rock" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Progressive Rock" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Indie Rock" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Grunge" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Garage Rock" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Psychedelic Rock" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Folk Rock" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Blues Rock" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Southern Rock" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Emo" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Post-Punk" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "New Wave" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Pop Punk" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Noise Rock" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Math Rock" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Stoner Rock" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Surf Rock" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Indie Folk" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Art Rock" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Rockabilly" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Ska Punk" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Metalcore" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Hardcore Punk" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Deathcore" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Melodic Hardcore" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Christian Rock" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Experimental Rock" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Space Rock" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Pop" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "R&B (Rhythm and Blues)" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Hip Hop" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Country" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Electronic" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Reggae" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Folk" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Classical" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Heavy Metal" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Ska" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Funk" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Gospel" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Punk" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Disco" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "World Music" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Experimental" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Post-Rock" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Shoegaze" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Emo" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Trap" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "K-pop" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "J-pop" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Metalcore" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Industrial" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Ambient" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Chiptune" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Neoclassical" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Electronic" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Techno" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "House" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Bass house" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Trance" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Dubstep" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Drum and Bass" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Ambient" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Electro" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Breakbeat" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Synthwave" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "IDM (Intelligent Dance Music)" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Chillout" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Hardstyle" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Gabber" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Progressive House" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Tech House" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Deep House" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Minimal Techno" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Future Bass" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Downtempo" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Electroswing" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Nu-disco" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Trip Hop" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Industrial" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Synthpop" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Glitch Hop" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Psytrance" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Progressive psytrance" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "UK Garage" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Grime" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Vaporwave" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Big Beat" },
                    new Genre { GenreId = Guid.NewGuid(), GenreName = "Wave" }
                });
                await context.SaveChangesAsync();
            }

            if (!(await context.EventType.AnyAsync()))
            {
                await context.EventType.AddRangeAsync(new List<EventType>
                {
                    new EventType
                    {
                        EventTypeId = Guid.NewGuid(),
                        EventTypeName = "Wedding"
                    },
                    new EventType
                    {
                        EventTypeId = Guid.NewGuid(),
                        EventTypeName = "House party"
                    },
                    new EventType
                    {
                        EventTypeId = Guid.NewGuid(),
                        EventTypeName = "Religious event"
                    },
                    new EventType
                    {
                        EventTypeId = Guid.NewGuid(),
                        EventTypeName = "Club residency"
                    },
                    new EventType
                    {
                        EventTypeId = Guid.NewGuid(),
                        EventTypeName = "Festival"
                    }
                });
                await context.SaveChangesAsync();
            }
        }
    }
}
