namespace Infrastructure.Models
{
    public class TrainingPrerequisiteModel : IModel
    {
        public byte PrerequisiteId { get; set; }
        public short TrainingId { get; set; }
    }
}
