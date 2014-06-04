#region Using Statements
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using zSprite;
using zSprite.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
#endregion

namespace GameName1
{
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {

        public class testme //: IQuadObject
        {
            public AxisAlignedBox Bounds { get; set; }
            public void Call()
            {
                Console.WriteLine("hello");
            }
        }

        public static void speedtest(int count)
        {
            speedtest1(1);
            speedtest2(1);

            var test2 = speedtest2(count);
            var test1 = speedtest1(count);
            var test3 = speedtest3(count);

            Console.WriteLine("IEnumerable: {0}", test1);
            Console.WriteLine("For Loop[]: {0}", test2);
            Console.WriteLine("For Loop[,]: {0}", test3);
        }

        public static TimeSpan speedtest1(int count)
        {
            var size = 1024;
            var data = new int[size * size];
            data[1] = 1;
            data[3] = 5;

            var sw = new Stopwatch();
            sw.Reset();
            sw.Start();

            for (var i = 0; i < count; i++)
            {
                foreach (var xy in data.xy(size))
                    data[xy.index] = (int)Math.Sqrt(Math.PI + data[xy.index]);
            }

            sw.Stop();
            return sw.Elapsed;
        }

        public static TimeSpan speedtest2(int count)
        {
            var size = 1024;
            var data = new int[size * size];
            data[1] = 1;
            data[3] = 5;

            var sw = new Stopwatch();
            sw.Reset();
            sw.Start();

            for (var i = 0; i < count; i++)
            {
                for (var x = 0; x < size; x++)
                {
                    for (var y = 0; y < size; y++)
                    {
                        var index = y * size + x;
                        data[index] = (int)Math.Sqrt(Math.PI + data[index]);
                    }
                }
            }

            sw.Stop();
            return sw.Elapsed;
        }

        public static TimeSpan speedtest3(int count)
        {
            var size = 1024;
            var data = new int[size, size];
            data[1, 0] = 1;
            data[3, 0] = 5;

            var sw = new Stopwatch();
            sw.Reset();
            sw.Start();

            for (var i = 0; i < count; i++)
            {
                for (var x = 0; x < size; x++)
                {
                    for (var y = 0; y < size; y++)
                    {
                        data[x, y] = (int)Math.Sqrt(Math.PI + data[x, y]);
                    }
                }
            }

            sw.Stop();
            return sw.Elapsed;
        }

        private static char[] basemap = new char[] { 
            ' ', 'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',
            //'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z',
            //'0','1','2','3','4','5','6','7','8','9'
        };

        /// <summary>
        /// Converts a ulong to a string base representation
        /// </summary>
        /// <param name="id">number to convert to string</param>
        /// <param name="map">String character mapping</param>
        /// <returns>String representation of the original number</returns>
        public static string BaseConversion(ulong id, char[] map)
        {
            if (id == 0)
                return new string(map[0], 1);

            var result = new char[64];
            var rindex = result.Length;
            var length = (uint)map.Length;
            while (id > 0)
            {
                var index = id % (uint)length;
                result[--rindex] = map[index];
                id /= (uint)length;
            }
            return new string(result, rindex, result.Length - rindex);
        }

        /// <summary>
        /// Converts a string to a number representation
        /// </summary>
        /// <param name="key">String to use to convert to a number</param>
        /// <param name="map">String character mapping</param>
        /// <returns></returns>
        public static ulong BaseConversion(string key, char[] map)
        {
            ulong result = 0;
            var length = (uint)map.Length;
            for (int i = 0; i < key.Length; i++)
            {
                var index = -1;
                for (int k = 0; k < map.Length; k++)
                {
                    if (map[k] == key[i])
                    {
                        index = k;
                        break;
                    }
                }

                if (index == -1)
                    throw new ArgumentOutOfRangeException("key");

                result = result * length + (uint)index;
            }

            return result;
        }



        public static void RadixSort(long[] a)
        {
            // our helper array 
            long[] t = new long[a.Length];

            // number of bits our group will be long 
            int r = 4; // try to set this also to 2, 8 or 16 to see if it is quicker or not 

            // number of bits of a C# int 
            int b = 64;

            // counting and prefix arrays
            // (note dimensions 2^r which is the number of all possible values of a r-bit number) 
            long[] count = new long[1 << r];
            long[] pref = new long[1 << r];

            // number of groups 
            long groups = (long)Math.Ceiling((double)b / (double)r);

            // the mask to identify groups 
            long mask = (1 << r) - 1;

            // the algorithm: 
            for (int c = 0, shift = 0; c < groups; c++, shift += r)
            {
                // reset count array 
                for (int j = 0; j < count.Length; j++)
                    count[j] = 0;

                // counting elements of the c-th group 
                for (int i = 0; i < a.Length; i++)
                    count[(a[i] >> shift) & mask]++;

                // calculating prefixes 
                pref[0] = 0;
                for (int i = 1; i < count.Length; i++)
                    pref[i] = pref[i - 1] + count[i - 1];

                // from a[] to t[] elements ordered by c-th group 
                for (int i = 0; i < a.Length; i++)
                    t[pref[(a[i] >> shift) & mask]++] = a[i];

                // a[]=t[] and start again until the last group 
                t.CopyTo(a, 0);
            }
            // a is sorted 
        }

        public static void RadixSort(int[] a)
        {
            // our helper array 
            int[] t = new int[a.Length];

            // number of bits our group will be long 
            int r = 4; // try to set this also to 2, 8 or 16 to see if it is quicker or not 

            // number of bits of a C# int 
            int b = 32;

            // counting and prefix arrays
            // (note dimensions 2^r which is the number of all possible values of a r-bit number) 
            int[] count = new int[1 << r];
            int[] pref = new int[1 << r];

            // number of groups 
            int groups = (int)Math.Ceiling((double)b / (double)r);

            // the mask to identify groups 
            int mask = (1 << r) - 1;

            // the algorithm: 
            for (int c = 0, shift = 0; c < groups; c++, shift += r)
            {
                // reset count array 
                for (int j = 0; j < count.Length; j++)
                    count[j] = 0;

                // counting elements of the c-th group 
                for (int i = 0; i < a.Length; i++)
                    count[(a[i] >> shift) & mask]++;

                // calculating prefixes 
                pref[0] = 0;
                for (int i = 1; i < count.Length; i++)
                    pref[i] = pref[i - 1] + count[i - 1];

                // from a[] to t[] elements ordered by c-th group 
                for (int i = 0; i < a.Length; i++)
                    t[pref[(a[i] >> shift) & mask]++] = a[i];

                // a[]=t[] and start again until the last group 
                t.CopyTo(a, 0);
            }
            // a is sorted 
        }


        public interface IKey
        {
            int Key { get; }
        }

      

        public static void RadixSort(Testing[] a)
        {
            // our helper array 
            Testing[] t = new Testing[a.Length];

            // number of bits our group will be long 
            int r = 4; // try to set this also to 2, 8 or 16 to see if it is quicker or not 

            // number of bits of a C# int 
            int b = 32;

            // counting and prefix arrays
            // (note dimensions 2^r which is the number of all possible values of a r-bit number) 
            int[] count = new int[1 << r];
            int[] pref = new int[1 << r];

            // number of groups 
            int groups = (int)Math.Ceiling((double)b / (double)r);

            // the mask to identify groups 
            int mask = (1 << r) - 1;

            // the algorithm: 
            for (int c = 0, shift = 0; c < groups; c++, shift += r)
            {
                // reset count array 
                for (int j = 0; j < count.Length; j++)
                    count[j] = 0;

                // counting elements of the c-th group 
                for (int i = 0; i < a.Length; i++)
                    count[(a[i].Key >> shift) & mask]++;

                // calculating prefixes 
                pref[0] = 0;
                for (int i = 1; i < count.Length; i++)
                    pref[i] = pref[i - 1] + count[i - 1];

                // from a[] to t[] elements ordered by c-th group 
                for (int i = 0; i < a.Length; i++)
                    t[pref[(a[i].Key >> shift) & mask]++] = a[i];

                // a[]=t[] and start again until the last group 
                t.CopyTo(a, 0);
            }
            // a is sorted 
        }


        public static int test1(int len)
        {
            var data = new long[len];
            var rand = new Random();

            for (var i = 0; i < len; i++)
                data[i] = (long)(rand.NextDouble() * long.MaxValue);

            var sw = new Stopwatch();
            sw.Start();
            RadixSort(data);
            sw.Stop();

            for (var i = 0; i < len - 1; i++)
                if (data[i] > data[i + 1])
                    throw new Exception("fuck");

            return (int)sw.ElapsedMilliseconds;
        }

        public static int test2(int len)
        {
            var data = new int[len];
            var rand = new Random();

            var allowedData = new int[100];
            for (var i = 0; i < allowedData.Length; i++)
                allowedData[i] = rand.Next(0, int.MaxValue);

            for (var i = 0; i < len; i++)
                data[i] = allowedData[rand.Next(0, allowedData.Length)];

            //for (var i = 0; i < len; i++)
            //    data[i] = rand.Next(0, int.MaxValue);

            var sw = new Stopwatch();
            sw.Start();
            RadixSort(data);
            sw.Stop();

            for (var i = 0; i < len - 1; i++)
                if (data[i] > data[i + 1])
                    throw new Exception("fuck");

            return (int)sw.ElapsedMilliseconds;
        }

        public static int test3(int len)
        {
            var data = new Testing[len];
            var rand = new Random();

            var allowedData = new int[100];
            for (var i = 0; i < allowedData.Length; i++)
                allowedData[i] = rand.Next(0, int.MaxValue);

            //for (var i = 0; i < len; i++)
            //    data[i] = new Testing() { key = allowedData[rand.Next(0, allowedData.Length)], index = i };
            for (var i = 0; i < len; i++)
                data[i] = new Testing() { key = i, index = i };

            //for (var i = 0; i < len; i++)
            //    data[i] = rand.Next(0, int.MaxValue);

            var sw = new Stopwatch();
            sw.Start();
            RadixSort(data);
            sw.Stop();


            return (int)sw.ElapsedMilliseconds;
        }


        public struct Testing : IKey
        {
            public int key;
            public int index;

            public int Key { get { return key; } }
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //var data = new int[] { 2, 1};
            //RadixSort(data);
            //var len = 10000;
            //Console.WriteLine(test3(len));
            //Console.WriteLine(test3(len));
            //Console.WriteLine(test3(len));
            //Console.WriteLine(4 >> 1);
            //return;

            //var file = "dump.txt";
            //if (System.IO.File.Exists(file))
            //    System.IO.File.Delete(file);

            //using (var sw = new StreamWriter(file))
            //{
            //    var sb1 = new StringBuilder();
            //    var sb2 = new StringBuilder();
            //    var sb3 = new StringBuilder();
            //    for (var i = 0; i < 10; i++)
            //    {

            //        sb1.AppendFormat("T{0}", i);
            //        sb2.AppendFormat("T{0} t{0}", i);
            //        sb3.AppendFormat("t{0}", i);

            //        sw.WriteLine("public static void sendMessage<{0}>(this GameObject target, string name, {1}) {{", sb1, sb2);
            //        sw.WriteLine("  if (target.enabled)");
            //        sw.WriteLine("    Event<{0}>.Invoke(target.id, name, {1});", sb1, sb3);
            //        sw.WriteLine("}");

            //        sw.WriteLine("public static void sendMessageDown<{0}>(this GameObject target, string name, {1}) {{", sb1, sb2);
            //        sw.WriteLine("  if (target.enabled)");
            //        sw.WriteLine("    foreach (var c in target.allEnabledChildren())");
            //        sw.WriteLine("      Event<{0}>.Invoke(target.id, name, {1});", sb1, sb3);
            //        sw.WriteLine("}");

            //        sw.WriteLine("public static void broadcast<{0}>(this GameObject target, string name, {1}) {{", sb1, sb2);
            //        sw.WriteLine("  target.sendMessage(name, {0});", sb3);
            //        sw.WriteLine("  target.sendMessageDown(name, {0});", sb3);
            //        sw.WriteLine("}");                


            //        sb1.Append(',');
            //        sb2.Append(',');
            //        sb3.Append(',');


            //    }
            //}
            //Console.WriteLine(IsoHelper.ToISO(1, 1, 1));
            ////Console.WriteLine(IsoHelper.ToISO(2, 0, 1));
            //Console.WriteLine(IsoHelper.FromISO(0f, 0, 0));
            ////Console.WriteLine(IsoHelper.FromISO(2, 1f, 1));
            //for (var x = 0; x < 3; x++)
            //    for (var y = 0; y < 3; y++)
            //        for (var z = 0; z < 3; z++)
            //    {
            //        //Console.WriteLine(IsoHelper.ToISO(x, y, 64, 64, 0, z * 32));
            //        //Console.WriteLine(IsoHelper.ToISO(x, y, z) * 32); 
            //    }

            //Console.Read();
            //return;
            //var data = new int[10];
            //data[1] = 1;
            //data[3] = 5;
            //foreach (var xy in data.xy(2))
            //{
            //    Console.WriteLine("{0}, {1}, {2}", xy.x, xy.y, xy.value);
            //    data.set(2, xy.x, xy.y, xy.y * xy.x);
            //}
            //foreach (var xy in data.xy(2))
            //{
            //    Console.WriteLine("{0}, {1}, {2}", xy.x, xy.y, xy.value);
            //    data.set(2, xy.x, xy.y, xy.y * xy.x);
            //}

            //speedtest(100);
            //Console.Read();
            //return;

            //var aabb1 = new AxisAlignedBox(new Vector2(0, 0), new Vector2(10, 10));
            //var aabb2 = new AxisAlignedBox(new Vector2(-9, 0), new Vector2(1, 10));
            //var shape1 = new Shape(aabb1);
            //var shape2 = new Shape(aabb2);



            //var mtv = Shape.intersects(shape1, shape2);
            //Console.Write(mtv);
            //Console.Read();
            //return;
            //   var k = 10f;
            //   for (var x = 0; x < 10; x++)
            //   {
            ////       Console.WriteLine(-k * x);
            //   }

            //   var f = 100f;
            //   var m = 100f;
            //   var v = 0f;
            //   var p = 0f;
            //   var d = 0.9f;
            //   for (var x = 0; x < 100; x++)
            //   {
            //       var xf = 1f / 60f;
            //       //var a = f / m;
            //       v += (f / m) * xf;
            //       p += v * xf;
            //       v *= d;
            //       Console.WriteLine(string.Format("{0}, {1}", v * 60, p));
            //   }
            //   Console.Read();
            //   return;
            //var r = Math.Atan2(Math.Cos(1), Math.Sin(0));
            //var r = new Vector2( (float)Math.Cos(0),(float)Math.Sin(0));
            //var m = Matrix.CreateTranslation(0, 1, 0);
            //    Matrix.CreateRotationZ(0);


            //return;
            //var go = GameEngine.RootObject.createChild();
            //var gochild = go.createChild();

            //var t = go.transform();
            //var tc = gochild.transform();


            //tc.Position = new Vector2(100, 0);

            //for (var i = 0; i < 5; i++)
            //{
            //    t.Orientation += 0.2f;
            //    Console.WriteLine(tc.DerivedPosition);
            //}
            //Console.Read();
            //return;

            //using (var sw = new StreamWriter("C:\\event.txt", false))
            //{
            //    var data = Event.Build();
            //    Console.WriteLine(data);
            //    sw.Write(data);
            //}
            //Console.Read();
            //return;

            //using (var game = new Space.Game1())
            //    game.Run();
            //var qt = new QuadTree<testme>(new Vector2(16, 16), 20);
            //qt.Insert(new testme() { Bounds = AxisAlignedBox.FromRect(-20, -20, 40, 40) });
            //qt.Insert(new testme() { Bounds = AxisAlignedBox.FromRect(20, -20, 40, 40) });
            //qt.Insert(new testme() { Bounds = AxisAlignedBox.FromRect(-20, 20, 40, 400) });
            //qt.Insert(new testme() { Bounds = AxisAlignedBox.FromRect(20, 20, 400, 40) });

            //Console.WriteLine(BaseConversion("aza", basemap));

            ////C:\Users\Xposure\Downloads\mword10
            //var sb = new StringBuilder();
            //var first = true;
            //var matches = new HashSet<ulong>();
            //var dict = new Dictionary<ulong, string>();

            //if (System.IO.File.Exists(@"C:\Users\Xposure\Downloads\mword10\common.lua"))
            //    System.IO.File.Delete(@"C:\Users\Xposure\Downloads\mword10\common.lua");

            //using (var sw = new StreamWriter(@"C:\Users\Xposure\Downloads\mword10\common.lua"))
            //using (var sr = new StreamReader(@"C:\Users\Xposure\Downloads\mword10\common.txt"))
            //{
            //    sw.Write("local dictionary = {");

            //    var line = sr.ReadLine();
            //    var index = 0;
            //    while (line != null)
            //    {
            //        var success = true;
            //        for (var i = 0; i < line.Length; i++)
            //        {
            //            var ch = (int)line[i];
            //            if (ch < (int)'a' || ch > (int)'z')
            //            {
            //                success = false;
            //                break;
            //            }
            //        }

            //        if (success)
            //        {
            //            var r = BaseConversion(line, basemap);// StringToInt(line);

            //            //if (r != StringToInt(IntToString(r)))
            //            //    Console.WriteLine("FAILED: {0}", r);
            //            if (!matches.Add(r))
            //            {
            //                BaseConversion(line, basemap);// StringToInt(line);
            //                var current = dict[r];
            //                Console.WriteLine("FAILED: {0}, {1}, {2}", r, line, current);
            //            }
            //            else
            //                dict.Add(r, line);

            //            //Console.WriteLine(StringToInt(line));
            //            if (first)
            //                sw.Write("[{0}] = true", r);
            //            else
            //                sw.Write(",[{0}] = true", r);

            //            first = false;
            //        }

            //        line = sr.ReadLine();
            //    }
            //    sw.Write("}");
            //}
            //Console.WriteLine("done");
            //Console.Read();
            //return;
            using (var game = new BulletHell.Game())
            {
                game.Run();
            }
        }


    }

}
