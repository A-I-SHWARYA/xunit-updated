using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using XunitAssessment.Models;
using XunitAssessment.Services.Interfaces;

namespace XunitAssessment.Services
{
    public class AotableRepository:AotableInterface
    {
        private readonly DemoContext demoContext;
        public AotableRepository(DemoContext demoContext)
        {
            this.demoContext = demoContext;
        }


        //Add record
        public async Task<Aotable> AddTable(Aotable aotable)
        {
            if (aotable != null)
            {
                await demoContext.Aotables.AddAsync(aotable);
                await demoContext.SaveChangesAsync();
                return aotable;
            }
            else
            {
                return null;
            }
            
        }


        //Edit a record
        public async Task<Aotable> EditTable(Guid Id, Aotable aotable)
        {
            var existingtable = await demoContext.Aotables.SingleOrDefaultAsync(option => option.Id == Id);
            if (existingtable != null)
            {
                //existingtable.Id = aotable.Id;
                existingtable.Type = aotable.Type;
                existingtable.Description = aotable.Description;
                existingtable.Comment = aotable.Comment;
                existingtable.History = aotable.History;
                existingtable.Boundary = aotable.Boundary;
                existingtable.Log= aotable.Log;
                existingtable.Cache = aotable.Cache;
                existingtable.Notify = aotable.Notify;
                existingtable.Identifier = aotable.Identifier;
                await demoContext.SaveChangesAsync();
                return aotable;
            }
            else
            {
                return null;
            }
        }






        //Get all records of Type "coverage" and "form". This Type needs to be a parameter.
        public async Task<IEnumerable<Aotable>> GetByType(string Type)
        {
            var records = await demoContext.Aotables
                .Where(r => (r.Type == "coverage" || r.Type == "form") && r.Type == Type)
                .ToListAsync();

            return records != null ? records : Enumerable.Empty<Aotable>();
        }



        //Get all records which has a particular word in the Name. The word needs to be a parameter.

        public async Task<IEnumerable<Aotable>> CheckName(string Name)
        {
            var record = await demoContext.Aotables.Include(t => t.Aocolumns).Where(t => t.Name == Name).ToListAsync();
            return record != null ? record : Enumerable.Empty<Aotable>();
        }
    }
}
