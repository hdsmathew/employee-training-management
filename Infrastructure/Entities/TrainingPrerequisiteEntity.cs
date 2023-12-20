namespace Infrastructure.Entities
{
    public class TrainingPrerequisiteEntity : EntityBase
    {
        public byte PrerequisiteId { get; set; }
        public ushort TrainingId { get; set; }
    }
}
