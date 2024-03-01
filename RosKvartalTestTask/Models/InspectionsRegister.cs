using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RosKvartalTestTask.Models
{
    //public enum StatusInspection { Planed, Assigned, Finished, Canceled }
    public enum ResultInspection { Unknown,Success, Failed }
    public class InspectionsRegister
    {
     
        public int Id { get; set; }

        //[Index(IsUnique = true)]
        //public required Guid RecordGuid { get; set; }
        public string SubjectName { get; set; }

        public string SubjectNumber { get; set; }

        public string? Purpose { get; set; }

        public string Status { get; set; }

        public ResultInspection Result { get; set; }

        [DataType(DataType.Date)]
        public DateTime CheckDate { get; set; }

    }
}
