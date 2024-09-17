using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using WebApplication1.Structure.Interfaces;

namespace WebApplication1.Structure.Data
{
    public class Event
        //(Guid Id, string EventName, string Description, DateTime Time, string Place, string Category)
    {
        [Required]
        public Guid EventId { get; set; }
        [Required]
        public string _EventName { get; set; }
        [Required]
        public string _Description { get; set; }
        [Required]
        public DateTime _Time { get; set; }
        [Required]
        public string _Place { get; set; }
        [Required]
        public string _Category { get; set; }
        public string FileName { get; set; }
        //-----------------------------------

    }
}
