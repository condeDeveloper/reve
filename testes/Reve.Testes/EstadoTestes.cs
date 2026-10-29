using Conde.Reve;
using Xunit;

namespace Conde.Reve.Testes;

public class EstadoTestes
{
    [Theory]
    [InlineData(3, 3, 27)]
    [InlineData(4, 12, 16777216)]
    [InlineData(5, 9, 1953125)]
    [InlineData(4, 0, 1)]
    public void AQuantidadeDeEstadosEHastesElevadoADiscos(int hastes, int discos, int esperado)
    {
        Assert.Equal(esperado, Estado.Quantidade(hastes, discos));
    }

    [Fact]
    public void TudoNumaHasteSoTemTodosOsDiscosLa()
    {
        var estado = Estado.TudoNa(2, 4, 5);
        for (var disco = 0; disco < 5; disco++)
        {
            Assert.Equal(2, Estado.HasteDo(estado, disco, 4));
        }
    }

    [Fact]
    public void OTopoDeUmaHasteEOMenorDiscoDela()
    {
        // Discos 0 e 2 na haste 1, discos 1 e 3 na haste 0.
        var estado = Estado.TudoNa(0, 4, 4);
        estado = Estado.Mover(estado, 0, 1, 4);
        estado = Estado.Mover(estado, 2, 1, 4);

        var topos = Estado.Topos(estado, 4, 4);
        Assert.Equal(1, topos[0]);
        Assert.Equal(0, topos[1]);
        Assert.Equal(-1, topos[2]);
        Assert.Equal(-1, topos[3]);
    }

    [Fact]
    public void AHasteVaziaTemTopoMenosUm()
    {
        var topos = Estado.Topos(Estado.TudoNa(0, 4, 3), 4, 3);
        Assert.Equal(0, topos[0]);
        Assert.Equal(-1, topos[1]);
        Assert.Equal(-1, topos[2]);
        Assert.Equal(-1, topos[3]);
    }

    [Fact]
    public void MoverTrocaSoAHasteDaquelerDisco()
    {
        var estado = Estado.TudoNa(0, 4, 5);
        var depois = Estado.Mover(estado, 3, 2, 4);
        for (var disco = 0; disco < 5; disco++)
        {
            Assert.Equal(disco == 3 ? 2 : 0, Estado.HasteDo(depois, disco, 4));
        }
    }

    [Fact]
    public void MoverParaAMesmaHasteNaoMudaNada()
    {
        var estado = Estado.TudoNa(1, 4, 4);
        Assert.Equal(estado, Estado.Mover(estado, 2, 1, 4));
    }

    [Fact]
    public void CadaEstadoTemUmaCodificacaoSo()
    {
        var vistos = new HashSet<string>();
        var total = Estado.Quantidade(4, 5);
        for (var e = 0; e < total; e++)
        {
            var chave = string.Join(",", Enumerable.Range(0, 5).Select(d => Estado.HasteDo(e, d, 4)));
            Assert.True(vistos.Add(chave));
        }

        Assert.Equal(total, vistos.Count);
    }

    [Fact]
    public void OTextoMostraAsPilhasDeBaixoParaCima()
    {
        var estado = Estado.TudoNa(0, 3, 3);
        Assert.Equal("2,1,0 |  | ", Estado.Texto(estado, 3, 3));
    }

    [Fact]
    public void ArgumentosInvalidosNaoSaoAceitos()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Estado.Quantidade(0, 3));
        Assert.Throws<ArgumentOutOfRangeException>(() => Estado.Quantidade(4, -1));
    }
}
