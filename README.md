# O quebra-cabeça de Reve

A torre de Hanói com **quatro hastes** em vez de três. Dudeney publicou a versão em 1907
e chamou de quebra-cabeça de Reve; Frame e Stewart propuseram uma conta para o número
mínimo de lances em 1941, cada um por sua conta, no mesmo número da mesma revista.

A conta é esta:

```
FS(n, p) = min sobre k de [ 2·FS(k, p) + FS(n−k, p−1) ]
```

Leve os `k` discos de cima para uma haste de descanso usando tudo o que há, leve os
`n−k` de baixo com uma haste a menos, e traga os `k` de volta. Com três hastes não há
escolha e a conta vira `2ⁿ − 1`.

**Que esse corte seja o melhor possível ficou em aberto por 73 anos** — só foi provado,
para quatro hastes, em 2014. Aqui ela não é usada como verdade: é conferida, disco por
disco, contra uma busca em largura sobre todos os estados.

```
dotnet test testes/Reve.Testes/Reve.Testes.csproj -c Release
dotnet run  --project ferramentas/Medidor/Medidor.csproj -c Release -- confronto
dotnet run  --project ferramentas/Medidor/Medidor.csproj -c Release -- cortes
dotnet run  --project ferramentas/Medidor/Medidor.csproj -c Release -- ganho
dotnet run  --project ferramentas/Medidor/Medidor.csproj -c Release -- alcance
```

## O juiz

Um estado é "em que haste está cada disco". Dentro de uma haste a ordem está determinada
pela regra do jogo, então o estado inteiro é um número na base `hastes`, com um algarismo
por disco. Com 4 hastes e 12 discos são **16.777.216 estados**, que cabem num vetor.

A busca em largura sai de tudo na primeira haste e anda até não haver mais para onde ir.
Ela não sabe o que é uma torre de Hanói, não tem recursão e não tem palpite sobre onde
cortar a pilha.

## Os achados

### 1. A conjectura de 1941 bate com a varredura em todos os casos medidos

```
Com 4 hastes:
discos  varredura  formula  concordam  estados     alcancados
1       1          1        sim        4           4
4       9          9        sim        256         256
7       25         25       sim        16384       16384
10      49         49       sim        1048576     1048576
11      65         65       sim        4194304     4194304
12      81         81       sim        16777216    16777216
```

Doze discos com quatro hastes, nove com cinco hastes, doze com três. Nenhuma
discordância. É uma conferência modesta perto da prova de 2014, e é o tipo de coisa que
dá para fazer num repositório: a conta diz 81 e a varredura de dezesseis milhões de
estados também diz 81.

### 2. O corte ótimo só é único nos números triangulares

```
discos  lances  cortes otimos  quantos
1       1       1              1
2       3       1,2            2
3       5       2              1
6       17      3              1
10      49      4              1
15      129     5              1
21      321     6              1
28      769     7              1
20      289     5,6            2
30      1025    7,8            2
```

Em 23 dos 30 tamanhos há **dois** cortes diferentes que dão exatamente o mesmo mínimo. O
corte é único em 1, 3, 6, 10, 15, 21 e 28 — e essa lista é exatamente a dos **números
triangulares**.

Não é coincidência de tabela pequena: a lista continua, e o padrão aparece nos sete
valores que cabem até 30 discos. O enunciado da conta de Frame e Stewart fala em "o" corte
ótimo, no singular, e na maioria dos tamanhos ele não existe no singular.

### 3. O estado mais distante do começo é sempre o próprio destino

```
hastes  discos  estados    alcancados  todos  ate o fim  mais distante
3       10      59049      59049       sim    1023       1023
4       8       65536      65536       sim    33         33
4       12      16777216   16777216    sim    81         81
5       8       390625     390625      sim    23         23
```

Duas coisas na mesma tabela. A primeira: **todo estado é alcançável**, nos três números de
hastes — não existe arrumação legal de discos que não dê para montar a partir da torre.

A segunda: entre todos os estados, o mais longe do começo é exatamente aquele em que tudo
está na última haste. Com três hastes isso é conhecido; com quatro e cinco eu não tinha
motivo para esperar, porque as hastes extras dão atalhos que poderiam deixar algum estado
torto mais longe do que o destino. Não deixam.

### 4. A quarta haste muda a escala, não a constante

```
discos  3 hastes          4 hastes  5 hastes  3/4
5       31                13        11        2.4
10      1023              49        31        20.9
20      1048575           289       111       3628.3
30      1073741823        1025      271       1047553
50      1125899906842623  6657      831       169130224852.4
```

Com 5 discos a quarta haste economiza menos de três vezes. Com 30, economiza **um milhão
de vezes**. Com 50 discos a torre de três hastes pede mais de um quatrilhão de lances e a
de quatro pede 6.657.

A quinta haste repete o truque em escala menor: de 6.657 para 831 com 50 discos.

## O que as medidas me corrigiram

- **Eu ia escrever "o corte ótimo".** A conta é escrita com um mínimo, e eu li o mínimo
  como se ele fosse atingido num ponto só. Ele é atingido em dois pontos na maioria dos
  tamanhos, e a tabela dos cortes só entrou no repositório porque eu resolvi imprimir qual
  era o corte e vi dois números na linha.

- **E não vi que a exceção eram os triangulares.** Olhei a coluna de "quantos" por um bom
  tempo achando que o 1 aparecia sem regra. Ele aparece em 1, 3, 6, 10, 15, 21 e 28. Um
  teste prende isso agora, inclusive conferindo que cada um deles é mesmo triangular.

- **Eu esperava algum estado inalcançável.** Com quatro hastes há muito mais arrumações do
  que caminhos óbvios até elas, e me parecia natural que alguma ficasse de fora. Todas são
  alcançáveis, em todos os casos medidos — o que também quer dizer que a busca em largura
  não estava deixando ramo nenhum para trás, e isso eu só soube por medir.

- **Eu ia comparar a varredura com a conta e parar aí.** O número de estados alcançados
  entrou na mesma tabela quase por acidente, e é ele que diz que a varredura está completa.
  Uma busca que concorda com a fórmula mas não visita tudo concordaria por sorte.

## Estrutura

```
src/Reve/
  Estado.cs        o estado como numero na base das hastes, e os topos de cada uma
  Varredura.cs     a busca em largura sobre todos os estados: o juiz
  FrameStewart.cs  a conta de 1941, com os cortes que alcancam o minimo
  Medidas.cs       as tabelas deste README
testes/Reve.Testes/
ferramentas/Medidor/
```

67 testes.
