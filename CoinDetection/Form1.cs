using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;
//importando a livraria AForge
using AForge.Video;
using AForge.Video.DirectShow;
using AForge.Imaging;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
//EMGU
using Emgu.CV;
using Emgu.Util;
using Emgu.CV.Structure;







namespace CoinDetection
{

    public partial class Form1 : Form
    {
        static public PictureBox mainPicture;
        static public Bitmap auxBitmap;
        static public Bitmap bitmapWithHough;
        public static FilterInfoCollection avaiableCams;
        static public bool canUse = false;
        static public List<Bitmap> bitmapsQueue = new List<Bitmap>();
        static public List<bool> bitmapsCanUse = new List<bool>();
        public int queueCount = 0;
        Thread threadCalc = new Thread(Calc);
        public Thread threadHough;
        public int currentFrame = 0;
        public int auxFrame = 0;
        public static int widthPic;
        public static int heightPic;
        public static Rectangle rect;
        public static IntPtr circulos;
        public static int recX;
        public static int recY;
        public static Pen recPen = new Pen(Color.Green);
        public static int maxX;
        public static int maxY;
        public static int r;
        public static int r2;
        public static int sleeper = 10;
        public static bool changed;
        public static bool calcColors = true;
        public static bool makingHough = false;
        public static bool makingUniformization = false;
        public static List<Knn> knnList;
        public static bool doubleCircle;
        public static float brilho;
        public static ImageAttributes imageAttributes = new ImageAttributes();
        public static double mediaR = 173.868575;//169.03593837535;
        public static double varianciaR = 2334.01390246937;//260.655655211888;
        public static double mediaG = 171.0905;//174.12762605042;
        public static double varianciaG = 3200.62365975;//415.569769014224;
        public static double mediaB = 148.301275;//164.138046218487;
        public static double varianciaB = 3189.01585837438;//579.97481579058;
        public static string textPila;
        public static System.Windows.Forms.Timer temporizador = new System.Windows.Forms.Timer();
        

        public Form1()
        {
            InitializeComponent();
        }
        
        //Cria um objeto WebCam
        VideoCaptureDevice cam;

        private void Form1_Load(object sender, EventArgs e)
        {
            temporizador.Tick += new EventHandler(SetPila);
            temporizador.Interval = 1000;
            temporizador.Start();
            knnList = new List<Knn>();
            LoadBase();
            infoCam.Visible = true;
            //lista todas as cameras disponíveis.
            avaiableCams = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if(avaiableCams != null)
            {
                for (int i = 0; i < avaiableCams.Count; i++)
                {
                    comboBoxCams.Items.Add(avaiableCams[i].Name);
                }
                //cam = new VideoCaptureDevice(avaiableCams[0].MonikerString);
                //LoadCam(cam);
                
                //threadCalc.Start();
            }
            else
            {
                Console.WriteLine("Erro, nenhuma camera foi encontrada");
                infoNotCam.Visible = true;
            }
        }

        void LoadCam(VideoCaptureDevice cam, int indexRes)
        {
            try
            {
                //checa se a camera provém de uma lista suportada de resoluções
                if (cam.VideoCapabilities.Length > 0)
                {
                    widthPic = cam.VideoCapabilities[indexRes].FrameSize.Width;
                    heightPic = cam.VideoCapabilities[indexRes].FrameSize.Height;
                    cam.VideoResolution = cam.VideoCapabilities[indexRes];
                    Console.WriteLine("Resolução total: " + widthPic + "x" + heightPic);
                    this.Height = heightPic + 40;
                    this.Width = widthPic + 20;
                    mainPicture = new PictureBox();
                    mainPicture.Height = heightPic;
                    mainPicture.Width = widthPic;
                    mainPicture.BorderStyle = BorderStyle.FixedSingle;
                    mainPicture.Visible = true;
                    this.Controls.Add(mainPicture);
                    rect = new Rectangle(0, 0, widthPic, heightPic);
                    recX = (widthPic - 200) / 2;
                    recY = (heightPic - 200) / 2;
                    auxBitmap = new Bitmap(mainPicture.Width, mainPicture.Height);
                    mainPicture.MouseClick += mainPictureOnClick;
                }
                else
                    Console.WriteLine("aaa");
            }
            catch (Exception)
            {
                Console.WriteLine("Erro inesperado");
                throw;
            }
            //grava frame a frame
            cam.NewFrame += new AForge.Video.NewFrameEventHandler(novoFrame);
            cam.Start();
            Console.WriteLine("Camera iniciada");
        }

