﻿namespace ApplicationCore.Entities
{
    public class Sponsor : BaseEntity
    {
        public int EventId { get; set; }
        public int SponsorId { get; set; }

        public virtual Event Event { get; private set; }
        public virtual ValueField ValueField { get; private set; }
    }
}
