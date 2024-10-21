using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using StarshipProject.Models;

public class StarshipService
{
    private readonly StarshipContext _context;
    private readonly HttpClient _httpClient;


    public StarshipService(StarshipContext context, HttpClient httpClient)
    {
        _context = context;
        _httpClient = httpClient;
    }

    public async Task SeedAsync()
    {
        HttpClient client = new HttpClient();
        //check to see if the database is already seeded
        if (_context.Starships.Any())
        {
            Console.WriteLine("Starships table already contains data.");
            return;
        }

        //
        List<Starship>starships = await FetchStarshipsFromAPI(client);
        Console.WriteLine($"Attempting to add {starships.Count} starships to database.");

        foreach (var starship in starships)
        {
            _context.Entry(starship).State = EntityState.Added;
            await _context.SaveChangesAsync();
            await AddPilotsAndFilms(starship);
        }

        try
        {
            await _context.SaveChangesAsync();
            Console.WriteLine("Starships added to database.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during save: {ex.Message}");
        }
    }

    //retrieve starships from SWAPI
    private async Task<List<Starship>> FetchStarshipsFromAPI(HttpClient client)
    {
        List<Starship> starships = new List<Starship>();

        //asynchronously call SWAPI to get the list of starships
        var response = await client.GetStringAsync("https://swapi.dev/api/starships/");
        //deserialize the response from the API
        StarshipResponse starshipData = JsonConvert.DeserializeObject<StarshipResponse>(response);
        
        //iterate through each of the starships returned in the response and convert them to the starship object
        foreach (StarshipDTO ship in starshipData.Results)
        {
            Starship starship = ConvertToStarship(ship);
            starships.Add(starship);
        }

        return starships;
    }

    //pull pilot and films for the starships
    public async Task AddPilotsAndFilms(Starship starship)
    {
        Console.WriteLine($"Adding pilots and films for {starship.Name}");
        //First get pilots. Iterate through every pilot url
        foreach (string pilotUrl in starship.Pilots)
        {
            Console.WriteLine($"Fetching pilot from {pilotUrl}");
            //make the API call to retrieve a pilot
            Pilot pilot = await FetchFromApi<Pilot>(pilotUrl);

            //Check if the pilot already exists in the DB
            Pilot existingPilot = await _context.Pilots.FirstOrDefaultAsync(p => p.Name == pilot.Name);
            
            //if Pilot doesn't exist then add it to the Pilot DB
            if (existingPilot == null)
            {
                _context.Pilots.Add(pilot);
                await _context.SaveChangesAsync();
                existingPilot = pilot;
            }

            //add the Pilot and ship relation to the StarshipPilot DB
            _context.StarshipPilots.Add(new StarshipPilot{StarshipId = starship.Id, PilotId = existingPilot.Id});
            Console.WriteLine($"Finished adding pilots and films for {starship.Name}");
        }

        //now get each film
        foreach (var filmUrl in starship.Films)
        {
            Console.WriteLine($"Fetching film from {filmUrl}");
            var film = await FetchFromApi<Film>(filmUrl);
            var existingFilm = await _context.Films.FirstOrDefaultAsync(f => f.Title == film.Title);

            if (existingFilm == null)
            {
                _context.Films.Add(film);
                await _context.SaveChangesAsync();
                existingFilm = film;
            }

            _context.StarshipFilms.Add(new StarshipFilm { StarshipId = starship.Id, FilmId = existingFilm.Id });
        }
    }

    //fetches information from SWAPI
    private async Task<T> FetchFromApi<T>(string url)
    {
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var responseBody = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(responseBody);
    }
    

    //DTO to help convert the response from SWAPI to starship model
    private Starship ConvertToStarship(StarshipDTO dto)
    {
        return new Starship
        {
            Name = dto.Name ?? "Unknown",
            Model = dto.Model ?? "Unknown",
            Manufacturer = dto.Manufacturer ?? "Unknown",
            CostInCredits = int.TryParse(dto.CostInCredits?.Replace(",", ""), out int costInCredits) ? costInCredits : 0,
            Length = int.TryParse(dto.Length?.Replace(",", ""), out int length) ? length : 0,
            MaxAtmospheringSpeed = dto.MaxAtmospheringSpeed ?? "Unknown",
            Crew = dto.Crew ?? "Unknown",
            Passengers = int.TryParse(dto.Passengers?.Replace(",", ""), out int passengers) ? passengers : 0,
            CargoCapacity = int.TryParse(dto.CargoCapacity?.Replace(",", ""), out int cargoCapacity) ? cargoCapacity : 0,
            Consumables = dto.Consumables ?? "Unknown",
            HyperdriveRating = double.TryParse(dto.HyperdriveRating, out double hyperdriveRating) ? hyperdriveRating : 0,
            MGLT = int.TryParse(dto.MGLT, out int mglt) ? mglt : 0,
            StarshipClass = dto.StarshipClass ?? "Unknown",
            Pilots = dto.Pilots ?? new List<string>(),
            Films = dto.Films ?? new List<string>(),
            Created = DateTime.TryParse(dto.Created, out DateTime created) ? created : DateTime.MinValue,
            Edited = DateTime.TryParse(dto.Edited, out DateTime edited) ? edited : DateTime.MinValue,
            Url = dto.Url ?? "Unknown"
        };

    }

}

public class StarshipResponse
{
    public List<StarshipDTO> Results{get; set;}
}