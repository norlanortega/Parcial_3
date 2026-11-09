using Newtonsoft.Json;

Console.WriteLine("Hello, World!");

var persona = new
{
    Nombre = "Cristell",
    Edad = 20,
    Carrera = "Ingenieria en Sistemas"
};

string json = JsonConvert.SerializeObject(persona);

Console.WriteLine(json);