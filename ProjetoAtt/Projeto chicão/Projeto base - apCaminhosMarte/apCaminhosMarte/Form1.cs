
using apExercicios1_a_10;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace apCaminhosMarte
{


    public partial class Form1 : Form
    {
        Arvore<CidadesMarte> cidades;
        Grafo mapa = new Grafo(null);
        private Font fnt = new Font("Arial", 10);
        ListaSimples<Rota> rotas = new ListaSimples<Rota>();
        Dictionary<String, int> cidadesId = new Dictionary<String, int>();

        public Form1()
        {
            InitializeComponent();
        }

        private void TxtCaminhos_DoubleClick(object sender, EventArgs e)
        {
           
        }

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            int profundidade = 0;
            int[,] caminho = new int[100, 100];
            
            MessageBox.Show(findIndexByString("Acheron") + "");
            BuscaCaminho(findIndexByString("Acheron"), findIndexByString("Arena"),ref profundidade,ref caminho);
            
        }

        private int findIndexByString(string cidade)
        {
            for(int index = 0; index < cidadesId.Count; index++)
                if (cidadesId.ElementAt(index).Key.Trim() == cidade)
                    return index;

            return -1;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cidades = new Arvore<CidadesMarte>();
            LerCidades(@"C:\Users\u18191\Desktop\Projeto\herculescusapeca-master\Projeto chicão\CidadesMarteOrdenado.txt");
            LerCaminhos(@"C:\Users\u18191\Desktop\Projeto\herculescusapeca-master\Projeto chicão\CaminhosEntreCidadesMarte.txt");
            criaMapa(cidades.Raiz);
            locaCidades(mapa, rotas);

        }

        private void LerCidades(String arq)
        {
            
            StreamReader leitor = new StreamReader(arq, UTF8Encoding.Default);
            while (!leitor.EndOfStream)
            {
                String linha = leitor.ReadLine();
                int id = int.Parse(linha.Substring(0, 3));
                String nome = linha.Substring(3, 16);
                int x = int.Parse(linha.Substring(19, 4));
                int y = int.Parse(linha.Substring(23, 5));

                CidadesMarte nova = new CidadesMarte(id, x, y, nome);
                cidades.Incluir(nova);
                cidadesId.Add(nova.nome, nova.id);
            }
            
            leitor.Close();
            
        }
        private void LerCaminhos(String arq)
        {

            StreamReader leitor = new StreamReader(arq,UTF8Encoding.Default);

            while (!leitor.EndOfStream)
            {
                String linha = leitor.ReadLine();
                int Cidade1 = int.Parse(linha.Substring(0, 3));
                int Cidade2 = int.Parse(linha.Substring(3, 3));
                int distancia = int.Parse(linha.Substring(6, 5));
                int tempo = int.Parse(linha.Substring(11, 4));
                int preco = int.Parse(linha.Substring(15, 5));
                Rota nova = new Rota(Cidade1,Cidade2,distancia,tempo,preco);
                rotas.InserirAposFim(nova);

            }

            leitor.Close();

        }

        private void pbMapa_Paint(object sender, PaintEventArgs e)
        {

            ExibirTodos(cidades.Raiz,e);
           
        }
        private void Pintar(PaintEventArgs e,NoArvore<CidadesMarte> atual)
        {
            Graphics g = e.Graphics;

            g.FillEllipse(Brushes.Black, new Rectangle(atual.Info.x / 4, atual.Info.y / 4, 8, 8));
            e.Graphics.DrawString(atual.Info.nome, fnt, Brushes.Black, atual.Info.x / 4, atual.Info.y / 4 + 10);

        }

        private void ExibirTodos(NoArvore<CidadesMarte> noAtual, PaintEventArgs e)
        {

            if (noAtual != null)
            {       

                Pintar(e,noAtual);                           // conta o nó atual
                ExibirTodos(noAtual.Esq,e);  // conta nós da subárvore esquerda
                ExibirTodos(noAtual.Dir,e);  // conta nós da subárvore direita
            }
            

        }
        private void criaMapa(NoArvore<CidadesMarte> noAtual)
        {
            if (noAtual != null)
            {

                mapa.NovoVertice(noAtual.Info.nome);                        // conta o nó atual
                criaMapa(noAtual.Esq);  // conta nós da subárvore esquerda
                criaMapa(noAtual.Dir);  // conta nós da subárvore direita
            }

        }

        private void locaCidades(Grafo mapa,ListaSimples<Rota> rotas)
        {
            NoLista<Rota> atual = rotas.Primeiro;
            while(atual != null)
            {
                mapa.NovaAresta(atual.Info.idCidade1, atual.Info.idCidade2);
                atual = atual.Prox;
            }
        }
        private void BuscaCaminho(int cidade1,int cidade2, ref int profundidade,ref int[,] caminho)
        { 
            
            int i = 0;
            while (true)
            {
                int atual = mapa.ObterVerticeAdjacenteNaoVisitado(cidade1);

                if (atual == -1)
                    break;

                if (atual == cidade2)
                {
                    caminho[profundidade,i] = atual;
                    break;
                }

                if(atual != cidade1)
                {
                    caminho[profundidade, i] = atual;
                   
                     BuscaCaminho(atual, cidade2, ref profundidade, ref caminho);
                    profundidade++;
                }

            }

            dataGridView1.RowCount = 10;
            dataGridView1.ColumnCount = 10;
            for (int j = 0; j < 10; j++)
                for(int g = 0; g < 10; g++)
                    dataGridView1[j, g].Value = caminho[j, g];           

        }

        private void pnlArvore_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            desenhaArvore(true, cidades.Raiz, (int)pnlArvore.Width / 2, 0, Math.PI / 2,
                                 Math.PI / 2.5, 300, g);
        }

        private void desenhaArvore(bool primeiraVez, NoArvore<CidadesMarte> raiz,
                           int x, int y, double angulo, double incremento,
                           double comprimento, Graphics g)
        {
            int xf, yf;
            if (raiz != null)
            {
                Pen caneta = new Pen(Color.Red);
                xf = (int)Math.Round(x + Math.Cos(angulo) * comprimento);
                yf = (int)Math.Round(y + Math.Sin(angulo) * comprimento);
                if (primeiraVez)
                    yf = 25;
                g.DrawLine(caneta, x, y, xf, yf);
                // sleep(100);
                desenhaArvore(false, raiz.Esq, xf, yf, Math.PI / 2 + incremento,
                                                 incremento * 0.60, comprimento * 0.8, g);
                desenhaArvore(false, raiz.Dir, xf, yf, Math.PI / 2 - incremento,
                                                  incremento * 0.60, comprimento * 0.8, g);
                // sleep(100);
                SolidBrush preenchimento = new SolidBrush(Color.Blue);
                g.FillEllipse(preenchimento, xf - 15, yf - 15, 30, 30);
                g.DrawString(Convert.ToString(raiz.Info), new Font("Comic Sans", 12),
                              new SolidBrush(Color.Yellow), xf - 15, yf - 10);
            }
        }
    } 
}
