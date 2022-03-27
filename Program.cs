using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project03
{
    class Program
    {
        static void Main(string[] args)
        {
            Random r = new Random();
            Müşteri mstr;
            List<Müşteri> müşteriler;
            Tree agac = new Tree();
            String[] duraklar = { "İnciraltı, 28, 2, 10", "Sahil Evleri, 8, 1, 11", "Doğal Yaşam Parkı, 17, 1, 6",
                                  "Bostanlı İskele, 7, 0, 5", "Göztepe , 15, 0, 15", "Konak İskele, 8, 0, 30",
                                  "Alsancak İskele, 7, 0, 13", "Buz Pisti, 9, 0, 11", "Bornova Metro, 8, 0, 12"};
            for (int i = 0; i < duraklar.Length; i++)
            {
                String[] durakList = duraklar[i].Split(',');
                Durak durak = new Durak(durakList[0], Convert.ToInt32(durakList[1]), Convert.ToInt32(durakList[2]), Convert.ToInt32(durakList[3]));

                müşteriler = new List<Müşteri>(); // Her bir durak için yeni müşteri listesi oluşturur
                int müşteriSay = r.Next(1,11);
                for (int j = 0; j < müşteriSay; j++)// 1-10 arası rastgele müşteri oluşturup listeye ekler
                {
                    mstr = new Müşteri(r.Next(21), (r.Next(24)), (r.Next(60)));
                    müşteriler.Add(mstr);
                }
                agac.insert(durak, müşteriler); // Durak nesnesini ve oluşan müşteri listesini ağaca ekler
            }
            int derinlik = 0;
            int leftSyc = 0;
            int rghtSyc = 0;
            int leftDepth(TreeNode local)// Sol taraftaki derinliği  hesaplar
            {
                if (local != null)
                {
                    leftSyc++;
                    leftDepth(local.leftChild);
                }
                return leftSyc;
            }
            int rightDepth(TreeNode local)// Sağ taraftaki derinliği hesaplar
            {
                if (local != null)
                {
                    rghtSyc++;
                    rightDepth(local.rightChild);
                }
                return rghtSyc;
            }
            leftSyc = leftDepth(agac.getRoot());
            rghtSyc = rightDepth(agac.getRoot());

            void derinlikYaz() // Sağ ve sol derinlikleri karşılaştırıp büyük olanı derinlik değişkene atar ve ekrana yazdırır
            {
                if (leftSyc>rghtSyc) { derinlik = leftSyc; }
                else { derinlik = rghtSyc; }
                Console.WriteLine("Ağaç derinliği: "+derinlik);
            }
            derinlikYaz();

            void agacListele(TreeNode localRoot) // Ağacı InOrder şekilde dolaşır ve her düğümün bilgisini listeler
            {
                if (localRoot != null)
                {
                    agacListele(localRoot.leftChild);
                    localRoot.displayNode();
                    agacListele(localRoot.rightChild);
                }
            }
            agacListele(agac.getRoot());

            Console.Write("Müşteri ID giriniz: ");
            int girilenID = Convert.ToInt32(Console.ReadLine());

            void musteriBilgi(TreeNode localRoot) // ID'si girilen müşterinin yaptığı kiralama işlemleri listelenir
            {
                if (localRoot != null)
                {
                    musteriBilgi(localRoot.leftChild); // Agaç InOrder şekilde dolaşılarak her düğümdeki müşteri listesi taranır
                    for (int i = 0; i < localRoot.müşteriList.Count; i++) // Eşleşme durumunda müşteri bilgileri yazdırılır
                    {
                        if (localRoot.müşteriList[i].id == girilenID)
                        {
                            Console.WriteLine("Bisiklet kiralanan istasyon: "+localRoot.drk.durakAdı+
                                              "/  Kiralama saati: "+localRoot.müşteriList[i].zaman);
                        }
                    }
                    musteriBilgi(localRoot.rightChild);
                }
            }
            if (girilenID < 0 || girilenID > 20) { Console.WriteLine("Müşteri kayıtlı değil"); }
            else 
            { 
                musteriBilgi(agac.getRoot()); 
            }

            Console.Write("Durak adı giriniz: ");
            string girDurak = Console.ReadLine();
            Console.Write("Müşteri ID giriniz: ");
            int girID = Convert.ToInt32(Console.ReadLine());
            void kiralamaYap(TreeNode localRoot) // ID'si girilen müşteri için girilen durakta kiralama işlemi yapılır
            {
                if (localRoot != null)
                {
                    kiralamaYap(localRoot.leftChild); // Agaç InOrder dolaşılarak uyuşan durak bulunur ve kiralama işlemi gerçekleştirilir.
                    if (string.Equals(localRoot.drk.durakAdı, girDurak))
                    {
                        Console.WriteLine("Önceki durak bilgisi...");
                        Console.WriteLine("Durak adı: " + localRoot.drk.durakAdı + " | Boş Park: " + localRoot.drk.bosPark +
                                          " | Tandem B: " + localRoot.drk.tandem + " | Normal B:" + localRoot.drk.normal);
                        Console.WriteLine("Müşteri sayısı: " + localRoot.müşteriList.Count);
                        localRoot.müşteriList.Add(new Müşteri(girID, (r.Next(24)), (r.Next(60))));
                        localRoot.drk.normal--;
                        localRoot.drk.bosPark++;
                        Console.WriteLine("Yeni durak bilgisi...");
                        Console.WriteLine("Durak adı: " + localRoot.drk.durakAdı + " | Boş Park: " + localRoot.drk.bosPark +
                                          " | Tandem B: " + localRoot.drk.tandem + " | Normal B:" + localRoot.drk.normal);
                        Console.WriteLine("Müşteri sayısı: " + localRoot.müşteriList.Count);

                    }
                    kiralamaYap(localRoot.rightChild);
                }
            }
            kiralamaYap(agac.getRoot());

            //------------------2. Soru------------------

            Hashtable hashtable = new Hashtable(); // Yeni bir hashtable nesnesi oluşturulur
            for (int i = 0; i < duraklar.Length; i++)
            {
                string[] durakList = duraklar[i].Split(',');
                int[] durakİçerik = { Convert.ToInt32(durakList[1]), Convert.ToInt32(durakList[2]), Convert.ToInt32(durakList[3])};
                hashtable.Add(durakList[0], durakİçerik); // Her bir durak; adları key, kalan veriler dizi halinde value
                                                         //  olacak şekilde hash tablosuna eklenir
            }
            foreach (int[] item in hashtable.Values)
            {
                if (item[0] > 5) // Hash tablosu dolaşılarak boş park yeri sayısı 5'ten fazla olanlar bulunur
                {
                    item[2] += 5; // Normal bisiklet sayısı 5 artırılır
                    item[0] -= 5; // Boş park yeri sayısı 5 azaltılır.
                }
            }

            //********************** 3. Soru **********************

            Heap maxheap = new Heap(duraklar.Length); // Aşağıda oluşturulan Heap sınıfından yeni bir nesne türetilir
            for (int i = 0; i < duraklar.Length; i++)
            {
                String[] durakList = duraklar[i].Split(',');
                Durak durak = new Durak(durakList[0], Convert.ToInt32(durakList[1]), Convert.ToInt32(durakList[2]), Convert.ToInt32(durakList[3]));
                maxheap.insert(durak); // Duraklar heap'e eklenir
            }
            Console.WriteLine("En fazla normal bisiklet olan ilk 3 istasyon:");
            Durak drk1 = maxheap.remove(); // 3 kez root silme işlemi yapılarak en fazla normal bisiklete sahip duraklar bulunur
            Durak drk2 = maxheap.remove();
            Durak drk3 = maxheap.remove();
            Console.WriteLine("Durak adı: " + drk1.durakAdı + " | Boş Park: " + drk1.bosPark + " | Tandem B: " + drk1.tandem + " | Normal B:" + drk1.normal);
            Console.WriteLine("Durak adı: " + drk2.durakAdı + " | Boş Park: " + drk2.bosPark + " | Tandem B: " + drk2.tandem + " | Normal B:" + drk2.normal);
            Console.WriteLine("Durak adı: " + drk3.durakAdı + " | Boş Park: " + drk3.bosPark + " | Tandem B: " + drk3.tandem + " | Normal B:" + drk3.normal);

            //********************** 4. Soru (a/bubble sort) **********************

            int[] array = { 11, 52, 7, 18, 69, 42, 24, 85, 30, 57, 91, 72};
            Console.Write("Array sırasız: ");
            foreach (int k in array) Console.Write("|"+ k + "|");
            int temp;
            for (int i=0; i < array.Length-1 ; i++)
            {
                for (int j=0; j < array.Length-1 ; j++)
                {
                    if (array[j] > array[j + 1]) // Her eleman kendisinden sonraki ile karşılaştırılarak gerekli ise yerleri değiştirilir
                    {
                        temp = array[j+1];
                        array[j + 1] = array[j];
                        array[j] = temp;
                    }
                }
            }
            Console.Write("\nArray sıralı(bubble sort): ");
            foreach (int k in array) Console.Write("|"+ k + "|");

            //********************** 4. Soru (b/quicksort) **********************

            void quickSort(int[] arr, int sol, int sag)
            {
                if (sol < sag)
                {
                    int pivot = parcalama(arr, sol, sag); // parcalamadan dönen sag indexi pivota atanır
                    if (pivot > 1)
                    {
                        quickSort(arr , sol, pivot-1);  // quicksort tekrarlanır
                    }
                    if (pivot + 1 < sag)
                    {
                        quickSort(arr, pivot + 1, sag); // quicksort tekrarlanır
                    }
                }
            }
            int parcalama(int[]arr, int sol, int sag)
            {
                int pivot = arr[sol]; // pivot degeri en soldan belirlenir
                while (true)
                {
                    while(arr[sol] < pivot)
                    {
                        sol++;
                    }
                    while (arr[sag] > pivot)
                    {
                        sag--;
                    }
                    if (sol < sag)// sol indexi sag indexinden küçükse indexlerdeki değerler yer değiştirir
                    {
                        if (arr[sol] == arr[sag]) return sag;
                        int temp1 = arr[sol];
                        arr[sol] = arr[sag];
                        arr[sag] = temp1;
                    }
                    else { return sag; }
                }
            }
            int[] array1 = { 11, 52, 7, 18, 69, 42, 24, 85, 30, 57, 91, 72 };
            quickSort(array1, 0, array1.Length - 1);
            Console.Write("\nArray sıralı(quicksort): ");
            foreach (int k in array) Console.Write("|" + k + "|");

            Console.ReadKey();
        }
    }
    class Durak
    {
        public string durakAdı;
        public int bosPark, tandem, normal;
        public Durak(string durak_adı, int bos_park, int tandem_bis, int normal_bis)
        {
            this.durakAdı = durak_adı;
            this.bosPark = bos_park;
            this.tandem = tandem_bis;
            this.normal = normal_bis;
        }
    }
    class Müşteri
    {
        public int id, saat, dk;
        public string zaman;
        public Müşteri(int id , int saat, int dk)
        {
            this.id = id;
            this.saat = saat;
            this.dk = dk;
            this.zaman = saat.ToString("00") +":"+ dk.ToString("00");
        }
    }
    class TreeNode
    {
        public Durak drk;
        public List<Müşteri> müşteriList;
        public TreeNode leftChild;
        public TreeNode rightChild;
        public void displayNode()
        {
            Console.WriteLine("Durak adı: "+drk.durakAdı+" | Boş Park: "+drk.bosPark+ " | Tandem B: "+drk.tandem+" | Normal B:"+drk.normal);
            Console.WriteLine("Müşteri sayısı: "+ müşteriList.Count);
            for (int i = 0; i < müşteriList.Count; i++)
            {
                Console.WriteLine("Müşteri ID: " + müşteriList[i].id.ToString("00") + " / Kiralama zamanı: " + müşteriList[i].zaman);
            }
            Console.WriteLine("*******************************");
        }
    }
    class Tree
    {
        private TreeNode root;
        public Tree() { root = null; }
        public TreeNode getRoot(){ return root;}
        public void insert (Durak newDrk, List<Müşteri> müşterilerList)
        {
            TreeNode newNode = new TreeNode();
            newNode.drk = newDrk;
            newNode.müşteriList = müşterilerList;
            newNode.drk.bosPark += müşterilerList.Count;
            newNode.drk.normal -= müşterilerList.Count;
            if (root == null)
            {
                root = newNode;
            }
            else
            {
                TreeNode current = root;
                TreeNode parent;
                while (true)
                {
                    parent = current;
                    int x = String.Compare(newNode.drk.durakAdı, current.drk.durakAdı);
                    if (x == -1)
                    {
                        current = current.leftChild;
                        if(current == null)
                        {
                            parent.leftChild = newNode;
                            return;
                        }
                    }
                    else
                    {
                        current = current.rightChild;
                        if(current == null)
                        {
                            parent.rightChild = newNode;
                            return;
                        }
                    }
                }
            }
        }
    }
    class Heap
    {
        private Durak[] heapArray;
        private int maxSize;
        private int currentSize;
        public Heap(int max)
        {
            maxSize = max;
            currentSize = 0;
            heapArray = new Durak[maxSize]; // Array oluşturuldu
        }
        public Boolean isEmpty()
        {
            return currentSize == 0;
        }
        public Boolean insert(Durak newDurak)
        {
            if(currentSize == maxSize) { return false; }
            heapArray[currentSize] = newDurak;
            trickleUp(currentSize++);
            return true;
        }
        public void trickleUp(int index)
        {
            int parent = (index - 1) / 2;
            Durak bottom = heapArray[index];
            while (index > 0 && heapArray[parent].normal < bottom.normal)
            {
                heapArray[index] = heapArray[parent];
                index = parent;
                parent = (parent - 1) / 2;
            }
            heapArray[index] = bottom;
        }
        public Durak remove()
        {
            Durak root = heapArray[0];
            heapArray[0] = heapArray[--currentSize];
            trickleDown(0);
            return root;
        }
        public void trickleDown(int index)
        {
            int largerChild;
            Durak top = heapArray[index];
            while(index < currentSize / 2)
            {
                int leftChild = 2 * index + 1;
                int rightChild = leftChild + 1;
                if (rightChild < currentSize && heapArray[leftChild].normal < heapArray[rightChild].normal)
                    largerChild = rightChild;
                else
                    largerChild = leftChild;

                if (top.normal >= heapArray[largerChild].normal) 
                    break;
                heapArray[index] = heapArray[largerChild];
                index = largerChild;
            }
            heapArray[index] = top;
        }
    }
}
