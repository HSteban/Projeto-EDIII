using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace apCaminhosMarte
{

    public class Grafo
    {
        private const int NUM_VERTICES = 23;
        private Vertice[] vertices;
        private int[,] adjMatrix;
        int numVerts;
        DataGridView dgv; // para exibir a matriz de adjacência num formulário
        public Grafo(DataGridView dgv)
        {
            this.dgv = dgv;
            vertices = new Vertice[NUM_VERTICES];
            adjMatrix = new int[NUM_VERTICES, NUM_VERTICES];
            numVerts = 0;
            for (int j = 0; j < NUM_VERTICES; j++) // zera toda a matriz
                for (int k = 0; k < NUM_VERTICES; k++)
                    adjMatrix[j, k] = 0;
        }
        public void NovoVertice(string label)
        {
            vertices[numVerts] = new Vertice(label);
            numVerts++;
            if (dgv != null) // se foi passado como parâmetro um dataGridView para exibição
            { // se realiza o seu ajuste para a quantidade de vértices
                dgv.RowCount = numVerts + 1;
                dgv.ColumnCount = numVerts + 1;
                dgv.Columns[numVerts].Width = 45;
            }
        }
        public void NovaAresta(int start, int eend)
        {
            adjMatrix[start, eend] = 1;
            // adjMatrix[eend, start] = 1; ISSO GERA CICLOS!!!
        }
        public void ExibirVertice(int v)
        {
            Console.Write(vertices[v].rotulo + " ");
        }

        public int SemSucessores() // encontra e retorna a linha de um vértice sem sucessores
        {
            bool temAresta;
            for (int linha = 0; linha < numVerts; linha++)
            {
                temAresta = false;
                for (int col = 0; col < numVerts; col++)
                    if (adjMatrix[linha, col] > 0)
                    {
                        temAresta = true;
                        break;
                    }
                if (!temAresta)
                    return linha;
            }
            return -1;
        }
        public void removerVertice(int vert)
        {
            if (dgv != null)
            {
                MessageBox.Show("Matriz de Adjacências antes de remover vértice " +
               Convert.ToString(vert));

                exibirAdjacencias();
            }
            if (vert != numVerts - 1)
            {
                for (int j = vert; j < numVerts - 1; j++) // remove vértice do vetor
                    vertices[j] = vertices[j + 1];
                // remove vértice da matriz
                for (int row = vert; row < numVerts; row++)
                    moverLinhas(row, numVerts - 1);
                for (int col = vert; col < numVerts; col++)
                    moverColunas(col, numVerts - 1);
            }
            numVerts--;
            if (dgv != null)
            {
                MessageBox.Show("Matriz de Adjacências após remover vértice " +
                Convert.ToString(vert));
                exibirAdjacencias();
                MessageBox.Show("Retornando à ordenação");
            }
        }
        private void moverLinhas(int row, int length)
        {
            if (row != numVerts - 1)
                for (int col = 0; col < length; col++)
                    adjMatrix[row, col] = adjMatrix[row + 1, col]; // desloca para excluir
        }
        private void moverColunas(int col, int length)
        {
            if (col != numVerts - 1)
                for (int row = 0; row < length; row++)
                    adjMatrix[row, col] = adjMatrix[row, col + 1]; // desloca para excluir
        }
        public void exibirAdjacencias()
        {
            dgv.RowCount = numVerts + 1;
            dgv.ColumnCount = numVerts + 1;
            for (int j = 0; j < numVerts; j++)
            {
                dgv.Rows[j + 1].Cells[0].Value = vertices[j].rotulo;
                dgv.Rows[0].Cells[j + 1].Value = vertices[j].rotulo;
                for (int k = 0; k < numVerts; k++)
                    dgv.Rows[j + 1].Cells[k + 1].Value = Convert.ToString(adjMatrix[j, k]);
            }
        }

        public String OrdenacaoTopologica()
        {
            Stack<String> gPilha = new Stack<String>(); // para guardar a sequência de vértices
            int origVerts = numVerts;
            while (numVerts > 0)
            {
                int currVertex = SemSucessores();
                if (currVertex == -1)
                    return "Erro: grafo possui ciclos.";
                gPilha.Push(vertices[currVertex].rotulo); // empilha vértice
                removerVertice(currVertex);
            }

            String resultado = "Sequência da Ordenação Topológica: ";
            while (gPilha.Count > 0)
                resultado += gPilha.Pop() + " "; // desempilha para exibir
            return resultado;
        }
        public int ObterVerticeAdjacenteNaoVisitado(int v)
        {
            for (int j = 0; j <= numVerts - 1; j++)
                if ((adjMatrix[v, j] == 1) && (!vertices[j].foiVisitado))
                    return j;
            return -1;
        }

       
    }

}
