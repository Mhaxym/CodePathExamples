Console.WriteLine("> Cuenta Letras");
Console.WriteLine("> Este programa cuenta las letras de una palabra o frase");
Console.WriteLine("> Puede introducir el valor del carácter que desea contar o dejarlo en blanco para contar todas las letras");
Console.WriteLine(" ------------------------------------------------------------- ");
Console.WriteLine();

var executionType = -1;
var exit = false;

while (!exit)
{
    if (executionType < 0)
    {
        Console.WriteLine("> ¿Qué tipo de ejecución desea realizar? (1: Simple, 2: Extendida)");
        switch (Console.ReadLine())
        {
            case "1":
                executionType = 1;
                break;
            case "2":
                executionType = 2;
                break;
            default:
                Console.WriteLine("> Opción no válida");
                break;
        }
    }

    switch (executionType)
    {
        case 1:
            CountCharMain();
            break;
        case 2:
            CountCharsMain();
            break;
    }

    Console.WriteLine("> ¿Desea salir del programa? (S/N)");
    switch (Console.ReadLine()?.ToUpper() ?? string.Empty)
    {
        case "S":
            exit = true;
            break;
        case "N":
            Console.WriteLine();
            break;
        default:
            Console.WriteLine("> Opción no válida");
            break;
    }
}

void CountCharMain()
{
    Console.WriteLine("> Introduzca el carácter que desea contar");
    var character = Console.ReadLine() ?? string.Empty;

    if (!string.IsNullOrWhiteSpace(character) && (character.Length != 1 || !char.IsLetter(character[0])))
    {
        Console.WriteLine("> El carácter introducido no es válido");
        return;
    }

    var text = string.Empty;
    while (string.IsNullOrWhiteSpace(text))
    {
        Console.WriteLine("> Introduzca la palabra o frase");
        text = Console.ReadLine();
    }

    var count = CountCharacters(text, character);

    Console.WriteLine($"> A continuación se muestra el recuento de letras para la palabra o frase '{text}'");
    if (string.IsNullOrEmpty(character))
    {
        Console.WriteLine($"> Tiene {count} letras en total");
    }
    else
    {
        Console.WriteLine($"> El carácter '{character}' aparece {count} veces");
    }

    Console.WriteLine();

}

void CountCharsMain()
{
    Console.WriteLine("> Introduzca el carácter que desea contar");
    var character = Console.ReadLine() ?? string.Empty;

    if (!string.IsNullOrWhiteSpace(character) && (character.Length != 1 || !char.IsLetter(character[0])))
    {
        Console.WriteLine("> El carácter introducido no es válido");
        return;
    }

    var text = string.Empty;
    while (string.IsNullOrWhiteSpace(text))
    {
        Console.WriteLine("> Introduzca la palabra o frase");
        text = Console.ReadLine();
    }

    var count = CountAllCharacters(text);

    Console.WriteLine($"> A continuación se muestra el recuento de letras para la palabra o frase '{text}'");
    if (string.IsNullOrEmpty(character))
    {
        Console.WriteLine($"> Tiene {count.Values.Sum()} letras en total");
        foreach (var item in count)
        {
            Console.WriteLine($"> El carácter '{item.Key}' aparece {item.Value}");
        }
    }
    else if (count.TryGetValue(character.ToLower(), out var value))
    {
        Console.WriteLine($"> El carácter '{character}' aparece {value} veces");
    }
    else
    {
        Console.WriteLine($"> El carácter '{character}' no aparece en la palabra o frase");
    }

    Console.WriteLine();
}



int CountCharacters(string text, string character)
{
    return text
        .Count(c => char.IsLetter(c) && (string.IsNullOrEmpty(character) || c.ToString().ToLower() == character.ToLower()));
}
// int CountCharacters(string text, string character)
// {
//     var count = 0;

//     foreach (var c in text)
//     {
//         if (!char.IsLetter(c)) continue;
//         if (string.IsNullOrEmpty(character)) count++;
//         else if (c.ToString().ToLower() == character.ToLower()) count++;
//     }

//     return count;
// }


Dictionary<string, int> CountAllCharacters(string text)
{
    return text
        .Where(char.IsLetter)
        .GroupBy(char.ToLower)
        .ToDictionary(g => g.Key.ToString(), g => g.Count());
}
// Dictionary<string, int> CountAllCharacters(string text)
// {
//     var count = new Dictionary<string, int>();

//     foreach (var c in text)
//     {
//         if (!char.IsLetter(c)) continue;
//         var key = c.ToString().ToLower();
//         if (count.ContainsKey(key))
//         {
//             count[key]++;
//         }
//         else
//         {
//             count.Add(key, 1);
//         }
//     }

//     return count;
// }
