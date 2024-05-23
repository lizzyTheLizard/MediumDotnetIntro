record Country(string Name, double Area, long Population, List<City> Cities)
{
    public static Country[] Countries = [
    new Country("Vatican City", 0.44, 526, [new City("Vatican City", 826)]),
        new Country("Monaco", 2.02, 38_000, [new City("Monte Carlo", 38_000)]),
        new Country("Nauru", 21, 10_900, [new City("Yaren", 1_100)]),
        new Country("Tuvalu", 26, 11_600, [new City("Funafuti", 6_200)]),
        new Country("San Marino", 61, 33_900, [new City("San Marino", 4_500)]),
        new Country("Liechtenstein", 160, 38_000, [new City("Vaduz", 5_200)]),
        new Country("Marshall Islands", 181, 58_000, [new City("Majuro", 28_000)]),
        new Country("Saint Kitts & Nevis", 261, 53_000, [new City("Basseterre", 13_000)])
];
}