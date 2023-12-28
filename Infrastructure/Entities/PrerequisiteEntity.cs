namespace Infrastructure.Entities
{
    public class PrerequisiteEntity : IEntity
    {
        public byte? PrerequisiteId { get; set; } = null;
        public string DocumentName { get; set; } = null;
    }
}
