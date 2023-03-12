using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bergs.ProvacSharp.BD;


namespace ClienteEmpresa
{
    public class Cliente
    {
        public String CpfCnpj { get; set; }
        public TipoPessoa TipoPessoa { get; set; }
        public String Nome { get; set; }
        public DateTime Data { get; set; }
        public Decimal Renda { get; set; }

    }

    public enum TipoPessoa
    {
        Fisica = 'F',
        Juridica = 'J'
    }
    
    public class Clientes
    {
        List<Cliente> ListaClientes = new List<Cliente>();
        public String endereco;
        //Cliente clienteList;

        //AcessoBancoDados db = new AcessoBancoDados(@"C:\Users\Vinícius\Desktop\leo\ProvaV4\ClienteEmpresa\dados\exercicioextra.mdb");

        public Clientes(String endereco)
        {
            using (AcessoBancoDados bd = new AcessoBancoDados(endereco))
            {
                this.endereco = endereco;
                bd.Abrir();
                List<Linha> linhas = bd.Consultar("SELECT CPF_CNPJ, TIPO_PESSOA, NOME, DATA, RENDA FROM CLIENTE");
                Cliente cliente;

                foreach(Linha linha in linhas)
                {
                    cliente = new Cliente();
                    cliente.TipoPessoa = (TipoPessoa)Char.Parse(linha.Campos[1].Conteudo.ToString());
                    cliente.CpfCnpj = linha.Campos[0].Conteudo.ToString();
                    cliente.Nome = linha.Campos[2].Conteudo.ToString();
                    cliente.Data = DateTime.Parse(linha.Campos[3].Conteudo.ToString());

                    if (cliente.TipoPessoa == TipoPessoa.Fisica)
                    {
                       cliente.Renda = Decimal.Parse(linha.Campos[4].Conteudo.ToString());
                    }

                    ListaClientes.Add(cliente);
                }
                
                


            }
        }

        public bool Incluir(Cliente cliente)
        {
            if(cliente.TipoPessoa == TipoPessoa.Fisica)
            {
                if(DateTime.Now.AddYears(-18).Year < cliente.Data.Year)
                {
                    Console.WriteLine("Entrei no erro da data.");
                    return false;
                }

                Cliente clientTeste = ListaClientes.Find(x => x.CpfCnpj == cliente.CpfCnpj);
                if(clientTeste != null)
                {
                    Console.WriteLine("Entrei no erro de cpfs iguais");
                    return false;
                }

                if(cliente.Renda <= 0)
                {
                    Console.WriteLine("Entrei no erro da renda.");
                    return false;
                }

                Console.WriteLine("Passei na parte de incluir cliente.");
                ListaClientes.Add(cliente);
                return true;
 
            }

            if(cliente.TipoPessoa == TipoPessoa.Juridica)
            {
                if(DateTime.Now.AddYears(-2).Year < cliente.Data.Year)
                {
                    Console.WriteLine("Empresa com Menos de 2 anos.");
                    return false;
                }

                Cliente clientTeste = ListaClientes.Find(x => x.CpfCnpj == cliente.CpfCnpj);
                if (clientTeste != null)
                {
                    Console.WriteLine("Entrei no erro de CNPJs iguais");
                    return false;
                }

                ListaClientes.Add(cliente);
                return true;

                
            }

            Console.WriteLine("Passe por tudo.");
            return false;
        }

