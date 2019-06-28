using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoinDetection
{
    public class Knn
    {
        int r;
        int g;
        int b;
        int distancia;
        int valor;
        
        public Knn(int r, int g, int b, int valor)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.valor = valor;
        }

        public int R { get => r; set => r = value; }
        public int G { get => g; set => g = value; }
        public int B { get => b; set => b = value; }
        public int Valor { get => valor; set => valor = value; }
        public int Distancia { get => distancia; set => distancia = value; }
    }
}
