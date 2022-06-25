


using System.Text.Json;

string pathGanadoresCsv = @"C:\TallerLenguajesC#\rpg-2022-SantiagoYalux\JuegoRol\JuegoRol\Archivos\ganadores.csv";
string pathJugadoresJson = @"C:\TallerLenguajesC#\rpg-2022-SantiagoYalux\JuegoRol\JuegoRol\Archivos\Jugadores.json";
Random random = new Random();
Personaje p1;
Personaje p2;
string[] newData = new string[1];
int indexPrimerJugador = 0;
int indexSegundoJugador = 0;

juego();

void juego()
{

    //Si el archivo no existe lo creamos
    if (!File.Exists(pathGanadoresCsv))
    {
        File.Create(pathGanadoresCsv);
    }

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
                Console.WriteLine("Nuevos Personajes Listos para pelear");
                Console.ReadKey();
                break;

        }
    } while (op.Key != ConsoleKey.F);

}

void Combate(ref Personaje p1, ref Personaje p2, ref List<Personaje> personajes)
{
    //
    string[] newData = new string[1];

    Console.WriteLine($"\n//////{p1.DATOS.NOMBRE} VS {p2.DATOS.NOMBRE}///////");
    //Realizamos el combate, el que pierda será eliminado de la lista
    Console.WriteLine("* Empieza la pelea *");
    for (int i = 0; i < 3; i++)
    {
        p1.Ataque(ref p2);
        p2.Ataque(ref p1);
    }

    if (p1.DATOS.SALUD > p2.DATOS.SALUD)
    {
        Console.WriteLine($"-----El ganador es {p1.DATOS.NOMBRE}-----");
        p1.CARACTERISTICAS.FUERZA += 2;


        personajes.Remove(p2);
        newData[0] = $"{p1.DATOS.NOMBRE} vs {p2.DATOS.NOMBRE}// GANADOR = {p1.DATOS.NOMBRE}";
        File.AppendAllLines(pathGanadoresCsv, newData);
    }
    else if (p1.DATOS.SALUD < p2.DATOS.SALUD)
    {
        Console.WriteLine($"-----El ganador es {p2.DATOS.NOMBRE}-----");
        p2.CARACTERISTICAS.FUERZA += 2;

        personajes.Remove(p1);
        newData[0] = $"{p2.DATOS.NOMBRE} vs {p1.DATOS.NOMBRE}// GANADOR = {p2.DATOS.NOMBRE}";
        File.AppendAllLines(pathGanadoresCsv, newData);
    }
    else
    {
        Console.WriteLine("--------Empate--------");
    }
    Console.WriteLine("\n");

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
            Personajes = JsonSerializer.Deserialize<List<Personaje>>(datosJugadoresJson);
        else
            Console.WriteLine("Elegiste no cambiar a los jugadores anteriores, los nuevos darán batalla");
    }
    else
    {
        string JsonString = JsonSerializer.Serialize(Personajes);
        File.WriteAllText(pathJugadoresJson, JsonString);
        Console.WriteLine("Cargamos los personajes actuales, para en un futuro usarlos!");
    }
}

void MostrarGanadores()
{
    string ganadoresString = File.ReadAllText(pathGanadoresCsv);
    if (ganadoresString.Length > 0)
    {
        Console.WriteLine(ganadoresString);
    }
    else
        Console.WriteLine("No hay ganadores para mostrar");

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
    Console.WriteLine($"GANADOR DEL TORNEO = {Personajes.First().DATOS.NOMBRE}");
    Personajes.First().MostrarDatos();
    newData[0] = $"GANADOR DEL TORNEO = {Personajes.First().DATOS.NOMBRE}({Personajes.First().DATOS.TIPO}), FECHA {DateTime.Now}";
    File.AppendAllLines(pathGanadoresCsv, newData);

    
}

List<Personaje> CrearPersonajes()
{
    List<Personaje> retorno = new List<Personaje>();
    
    for (int x = 0; x < 8; x++)
    {
        Personaje personaje = new Personaje();
        retorno.Add(personaje);
    }

    return retorno;
}


class Personaje
{

    private Caracteristicas caracteristicas;
    private Datos datos;

    public Caracteristicas CARACTERISTICAS { get => caracteristicas; set => caracteristicas = value; }
    public Datos DATOS { get => datos; set => datos = value; }

    public Personaje()
    {
        CARACTERISTICAS = new Caracteristicas();
        DATOS = new Datos();
    }


    //Habilidades del personaje
    public void Ataque(ref Personaje Enemigo)
    {
        Random rnd = new Random();
        double poderDisparo = CARACTERISTICAS.DESTREZA * CARACTERISTICAS.FUERZA * CARACTERISTICAS.NIVEL;
        double EfectividadDisparo = ((double)rnd.Next(1, 100)) / 100;
        double valorAtaque = poderDisparo * EfectividadDisparo;
        double poderDefensa = Enemigo.CARACTERISTICAS.ARMADURA * Enemigo.CARACTERISTICAS.VELOCIDAD;
        double MaximoDaño = 5000;
        double dañoProvocado = (((valorAtaque * EfectividadDisparo) - poderDefensa) / MaximoDaño) * 100;

        if (dañoProvocado < 1)
            dañoProvocado = 1;

        Enemigo.DATOS.SALUD -= (int)dañoProvocado;
    }