        void LoadBase()
        {
            int r, g, b;
            FileStream file;

            //Verifica se o arquivo existe
            if(!File.Exists("./base50.txt"))
            {
                Console.WriteLine("Erro arquivo de moeda não encontrado");
            }


            //testes moedas 5
            file = new FileStream("./base5.txt", FileMode.Open);
            using (StreamReader sr = new StreamReader(file))
            {
                string input;
                while (sr.Peek() > -1)
                {
                    input = sr.ReadLine();
                    r = int.Parse(input);
                    input = sr.ReadLine();
                    g = int.Parse(input);
                    input = sr.ReadLine();
                    b = int.Parse(input);
                    knnList.Add(new Knn(r, g, b, 5));
                    //Console.WriteLine(r + " " + g + " " + b);
                }
            }

            //testes moedas 10
            file = new FileStream("./base10.txt", FileMode.Open);
            using (StreamReader sr = new StreamReader(file))
            {
                string input;
                while (sr.Peek() > -1)
                {
                    input = sr.ReadLine();
                    r = int.Parse(input);
                    input = sr.ReadLine();
                    g = int.Parse(input);
                    input = sr.ReadLine();
                    b = int.Parse(input);
                    knnList.Add(new Knn(r, g, b, 10));
                    //Console.WriteLine(r + " " + g + " " + b);
                }
            }

            ////testes moedas 25
            //file = new FileStream("./base25.txt", FileMode.Open);
            //using (StreamReader sr = new StreamReader(file))
            //{
            //    string input;
            //    while (sr.Peek() > -1)
            //    {
            //        input = sr.ReadLine();
            //        r = int.Parse(input);
            //        input = sr.ReadLine();
            //        g = int.Parse(input);
            //        input = sr.ReadLine();
            //        b = int.Parse(input);
            //        knnList.Add(new Knn(r, g, b, 25));
            //        //Console.WriteLine(r + " " + g + " " + b);
            //    }
            //}

            //testes moedas 50 
            file = new FileStream("./base50.txt", FileMode.Open);
            using (StreamReader sr = new StreamReader(file))
            {
                string input;
                while(sr.Peek() > -1)
                {
                    input = sr.ReadLine();
                    r = int.Parse(input);
                    input = sr.ReadLine();
                    g = int.Parse(input);
                    input = sr.ReadLine();
                    b = int.Parse(input);
                    knnList.Add(new Knn(r, g, b, 50));
                    //Console.WriteLine(r + " " + g + " " + b);
                }
            }

            //testes moedas 100
            file = new FileStream("./base100.txt", FileMode.Open);
            using (StreamReader sr = new StreamReader(file))
            {
                string input;
                while (sr.Peek() > -1)
                {
                    input = sr.ReadLine();
                    r = int.Parse(input);
                    input = sr.ReadLine();
                    g = int.Parse(input);
                    input = sr.ReadLine();
                    b = int.Parse(input);
                    knnList.Add(new Knn(r, g, b, 100));
                    //Console.WriteLine(r + " " + g + " " + b);
                }
            }



        }

        void novoFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            //Obtém o frame como BITMAP como um ".Clone()"
            if(bitmapsQueue.Count <= 5)
            {
                auxBitmap = (Bitmap)eventArgs.Frame.Clone();
                bitmapsQueue.Add((Bitmap)eventArgs.Frame.Clone());
                bitmapsCanUse.Add(true);
                queueCount++;
            }
            canUse = true;
        }

