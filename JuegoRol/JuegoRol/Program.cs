


using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;


Console.CursorVisible = false;

//var arr = new[]
//{
//            @"           _____________.  ___     .___     .___     ._____.   .____.     ",
//            @"           |            | /   \    |  |     |  |     |  ___|  |   _  |    ",
//            @"           `----|  |----`/  ^  \   |  |     |  |     | |___.  |  |_|  |   ",
//            @"                |  |    /  /_\  \  |  |     |  |     |  ___|  |    _   -. ",
//            @"                |  |   /  _____  \ |  ----. |  ----. | |___.  |   | |   |.",
//            @"                |__|  /__/     \__\|______| |______| | ____|  |__ | |__  |",
//            @"              .______       ______        ____     ______         ",
//            @"              |  .__  \   /  ____  \      |  |   /  ____  \       ",
//            @"              |  |  \  | |  |    |  |     |  |  |  |    |  |      ",
//            @"              |  |   | | |  |    |  |     | .|  |  |    |  |      ",
//            @"              |  |__/  | |  |____|  |  _./ ./   |  |____|  |      ",
//            @"              |______ /   \ ______ /  /___./     \ ______ /       ",
//};


//for (int i = 0; i < arr.Length; i++)
//{
//    Console.SetCursorPosition((Console.WindowWidth - arr[i].Length) / 2, Console.CursorTop);
//    Console.WriteLine(arr[i]);
//    Thread.Sleep(500);
//}




string currentDirectory = getCurrentDirectory();


string pathGanadoresTxt = currentDirectory + @"Archivos\ganadores.txt";
string pathJugadoresJson = currentDirectory + @"Archivos\Jugadores.json";
string pathGanadoresTorneoCsv = currentDirectory + @"Archivos\ganadoresTorneo.csv";
Random random = new Random();

Personaje p1;
Personaje p2;
string[] newData = new string[1];
string[] newDataCsv = new string[1];
int indexPrimerJugador = 0;
int indexSegundoJugador = 0;

juego();

void juego()
{
    
    //Si el archivo no existe lo creamos
    if (!File.Exists(pathGanadoresTxt)) 
        File.Create(pathGanadoresTxt);
    if (!File.Exists(pathGanadoresTorneoCsv))
        File.Create(pathGanadoresTorneoCsv);

    List<Personaje> Personajes = CrearPersonajes();


    ConsoleKeyInfo op;
    Menu miMenu = new Menu();
    miMenu.DibujarMenu();

    do
    {
        miMenu.DibujarMenu();
        op = Console.ReadKey();
        switch (op.Key)
        {
            case ConsoleKey.A:
                Console.Clear();
                MostrarGanadores();
                Console.ReadKey();
                break;
            case ConsoleKey.B:
                Console.Clear();
                CargarPersonajesAnteriores(ref Personajes);

                Console.ReadKey();
                break;
            case ConsoleKey.C:
                Console.Clear();
                IniciarCombate(ref Personajes);
                Personajes = CrearPersonajes();
                Console.WriteLine("\n-----Nuevos Personajes Listos para pelear-----");
                Console.ReadKey();
                break;            
            case ConsoleKey.D:
                Console.Clear();

                foreach (var Personaje in Personajes)
                {
                    Personaje.MostrarDatos();
                    Console.WriteLine("\n");
                }

                Console.ReadKey();
                break;

        }
    } while (op.Key != ConsoleKey.F);

}

//Utilizaremos este metodo para saber la ubicacion 
string getCurrentDirectory()
{
    return (Directory.GetCurrentDirectory().Split("bin"))[0];
}

void IniciarCombate(ref List<Personaje> Personajes)
{
    do
    {
        indexPrimerJugador = random.Next(Personajes.Count);
        do
        {
            indexSegundoJugador = random.Next(Personajes.Count);

        } while (indexSegundoJugador == indexPrimerJugador);

        p1 = Personajes.ElementAt(indexPrimerJugador);
        p2 = Personajes.ElementAt(indexSegundoJugador);

        Combate(ref p1, ref p2, ref Personajes);

    } while (Personajes.Count > 1);

    Console.WriteLine("\n");


    Console.WriteLine("-------------------------------------------------------");
    Personaje ganador = Personajes.First();
    Console.WriteLine($"GANADOR DEL TORNEO = {ganador.NOMBRE}|");
    ganador.MostrarDatos();

    newData[0] = $"GANADOR DEL TORNEO = {ganador.NOMBRE}({ganador.TIPO}), FECHA {DateTime.Now.ToString("dddd")} {DateTime.Now}";
    newDataCsv[0] = $"{ganador.NOMBRE},{ganador.TIPO},{ganador.EDAD}, {ganador.FUERZA}, {ganador.NIVEL}";
    File.AppendAllLines(pathGanadoresTxt, newData);
    File.AppendAllLines(pathGanadoresTorneoCsv, newDataCsv);
}

