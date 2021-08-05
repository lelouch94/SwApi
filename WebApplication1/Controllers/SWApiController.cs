using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SWApiController : ControllerBase
    {
        private static readonly HttpClient client = new HttpClient();

        private readonly ILogger<SWApiController> _logger;

        public SWApiController(ILogger<SWApiController> logger)
        {
            _logger = logger;
        }

        [HttpGet(nameof(GetStarshipID))]
        public async IAsyncEnumerable<object> GetStarshipID(int id)
        {
            string url = "https://swapi.dev/api/starships/" + id.ToString() + "/";
            var response = await client.GetAsync(url);
            var responseString = await response.Content.ReadAsStringAsync();
            object newStarship = JsonSerializer.Deserialize<object>(responseString);

            yield return newStarship;
        }

        [HttpGet(nameof(GetStarshipByName))]
        public async IAsyncEnumerable<object> GetStarshipByName(string name)
        {
            string url = "https://swapi.dev/api/starships/";
            var response = await client.GetAsync(url);
            var responseString = await response.Content.ReadAsStringAsync();
            AllStarships allStarships = JsonSerializer.Deserialize<AllStarships>(responseString);

            foreach (Starships starship in allStarships.results)
            {
                if (starship.name.Contains(' '))
                {
                    string[] personSurAndForName = starship.name.Split(' ');
                    for (int i = 0; i < personSurAndForName.Length; i++)
                    {
                        if (personSurAndForName[i].ToLower() == name.ToLower())
                        {
                            yield return starship;
                        }
                    }
                }
                if (starship.name.ToLower() == name.ToLower())
                {
                    yield return starship;
                }

            }

            yield return null;
        }

        [HttpGet(nameof(GetAllStarships))]
        public async IAsyncEnumerable<object> GetAllStarships()
        {
            string url = "https://swapi.dev/api/starships/";
            var response = await client.GetAsync(url);
            var responseString = await response.Content.ReadAsStringAsync();
            AllStarships currentPage = JsonSerializer.Deserialize<AllStarships>(responseString);

            bool running = true;
            if (currentPage.next != null)
            {
                List<Starships> tempStarship = new List<Starships>();
                while (running)
                {
                    response = await client.GetAsync(currentPage.next);
                    responseString = await response.Content.ReadAsStringAsync();
                    AllStarships nextPage = JsonSerializer.Deserialize<AllStarships>(responseString);
                    currentPage.next = new string(nextPage.next);
                    foreach (Starships starship in currentPage.results)
                    {
                        tempStarship.Add(starship);
                    }
                    foreach (Starships starship in nextPage.results)
                    {
                        tempStarship.Add(starship);
                    }
                    currentPage.results = tempStarship.ToArray();
                    tempStarship.Clear();
                    if (currentPage.results.Length == currentPage.count)
                    {
                        running = false;
                    }
                }
            }
            yield return currentPage;
        }

        [HttpGet(nameof(GetSpeciesID))]
        public async IAsyncEnumerable<object> GetSpeciesID(int id)
        {
            string url = "https://swapi.dev/api/species/" + id.ToString() + "/";
            var response = await client.GetAsync(url);
            var responseString = await response.Content.ReadAsStringAsync();
            object newSpecies = JsonSerializer.Deserialize<object>(responseString);

            yield return newSpecies;
        }

        [HttpGet(nameof(GetSpeciesByName))]
        public async IAsyncEnumerable<object> GetSpeciesByName(string name)
        {
            string url = "https://swapi.dev/api/species/";
            var response = await client.GetAsync(url);
            var responseString = await response.Content.ReadAsStringAsync();
            AllSpecies allSpecies = JsonSerializer.Deserialize<AllSpecies>(responseString);

            foreach (Species species in allSpecies.results)
            {
                if (species.name.Contains(' '))
                {
                    string[] personSurAndForName = species.name.Split(' ');
                    for (int i = 0; i < personSurAndForName.Length; i++)
                    {
                        if (personSurAndForName[i].ToLower() == name.ToLower())
                        {
                            yield return species;
                        }
                    }
                }

                if (species.name.ToLower() == name.ToLower())
                {
                    yield return species;
                }

            }

            yield return null;
        }

        [HttpGet(nameof(GetAllSpecies))]
        public async IAsyncEnumerable<object> GetAllSpecies()
        {
            string url = "https://swapi.dev/api/species/";
            var response = await client.GetAsync(url);
            var responseString = await response.Content.ReadAsStringAsync();
            AllSpecies currentPage = JsonSerializer.Deserialize<AllSpecies>(responseString);

            bool running = true;
            if (currentPage.next != null)
            {
                List<Species> tempSpecies = new List<Species>();
                while (running)
                {
                    response = await client.GetAsync(currentPage.next);
                    responseString = await response.Content.ReadAsStringAsync();
                    AllSpecies nextPage = JsonSerializer.Deserialize<AllSpecies>(responseString);
                    currentPage.next = new string(nextPage.next);
                    foreach (Species species in currentPage.results)
                    {
                        tempSpecies.Add(species);
                    }
                    foreach (Species species in nextPage.results)
                    {
                        tempSpecies.Add(species);
                    }
                    currentPage.results = tempSpecies.ToArray();
                    tempSpecies.Clear();
                    if (currentPage.results.Length == currentPage.count)
                    {
                        running = false;
                    }
                }
            }
            yield return currentPage;
        }

        //Refactor the methods above into single methods



        [HttpGet(nameof(SortPlanetsByDiameter))]
        public async IAsyncEnumerable<object> SortPlanetsByDiameter()
        {
            string url = "https://swapi.dev/api/planets/";
            var response = await client.GetAsync(url);
            var responseString = await response.Content.ReadAsStringAsync();
            AllPlanets currentPage = JsonSerializer.Deserialize<AllPlanets>(responseString);
            List<Planets> allPlanets = new List<Planets>();

            bool running = true;
            if (currentPage.next != null)
            {
                while (running)
                {
                    response = await client.GetAsync(currentPage.next);
                    responseString = await response.Content.ReadAsStringAsync();
                    AllPlanets nextPage = JsonSerializer.Deserialize<AllPlanets>(responseString);
                    currentPage.next = new string(nextPage.next);
                    foreach (Planets planet in currentPage.results)
                    {
                        allPlanets.Add(planet);
                    }
                    foreach (Planets planet in nextPage.results)
                    {
                        allPlanets.Add(planet);
                    }
                    currentPage.results = allPlanets.ToArray();
                    allPlanets.Clear();
                    if (currentPage.results.Length == currentPage.count)
                    {
                        running = false;
                    }
                }
            }

            allPlanets.AddRange(currentPage.results);
            bool didApositionChange = false;
            int i = 0;
            running = true;
            while (running)
            {
                if (i == 0)
                {
                    didApositionChange = false;
                }
                if (i + 1 < allPlanets.Count)
                {
                    if (allPlanets[i].diameter.ToLower().Equals("unknown"))
                    {
                        allPlanets[i].diameter = Int32.MaxValue.ToString();
                    }
                    if (allPlanets[i+1].diameter.ToLower().Equals("unknown"))
                    {
                        allPlanets[i+1].diameter = Int32.MaxValue.ToString();
                    }
                    if (Int32.Parse(allPlanets[i].diameter) > Int32.Parse(allPlanets[i + 1].diameter))
                    {
                        Planets placeholder = allPlanets[i];
                        allPlanets[i] = allPlanets[i + 1];
                        allPlanets[i + 1] = placeholder;
                        didApositionChange = true;
                    }
                }
                if (i == allPlanets.Count)
                {
                    if (didApositionChange)
                    {
                        i = 0;
                    }
                    else
                    {
                        running = false;
                    }
                }
                else
                {
                    i++;
                }
            }

            foreach(Planets planet in allPlanets)
            {
                if (planet.diameter.Equals(Int32.MaxValue.ToString()))
                {
                    planet.diameter = "unknown";
                }
            }
            yield return allPlanets.ToArray();
        }

        [HttpGet(nameof(GetRandomThing))]
        public async IAsyncEnumerable<object> GetRandomThing()
        {
            string url = "https://swapi.dev/api/";

            string[] classes = new string[]{ "people", "films", "starships", "vehicles", "species", "planets" };
            Random classValue = new Random();
            int randomNumber = classValue.Next(0, classes.Length);
            url += classes[randomNumber] + "/";

            var response = await client.GetAsync(url);
            var responseString = await response.Content.ReadAsStringAsync();

            switch (randomNumber)
            {
                case 0:
                    {
                        AllPeople allPeople = JsonSerializer.Deserialize<AllPeople>(responseString);
                        yield return allPeople.results[classValue.Next(0, allPeople.results.Length)];
                        break;
                    }
                case 1:
                    {
                        AllFilms allFilms = JsonSerializer.Deserialize<AllFilms>(responseString);
                        yield return allFilms.results[classValue.Next(0, allFilms.results.Length)];
                        break;
                    }
                case 2:
                    {
                        AllStarships allStarships = JsonSerializer.Deserialize<AllStarships>(responseString);
                        yield return allStarships.results[classValue.Next(0, allStarships.results.Length)];
                        break;
                    }
                case 3:
                    {
                        AllVehicles allVehicles = JsonSerializer.Deserialize<AllVehicles>(responseString);
                        yield return allVehicles.results[classValue.Next(0, allVehicles.results.Length)];
                        break;
                    }
                case 4:
                    {
                        AllSpecies allSpecies = JsonSerializer.Deserialize<AllSpecies>(responseString);
                        yield return allSpecies.results[classValue.Next(0, allSpecies.results.Length)];
                        break;
                    }
                case 5:
                    {
                        AllPlanets allPlanets = JsonSerializer.Deserialize<AllPlanets>(responseString);
                        yield return allPlanets.results[classValue.Next(0, allPlanets.results.Length)];
                        break;
                    }
            }
        }

        [HttpGet(nameof(Download))]
        public async Task Download(string path ,string type, int id)
        {
            string url = "https://swapi.dev/api/";
            url += type + "/" + id;
            var response = await client.GetAsync(url);
            var responseString = await response.Content.ReadAsStringAsync();
            
            if (path == null)
            {
                path = @"C:\Users\Max\Desktop\Testing\SWApi\Content.txt";
            }

            switch (type)
            {
                case "people":
                    {
                        if (id != 0)
                        {
                            FileController.CreateTextFile(path, responseString);
                        }else
                        {
                            url = "https://swapi.dev/api/";
                            string newUrl = url + type + "/";
                            response = await client.GetAsync(newUrl);
                            responseString = await response.Content.ReadAsStringAsync();


                            AllPeople currentPage = JsonSerializer.Deserialize<AllPeople>(responseString);
                            bool running = true;
                            if (currentPage.next != null)
                            {
                                List<People> tempPeople = new List<People>();
                                while (running)
                                {
                                    response = await client.GetAsync(currentPage.next);
                                    responseString = await response.Content.ReadAsStringAsync();
                                    AllPeople nextPage = JsonSerializer.Deserialize<AllPeople>(responseString);
                                    currentPage.next = new string(nextPage.next);
                                    foreach (People person in currentPage.results)
                                    {
                                        tempPeople.Add(person);
                                    }
                                    foreach (People person in nextPage.results)
                                    {
                                        tempPeople.Add(person);
                                    }
                                    currentPage.results = tempPeople.ToArray();
                                    tempPeople.Clear();
                                    if (currentPage.results.Length == currentPage.count)
                                    {
                                        running = false;
                                    }
                                }
                            }

                            FileController.CreateTextFile(path, currentPage.results);
                        }
                        break;
                    }

                /*case "films":
                    {
                        AllFilms allFilms = JsonSerializer.Deserialize<AllFilms>(responseString);
                        yield return allFilms.results[classValue.Next(0, allFilms.results.Length)];
                        break;
                    }
                case "starships":
                    {
                        AllStarships allStarships = JsonSerializer.Deserialize<AllStarships>(responseString);
                        yield return allStarships.results[classValue.Next(0, allStarships.results.Length)];
                        break;
                    }
                case "vehicles":
                    {
                        AllVehicles allVehicles = JsonSerializer.Deserialize<AllVehicles>(responseString);
                        yield return allVehicles.results[classValue.Next(0, allVehicles.results.Length)];
                        break;
                    }
                case "species":
                    {
                        AllSpecies allSpecies = JsonSerializer.Deserialize<AllSpecies>(responseString);
                        yield return allSpecies.results[classValue.Next(0, allSpecies.results.Length)];
                        break;
                    }
                case "planets":
                    {
                        AllPlanets allPlanets = JsonSerializer.Deserialize<AllPlanets>(responseString);
                        yield return allPlanets.results[classValue.Next(0, allPlanets.results.Length)];
                        break;
                    }*/
            }
        }

        [HttpGet(nameof(Rearange))]
        public void Rearange(string path)
        {
            if (path == null)
            {
                path = path = @"C:\Users\Max\Desktop\Testing\SWApi\Content.txt";
            }

            FileController.ReadAndRearrange(path);
        }

        [HttpGet(nameof(People))]
        public async IAsyncEnumerable<object> People(string searchCommand, string searchValue)
        {
            var response = await client.GetAsync("https://swapi.dev/api/people");
            var responseString = await response.Content.ReadAsStringAsync();
            AllPeople currentPage = JsonSerializer.Deserialize<AllPeople>(responseString);
            bool running = true;


            //This block is adding all page entries, without it, it would only display the first 10ish entries when doing a request.
            if (currentPage.next != null)
            {
                List<People> tempPeople = new List<People>();
                while (running)
                {
                    response = await client.GetAsync(currentPage.next);
                    responseString = await response.Content.ReadAsStringAsync();
                    AllPeople nextPage = JsonSerializer.Deserialize<AllPeople>(responseString);
                    currentPage.next = new string(nextPage.next);
                    tempPeople.AddRange(currentPage.results);
                    tempPeople.AddRange(nextPage.results);
                    currentPage.results = tempPeople.ToArray();
                    tempPeople.Clear();
                    if (currentPage.results.Length == currentPage.count)
                    {
                        running = false;
                    }
                }
            }

            switch (searchCommand)
            {
                case "name":
                    var allPeopleQuery = from ppl in currentPage.results
                                         where ppl.name.ToLower().Trim().Contains(searchValue.ToLower().Trim())
                                         select ppl;
                    yield return allPeopleQuery;
                    break;
                case "not":
                    allPeopleQuery = from ppl in currentPage.results
                                     where !ppl.name.ToLower().Trim().Contains(searchValue.ToLower().Trim())
                                     select ppl;
                    yield return allPeopleQuery;
                    break;
                case "all":
                    allPeopleQuery = from ppl in currentPage.results
                                     select ppl;
                    yield return allPeopleQuery;
                    break;
                case "hair_color":
                    allPeopleQuery = from ppl in currentPage.results
                                     where ppl.hair_color.ToLower().Trim().Equals(searchValue.ToLower().Trim())
                                     select ppl;
                    yield return allPeopleQuery;
                    break;
                case "skin_color":
                    allPeopleQuery = from ppl in currentPage.results
                                     where ppl.skin_color.ToLower().Trim().Equals(searchValue.ToLower().Trim())
                                     select ppl;
                    yield return allPeopleQuery;
                    break;
                case "eye_color":
                    allPeopleQuery = from ppl in currentPage.results
                                     where ppl.eye_color.ToLower().Trim().Equals(searchValue.ToLower().Trim())
                                     select ppl;
                    yield return allPeopleQuery;
                    break;
                case "birth_year":
                    allPeopleQuery = from ppl in currentPage.results
                                     where ppl.birth_year.ToLower().Trim().Equals(searchValue.ToLower().Trim())
                                     select ppl;
                    yield return allPeopleQuery;
                    break;
                case "gender":
                    allPeopleQuery = from ppl in currentPage.results
                                     where ppl.gender.ToLower().Trim().Equals(searchValue.ToLower().Trim())
                                     select ppl;
                    yield return allPeopleQuery;
                    break;
                case "id":
                    object newPersons = null;
                    try
                    {
                        string url = "https://swapi.dev/api/people/" + Int32.Parse(searchValue) + "/";
                        response = await client.GetAsync(url);
                        responseString = await response.Content.ReadAsStringAsync();
                        newPersons = JsonSerializer.Deserialize<object>(responseString);
                    }catch(Exception)
                    {

                    }
                    yield return newPersons;
                    break;
                case "orderByName":
                    allPeopleQuery = from ppl in currentPage.results
                                     orderby ppl.name
                                     select ppl;
                    yield return allPeopleQuery;
                    break;
                case "nameStartsWith":
                    allPeopleQuery = from ppl in currentPage.results
                                     where ppl.name.ToLower().StartsWith(searchValue.ToLower())
                                     select ppl;
                    yield return allPeopleQuery;
                    break;
                default:
                    break;
            }
        }

        [HttpGet(nameof(Planets))]
        public async IAsyncEnumerable<object> Planets(string searchCommand, string searchValue)
        {
            var response = await client.GetAsync("https://swapi.dev/api/planets");
            var responseString = await response.Content.ReadAsStringAsync();
            AllPlanets currentPage = JsonSerializer.Deserialize<AllPlanets>(responseString);
            bool running = true;


            //This block is adding all page entries, without it, it would only display the first 10ish entries when doing a request.
            if (currentPage.next != null)
            {
                List<Planets> tempPeople = new List<Planets>();
                while (running)
                {
                    response = await client.GetAsync(currentPage.next);
                    responseString = await response.Content.ReadAsStringAsync();
                    AllPlanets nextPage = JsonSerializer.Deserialize<AllPlanets>(responseString);
                    currentPage.next = new string(nextPage.next);
                    tempPeople.AddRange(currentPage.results);
                    tempPeople.AddRange(nextPage.results);
                    currentPage.results = tempPeople.ToArray();
                    tempPeople.Clear();
                    if (currentPage.results.Length == currentPage.count)
                    {
                        running = false;
                    }
                }
            }

            switch (searchCommand)
            {
                case "name":
                    var allPeopleQuery = from plnt in currentPage.results
                                         where plnt.name.ToLower().Trim().Contains(searchValue.ToLower().Trim())
                                         select plnt;
                    yield return allPeopleQuery;
                    break;
                case "not":
                    allPeopleQuery = from plnt in currentPage.results
                                     where !plnt.name.ToLower().Trim().Contains(searchValue.ToLower().Trim())
                                     select plnt;
                    yield return allPeopleQuery;
                    break;
                case "all":
                    allPeopleQuery = from plnt in currentPage.results
                                     select plnt;
                    yield return allPeopleQuery;
                    break;
                case "climate":
                    allPeopleQuery = from plnt in currentPage.results
                                     where plnt.climate.ToLower().Trim().Contains(searchValue.ToLower().Trim())
                                     select plnt;
                    yield return allPeopleQuery;
                    break;
                case "diameter":
                    allPeopleQuery = from plnt in currentPage.results
                                     where plnt.diameter.ToLower().Trim().Equals(searchValue.ToLower().Trim())
                                     select plnt;
                    yield return allPeopleQuery;
                    break;
                case "gravity":
                    allPeopleQuery = from plnt in currentPage.results
                                     where plnt.gravity.ToLower().Trim().Equals(searchValue.ToLower().Trim())
                                     select plnt;
                    yield return allPeopleQuery;
                    break;
                case "population":
                    allPeopleQuery = from plnt in currentPage.results
                                     where plnt.population.ToLower().Trim().Equals(searchValue.ToLower().Trim())
                                     select plnt;
                    yield return allPeopleQuery;
                    break;
                case "terrain":
                    allPeopleQuery = from plnt in currentPage.results
                                     where plnt.terrain.ToLower().Trim().Contains(searchValue.ToLower().Trim())
                                     select plnt;
                    yield return allPeopleQuery;
                    break;
                case "id":
                    object newPersons = null;
                    try
                    {
                        string url = "https://swapi.dev/api/planets/" + Int32.Parse(searchValue) + "/";
                        response = await client.GetAsync(url);
                        responseString = await response.Content.ReadAsStringAsync();
                        newPersons = JsonSerializer.Deserialize<object>(responseString);
                    }
                    catch (Exception)
                    {

                    }
                    yield return newPersons;
                    break;
                case "orderByName":
                    allPeopleQuery = from plnt in currentPage.results
                                     orderby plnt.name
                                     select plnt;
                    yield return allPeopleQuery;
                    break;
                case "nameStartsWith":
                    allPeopleQuery = from plnt in currentPage.results
                                     where plnt.name.ToLower().StartsWith(searchValue.ToLower())
                                     select plnt;
                    yield return allPeopleQuery;
                    break;
                default:
                    break;
            }
        }

        [HttpGet(nameof(Films))]
        public async IAsyncEnumerable<object> Films(string searchCommand, string searchValue)
        {
            var response = await client.GetAsync("https://swapi.dev/api/films");
            var responseString = await response.Content.ReadAsStringAsync();
            AllFilms currentPage = JsonSerializer.Deserialize<AllFilms>(responseString);
            bool running = true;


            //This block is adding all page entries, without it, it would only display the first 10ish entries when doing a request.
            if (currentPage.next != null)
            {
                List<Films> tempPeople = new List<Films>();
                while (running)
                {
                    response = await client.GetAsync(currentPage.next);
                    responseString = await response.Content.ReadAsStringAsync();
                    AllFilms nextPage = JsonSerializer.Deserialize<AllFilms>(responseString);
                    currentPage.next = new string(nextPage.next);
                    tempPeople.AddRange(currentPage.results);
                    tempPeople.AddRange(nextPage.results);
                    currentPage.results = tempPeople.ToArray();
                    tempPeople.Clear();
                    if (currentPage.results.Length == currentPage.count)
                    {
                        running = false;
                    }
                }
            }

            switch (searchCommand)
            {
                case "name":
                    var allEntryQuery = from entry in currentPage.results
                                         where entry.title.ToLower().Trim().Contains(searchValue.ToLower().Trim())
                                         select entry;
                    yield return allEntryQuery;
                    break;
                case "not":
                    allEntryQuery = from entry in currentPage.results
                                     where !entry.title.ToLower().Trim().Contains(searchValue.ToLower().Trim())
                                     select entry;
                    yield return allEntryQuery;
                    break;
                case "all":
                    allEntryQuery = from entry in currentPage.results
                                     select entry;
                    yield return allEntryQuery;
                    break;
                case "opening_crawl":
                    allEntryQuery = from entry in currentPage.results
                                     where entry.opening_crawl.ToLower().Trim().Contains(searchValue.ToLower().Trim())
                                     select entry;
                    yield return allEntryQuery;
                    break;
                case "id":
                    object newPersons = null;
                    try
                    {
                        string url = "https://swapi.dev/api/films/" + Int32.Parse(searchValue) + "/";
                        response = await client.GetAsync(url);
                        responseString = await response.Content.ReadAsStringAsync();
                        newPersons = JsonSerializer.Deserialize<object>(responseString);
                    }
                    catch (Exception)
                    {

                    }
                    yield return newPersons;
                    break;
                case "orderByName":
                    allEntryQuery = from plnt in currentPage.results
                                     orderby plnt.title
                                     select plnt;
                    yield return allEntryQuery;
                    break;
                case "orderByEpisode":
                    allEntryQuery = from entry in currentPage.results
                                    orderby entry.episode_id
                                    select entry;
                    yield return allEntryQuery;
                    break;
                case "nameStartsWith":
                    allEntryQuery = from plnt in currentPage.results
                                     where plnt.title.ToLower().StartsWith(searchValue.ToLower())
                                     select plnt;
                    yield return allEntryQuery;
                    break;
                default:
                    break;
            }
        }
        
        [HttpGet(nameof(Vehicles))]
        public async IAsyncEnumerable<object> Vehicles(string searchCommand, string searchValue)
        {
            var response = await client.GetAsync("https://swapi.dev/api/vehicles/");
            var responseString = await response.Content.ReadAsStringAsync();
            AllVehicles currentPage = JsonSerializer.Deserialize<AllVehicles>(responseString);
            bool running = true;


            //This block is adding all page entries, without it, it would only display the first 10ish entries when doing a request.
            if (currentPage.next != null)
            {
                List<Vehicles> tempPeople = new List<Vehicles>();
                while (running)
                {
                    response = await client.GetAsync(currentPage.next);
                    responseString = await response.Content.ReadAsStringAsync();
                    AllVehicles nextPage = JsonSerializer.Deserialize<AllVehicles>(responseString);
                    currentPage.next = new string(nextPage.next);
                    tempPeople.AddRange(currentPage.results);
                    tempPeople.AddRange(nextPage.results);
                    currentPage.results = tempPeople.ToArray();
                    tempPeople.Clear();
                    if (currentPage.results.Length == currentPage.count)
                    {
                        running = false;
                    }
                }
            }

            switch (searchCommand)
            {
                case "name":
                    var allEntryQuery = from entry in currentPage.results
                                        where entry.name.ToLower().Trim().Contains(searchValue.ToLower().Trim())
                                        select entry;
                    yield return allEntryQuery;
                    break;
                case "not":
                    allEntryQuery = from entry in currentPage.results
                                    where !entry.name.ToLower().Trim().Contains(searchValue.ToLower().Trim())
                                    select entry;
                    yield return allEntryQuery;
                    break;
                case "all":
                    allEntryQuery = from entry in currentPage.results
                                    select entry;
                    yield return allEntryQuery;
                    break;
                case "model":
                    allEntryQuery = from entry in currentPage.results
                                    where entry.model.ToLower().Trim().Contains(searchValue.ToLower().Trim())
                                    select entry;
                    yield return allEntryQuery;
                    break;
                case "id":
                    object newPersons = null;
                    try
                    {
                        string url = "https://swapi.dev/api/vehicles/" + Int32.Parse(searchValue) + "/";
                        response = await client.GetAsync(url);
                        responseString = await response.Content.ReadAsStringAsync();
                        newPersons = JsonSerializer.Deserialize<object>(responseString);
                    }
                    catch (Exception )
                    {

                    }
                    yield return newPersons;
                    break;
                case "orderByName":
                    allEntryQuery = from plnt in currentPage.results
                                    orderby plnt.name
                                    select plnt;
                    yield return allEntryQuery;
                    break;
                case "nameStartsWith":
                    allEntryQuery = from plnt in currentPage.results
                                    where plnt.name.ToLower().StartsWith(searchValue.ToLower())
                                    select plnt;
                    yield return allEntryQuery;
                    break;
                default:
                    break;
            }
        }

        /**
         * TODO:
         * -Add Species
         * -Add Starships
         * -Add Better Compare Function
         *  -->Perhaps a compare class so we can compare uncomparable things
         */
       
        [HttpGet(nameof(CompareStarships))]
        public async IAsyncEnumerable<object> CompareStarships(string starship1, string starship2, string compareParameter)
        {
            string url = "https://swapi.dev/api/starships/";
            var response = await client.GetAsync(url);
            var responseString = await response.Content.ReadAsStringAsync();
            AllStarships allStarships = JsonSerializer.Deserialize<AllStarships>(responseString);

            List<string> starshipList = new List<string>();
            foreach (Starships starship in allStarships.results)
            {
                Console.WriteLine(starship.name);
                if(starship.name.ToLower().Trim().Equals(starship1.ToLower().Trim()) || starship.name.ToLower().Trim().Equals(starship2.ToLower().Trim()))
                {
                    switch (compareParameter.ToLower().Trim())
                    {
                        case "length":
                            starshipList.Add(starship.name + " length:" + starship.length);
                            break;

                        case "manufacturer":
                            starshipList.Add(starship.name + " manufactorer:" + starship.manufacturer);
                            break;

                        case "cost in credits":
                            starshipList.Add(starship.name + " cost:" + starship.cost_in_credits);
                            break;

                        case "crew":
                            starshipList.Add(starship.name + " crew:" + starship.crew);
                            break;

                        case "passengers":
                            starshipList.Add(starship.name + " passengers:" + starship.passengers);
                            break;

                        default:
                            break;
                    }

                }
                if(starshipList.Count == 2)
                {
                    if(starshipList[0] != starship1)
                    {
                        string starshipTemp = starshipList[0];
                        starshipList[0] = starshipList[1];
                        starshipList[1] = starshipTemp;
                    }
                    
                    break;
                }
            }
            yield return starshipList;
        }
    }
}
