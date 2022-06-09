// See https://aka.ms/new-console-template for more information


class Personaje
{
    //Caracteristicas 
    private int Velocidad; // 1 a 10
    private int Destreza; //1 a 5
    private int Fuerza; //1 a 10
    private int Nivel; //1 a 10
    private int Armadura; //1 a 10

    //Datos
    private string Tipo;
    private string Nombre;
    private string Apodo;
    private DateTime FechaNacimiento;
    private int Edad; //Entre 0 a 300
    private int Salud;


}

public enum Tipo
{
    Arquero = 1,
    Guerrero = 2,
    Valkiria = 3,
    Vikingo = 4,
}