void Combate(ref Personaje p1, ref Personaje p2, ref List<Personaje> personajes)
{
    //
    string[] newData = new string[1];

    Console.WriteLine($"\n//////{p1.NOMBRE} VS {p2.NOMBRE}///////");
    //Realizamos el combate, el que pierda será eliminado de la lista
    Console.WriteLine("* Empieza la pelea *");
    for (int i = 0; i < 3; i++)
    {
        ataqueCombate(ref p1, ref p2);
        ataqueCombate(ref p2, ref p1);
    }

    if (p1.SALUD > p2.SALUD)
    {
        Console.WriteLine($"-----El ganador es {p1.NOMBRE} Recibe 10+ de fuerza -----");
        p1.FUERZA += 10;


        newData[0] = $"{p1.NOMBRE} vs {p2.NOMBRE}// GANADOR = {p1.NOMBRE}";
        personajes.Remove(p2);
    }
    else if (p1.SALUD < p2.SALUD)
    {
        Console.WriteLine($"-----El ganador es {p2.NOMBRE} Recibe 10+ de fuerza -----");
        p2.FUERZA += 10;

        newData[0] = $"{p2.NOMBRE} vs {p1.NOMBRE}// GANADOR = {p2.NOMBRE}";
        personajes.Remove(p1);
    }
    else
    {
        Console.WriteLine("--------Empate--------");
    }

    //Verificamos que no sea null, ya si es un empate no hay datos para agregar
    if (newData[0] != null)
    {
        File.AppendAllLines(pathGanadoresTxt, newData);
    }

    Console.WriteLine("\n");

}

void ataqueCombate(ref Personaje p1, ref Personaje p2)
{
    Console.WriteLine("----------------------------");
    Console.WriteLine($"- Ataca: '{p1.NOMBRE.ToUpper()}' vida '{p1.SALUD}'");
    //Thread.Sleep(1000);
    p1.Ataque(ref p2);
    //Thread.Sleep(1000);
    Console.WriteLine($"- Vida Oponente: '{p2.NOMBRE.ToUpper()}' después del ataque: '{p2.SALUD}'");
    Console.WriteLine("----------------------------");
}

void CargarPersonajesAnteriores(ref List<Personaje> Personajes)
{
    int eleccion = 0;


    if (!File.Exists(pathJugadoresJson))
        File.Create(pathJugadoresJson);

    var datosJugadoresJson = File.ReadAllText(pathJugadoresJson);
    if (datosJugadoresJson.Length > 0)
    {
        Console.WriteLine("Tienes datos de jugadores, ¿quieres cargarlos? 1-si 2-no");
        eleccion = int.Parse(Console.ReadLine());

        if (eleccion == 1)
        {
            try
            {
                Personajes = JsonSerializer.Deserialize<List<Personaje>>(datosJugadoresJson);
                Console.WriteLine("PERSONAJES ANTERIORES CARGADOS");
            }
            catch (Exception)
            {
                Console.WriteLine("no podemos leer los persoanjes que contiene el archivo");
               throw;
            }
        }
        else
            Console.WriteLine("Elegiste no cambiar a los jugadores anteriores, los nuevos pelearan");
    }
    else
    {
        string JsonString = JsonSerializer.Serialize(Personajes);
        File.WriteAllText(pathJugadoresJson, JsonString);
        Console.WriteLine("No tienes jugadores anteriores, guardamos los jugadores actuales para en un futuro usarlos!");
    }
}

void MostrarGanadores()
{
    string ganadoresString = File.ReadAllText(pathGanadoresTxt);
    if (ganadoresString.Length > 0)
    {
        Console.WriteLine(ganadoresString);
    }
    else
        Console.WriteLine("No hay ganadores para mostrar");

}



List<Personaje> CrearPersonajes()
{
    List<Personaje> retorno = new List<Personaje>();

    for (int x = 0; x < 8; x++)
    {
        Personaje personaje = new Personaje(RecuperarNombre());
        retorno.Add(personaje);
    }

    return retorno;
}

string RecuperarNombre()
{
    string retorno = "";
    Nombre nombre = new Nombre();
    string urlApiNombre = @"https://random-names-api.herokuapp.com/random";
    var Request = (HttpWebRequest)WebRequest.Create(urlApiNombre);

    Request.Method = "GET";
    Request.ContentType = "application/json";
    Request.Accept = "application/json";

    using (WebResponse response = Request.GetResponse())
    {
        using (Stream strStream = response.GetResponseStream())
        {
            if (strStream != null)
            {
                using (StreamReader objReader = new StreamReader(strStream))
                {
                    string jsonReturn = objReader.ReadToEnd();

                    try
                    {
                        nombre = JsonSerializer.Deserialize<Nombre>(jsonReturn);
                        retorno = nombre.Body.Name;
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Hubo un error " + e.Message);
                        throw;
                    }
                }
            }
        }
    }

    return retorno;
}