        public void SetPila(Object myObject, EventArgs eventArgs)
        {
            labelPila.Text = textPila;
        }

        static void Calc()
        {
            int i = 1;
            while(i != 0)
            {
                if (bitmapsQueue.Count != 0 && canUse)
                {
                    canUse = false;

                    
                    auxBitmap = Uniformization(auxBitmap);
                    bitmapsQueue[bitmapsQueue.Count - 1] = MakeGrayscale3(auxBitmap);
                    bitmapWithHough = (Bitmap)auxBitmap.Clone();
                    bitmapsQueue[bitmapsQueue.Count - 1] = EdgeDetection(bitmapsQueue[bitmapsQueue.Count - 1]);

                    if (!changed)
                    {
                        if (sleeper == 0)
                        { 
                            MakeHough(bitmapsQueue[bitmapsQueue.Count - 1]);
                        }
                        else
                            sleeper--;
                        mainPicture.Image = bitmapWithHough;                        
                    }
                    else
                    {
                        mainPicture.Image = bitmapsQueue[bitmapsQueue.Count - 1];
                    }
                    
                    
                    //
                    //mainPicture.Image = bitmapsQueue[bitmapsQueue.Count - 1];
                    //ConvertToFormat(bitmapsQueue[bitmapsQueue.Count - 1], System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                    //mainPicture.Image = bitmapsQueue[bitmapsQueue.Count - 1];
                    //mainPicture.Image = EdgeDetection(bitmapsQueue[bitmapsQueue.Count - 1]);


                    bitmapsQueue.RemoveAt(bitmapsQueue.Count - 1);
                    canUse = false;
                }
                //else
                //{
                    //canUse = false;
                //}
            }
        }


        public static void CalcKnn(int r, int g, int b)
        {
            List<Knn> ordenada = null;
            int aux;
            int maisFrequente = 0;
            int indexFrequente = 0;
            List<Knn> lista5Melhores = new List<Knn>();
            //cria um vetor que irá armazenar a quantidade de moedas tem no vetor dos 5 melhores
            //pos 0 = 5    pos 1 = 10 ... pos 4 = 100
            int[] qntdMoedasEncontradas = new int[5];
            
            //faz o calculo da distancia euclidiana da moeda com a base de dados inserindo a distancia na classe
            foreach (Knn k in knnList)
            {
                aux = (int)Math.Sqrt(Math.Pow((r - k.R), 2) + Math.Pow((g - k.G), 2) + Math.Pow((b - k.B), 2));
                k.Distancia = aux;
            }

            //ordena a lista da distancia
            ordenada = knnList.OrderBy(knnList => knnList.Distancia).ToList();

            //obtém os 5 primeiros testes encontrados (os testes mais próximos)
            for (int i = 0; i < 5; i++)
            {
                lista5Melhores.Add(ordenada[i]);
            }
            foreach (Knn k in lista5Melhores)
            {
                if (k.Valor == 5)
                    qntdMoedasEncontradas[0]++;
                if (k.Valor == 10)
                    qntdMoedasEncontradas[1]++;
                if (k.Valor == 25)
                    qntdMoedasEncontradas[2]++;
                if (k.Valor == 50)
                    qntdMoedasEncontradas[3]++;
                if (k.Valor == 100)
                    qntdMoedasEncontradas[4]++;
            }

            //procura a moeda que mais aparece no vetor
            for (int i = 0; i < 5; i++)
            {
                if (qntdMoedasEncontradas[i] > maisFrequente)
                {
                    maisFrequente = qntdMoedasEncontradas[i];
                    indexFrequente = i;
                }
            }

            //pritna a moeda encontrada
            switch (indexFrequente)
            {
                case 0:
                    textPila = "0.05 Pila";
                    Console.WriteLine("Moeda no valor de 5 centavos");
                    break;
                case 1:
                    textPila = "0.10 Pila";
                    if (r > 50 )
                        textPila = "0.25 pila";
                    Console.WriteLine(r);
                    break;
                case 2:
                    textPila = "0.25 Pila";
                    Console.WriteLine("Moeda no valor de 25 centavos");
                    break;
                case 3:
                    textPila = "0.50 Pila";
                    Console.WriteLine("Moeda no valor de 50 centavos");
                    break;
                case 4:
                    textPila = "1 Pila";
                    Console.WriteLine("Moeda no valor de 1 real");
                    break;
                default:
                    break;
            }
            
        }



