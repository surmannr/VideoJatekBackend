using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using VideoJatekBackend.Dal;
using VideoJatekBackend.Dal.Seed;
using static System.Formats.Asn1.AsnWriter;

namespace VideoJatekBackend.Services
{
    public class TimingService
    {
        private readonly VideogameDbContext dbContext;
        public TimingService(VideogameDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void LoadDataFromFile()
        {
            PublisherJsonFileProcessor.DeserializeDb(dbContext);
            VideogameJsonFileProcessor.DeserializeDb(dbContext);
        }
    }
}