class Personaje
{


    //Datos
    private string tipo;
    private string nombre;
    private int edad; //Entre 0 a 300
    private int salud;

    //Caracteristicas 
    private int Velocidad; // 1 a 10
    private int Destreza; //1 a 5
    private int Fuerza; //1 a 10
    private int Nivel; //1 a 10
    private int Armadura; //1 a 10

    public int VELOCIDAD { get => Velocidad; set => Velocidad = value; }
    public int DESTREZA { get => Destreza; set => Destreza = value; }
    public int FUERZA { get => Fuerza; set => Fuerza = value; }
    public int NIVEL { get => Nivel; set => Nivel = value; }
    public int ARMADURA { get => Armadura; set => Armadura = value; }
    public string TIPO { get => tipo; set => tipo = value; }
    public string NOMBRE { get => nombre; set => nombre = value; }
    public int EDAD { get => edad; set => edad = value; }
    public int SALUD { get => salud; set => salud = value; }

    public Personaje(string nombre)
    {
        Random rnd = new Random();

        VELOCIDAD = rnd.Next(1, 11);
        DESTREZA = rnd.Next(1, 6);
        FUERZA = rnd.Next(1, 11);
        NIVEL = rnd.Next(1, 11);
        ARMADURA = rnd.Next(1, 11);

        TIPO = Enum.GetName(typeof(TipoPersonaje), rnd.Next(1, Enum.GetNames(typeof(TipoPersonaje)).Length));

        //NOMBRE = Enum.GetName(typeof(Nombres), rnd.Next(1, Enum.GetNames(typeof(Nombres)).Length));
        NOMBRE = nombre;

        EDAD = rnd.Next(0, 300);

        SALUD = 100;
    }


    //Habilidades del personaje
    public void Ataque(ref Personaje Enemigo)
    {
        Random rnd = new Random();
        double poderDisparo = DESTREZA * FUERZA * NIVEL;
        double EfectividadDisparo = ((double)rnd.Next(1, 100)) / 100;
        double valorAtaque = poderDisparo * EfectividadDisparo;
        double poderDefensa = Enemigo.ARMADURA * Enemigo.VELOCIDAD;
        double MaximoDaño = 5000;
        double dañoProvocado = (((valorAtaque * EfectividadDisparo) - poderDefensa) / MaximoDaño) * 100;

        if (dañoProvocado < 1)
            dañoProvocado = 1;

        Enemigo.SALUD -= (int)dañoProvocado;
    }


    public void MostrarDatos()
    {
        Console.WriteLine("--------Datos Personaje--------");
        Console.WriteLine("Nombre: " + NOMBRE);
        Console.WriteLine("Tipo " + TIPO.ToString());
        Console.WriteLine("Edad: " + EDAD.ToString());
        Console.WriteLine("Salud: " + SALUD.ToString());

        Console.WriteLine("------caracteristicas--------");
        Console.WriteLine("Velocidad: " + VELOCIDAD);
        Console.WriteLine("Destreza: " + DESTREZA);
        Console.WriteLine("Fuerza: " + FUERZA);
        Console.WriteLine("Nivel: " + NIVEL);
        Console.WriteLine("Armadura: " + ARMADURA);

    }

}


class Menu
{
    #region Metodos
    public void DibujarMenu()
    {
        Console.Clear();
        Console.WriteLine("*************************");
        Console.WriteLine("A- Mostrar Ganadores \n");
        Console.WriteLine("B- Cargar jugadores anteriores\n");
        Console.WriteLine("C- Iniciar combate\n");
        Console.WriteLine("D- Mostrar jugadores por luchar\n");
        Console.WriteLine("F- Salir\n");
        Console.WriteLine("*************************");
    }
    #endregion
}


#region clases consumir api nombres
public class ApiOwner
{
    [JsonPropertyName("author")]
    public string Author { get; set; }

    [JsonPropertyName("cafecito")]
    public string Cafecito { get; set; }

    [JsonPropertyName("instagram")]
    public string Instagram { get; set; }

    [JsonPropertyName("github")]
    public string Github { get; set; }

    [JsonPropertyName("linkedin")]
    public string Linkedin { get; set; }

    [JsonPropertyName("twitter")]
    public string Twitter { get; set; }
}

public class Body
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("genre")]
    public string Genre { get; set; }
}

public class Nombre
{
    [JsonPropertyName("api_owner")]
    public ApiOwner ApiOwner { get; set; }

    [JsonPropertyName("body")]
    public Body Body { get; set; }
}
#endregion

public enum TipoPersonaje
{
    Arquero,
    Guerrero,
    Valkiria,
    Vikingo,
    Luchador,
    Bandido,
    Vaquero
}
