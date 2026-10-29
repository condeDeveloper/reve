using Conde.Reve;

if (args.Length == 0)
{
    Console.WriteLine("uso: medidor <confronto|cortes|ganho|alcance>");
    return 1;
}

switch (args[0])
{
    case "confronto":
        Confronto();
        break;
    case "cortes":
        Cortes();
        break;
    case "ganho":
        Ganho();
        break;
    case "alcance":
        Alcance();
        break;
    default:
        Console.WriteLine($"comando desconhecido: {args[0]}");
        return 1;
}

return 0;

static void Confronto()
{
    Console.WriteLine("A varredura de todos os estados contra a conta de Frame e Stewart");
    Console.WriteLine();
    Console.WriteLine("Com 4 hastes:");
    Console.WriteLine("discos  varredura  formula  concordam  estados     alcancados");
    foreach (var l in Medidas.Confronto(4, 12))
    {
        Console.WriteLine($"{l.Discos,-8}{l.Varredura,-11}{l.Formula,-9}{(l.Concordam ? "sim" : "NAO"),-11}{l.Estados,-12}{l.Alcancados}");
    }

    Console.WriteLine();
    Console.WriteLine("Com 5 hastes:");
    Console.WriteLine("discos  varredura  formula  concordam  estados     alcancados");
    foreach (var l in Medidas.Confronto(5, 9))
    {
        Console.WriteLine($"{l.Discos,-8}{l.Varredura,-11}{l.Formula,-9}{(l.Concordam ? "sim" : "NAO"),-11}{l.Estados,-12}{l.Alcancados}");
    }

    Console.WriteLine();
    Console.WriteLine("Com 3 hastes:");
    Console.WriteLine("discos  varredura  formula  concordam  estados     alcancados");
    foreach (var l in Medidas.Confronto(3, 12))
    {
        Console.WriteLine($"{l.Discos,-8}{l.Varredura,-11}{l.Formula,-9}{(l.Concordam ? "sim" : "NAO"),-11}{l.Estados,-12}{l.Alcancados}");
    }
}

static void Cortes()
{
    Console.WriteLine("Os cortes que alcancam o minimo, com 4 hastes");
    Console.WriteLine("discos  lances  cortes otimos  quantos");
    foreach (var l in Medidas.Cortes(30))
    {
        Console.WriteLine($"{l.Discos,-8}{l.Lances,-8}{l.Cortes,-15}{l.Quantos}");
    }
}

static void Ganho()
{
    Console.WriteLine("O que a quarta e a quinta haste economizam");
    Console.WriteLine("discos  3 hastes       4 hastes  5 hastes  3/4");
    foreach (var l in Medidas.Ganho([5, 10, 15, 20, 25, 30, 40, 50]))
    {
        Console.WriteLine($"{l.Discos,-8}{l.TresHastes,-18}{l.QuatroHastes,-10}{l.CincoHastes,-10}{l.Razao}");
    }
}

static void Alcance()
{
    Console.WriteLine("Quantos estados a varredura alcanca, e o mais distante do comeco");
    Console.WriteLine("hastes  discos  estados    alcancados  todos  ate o fim  mais distante");
    foreach (var (hastes, discos) in new[] { (3, 10), (4, 8), (4, 10), (4, 12), (5, 8) })
    {
        var (estados, alcancados, todos) = Medidas.Alcance(hastes, discos);
        var (ateOFim, maisDistante) = Medidas.Diametro(hastes, discos);
        Console.WriteLine($"{hastes,-8}{discos,-8}{estados,-11}{alcancados,-12}{(todos ? "sim" : "NAO"),-7}{ateOFim,-11}{maisDistante}");
    }
}
