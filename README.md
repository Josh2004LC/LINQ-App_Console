
# LinkQ_App - Gestión de Empleados con XML y LINQ

LinkQ_App es una aplicación de consola escrita en C# que permite gestionar una lista de empleados que se almacenan en un archivo XML. Los usuarios pueden:

- **Cargar datos desde un archivo XML**: La aplicación lee el archivo XML y carga la información de los empleados.
- **Actualizar el salario de un empleado**: Permite modificar el salario de un empleado específico utilizando su ID.
- **Eliminar un empleado**: Se puede eliminar un empleado de la lista usando su ID.
- **Agregar un nuevo empleado**: Los usuarios pueden agregar nuevos empleados al archivo XML con todos los datos necesarios.
- **Consultar empleados**: La aplicación ofrece la capacidad de realizar diversas consultas, como:
  - Ver empleados mayores de 30 años.
  - Filtrar empleados con salario mayor a 50,000.
  - Calcular el promedio de salario por departamento.
  - Encontrar al empleado con el salario más alto.
  - Contar la cantidad de empleados por departamento.

La aplicación hace uso de **LINQ to XML** para gestionar los datos del archivo XML


### Funciones de LINQ utilizadas:

1. **`Where`**: 
   Esta función se utiliza para filtrar a los empleados que cumplen con una condición específica. Por ejemplo, para obtener los empleados mayores de 30 años, podemos usar:

   ```csharp
   var mayoresDe30 = empleados.Where(e => e.Edad > 30);
   ```


2. **`GroupBy`**: 
   Permite agrupar a los empleados según un atributo, como el departamento. Esto nos permite calcular el promedio de salario por departamento, por ejemplo:

   ```csharp
   var promedioSalarioPorDepartamento = empleados
       .GroupBy(e => e.Departamento)
       .Select(g => new { Departamento = g.Key, PromedioSalario = g.Average(e => e.Salario) });
   ```

   En este caso, los empleados se agrupan por departamento y luego se calcula el salario promedio dentro de cada grupo.

3. **`OrderByDescending` y `First`**:
   Estas funciones se usan para ordenar los empleados por un atributo (como el salario) en orden descendente y luego obtener el primer empleado de la lista, que será el que tiene el salario más alto:

   ```csharp
   var empleadoMayorSalario = empleados.OrderByDescending(e => e.Salario).First();
   ```

4. **`Select`**:
   Se utiliza para proyectar datos o transformar los elementos de una colección en un nuevo formato. Por ejemplo, para calcular la cantidad de empleados por departamento:

   ```csharp
   var empleadosPorDepartamento = empleados
       .GroupBy(e => e.Departamento)
       .Select(g => new { Departamento = g.Key, Cantidad = g.Count() });
   ```

   Aquí, se cuenta la cantidad de empleados en cada departamento.

5. **`FirstOrDefault`**:
   Esta función se usa para buscar el primer empleado que cumpla con una condición específica, devolviendo `null` si no se encuentra ninguno. Por ejemplo:

   ```csharp
   var empleado = empleados.FirstOrDefault(e => e.Id == id);
   ```

   Esto busca al primer empleado con el ID especificado y devuelve `null` si no existe.

----------------------------------------------------------------------------------------------------------------------------------------------------------------------

## Referencias

- **Documentación sobre filtrado de datos con LINQ**:
  - [Filtrando datos con LINQ - Microsoft](https://learn.microsoft.com/es-es/dotnet/csharp/linq/standard-query-operators/filtering-data)

- **Referencia completa de LINQ en C#**:
  - [Referencia de LINQ - Microsoft](https://learn.microsoft.com/es-es/dotnet/api/system.linq?view=net-6.0)


