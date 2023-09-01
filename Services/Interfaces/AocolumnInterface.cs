using XunitAssessment.Models;

namespace XunitAssessment.Services.Interfaces
{
    public interface AocolumnInterface
    {

        //Add a column for the AOTable record created in the above  API
        public Task<Aocolumn>AddColumn(Aocolumn column);

        //Delete a record
        public Task<Aocolumn> DeleteColumn(Guid Id);

        //Get all records with DataType "int" and "uniqueidentifier" for a particular Table by passing TableName(AOTable) as parameter
        public Task<IEnumerable<Aocolumn>> GetColumnsByType(string Name);
    }
}
