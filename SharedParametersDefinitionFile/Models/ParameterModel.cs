using System;
using System.ComponentModel.DataAnnotations;

namespace SharedParametersFile.Models
{
    public class ParameterModel : BaseModel
    {
        public Guid Guid { get; set; } = System.Guid.NewGuid();
        [Required]
        public string Name { get; set; }
        public string DataType { get; set; } = "TEXT";
        public int? DataCategory { get; set; }
        public int Group { get; set; } = 1;
        public bool Visible { get; set; } = true;
        public string Description { get; set; } 
        public bool UserModifiable { get; set; } = true;
        public bool HideWhenNoValue { get; set; } = false;
    }
}
