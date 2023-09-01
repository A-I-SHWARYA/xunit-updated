using XunitAssessment.Models;

namespace XunitAssessment.Services.Interfaces
{
    public interface AotableInterface
    {
        //Add record
        public Task<Aotable> AddTable(Aotable aotable);

        //Edit a record
        public Task<Aotable> EditTable(Guid Id, Aotable aotable);


        //Get all records which has a particular word in the Name. The word needs to be a parameter.
        public Task<IEnumerable<Aotable>> CheckName(string Name);


        //Get all records of Type "coverage" and "form". This Type needs to be a parameter.
        public Task<IEnumerable<Aotable>> GetByType(string Type);
    }
}
