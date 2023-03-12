using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClienteEmpresa;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            Cliente cliente = new Cliente();
            cliente.TipoPessoa = (TipoPessoa)'F';
            Console.WriteLine(cliente.TipoPessoa);
            cliente.TipoPessoa = TipoPessoa.Fisica;
            Console.WriteLine(cliente.TipoPessoa);
            Console.ReadKey();

            if(cliente.TipoPessoa == TipoPessoa.Fisica)
            {
                Console.WriteLine("É pessoa física");
                Console.ReadKey();
            }

            if(cliente.TipoPessoa == (TipoPessoa)'F')
            {
                Console.WriteLine("É pessoa física");
                Console.ReadKey();
            }
            */ //Tem colocar a propriedade pública para funcionar o teste acima.

            try
            {
                Clientes listaClientes = new Clientes(@"C:\Users\Vinícius\Desktop\leo\ProvaV4\ClienteEmpresa\dados\exercicioextra.mdb");
                Cliente cliente;
                bool sair = false;
                String controle;

                do
                {
                    Console.WriteLine("1 - Inserir Cliente");
                    Console.WriteLine("2 - Listar Clientes.");
                    Console.WriteLine("3 - Excluir Cliente.");
                    Console.WriteLine("4 - Salvar");
                    Console.WriteLine("0 - Sair.");

                    controle = Console.ReadLine();

                    switch (int.Parse(controle))
                    {

                        case 1:
                            cliente = new Cliente();
                            Console.WriteLine("O cliente é pessoa física ou jurídica ? (Digite F para Física e J para jurídica.)");
                            cliente.TipoPessoa = (TipoPessoa)char.Parse(Console.ReadLine());

                            if (cliente.TipoPessoa == TipoPessoa.Fisica)
                            {
                                Console.WriteLine("Diget o CPF do cliente:");
                                cliente.CpfCnpj = Console.ReadLine();
                                Console.WriteLine("Digite o nome do cliente:");
                                cliente.Nome = Console.ReadLine();
                                Console.WriteLine("Digite a data de nascimento do Cliente: ");
                                cliente.Data = DateTime.Parse(Console.ReadLine());
                                Console.WriteLine("Digite a renda do cliente: ");
                                cliente.Renda = Decimal.Parse(Console.ReadLine());
                            }

                            if (cliente.TipoPessoa == TipoPessoa.Juridica)
                            {
                                Console.WriteLine("Diget o CNPJ da Empresa:");
                                cliente.CpfCnpj = Console.ReadLine();
                                Console.WriteLine("Digite o nome da Empresa:");
                                cliente.Nome = Console.ReadLine();
                                Console.WriteLine("Digite a data de constituição da empresa:");
                                cliente.Data = DateTime.Parse(Console.ReadLine());

                            }





                            if (listaClientes.Incluir(cliente))
                            {
                                Console.WriteLine("Cliente Inserido com Sucesso");
                            }
                            else
                            {
                                Console.WriteLine("Erro em inserir o cliente.");
                            }

                            break;

                        case 2:
                            listaClientes.Listar();
                            break;

                        case 3:
                            Console.WriteLine("Digite \"F\" para pessoa física ou \"J\" para pessoa jurídica: ");
                            TipoPessoa tipo = (TipoPessoa)char.Parse(Console.ReadLine());
                            String CpfCnpj;
                            if (tipo == TipoPessoa.Fisica)
                            {
                                Console.WriteLine("Digite o CPF do cliente:");
                                CpfCnpj = Console.ReadLine();

                                if (listaClientes.Excluir(CpfCnpj, tipo))
                                {
                                    Console.WriteLine("Pessoa física excluída com sucesso.");
                                }
                                else
                                {
                                    Console.WriteLine("Problema na exclusão da pessoa física.");
                                }


                            }

                            if (tipo == TipoPessoa.Juridica)
                            {
                                Console.WriteLine("Digite o CNPJ da empresa: ");
                                CpfCnpj = Console.ReadLine();

                                if (listaClientes.Excluir(CpfCnpj, tipo))
                                {
                                    Console.WriteLine("Empresa excluída com sucesso.");
                                }
                                else
                                {
                                    Console.WriteLine("Problema na exclusão da empresa.");
                                }
                            }
                            
                            break;

                        case 4:
                            if (listaClientes.Salvar())
                            {
                                Console.WriteLine("Dados salvos com Sucesso.");
                            }
                            else
                            {
                                Console.WriteLine("Erro no salvamento.");
                            }
                            break;

                        case 0:
                            sair = true;
                            break;

                        default:
                            break;
                    }



                } while (!sair);
            }
            catch (Exception e)
            {

                Console.WriteLine("O seguinte erro aconteceu: {0}", e);
            }

            
            
        

            
            
        }
    }
}
