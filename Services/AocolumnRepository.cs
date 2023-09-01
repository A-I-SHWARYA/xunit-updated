using Microsoft.EntityFrameworkCore;
using XunitAssessment.Models;
using XunitAssessment.Services.Interfaces;

namespace XunitAssessment.Services
{
    public class AocolumnRepository:AocolumnInterface
    {
        private readonly DemoContext demoContext;
        public AocolumnRepository(DemoContext demoContext)
        {
            this.demoContext = demoContext;
        }

        public async Task<Aocolumn> AddColumn(Aocolumn column)
        {
            
            
               
                    await demoContext.Aocolumns.AddAsync(column);
                    await demoContext.SaveChangesAsync();
                    return column;
              
            
            

        }


        //Delete a record
        public async Task<Aocolumn> DeleteColumn(Guid Id)
        {
            var deletecolumn = await demoContext.Aocolumns.FindAsync(Id);
            if (deletecolumn != null)
            {
                demoContext.Aocolumns.Remove(deletecolumn);
                await demoContext.SaveChangesAsync();
                return deletecolumn;
            }
            else
            {
                return null;
            }

        }


        public async Task<IEnumerable<Aocolumn>> GetColumnsByType(string Name)
        {
            var records = await demoContext.Aocolumns
                .Where(r => (r.DataType == "int" || r.DataType == "uniqueidentifier") && r.Table.Name == Name)
                .ToListAsync();

            return records;
        }
    }
}
