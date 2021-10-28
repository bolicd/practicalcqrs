using System;
using System.ComponentModel;

namespace Infrastructure.Model.ReadModels
{
    public abstract class ReadModel
    {
        [Description("ignore")]
        public int Id { get; set; }

        public DateTime UpdatedAt { get; set; }

        public int Sequence { get; set; }

        public string EventId { get; set; }
    }
}