        public List<Cliente> Listar()
        {
            StringBuilder lista = new StringBuilder();
            StringBuilder pf;
            StringBuilder pj;
            foreach(Cliente cliente in ListaClientes)
            {
                if(cliente.TipoPessoa == TipoPessoa.Fisica)
                {
                    pf = new StringBuilder();
                    pf.AppendFormat("{0}.{1}.{2}-{3}", cliente.CpfCnpj.Substring(0, 3), cliente.CpfCnpj.Substring(3, 3),
                        cliente.CpfCnpj.Substring(6, 3), cliente.CpfCnpj.Substring(9, 2));
                    lista.AppendFormat("{0} - {1:dd/MM/yyyy} - {2} - {3}\n", pf, cliente.Data, cliente.Nome, cliente.Renda.ToString("C"));
                }

                if(cliente.TipoPessoa == TipoPessoa.Juridica)
                {
                    pj = new StringBuilder();
                    pj.AppendFormat("{0}.{1}.{2}/{3}-{4}", cliente.CpfCnpj.Substring(0, 2), cliente.CpfCnpj.Substring(2, 3),
                        cliente.CpfCnpj.Substring(5, 3), cliente.CpfCnpj.Substring(8, 4), cliente.CpfCnpj.Substring(12, 2));
                    lista.AppendFormat("{0} - {1:dd/MM/yyyy} - {2}\n", pj, cliente.Data, cliente.Nome);
                }
                   
            }
            Console.WriteLine(lista);
            return ListaClientes;
        }

        public bool Excluir(String cpfcnpj, TipoPessoa tipoPessoa)
        {
            foreach(Cliente cliente in ListaClientes)
            {
                if(cliente.CpfCnpj == cpfcnpj && cliente.TipoPessoa == TipoPessoa.Fisica)
                {
                    ListaClientes.Remove(cliente);
                    return true;
                }

                if(cliente.CpfCnpj == cpfcnpj && cliente.TipoPessoa == TipoPessoa.Juridica)
                {
                    ListaClientes.Remove(cliente);
                    return true;
                }

            }
            return false;
        }

        public bool Salvar()
        {
            using (AcessoBancoDados bd = new AcessoBancoDados(endereco))
            {
                Console.WriteLine("Cheguei aqui.");
                
                bd.Abrir();
                bd.ExecutarDelete("DELETE FROM CLIENTE");

                foreach (Cliente cliente in ListaClientes)
                {
                    StringBuilder sql = new StringBuilder();

                    if(cliente.TipoPessoa == TipoPessoa.Fisica)
                    {
                        sql.Append("INSERT INTO CLIENTE (CPF_CNPJ, TIPO_PESSOA, NOME, DATA, RENDA) VALUES (");
                        sql.AppendFormat("'{0}' , ", cliente.CpfCnpj);
                        sql.AppendFormat("'{0}', ", ((char)cliente.TipoPessoa).ToString());
                        sql.AppendFormat("'{0}', ", cliente.Nome);
                        sql.AppendFormat("'{0}', ", cliente.Data.ToString("MM/dd/yyyy"));
                        sql.Append(cliente.Renda);
                        sql.Append(")");
                        Console.WriteLine(sql.ToString());
                    }

                    Console.WriteLine("Cheguei aqui 2.");
                    Console.ReadKey();

                    if(cliente.TipoPessoa == TipoPessoa.Juridica)
                    {
                        sql.Append("INSERT INTO CLIENTE (CPF_CNPJ, TIPO_PESSOA, NOME, DATA) VALUES (");
                        sql.AppendFormat("'{0}' , ", cliente.CpfCnpj);
                        sql.AppendFormat("'{0}', ", ((char)cliente.TipoPessoa).ToString());
                        sql.AppendFormat("'{0}', ", cliente.Nome);
                        sql.AppendFormat("'{0}')", cliente.Data.ToString("MM/dd/yyyy"));
                        
                        Console.WriteLine(sql.ToString());
                    }

                    Console.WriteLine("Cheguei aqui 3.");
                    Console.ReadKey();

                    if (!bd.ExecutarInsert(sql.ToString()))
                    {
                        Console.WriteLine("Cheguei aqui 4.");
                        Console.ReadKey();
                        return false;
                    }

                }
                Console.WriteLine("Cheguei aqui 5.");
                Console.ReadKey();
                bd.EfetivarComandos();
                return true;
            }
        }
    }
    
}
