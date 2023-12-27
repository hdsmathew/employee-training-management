namespace Infrastructure.Entities
{
    public class TrainingPrerequisiteEntity : IEntity
    {
        public byte PrerequisiteId { get; set; }
        public ushort TrainingId { get; set; }
    }
}
