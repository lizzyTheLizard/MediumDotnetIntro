// Very Simple Query. Execution is defered as long as possible.
var deferedQuery = from city in City.Cities
                   where city.Population > 10_000_000
                   select city;

// This is the same in functioanl style
var deferedQuery2 = City.Cities.Where(city => city.Population > 10_000_000).Select(city => city);

// In functional style you can use additional methods
var deferedQuery3 = Country.Countries
    .SelectMany(country => country.Cities)
    .OfType<City>()
    .Zip(Country.Countries, (city, country) => $"Is {city.Name} in {country.Name}?")
    .Distinct()
    .Except(["Is Paris in France?"])
    .Take(2);


//Only if a result is needed, the query is executed. These are called 'Immediate Execution'
//If the query is executed multiple times, it is executed multiple times (no caching or something)
List<City> r2 = deferedQuery.ToList();
bool r1 = deferedQuery.All(x => x.Population > 10_000_000);
bool r3 = deferedQuery.Any(x => x.Population > 10_000_000);
int r4 = deferedQuery.Count();
double r5 = deferedQuery.Average(x => x.Population);
City r6 = deferedQuery.First();
City? r7 = deferedQuery.FirstOrDefault();

//Foreach and similar loops are also immediate execution
foreach (var city in deferedQuery)
{
    Console.WriteLine(city);
}

// Some operations are deferred, but if the result is needed all items are processed at once. There are called deferred non-streaming operations
IEnumerable<City> r8 = deferedQuery.Reverse();

//Some operations are deferred and processed one by one. These are called deferred streaming operations
IEnumerable<City> r9 = deferedQuery.Distinct();

// Query can be more complex
IEnumerable<City> complexQuery =
                   // First a number of 'from' clauses
                   from country in Country.Countries
                   from city in country.Cities

                       // Then a number of 'orderby' and 'where' clauses (can be mixed)
                   where city.Population > 10_000_000
                   orderby city.Population descending
                   where city.Name.Length > 2

                   //Then a 'select' clause
                   select city;

// Select can also be complex
IEnumerable<string> selectQuery = from country in Country.Countries
                                  select country.Name + " has " + country.Cities.Count + " cities";

// Instead of 'select' you can also use 'group by'
IEnumerable<IGrouping<string, Country>> groupByQuery = from country in Country.Countries
                                                       group country by country.Name;

//You can also create anonymous objects in select
var selectAnonymousQuery = from country in Country.Countries
                           select new { Name = country.Name, CityCount = country.Cities.Count };

// You can feed one query into another using 'into'
var intoQuery = from country in Country.Countries
                select country.Name + " has " + country.Cities.Count + " cities" into descriptions
                select descriptions.Length > 20 ? descriptions[20..] + "..." : descriptions;

// You can store the result of an expression using 'let'
var letQuery = from country in Country.Countries
               let shortName = country.Name[..10]
               group country by shortName;

// You can use 'join' to combine two collections
var joinQuery = from country in Country.Countries
                join city in City.Cities on country.Name equals city.Name
                group city by country;



//You can use subqueries
var queryGroupMax = from country in Country.Countries
                    let maxPopulation = (
                        from city in country.Cities
                        select city.Population
                    ).Max()
                    where maxPopulation > 10_000_000
                    select country.Name;
