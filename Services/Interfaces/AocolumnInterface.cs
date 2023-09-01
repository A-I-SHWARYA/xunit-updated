using XunitAssessment.Models;

namespace XunitAssessment.Services.Interfaces
{
    public interface AocolumnInterface
    {
        public Task<Aocolumn>AddColumn(Aocolumn column);
        public Task<Aocolumn> DeleteColumn(Guid Id);
        public Task<IEnumerable<Aocolumn>> GetColumnsByType(string Name);
    }
}