        public static void CalcColors(Bitmap img)
        {
            if (!calcColors)
                return;
            int corR = 0;
            int corG = 0;
            int corB = 0;
            int count = 0;
            int i = 0;
            int j = 0;
            int auxR;

            if (r == 0)
                return;
            for (i = (maxX + 1 + recX) - r; i < (maxX + 1 + recX) + r; i++)
            {
                for (j = (maxY + 1 + recY) - r; j < (maxY + 1 + recY) + r; j++)
                {
                    if (i < 0 || j < 0 || i > widthPic - 5 || j > heightPic - 5)
                        return;

                    auxR = (int)Math.Round(Math.Sqrt(Math.Pow((maxX + 1 + recX - i), 2) + Math.Pow((maxY + 1 + recY - j), 2)));
                    if (auxR < r)
                    {
                        count++;
                        corR += img.GetPixel(i, j).R;
                        corG += img.GetPixel(i, j).G;
                        corB += img.GetPixel(i, j).B;
                    }
                }
            }
            if (count == 0)
                return;
            corR = corR / count;
            corG = corG / count;
            corB = corB / count;

            count = 0;

            //Console.WriteLine(corR + "," + corG + "," + corB);
            //Console.WriteLine(corR);
            //Console.WriteLine(corG);
            //Console.WriteLine(corB);
            CalcKnn(corR, corG, corB);
            //Console.WriteLine("Valor: " + CalcKnn(corR, corG, corB));

        }

        public static void MakeHough(Bitmap original)
        {
            double aux;
            double aux2;
            int max = 0;
            int minR = 10;
            int maxR = 50;
            int auxR = 0;
            int[,,] mat = new int[201, 201, maxR - minR + 1];
            int maxPixBranc = 200;
            int pixBranc = 0;

            //percorre a imagem procurando pixels brancos
            for (int y = 1 + recY; y < original.Height - (recY - 1); y++)
            {
                y++;
                for (int x = 1 + recX; x < original.Width - (recX - 1); x++)
                {
                    x++;
                    if(original.GetPixel(x,y).R > 180 && pixBranc < maxPixBranc)
                    {
                        pixBranc++;
                        for (int yi = 1 + recY; yi < original.Height - (recY - 1); yi++)
                        {
                            for (int xi = 1 + recX; xi < original.Width - (recX - 1); xi++)
                            {
                                aux = Math.Pow((x - xi), 2);
                                aux2 = Math.Pow((y - yi), 2);
                                r = (int)Math.Round(Math.Sqrt(aux + aux2));
                                if(r >= minR && r<=maxR)
                                {
                                    if ((xi - 1) - (1 + recX) - 1 > 0 && (yi - 1) - (1 + recY) > 0)
                                    {
                                        mat[(xi) - (1 + recX), (yi) - (1 + recY), r - minR]++;
                                    }

                                }
                            }
                        }
                    }
                }
            }



            for (int i = 0; i < 201; i++)
            {
                for (int j = 0; j < 201; j++)
                {
                    for (int k = 0; k < maxR - minR; k++)
                    {
                        if (max < mat[i, j, k])
                        {
                            max = mat[i, j, k];
                            maxX = i;
                            maxY = j;
                            r = k + minR;
                        }
                    }
                }
            }
            if (max < 10)
            {
                r = 0;
                return;
            }
            Graphics g = Graphics.FromImage(bitmapWithHough);
            g.DrawRectangle(recPen, maxX + 1 + recX, maxY + 1 + recY, 2, 2);
            CalcColors(bitmapWithHough);


            for (int i = (maxX + 1 + recX) - r; i < (maxX + 1 + recX) + r; i++)
            {
                for (int j = (maxY + 1 + recY) - r; j < (maxY + 1 + recY) + r; j++)
                {
                    if (i < 0 || j < 0 || i > widthPic - 5 || j > heightPic - 5)
                        return;

                    auxR = (int)Math.Round(Math.Sqrt(Math.Pow((maxX + 1 + recX - i), 2) + Math.Pow((maxY + 1 + recY - j), 2)));
                    if (auxR == r)
                    {
                        g.DrawRectangle(recPen, i, j, 1, 1);
                    }
                }
            }
            //g.DrawRectangle(recPen, (maxX + 1 + recX) - r, (maxY + 1 + recY) - r, 2*r, 2*r);

            g.Dispose();
        }


