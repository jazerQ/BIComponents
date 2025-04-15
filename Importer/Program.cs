using System.Globalization;
using System.Text;
using Core.Models;
using CsvHelper;
using CsvHelper.Configuration;
using Dapper;
using Npgsql;

string csvPath = @"Амортизаторы Remo Hobby (1).csv";

string connectionString = "Host=localhost;Username=postgres;Password=admin;Database=BI";

var config = new CsvConfiguration(CultureInfo.InvariantCulture)
{
    Delimiter = ",",
    Encoding = Encoding.UTF8,
    HasHeaderRecord = true
};

using var reader = new StreamReader(csvPath);
using var csv = new CsvReader(reader, config);

var products = csv.GetRecords<Damper>().ToList();

var conn = new NpgsqlConnection(connectionString);

var productsTable = products.Select(p =>
{
    var defaultPriceInt = int.Parse(p.DefaultPrice.Replace("\u20bd", "").Replace("\u2009", ""));
    var priceWithCardInt = string.IsNullOrEmpty(p.PriceWithCard) ? 0 : int.Parse(p.PriceWithCard.Replace("\u20bd", "").Replace("\u2009", ""));
    return new
    {
        p.Id,
        p.Name,
        p.Link,
        defaultPriceInt,
        priceWithCardInt,
        p.ProductRating,
        p.CountOfComments,
        p.CountOfQuestions,
        p.Article,
        p.Country,
    };
}).ToList();

var modelsTable = products.Select(p =>
{

    var type = DamperType.Types.IndexOf(p.Type) + 1;
    return new
    {
        p.PartNumber,
        p.Article,
        type,
        p.Color,
        p.Transport,
        p.TypeOfControl,
        p.Scale,
        p.ActionRadius,
        p.Peculiarities
    };
}).ToList();

conn.Open();
conn.Execute(@"DROP TABLE IF EXISTS Types CASCADE");
conn.Execute(@"CREATE TABLE Types (
    id SERIAL PRIMARY KEY ,
    type varchar(255)
    )");
conn.Execute(@"DROP TABLE IF EXISTS Products");
conn.Execute(@"CREATE TABLE Products(
    id SERIAL PRIMARY KEY,
    Name varchar(255),
    Link TEXT,
    DefaultPrice int,
    PriceWithCard int,
    ProductRating real,
    CountOfComments int,
    CountOfQuestions int,
    article BIGINT UNIQUE, 
    Country varchar(255)
)");

conn.Execute(@"DROP TABLE IF EXISTS Models");
conn.Execute(@"CREATE TABLE Models(
    article BIGINT PRIMARY KEY,
    part_number varchar(255) ,
    color varchar(255),
    transport varchar(255),
    type_of_control varchar(255),
    scale varchar(255),
    action_radius varchar(255),
    peculiarities varchar(255),
    type_id INT,
    FOREIGN KEY (type_id) REFERENCES Types(id),
    FOREIGN KEY (article) REFERENCES Products(article)
)");

foreach (var type in DamperType.Types)
{
    conn.Execute(@"INSERT INTO Types (type) VALUES (@type)", new {type});
}
conn.Execute(@"INSERT INTO Products(id,
    Name,
    Link ,
    DefaultPrice,
    PriceWithCard,
    ProductRating ,
    CountOfComments,
    CountOfQuestions,
    article,
    Country) VALUES (@Id, @Name, @Link, @defaultPriceInt, @priceWithCardInt, @ProductRating, @CountOfComments, @CountOfQuestions, @Article, @Country)",
    productsTable);

conn.Execute(@"INSERT INTO Models 
(part_number, article, color, transport, type_of_control, scale, action_radius, peculiarities, type_id)
VALUES (@PartNumber, @Article, @Color, @Transport, @TypeOfControl, @Scale, @ActionRadius, @Peculiarities, @type)",
    modelsTable);



Console.WriteLine("Успешно сохранилось!");
// foreach (var product in products)
// {
//     Console.WriteLine($"{product.Id} {product.Article} {product.Name} {product.Color} {product.DefaultPrice}");
// }