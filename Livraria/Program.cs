using System;
using System.Collections.Generic;
using System.Linq;

public class Emprestimo
{
    public DateTime dtEmprestimo { get; set; }
    public DateTime dtDevolucao { get; set; }
}

public class Exemplar
{
    public int tombo { get; set; }
    public List<Emprestimo> emprestimos { get; set; } = new List<Emprestimo>();

    public bool emprestar()
    {
        if (disponivel())
        {
            Emprestimo emprestimo = new Emprestimo
            {
                dtEmprestimo = DateTime.Now,
                dtDevolucao = DateTime.Now.AddDays(15) // Exemplo: devolução em 15 dias
            };

            emprestimos.Add(emprestimo);
            return true;
        }
        return false;
    }

    public bool devolver()
    {
        if (emprestimos.Any())
        {
            Emprestimo emprestimo = emprestimos.Last();
            emprestimo.dtDevolucao = DateTime.Now;
            return true;
        }
        return false;
    }

    public bool disponivel()
    {
        return !emprestimos.Any(e => e.dtDevolucao >= DateTime.Now);
    }

    public int qtdeEmprestimos()
    {
        return emprestimos.Count;
    }
}

public class Livro
{
    public int isbn { get; set; }
    public string titulo { get; set; }
    public string autor { get; set; }
    public string editora { get; set; }
    public List<Exemplar> exemplares { get; set; } = new List<Exemplar>();

    public void adicionarExemplar(Exemplar exemplar)
    {
        exemplares.Add(exemplar);
    }

    public int qtdeExemplares()
    {
        return exemplares.Count;
    }

    public int qtdeDisponiveis()
    {
        return exemplares.Count(e => e.disponivel());
    }

    public int qtdeEmprestimos()
    {
        return exemplares.Sum(e => e.qtdeEmprestimos());
    }

    public double percDisponibilidade()
    {
        if (qtdeExemplares() == 0)
            return 0.0;
        else
            return (double)qtdeDisponiveis() / qtdeExemplares() * 100.0;
    }
}

public class Livros
{
    public List<Livro> acervo { get; set; } = new List<Livro>();

    public void adicionar(Livro livro)
    {
        acervo.Add(livro);
    }

    public Livro pesquisar(Livro livro)
    {
        return acervo.FirstOrDefault(l => l.isbn == livro.isbn);
    }
}

