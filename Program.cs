using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace LinkQ_App
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Mapear la ruta XML 
            string rutaXML = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Empleados.xml");

            // Cargar datos desde XML a la lista de empleados
            List<Empleado> empleados = CargarEmpleados(rutaXML);

            bool continuar = true;

            while (continuar)
            {
                // Mostrar el menú
                Console.Clear();
                Console.WriteLine("===== MENÚ DE OPCIONES =====");
                Console.WriteLine("1. Actualizar salario de un empleado");
                Console.WriteLine("2. Eliminar un empleado");
                Console.WriteLine("3. Agregar un nuevo empleado");
                Console.WriteLine("4. Mostrar todos los empleados");
                Console.WriteLine("5. Empleados mayores de 30 años");
                Console.WriteLine("6. Empleados con salario mayor a 50000");
                Console.WriteLine("7. Promedio de salario por departamento");
                Console.WriteLine("8. Empleado con el salario más alto");
                Console.WriteLine("9. Cantidad de empleados por departamento");
                Console.WriteLine("10. Buscar empleado por ID");
                Console.WriteLine("11. Salir");
                Console.WriteLine("===========================");
                Console.Write("Seleccione una opción: ");
                string opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        // Actualizar salario
                        Console.Write("Ingrese el ID del empleado a actualizar el salario: ");
                        int idActualizar = int.Parse(Console.ReadLine());
                        bool empleadoEncontrado = BuscarEmpleadoPorId(empleados, idActualizar);

                        if (empleadoEncontrado)
                        {
                            Console.Write("Ingrese el nuevo salario: ");
                            decimal nuevoSalario = decimal.Parse(Console.ReadLine());
                            ActualizarSalario(rutaXML, idActualizar, nuevoSalario);
                        }
                        else
                        {
                            Console.WriteLine("Empleado no encontrado. No se puede actualizar el salario.");
                        }
                        break;

                    case "2":
                        // Eliminar empleado
                        Console.Write("Ingrese el ID del empleado a eliminar: ");
                        int idEliminar = int.Parse(Console.ReadLine());
                        EliminarEmpleado(rutaXML, idEliminar);
                        break;

                    case "3":
                        // Agregar nuevo empleado
                        Console.Write("Ingrese el ID del nuevo empleado: ");
                        int idNuevo = int.Parse(Console.ReadLine());
                        Console.Write("Ingrese el nombre del nuevo empleado: ");
                        string nombreNuevo = Console.ReadLine();
                        Console.Write("Ingrese la edad del nuevo empleado: ");
                        int edadNuevo = int.Parse(Console.ReadLine());
                        Console.Write("Ingrese el departamento del nuevo empleado: ");
                        string deptoNuevo = Console.ReadLine();
                        Console.Write("Ingrese el salario del nuevo empleado: ");
                        decimal salarioNuevo = decimal.Parse(Console.ReadLine());

                        Empleado nuevoEmpleado = new Empleado
                        {
                            Id = idNuevo,
                            Nombre = nombreNuevo,
                            Edad = edadNuevo,
                            Departamento = deptoNuevo,
                            Salario = salarioNuevo
                        };
                        AgregarEmpleado(rutaXML, nuevoEmpleado);
                        break;

                    case "4":
                        // Mostrar todos los empleados
                        empleados = CargarEmpleados(rutaXML);
                        MostrarEmpleados(empleados);
                        break;

                    case "5":
                        // Empleados mayores de 30 años
                        var mayoresDe30 = empleados.Where(e => e.Edad > 30);
                        Console.WriteLine("Empleados mayores de 30 años:");
                        foreach (var emp in mayoresDe30)
                            Console.WriteLine($"Nombre: {emp.Nombre}, Edad: {emp.Edad}");
                        break;

                    case "6":
                        // Empleados con salario mayor a 50000
                        var salarioMayor50000 = empleados.Where(e => e.Salario > 50000);
                        Console.WriteLine("Empleados con salario mayor a 50000:");
                        foreach (var emp in salarioMayor50000)
                            Console.WriteLine($"ID: {emp.Id}, Nombre: {emp.Nombre}, Salario: {emp.Salario}");
                        break;

                    case "7":
                        // Promedio de salario por departamento
                        var promedioSalarioPorDepartamento = empleados
                            .GroupBy(e => e.Departamento)
                            .Select(g => new { Departamento = g.Key, PromedioSalario = g.Average(e => e.Salario) });
                        Console.WriteLine("Promedio de salario por departamento:");
                        foreach (var item in promedioSalarioPorDepartamento)
                            Console.WriteLine($"{item.Departamento}: {item.PromedioSalario}");
                        break;

                    case "8":
                        // Empleado con el salario más alto
                        var empleadoMayorSalario = empleados.OrderByDescending(e => e.Salario).First();
                        Console.WriteLine("Empleado con el salario más alto:");
                        Console.WriteLine($"ID: {empleadoMayorSalario.Id}, Nombre: {empleadoMayorSalario.Nombre}, Salario: {empleadoMayorSalario.Salario}");
                        break;

                    case "9":
                        // Cantidad de empleados por departamento
                        var empleadosPorDepartamento = empleados.GroupBy(e => e.Departamento)
                            .Select(g => new { Departamento = g.Key, Cantidad = g.Count() });
                        Console.WriteLine("Cantidad de empleados por departamento:");
                        foreach (var item in empleadosPorDepartamento)
                            Console.WriteLine($"{item.Departamento}: {item.Cantidad}");
                        break;

                    case "10":
                        // Buscar empleado
                        Console.Write("Ingrese el ID del empleado a buscar: ");
                        int idBuscar = int.Parse(Console.ReadLine());
                        BuscarEmpleadoPorId(empleados, idBuscar);
                        break;

                    case "11":
                        // Salir
                        continuar = false;
                        break;

                    default:
                        Console.WriteLine("Opción no válida. Intente nuevamente.");
                        break;
                }

                if (continuar)
                {
                    // Da chance para ver el resultado 
                    Console.WriteLine("\nPresione cualquier tecla para continuar...");
                    Console.ReadKey();
                }
            }

            Console.WriteLine("¡Hasta luego!");
        }

        // cargar empleados 
        static List<Empleado> CargarEmpleados(string rutaXML)
        {
            return XDocument.Load(rutaXML)
                .Descendants("Empleado")
                .Select(e => new Empleado
                {
                    Id = int.Parse(e.Element("Id").Value),
                    Nombre = e.Element("Nombre").Value,
                    Edad = int.Parse(e.Element("Edad").Value),
                    Departamento = e.Element("Departamento").Value,
                    Salario = decimal.Parse(e.Element("Salario").Value)
                }).ToList();
        }

        // buscar empleado
        static bool BuscarEmpleadoPorId(List<Empleado> empleados, int id)
        {
            var empleado = empleados.FirstOrDefault(e => e.Id == id);
            if (empleado != null)
            {
                Console.WriteLine("Empleado encontrado:");
                Console.WriteLine($"ID: {empleado.Id}, Nombre: {empleado.Nombre}, Edad: {empleado.Edad}, Departamento: {empleado.Departamento}, Salario: {empleado.Salario}");
                return true;
            }
            else
            {
                Console.WriteLine($"Empleado con ID {id} no encontrado.");
                return false;
            }
        }


        // actualizar el salario por Id
        static void ActualizarSalario(string rutaXML, int id, decimal nuevoSalario)
        {
            XDocument doc = XDocument.Load(rutaXML);
            var empleado = doc.Descendants("Empleado")
                .FirstOrDefault(e => int.Parse(e.Element("Id").Value) == id);
            if (empleado != null)
            {
                empleado.Element("Salario").Value = nuevoSalario.ToString();
                doc.Save(rutaXML);
                Console.WriteLine($"Salario del empleado con ID {id} actualizado a {nuevoSalario}");
            }
            else
            {
                Console.WriteLine($"Empleado con ID {id} no encontrado.");
            }
        }

        // eliminar empleado por Id
        static void EliminarEmpleado(string rutaXML, int id)
        {
            XDocument doc = XDocument.Load(rutaXML);
            var empleado = doc.Descendants("Empleado")
                .FirstOrDefault(e => int.Parse(e.Element("Id").Value) == id);
            if (empleado != null)
            {
                empleado.Remove();
                doc.Save(rutaXML);
                Console.WriteLine($"Empleado con ID {id} eliminado.");
            }
            else
            {
                Console.WriteLine($"Empleado con ID {id} no encontrado.");
            }
        }

        // agregar un nuevo empleado
        static void AgregarEmpleado(string rutaXML, Empleado nuevoEmpleado)
        {
            XDocument doc = XDocument.Load(rutaXML);
            var nuevoElemento = new XElement("Empleado",
                new XElement("Id", nuevoEmpleado.Id),
                new XElement("Nombre", nuevoEmpleado.Nombre),
                new XElement("Edad", nuevoEmpleado.Edad),
                new XElement("Departamento", nuevoEmpleado.Departamento),
                new XElement("Salario", nuevoEmpleado.Salario)
            );
            doc.Element("Empleados").Add(nuevoElemento);
            doc.Save(rutaXML);
            Console.WriteLine($"Nuevo empleado {nuevoEmpleado.Nombre} agregado.");
        }

        //mostrar lista de empleados
        static void MostrarEmpleados(List<Empleado> empleados)
        {
            Console.WriteLine("Lista de empleados:");
            foreach (var emp in empleados)
            {
                Console.WriteLine($"ID: {emp.Id}, Nombre: {emp.Nombre}, Edad: {emp.Edad}, Departamento: {emp.Departamento}, Salario: {emp.Salario}");
            }
        }
    }


}
