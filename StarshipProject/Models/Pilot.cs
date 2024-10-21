using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.SignalR;


namespace StarshipProject.Models
{
    public class Pilot
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}