using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.SignalR;


namespace StarshipProject.Models
{
    //class is to cover the many to many relationship between pilot and starship
    public class StarshipPilot
    {

        public int StarshipId { get; set; }
        public Starship Starship { get; set; }

        public int PilotId { get; set; }
        public Pilot Pilot { get; set; }
    }
}