        //Edge detection
        public static Bitmap EdgeDetection(Bitmap original)
        {
            unsafe
            {
                int sum = 0;
                //create an empty bitmap the same size as original
                Bitmap newBitmap = (Bitmap)original.Clone();
                
                //lock the original bitmap in memory
                BitmapData originalData = original.LockBits(
                   new Rectangle(0, 0, original.Width, original.Height),
                   ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

                //lock the new bitmap in memory
                BitmapData newData = newBitmap.LockBits(
                   new Rectangle(0, 0, original.Width, original.Height),
                   ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);

                //set the number of bytes per pixel
                int pixelSize = 3;
                byte*[,] oRow = new byte*[3, 3];
                for (int y = 1 + recY; y < original.Height - (recY - 1); y++)
                {
                    for (int x = 1 + recX; x < original.Width - (recX - 1); x++)
                    {
                        byte* nRow = (byte*)newData.Scan0 + (y * originalData.Stride);

                        
                        oRow[0, 0] = (byte*)originalData.Scan0 + (((x - 1) * pixelSize) + ((y - 1) * originalData.Stride));
                        oRow[0, 1] = (byte*)originalData.Scan0 + (((x) * pixelSize) + ((y - 1) * originalData.Stride));
                        oRow[0, 2] = (byte*)originalData.Scan0 + (((x + 1) * pixelSize) + ((y - 1) * originalData.Stride));
                        oRow[1, 0] = (byte*)originalData.Scan0 + (((x - 1) * pixelSize) + ((y) * originalData.Stride)); 
                        oRow[1, 1] = (byte*)originalData.Scan0 + (((x) * pixelSize) + ((y) * originalData.Stride));
                        oRow[1, 2] = (byte*)originalData.Scan0 + (((x + 1) * pixelSize) + ((y) * originalData.Stride));
                        oRow[2, 0] = (byte*)originalData.Scan0 + (((x - 1) * pixelSize) + ((y + 1) * originalData.Stride));
                        oRow[2, 1] = (byte*)originalData.Scan0 + (((x) * pixelSize) + ((y + 1) * originalData.Stride));
                        oRow[2, 2] = (byte*)originalData.Scan0 + (((x + 1) * pixelSize) + ((y + 1) * originalData.Stride));

                        //                      \/---- é o bit R do RGB
                        //sum = ((oRow[0, 0][1] * (0)) + (oRow[0, 1][1] * (-1)) + (oRow[0, 2][1] * (-2)) +
                        //    (oRow[1, 0][1] * (1)) + (oRow[1, 1][1] * (0)) + (oRow[1, 2][1] * (-1)) +
                        //    (oRow[2, 0][1] * (2)) + (oRow[2, 1][1] * (1)) + (oRow[2, 2][1] * (0)));

                        //sum += ((oRow[0, 0][1] * (-1)) + (oRow[0, 1][1] * (-1)) + (oRow[0, 2][1] * (0)) +
                        //    (oRow[1, 0][1] * (-1)) + (oRow[1, 1][1] * (0)) + (oRow[1, 2][1] * (1)) +
                        //    (oRow[2, 0][1] * (0)) + (oRow[2, 1][1] * (1)) + (oRow[2, 2][1] * (1)));

                        sum = ((oRow[0, 0][1] * (-1)) + (oRow[0, 1][1] * (-1)) + (oRow[0, 2][1] * (-1)) +
                            (oRow[1, 0][1] * (-1)) + (oRow[1, 1][1] * (8)) + (oRow[1, 2][1] * (-1)) +
                            (oRow[2, 0][1] * (-1)) + (oRow[2, 1][1] * (-1)) + (oRow[2, 2][1] * (-1)));

                        if (sum <= 0)
                        {
                            nRow[x * pixelSize] = (byte)0; //B
                            nRow[x * pixelSize + 1] = (byte)0;  //G
                            nRow[x * pixelSize + 2] = (byte)0;  //R
                        }
                        else if (sum >= 255)
                        {
                            nRow[x * pixelSize] = (byte)255; //B
                            nRow[x * pixelSize + 1] = (byte)255;  //G
                            nRow[x * pixelSize + 2] = (byte)255;  //R
                        }
                        else
                        {
                            sum += 10;
                            if (sum > 150)
                                sum = 255;
                            else if (sum <= 150)
                                sum = 0;
                            nRow[x * pixelSize] = (byte)sum; //B
                            nRow[x * pixelSize + 1] = (byte)sum;  //G
                            nRow[x * pixelSize + 2] = (byte)sum;  //R
                        }
                    }
                }

                //unlock the bitmaps
                newBitmap.UnlockBits(newData);
                original.UnlockBits(originalData);

                Graphics g = Graphics.FromImage(auxBitmap);
                g.DrawRectangle(recPen, recX, recY, 200, 200);
                g.Dispose();
                return newBitmap;
            }
               
        }


        //Faz escala de cinza rapidamente usando Graphics
        public static Bitmap MakeGrayscale3(Bitmap original)
        {
            //create a blank bitmap the same size as original
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);

            //get a graphics object from the new image
            Graphics g = Graphics.FromImage(newBitmap);

            //create the grayscale ColorMatrix
            ColorMatrix colorMatrix = new ColorMatrix(
               new float[][]
               {
                    new float[] {.3f, .3f, .3f, 0, 0},
                    new float[] {.59f, .59f, .59f, 0, 0},
                    new float[] {.11f, .11f, .11f, 0, 0},
                    new float[] {0, 0, 0, 1, 0},
                    new float[] {0, 0, 0, 0, 1}
               });

            //create some image attributes
            ImageAttributes attributes = new ImageAttributes();

            //set the color matrix attribute
            attributes.SetColorMatrix(colorMatrix);

            //draw the original image on the new image
            //using the grayscale color matrix
            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
               0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

            //dispose the Graphics object
            g.Dispose();
            return newBitmap;
        }