    public void MostrarDatos()
    {
        Console.WriteLine("--------Datos Personaje--------");
        Console.WriteLine("Nombre: " + DATOS.NOMBRE);
        Console.WriteLine("Tipo " + DATOS.TIPO.ToString());
        Console.WriteLine("Fecha Nacimiento: " + DATOS.FECHANACIMIENTO.ToString());
        Console.WriteLine("Edad: " + DATOS.EDAD.ToString());
        Console.WriteLine("Salud: " + DATOS.SALUD.ToString());

        Console.WriteLine("------caracteristicas--------");
        Console.WriteLine("Velocidad: " + CARACTERISTICAS.VELOCIDAD);
        Console.WriteLine("Destreza: " + CARACTERISTICAS.DESTREZA);
        Console.WriteLine("Fuerza: " + CARACTERISTICAS.FUERZA);
        Console.WriteLine("Nivel: " + CARACTERISTICAS.NIVEL);
        Console.WriteLine("Armadura: " + CARACTERISTICAS.ARMADURA);

    }

}


class Caracteristicas
{
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

    public Caracteristicas()
    {
        Random rnd = new Random();

        VELOCIDAD = rnd.Next(1, 11);
        DESTREZA = rnd.Next(1, 6);
        FUERZA = rnd.Next(1, 11);
        NIVEL = rnd.Next(1, 11);
        ARMADURA = rnd.Next(1, 11);

    }

}

class Datos
{
    //Datos
    private string tipo;
    private string nombre;
    private DateTime fechaNacimiento;
    private int edad; //Entre 0 a 300
    private int salud;

    public string TIPO { get => tipo; set => tipo = value; }
    public string NOMBRE { get => nombre; set => nombre = value; }
    public DateTime FECHANACIMIENTO { get => fechaNacimiento; set => fechaNacimiento = value; }
    public int EDAD { get => edad; set => edad = value; }
    public int SALUD { get => salud; set => salud = value; }

    public Datos()
    {
        Random rnd = new Random();


        TIPO = Enum.GetName(typeof(TipoPersonaje), rnd.Next(1, Enum.GetNames(typeof(TipoPersonaje)).Length));

        NOMBRE = Enum.GetName(typeof(Nombres), rnd.Next(1, Enum.GetNames(typeof(Nombres)).Length));


        FECHANACIMIENTO = DateTime.Now;

        EDAD = rnd.Next(0, 300);

        SALUD = 100;

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
        Console.WriteLine("B- Cargar jugadores json\n");
        Console.WriteLine("C- Iniciar combate\n");
        Console.WriteLine("F- Salir\n");
        Console.WriteLine("*************************");
    }
    #endregion
}

public enum TipoPersonaje
{
    Arquero,
    Guerrero,
    Valkiria,
    Vikingo
}

public enum Nombres
{
    Santiago,
    Sergio,
    Josefina,
    Agustina,
    Mariana,
    Messi,
    Roberto,
    LuisMiguel,
    Amets,
    Amaro,
    Aquiles,
    Algimantas,
    Alpidio,
    Amrane,
    Anish,
    Arián,
    Ayun,
    Azariel,
    Bagrat,
    Bencomo,
    Bertino,
    Candi,
    Cesc,
    Cirino,
    Crisólogo,
    Cruz,
    Danilo,
    Dareck,
    Dariel,
    Darin,
    Delmiro,
    Damen,
    Dilan,
    Dipa,
    Doménico,
    Drago,
    Edivaldo,
    Elvis,
    Elyan,
    Emeric,
    Engracio,
    Ensa,
    Eñaut,
    Eleazar,
    Eros,
    Eufemio,
    Feiyang,
    Fiorenzo,
    Foudil,
    Galo,
    Gastón,
    Giulio,
    Gautam,
    Gentil,
    Gianni,
    Gianluca,
    Giorgio,
    Gourav,
    Grober,
    Guido,
    Guifre,
    Guim,
    Hermes,
    Inge,
    Irai,
    Iraitz,
    Iyad,
    Iyán,
    Joao,
    Jun,
    Khaled,
    Leónidas,
    Lier,
    Lionel,
    Lisandro,
    Lucián,
    Mael,
    Misael,
    Moad,
    Munir,
    Nael,
    Najim,
    Neo,
    Neil,
    Nikita,
    Nilo,
    Otto,
    Pep,
    Policarpo,
    Radu,
    Ramsés,
    Rómulo,
    Roy,
    Severo,
    Sidi,
    Simeón,
    Taha,
    Tao,
    Vadim,
    Vincenzo,
    Zaid,
    Zigor,
    Zouhair,
}