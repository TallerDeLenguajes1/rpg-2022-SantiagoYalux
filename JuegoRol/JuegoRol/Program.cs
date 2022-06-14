// See https://aka.ms/new-console-template for more information


juego();

void juego()
{
    List<Personaje> Personajes = new List<Personaje>();
    for (int x = 0; x < 8; x++)
    {
        /*Personaje personaje = new Personaje();
        Personajes.Add(personaje);*/
    }

    Personaje p1 = new Personaje();
    Personaje p2 = new Personaje();

    Combate(ref p1, ref p2);
}

void Combate(ref Personaje p1, ref Personaje p2)
{
    Console.WriteLine("Empieza la pelea");
    for (int i = 0; i < 3; i++)
    {
        Console.WriteLine($"Ronda {i+1}");
        p1.Ataque(ref p2);
        p2.Ataque(ref p1);
    }

    if(p1.DATOS.SALUD > p2.DATOS.SALUD)
    {
        Console.WriteLine($"-----El ganador es {p1.DATOS.NOMBRE}-----");
        p1.MostrarDatos();
    }
    else
    {
        Console.WriteLine($"-----El ganador es {p2.DATOS.NOMBRE}-----");
        p2.MostrarDatos();
    }
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
    public int Ataque(ref Personaje Enemigo)
    {
        Random rnd = new Random();
        int poderDisparo = CARACTERISTICAS.DESTREZA * CARACTERISTICAS.FUERZA * Enemigo.CARACTERISTICAS.NIVEL;
        int EfectividadDisparo = rnd.Next(1, 100);

        int valorAtaque = poderDisparo * EfectividadDisparo;
        return valorAtaque;
    }
    public void Defensa()
    {
        DATOS.SALUD = CARACTERISTICAS.ARMADURA * CARACTERISTICAS.VELOCIDAD;
    }


    public void MostrarDatos()
    {
        Console.WriteLine("--------Datos Personaje--------");
        Console.WriteLine("Nombre: " + DATOS.NOMBRE);
        Console.WriteLine("Apodo: " + DATOS.APODO);
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
        Console.WriteLine("---------------Cargamos las caracteristicas------------");

        VELOCIDAD = rnd.Next(1, 10);
        DESTREZA = rnd.Next(1, 5);
        FUERZA = rnd.Next(1, 10);
        NIVEL = rnd.Next(1, 10);
        ARMADURA = rnd.Next(1, 10);

        Console.WriteLine("-------------------------------------------------------");
    }

}

class Datos
{
    //Datos
    private string tipo;
    private string nombre;
    private string apodo;
    private DateTime fechaNacimiento;
    private int edad; //Entre 0 a 300
    private int salud;

    public string TIPO { get => tipo; set => tipo = value; }
    public string NOMBRE { get => nombre; set => nombre = value; }
    public string APODO { get => apodo; set => apodo = value; }
    public DateTime FECHANACIMIENTO { get => fechaNacimiento; set => fechaNacimiento = value; }
    public int EDAD { get => edad; set => edad = value; }
    public int SALUD { get => salud; set => salud = value; }

    public Datos()
    {
        Random rnd = new Random();

        Console.WriteLine("--------------Cargamos los datos-------------");

        Console.WriteLine("Ingresa el tipo");
        TIPO = Console.ReadLine();

        Console.WriteLine("Ingresa el nombre");
        NOMBRE = Console.ReadLine();

        Console.WriteLine("Ingresa el apodo");
        APODO = Console.ReadLine();

        Console.WriteLine("Ingresa la fecha nacimiento");
        FECHANACIMIENTO = DateTime.Now;

        Console.WriteLine("Ingresa la edad");
        EDAD = rnd.Next(0, 300);

        SALUD = 100;

        Console.WriteLine("---------------------------------------------");
    }
}

public enum Tipo
{
    Arquero = 1,
    Guerrero = 2,
    Valkiria = 3,
    Vikingo = 4,
}