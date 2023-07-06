class GPSEdit
{
    static void Main(string[] args)
    {
        StreamReader reader = null;

        try
        {
            reader = new StreamReader("GPSPoints.txt");
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception.Message);
            Console.WriteLine("Nie znaleziono pliku.");

            StreamWriter streamWriter = new("GPSPoints.txt");
            streamWriter.Close();

            Console.WriteLine("Utorzono pusty plik. Spróbuj ponownie.");
            return;
        }

        List<Point> pointList = new();

        DataRead();

        if (args.Length > 0)
        {
            string comand = args[0].ToLower();

            switch (comand)
            {
                case "add":
                    Add();
                    break;

                case "remove":
                    Remove();
                    break;

                case "clear":
                    Clear();
                    break;

                case "insert":
                    Insert();
                    break;

                case "replace":
                    Replace();
                    break;

                case "print":
                    Print();
                    break;

                default:
                    Console.WriteLine("Nie rozpoznano argumentu: " + args[0]);
                    break;
            }
        }
        else
        {
            Console.WriteLine("Podaj jedeną z komend:");
            Console.WriteLine("- add [dd] jeżeli wpisywane dane są w formacie dziesiętnym (google maps)");
            Console.WriteLine("- remove");
            Console.WriteLine("- clear");
            Console.WriteLine("- insert");
            Console.WriteLine("- replace");
            Console.WriteLine("- print");
        }

        void Add()
        {
            int j = 0;

            if (args[1] == "dd")
            {
                j = 1;
            }
            int pointQuanity = (args.Length - 1 - j) / 2;

            if (pointQuanity > 0)
            {

                for (int i = 0; i < pointQuanity; i++)
                {
                    Point point = new();

                    try
                    {
                        point.latitude = Double.Parse(args[(i * 2) + 1 + j], System.Globalization.CultureInfo.InvariantCulture);
                        point.longitude = Double.Parse(args[(i * 2) + 2 + j], System.Globalization.CultureInfo.InvariantCulture);

                        //Console.WriteLine(point);
                        if (j == 1)
                        {
                            Point unconvertedPoint = new Point();
                            unconvertedPoint.latitude = point.latitude;
                            unconvertedPoint.longitude = point.longitude;

                            int latDeg = (int)Math.Floor(point.latitude);
                            int longDeg = (int)Math.Floor(point.longitude);

                            //Console.WriteLine("{0}\t{1}", latDeg, longDeg);
                            double latDec = (point.latitude - latDeg) * 60;
                            double longDec = (point.longitude - longDeg) * 60;

                            //Console.WriteLine("{0}\t{1}", latDec, longDec);
                            point.latitude = (latDeg * 100) + latDec;
                            point.longitude = (longDeg * 100) + longDec;

                            point.latitude = Math.Round(point.latitude, 4);
                            point.longitude = Math.Round(point.longitude, 4);

                            Console.WriteLine("Przekonwertowano punkt {0} na {1}", unconvertedPoint, point);
                        }

                        pointList.Add(point);

                        Console.WriteLine("Dodano punkt: " + point);

                    }
                    catch (Exception exception)
                    {
                        Console.WriteLine(exception.Message);
                        Console.WriteLine("Niewłaściwy format danych.");
                        Console.WriteLine("Przykładowy format punktu = ./GPSEdit add 5423.2844 1456.8425");
                    }
                }

                DataWrite();
            }
            else
            {
                Console.WriteLine("Należy podać przynajmniej jeden pełny punkt");
                Console.WriteLine("Przykładowy format punktu = ./GPSEdit add 5423.2844 1456.8425");
            }
        }

        void Remove()
        {
            if (args.Length > 1)
            {
                try
                {
                    int pointIndex = int.Parse(args[1]);

                    pointList.RemoveAt(pointIndex - 1);

                    Console.WriteLine($"Usunięto punkt na pozycji {pointIndex}.");

                    DataWrite();
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                    Console.WriteLine("Jako indeks punktu należy podać liczbę całkowitą.");
                    Console.WriteLine("Przykład użycia: ./GPSEdit remove 3");
                }
            }
            else
            {
                Console.WriteLine("Po komendzie należy podać indeks punktu do usunięcia.");
                Console.WriteLine("Przykład użycia: ./GPSEdit remove 3");
            }
        }

        void Clear()
        {
            pointList.Clear();

            Console.WriteLine("Usunięto wszystkie punkty z listy.");

            DataWrite();
        }

        void Insert()
        {
            if (args.Length > 3)
            {
                Point point = new();

                try
                {
                    int pointIndex = int.Parse(args[1]);

                    point.latitude = Double.Parse(args[2], System.Globalization.CultureInfo.InvariantCulture);
                    point.longitude = Double.Parse(args[3], System.Globalization.CultureInfo.InvariantCulture);

                    if (point.latitude < 100) point.latitude *= 100;
                    if (point.longitude < 100) point.longitude *= 100;

                    pointList.Insert(pointIndex - 1, point);

                    Console.WriteLine($"Dodano punkt {point} na pozycji {pointIndex}.");

                    DataWrite();
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                    Console.WriteLine("Niewłaściwy format danych punktu lub indeksu.");
                    Console.WriteLine("Przykład użycia: ./GPSEdit insert 2 54.232844 14.568425");
                }
            }
            else
            {
                Console.WriteLine("Po komendzie należy podać indeks punktu, a następnie współrzędne.");
                Console.WriteLine("Przykład użycia: ./GPSEdit insert 2 54.232844 14.568425");
            }
        }

        void Replace()
        {
            if (args.Length > 3)
            {
                Point point = new();

                try
                {
                    int pointIndex = int.Parse(args[1]);

                    point.latitude = Double.Parse(args[2], System.Globalization.CultureInfo.InvariantCulture);
                    point.longitude = Double.Parse(args[3], System.Globalization.CultureInfo.InvariantCulture);

                    if (point.latitude < 100) point.latitude *= 100;
                    if (point.longitude < 100) point.longitude *= 100;

                    pointList.RemoveAt(pointIndex - 1);
                    pointList.Insert(pointIndex - 1, point);

                    Console.WriteLine($"Zastąpiono punkt na pozycji {pointIndex} punktem {point}.");

                    DataWrite();
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                    Console.WriteLine("Niewłaściwy format danych punktu lub indeksu.");
                    Console.WriteLine("Przykład użycia: ./GPSEdit replace 2 54.232844 14.568425");
                }
            }
            else
            {
                Console.WriteLine("Po komendzie należy podać indeks punktu, a następnie współrzędne.");
                Console.WriteLine("Przykład użycia: ./GPSEdit replace 2 54.232844 14.568425");
            }
        }

        void Print()
        {
            Console.WriteLine("Aktualna lista punktów:");
            Console.WriteLine("Lp.\tSzerokość\tDługość");

            for (int i = 0; i < pointList.Count; i++)
            {
                Console.WriteLine($"{i + 1}\t{pointList[i]}");
            }

            Console.WriteLine($"Lista zawiera {pointList.Count} elementów.");
        }

        void DataRead()
        {
            string data;

            while (true)
            {
                data = reader.ReadLine();

                if (data != null)
                {
                    string[] splittedData = new string[2];

                    splittedData = data.Split("\t");

                    Point point = new Point();
                    point.latitude = Double.Parse(splittedData[0], System.Globalization.CultureInfo.InvariantCulture);
                    point.longitude = Double.Parse(splittedData[1], System.Globalization.CultureInfo.InvariantCulture);

                    pointList.Add(point);
                }
                else break;
            }

            reader.Close();
        }

        void DataWrite()
        {
            pointList.TrimExcess();

            StreamWriter writer = new("GPSPoints.txt");

            foreach (Point point in pointList)
            {
                writer.WriteLine(point);
            }

            writer.Close();
        }
    }

    class Point
    {
        public double latitude;
        public double longitude;
        public override string ToString()
        {
            return latitude + "\t" + longitude;
        }
    }
}