        public static Bitmap Uniformization(Bitmap bitmap)
        {
            
            double sxr = 0;
            double sx2r = 0;
            double sxg = 0;
            double sx2g = 0;
            double sxb = 0;
            double sx2b = 0;
            int count = 0;

            for (int i = recX; i < recX + 200; i++)
            {
                for (int j = recY; j < recY + 200; j++)
                {
                    sxr += bitmap.GetPixel(i, j).R;
                    sx2r += Math.Pow(bitmap.GetPixel(i, j).R, 2);
                    sxg += bitmap.GetPixel(i, j).G;
                    sx2g += Math.Pow(bitmap.GetPixel(i, j).G, 2);
                    sxb += bitmap.GetPixel(i, j).B;
                    sx2b += Math.Pow(bitmap.GetPixel(i, j).B, 2);
                    count++;
                }
            }
            double mediar2 = sxr / count;
            double varianciar2 = (sx2r - (Math.Pow(sxr, 2)) / count) / count;
            double mediag2 = sxg / count;
            double varianciag2 = (sx2g - (Math.Pow(sxg, 2)) / count) / count;
            double mediab2 = sxb / count;
            double varianciab2 = (sx2b - (Math.Pow(sxb, 2)) / count) / count;
            if (makingUniformization)
            {
                Console.WriteLine("MediaR:" + mediar2 + " VarianciaR: " + varianciar2);
                Console.WriteLine("MediaG:" + mediag2 + " VarianciaG: " + varianciag2);
                Console.WriteLine("MediaB:" + mediab2 + " VarianciaB: " + varianciab2);
                mediaR = mediar2;
                mediaG = mediag2;
                mediaB = mediab2;
                varianciaR = varianciar2;
                varianciaG = varianciag2;
                varianciaB = varianciab2;
                makingUniformization = false;
            }
            double ganhoR = Math.Sqrt(mediaR / mediar2);
            double ganhoG = Math.Sqrt(mediaG / mediag2);
            double ganhoB = Math.Sqrt(mediaB / mediab2);
            double offsetR = mediaR - ganhoR * mediar2;
            double offsetG = mediaG - ganhoG * mediag2;
            double offsetB = mediaB - ganhoB * mediab2;

            int corR;
            int corG;
            int corB;
            for (int i = recX; i < recX + 200; i++)
            {
                for (int j = recY; j < recY + 200; j++)
                {

                    corR = (int)Math.Round(ganhoR * bitmap.GetPixel(i, j).R + offsetR);
                    corG = (int)Math.Round(ganhoG * bitmap.GetPixel(i, j).G + offsetG);
                    corB = (int)Math.Round(ganhoB * bitmap.GetPixel(i, j).B + offsetB);
                    if (corR > 255)
                        corR = 255;
                    if (corG > 255)
                        corG = 255;
                    if (corB > 255)
                        corB = 255;

                    if (corR < 0)
                        corR = 0;
                    if (corG < 0)
                        corG = 0;
                    if (corB < 0)
                        corB = 0;
                    bitmap.SetPixel(i, j, Color.FromArgb(corR,corG,corB));
                }
            }
            return bitmap;
        }


        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Stop and free the webcam object if application is closing
            if (cam != null && cam.IsRunning)
            {
                cam.SignalToStop();
                cam = null;
            }
            this.Close();
            System.Threading.Thread.CurrentThread.Abort();
            this.Close();
        }

        private void mainPictureOnClick(object sender, MouseEventArgs e)
        {
            //Console.WriteLine(((Bitmap)mainPicture.Image).GetPixel(e.X, e.Y));
        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_MouseClick(object sender, MouseEventArgs e)
        {
            //CalcColors((Bitmap)mainPicture.Image);
            //if (calcColors)
            //    calcColors = false;
            //else
            //    calcColors = true;
            //Uniformization(auxBitmap);
            makingUniformization = true;
        }

        private void checkChange_CheckedChanged(object sender, EventArgs e)
        {
            changed = checkChange.Checked;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            //brilho = (float)(trackBar1.Value)/100;
            //Console.WriteLine(trackBar1.Value);
        }

        private void comboBoxCams_SelectedIndexChanged(object sender, EventArgs e)
        {
            cam = new VideoCaptureDevice(avaiableCams[comboBoxCams.SelectedIndex].MonikerString);
            comboBoxRes.Items.Clear();
            for (int i = 0; i < cam.VideoCapabilities.Length; i++)
            {
                comboBoxRes.Items.Add(cam.VideoCapabilities[i].FrameSize.Height + "x" + cam.VideoCapabilities[i].FrameSize.Width);
            }

        }

        private void comboBoxRes_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadCam(cam,comboBoxRes.SelectedIndex);
            threadCalc = new Thread(Calc);
            cam.Start();
            threadCalc.Start();
            infoCam.Visible = false;
            comboBoxCams.Visible = false;
            comboBoxRes.Visible = false;
            infoRes.Visible = false;
        }

        private void Form1_FormClosed_1(object sender, FormClosedEventArgs e)
        {
            threadCalc.Abort();
            if(cam != null)
                cam.Stop();
            Application.Exit();
        }
    }
}