class Program
{
    static void Main()
    {
        Livros biblioteca = new Livros();

        while (true)
        {
            Console.WriteLine("\nMENU DE OPÇÕES:");
            Console.WriteLine("0. Sair");
            Console.WriteLine("1. Adicionar livro");
            Console.WriteLine("2. Pesquisar livro (sintético)");
            Console.WriteLine("3. Pesquisar livro (analítico)");
            Console.WriteLine("4. Adicionar exemplar");
            Console.WriteLine("5. Registrar empréstimo");
            Console.WriteLine("6. Registrar devolução");

            Console.Write("Escolha uma opção: ");
            int opcao;
            if (int.TryParse(Console.ReadLine(), out opcao))
            {
                switch (opcao)
                {
                    case 0:
                        Environment.Exit(0);
                        break;
                    case 1:
                        AdicionarLivro(biblioteca);
                        break;
                    case 2:
                        PesquisarLivroSintetico(biblioteca);
                        break;
                    case 3:
                        PesquisarLivroAnalitico(biblioteca);
                        break;
                    case 4:
                        AdicionarExemplar(biblioteca);
                        break;
                    case 5:
                        RegistrarEmprestimo(biblioteca);
                        break;
                    case 6:
                        RegistrarDevolucao(biblioteca);
                        break;
                    default:
                        Console.WriteLine("Opção inválida. Tente novamente.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Opção inválida. Tente novamente.");
            }
        }
    }

    private static void AdicionarLivro(Livros biblioteca)
    {
        Console.WriteLine("\nADICIONAR LIVRO");

        Livro novoLivro = new Livro();

        Console.Write("ISBN: ");
        novoLivro.isbn = int.Parse(Console.ReadLine());

        Console.Write("Título: ");
        novoLivro.titulo = Console.ReadLine();

        Console.Write("Autor: ");
        novoLivro.autor = Console.ReadLine();

        Console.Write("Editora: ");
        novoLivro.editora = Console.ReadLine();

        biblioteca.adicionar(novoLivro);
        Console.WriteLine("Livro adicionado com sucesso!");
    }

    private static void PesquisarLivroSintetico(Livros biblioteca)
    {
        Console.WriteLine("\nPESQUISAR LIVRO (SINTÉTICO)");

        Console.Write("ISBN do livro a pesquisar: ");
        int isbn = int.Parse(Console.ReadLine());

        Livro livroEncontrado = biblioteca.pesquisar(new Livro { isbn = isbn });

        if (livroEncontrado != null)
        {
            Console.WriteLine($"Título: {livroEncontrado.titulo}");
            Console.WriteLine($"Autor: {livroEncontrado.autor}");
            Console.WriteLine($"Editora: {livroEncontrado.editora}");
            Console.WriteLine($"Total de Exemplares: {livroEncontrado.qtdeExemplares()}");
            Console.WriteLine($"Exemplares Disponíveis: {livroEncontrado.qtdeDisponiveis()}");
            Console.WriteLine($"Total de Empréstimos: {livroEncontrado.qtdeEmprestimos()}");
            Console.WriteLine($"Percentual de Disponibilidade: {livroEncontrado.percDisponibilidade()}%");
        }
        else
        {
            Console.WriteLine("Livro não encontrado.");
        }
    }

    private static void PesquisarLivroAnalitico(Livros biblioteca)
    {
        Console.WriteLine("\nPESQUISAR LIVRO (ANALÍTICO)");

        Console.Write("ISBN do livro a pesquisar: ");
        int isbn = int.Parse(Console.ReadLine());

        Livro livroEncontrado = biblioteca.pesquisar(new Livro { isbn = isbn });

        if (livroEncontrado != null)
        {
            Console.WriteLine($"Título: {livroEncontrado.titulo}");
            Console.WriteLine($"Autor: {livroEncontrado.autor}");
            Console.WriteLine($"Editora: {livroEncontrado.editora}");
            Console.WriteLine($"Total de Exemplares: {livroEncontrado.qtdeExemplares()}");
            Console.WriteLine($"Exemplares Disponíveis: {livroEncontrado.qtdeDisponiveis()}");
            Console.WriteLine($"Total de Empréstimos: {livroEncontrado.qtdeEmprestimos()}");
            Console.WriteLine($"Percentual de Disponibilidade: {livroEncontrado.percDisponibilidade()}%");

            Console.WriteLine("\nDETALHES DOS EXEMPLARES E EMPRÉSTIMOS:");

            foreach (var exemplar in livroEncontrado.exemplares)
            {
                Console.WriteLine($"Tombo: {exemplar.tombo}");
                Console.WriteLine($"Empréstimos: {exemplar.qtdeEmprestimos()}");

                if (exemplar.qtdeEmprestimos() > 0)
                {
                    Console.WriteLine("Detalhes dos Empréstimos:");

                    foreach (var emprestimo in exemplar.emprestimos)
                    {
                        Console.WriteLine($"Data de Empréstimo: {emprestimo.dtEmprestimo}");
                        Console.WriteLine($"Data de Devolução: {emprestimo.dtDevolucao}");
                    }
                }

                Console.WriteLine("----------------------------------------");
            }
        }
        else
        {
            Console.WriteLine("Livro não encontrado.");
        }
    }

    private static void AdicionarExemplar(Livros biblioteca)
    {
        Console.WriteLine("\nADICIONAR EXEMPLAR");

        Console.Write("ISBN do livro a adicionar exemplar: ");
        int isbn = int.Parse(Console.ReadLine());

        Livro livroEncontrado = biblioteca.pesquisar(new Livro { isbn = isbn });

        if (livroEncontrado != null)
        {
            Exemplar novoExemplar = new Exemplar();

            Console.Write("Tombo do exemplar: ");
            novoExemplar.tombo = int.Parse(Console.ReadLine());

            livroEncontrado.adicionarExemplar(novoExemplar);
            Console.WriteLine("Exemplar adicionado com sucesso!");
        }
        else
        {
            Console.WriteLine("Livro não encontrado.");
        }
    }

    private static void RegistrarEmprestimo(Livros biblioteca)
    {
        Console.WriteLine("\nREGISTRAR EMPRÉSTIMO");

        Console.Write("ISBN do livro: ");
        int isbn = int.Parse(Console.ReadLine());

        Livro livroEncontrado = biblioteca.pesquisar(new Livro { isbn = isbn });

        if (livroEncontrado != null)
        {
            Console.Write("Tombo do exemplar: ");
            int tombo = int.Parse(Console.ReadLine());

            Exemplar exemplar = livroEncontrado.exemplares.FirstOrDefault(e => e.tombo == tombo);

            if (exemplar != null)
            {
                if (exemplar.emprestar())
                {
                    Emprestimo emprestimo = new Emprestimo
                    {
                        dtEmprestimo = DateTime.Now,
                        dtDevolucao = DateTime.Now.AddDays(15) // Exemplo: devolução em 15 dias
                    };

                    exemplar.emprestimos.Add(emprestimo);
                    Console.WriteLine("Empréstimo registrado com sucesso!");
                }
                else
                {
                    Console.WriteLine("Exemplar não disponível para empréstimo.");
                }
            }
            else
            {
                Console.WriteLine("Exemplar não encontrado.");
            }
        }
        else
        {
            Console.WriteLine("Livro não encontrado.");
        }
    }

    private static void RegistrarDevolucao(Livros biblioteca)
    {
        Console.WriteLine("\nREGISTRAR DEVOLUÇÃO");

        Console.Write("ISBN do livro: ");
        int isbn = int.Parse(Console.ReadLine());

        Livro livroEncontrado = biblioteca.pesquisar(new Livro { isbn = isbn });

        if (livroEncontrado != null)
        {
            Console.Write("Tombo do exemplar: ");
            int tombo = int.Parse(Console.ReadLine());

            Exemplar exemplar = livroEncontrado.exemplares.FirstOrDefault(e => e.tombo == tombo);

            if (exemplar != null)
            {
                if (exemplar.devolver())
                {
                    Console.WriteLine("Devolução registrada com sucesso!");
                }
                else
                {
                    Console.WriteLine("Exemplar já foi devolvido ou não estava emprestado.");
                }
            }
            else
            {
                Console.WriteLine("Exemplar não encontrado.");
            }
        }
        else
        {
            Console.WriteLine("Livro não encontrado.");
        }
    }
}
