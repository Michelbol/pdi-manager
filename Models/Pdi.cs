using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace PdiManager.Models
{
    public class Pdi
    {
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public bool IsDone { get; set; }
        public IdentityUser User { get; set; }
        
        public List<Task> Tasks { get; set; }
        [Column(TypeName="Date")]
        public DateTime StartAt { get; set; }
        [Column(TypeName="Date")]
        public DateTime EndAt { get; set; }
        
    }
}