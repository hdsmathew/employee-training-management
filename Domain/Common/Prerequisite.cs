﻿namespace Core.Domain
{
    public class Prerequisite : IEntity
    {
        public byte PrerequisiteId { get; set; }
        public string DocumentName { get; set; }
    }
}
