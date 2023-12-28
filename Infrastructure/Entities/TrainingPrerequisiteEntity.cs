namespace Infrastructure.Entities
{
    public class TrainingPrerequisiteEntity : IEntity
    {
        public byte PrerequisiteId { get; set; }
        public short TrainingId { get; set; }
    }
}
