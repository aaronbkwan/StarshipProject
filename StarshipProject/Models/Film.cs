using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.SignalR;


namespace StarshipProject.Models
{
    public class Film
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }

    
}