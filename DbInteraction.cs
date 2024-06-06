using System.Formats.Asn1;

namespace HtmlToCsv2
{
    public class DbInteraction
    {
        private readonly DataToSql _dbContext;


        public DbInteraction(DataToSql dbContext)
        {
            _dbContext = dbContext;
          
        }
   
          public async Task AddAreas(Area area){
            await _dbContext.areas.AddAsync(area);
            await _dbContext.SaveChangesAsync();
          }
          public async Task AddCandidates(Candidate candidate){
            await _dbContext.candidates.AddAsync(candidate);
            await _dbContext.SaveChangesAsync();
          }

    }
}