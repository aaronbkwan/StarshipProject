using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.SignalR;



namespace StarshipProject.Models
{
    //class is to setup the many to many relatonship between starships and filmms
    public class StarshipFilm
    {
        public int StarshipId { get; set; }
        public Starship Starship { get; set; }

        public int FilmId { get; set; }
        public Film Film { get; set; }
    }

}