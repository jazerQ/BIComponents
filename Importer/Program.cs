using System.Text;
using Aspose.Cells;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

//Настраиваем кодировку ввода/вывода
Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.InputEncoding = System.Text.Encoding.UTF8;

//подключаем конфиги
var connectionString = JObject.Parse(File.ReadAllText("appsettings.json"));

//если нет, то выбрасываем ошибку с тем что конфиг не найден
if (connectionString["MongoConnectionString"] == null || connectionString["MongoDatabaseName"] == null)
    throw new FileNotFoundException("Mongo ConnectionString not found");

//генерируем объект MongoUrl для создания MongoClient
var mongoUrl = new MongoUrl(connectionString["MongoConnectionString"].ToString());
var dbName = connectionString["MongoDatabaseName"].ToString();

var db = new MongoClient(mongoUrl).GetDatabase(dbName);

//получаю коллекцию с BI объектами
var collection = db.GetCollection<BsonDocument>("BIObjects");

//для нас файл этот название у него - Амортизаторы Remo Hobby (1).csv
Console.WriteLine("Впишите имя файла или полный путь до файла - ");
var filename = Console.ReadLine() ?? throw new NullReferenceException("Ввод не может быть пустым");

var wb = new Workbook(filename, new TxtLoadOptions(LoadFormat.Csv)
{
    Separator = ',',
    Encoding = Encoding.UTF8,
    HasFormula = false
});

var sheet = wb.Worksheets.First();
for (int row = 1; row < sheet.Cells.MaxDataRow; row++)
{
    var doc = new BsonDocument();
    for (int col = 0; col < sheet.Cells.MaxDataColumn; col++)
    {
        var header = sheet.Cells[0, col].Value.ToString();
        doc[header] = sheet.Cells[row, col].StringValue;
    }
    
    collection.InsertOne(doc);
}