using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.SignalR;


namespace StarshipProject.Models
{
    public class Starship
    {
        [Key]
        public int Id{get; set;}
        public required string Name {get; set;}
        public required string Model {get; set;}
        public required string Manufacturer {get; set;}
        public int CostInCredits {get; set;}
        public int Length {get; set;}
        public required string MaxAtmospheringSpeed {get; set;}
        public required string Crew {get; set;}
        public int Passengers {get; set;}
        public int CargoCapacity {get; set;}
        public required string Consumables {get; set;}
        public double HyperdriveRating {get; set;}
        public int MGLT {get; set;}
        public required string StarshipClass {get; set;}
        public required List<string> Pilots {get; set;}
        public required List<string> Films{get; set;}
        public required DateTime Created {get; set;}
        public required DateTime Edited {get; set;}
        public required string Url {get; set;}
    }
}

