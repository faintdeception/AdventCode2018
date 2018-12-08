using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text.RegularExpressions;

namespace AdventCode_3a {
    class Program {
        static List<Point> usedPoints = new List<Point>();
        static List<Claim> usedClaimAreas = new List<Claim>();
        static Dictionary<string, Point> usedPointsHash = new Dictionary<string, Point>();

        static void Main(string[] args)
        {

            var startTime = DateTime.Now;
            var endTime = DateTime.Now;
            
            //#1 @ 1,3: 4x4
            //#2 @ 3,1: 4x4
            //#3 @ 5,5: 2x2

            // Id regex: "#\d"
            // Top, Left regex: ""
            // Width x Height regex: ": \dx\d"
            string[] claimsList = File.ReadAllLines(@"input.txt");
            List<Claim> claims = new List<Claim>();

            foreach (var claimInput in claimsList)
            {

                string idPattern = @"#[0-9]+";
                int.TryParse(Regex.Match(Regex.Match(claimInput, idPattern).ToString(), @"\d+").ToString(), out int id);

                string pointPattern = @"[0-9]+,[0-9]+";
                var pointString = Regex.Match(claimInput, pointPattern).ToString();
                int.TryParse(pointString.Split(',')[0], out int left);
                int.TryParse(pointString.Split(',')[1], out int top);

                string widthHeightPattern = @"[0-9]+x[0-9]+";
                var widthByHeight = Regex.Match(claimInput, widthHeightPattern).ToString();
                int.TryParse(widthByHeight.Split('x')[0], out int width);
                int.TryParse(widthByHeight.Split('x')[1], out int height);


                claims.Add(new Claim()
                {
                    Id = id,
                    Top = top,
                    Left = left,
                    HeightInInches = height,
                    WidthInInches = width,
                    X1 = left,
                    X2 = left + width,
                    Y1 = top,
                    Y2 = top + height
                });
            }

           

            int overlapSquareInches = 0;
            List<int> conflictingClaimIds = new List<int>();
            //First, see which claims intersect, compare each claim to every other claim.
            foreach (var RectA in claims)
            {
                bool rectAHasNoIntersections = true;
                //Console.WriteLine("Claim A Id: {0} Left:{1} Right:{2} Top:{3} Bottom: {4} Height: {5} Width: {6}", RectA.Id, RectA.Left, RectA.Right, RectA.Top, RectA.Bottom, RectA.HeightInInches, RectA.WidthInInches);
                foreach (var RectB in claims)
                {
                    if (RectA.Id != RectB.Id)
                    {
                        if ((RectA.X1 < RectB.X2) && (RectA.X2 > RectB.X1) && (RectA.Y1 < RectB.Y2) && (RectA.Y2 > RectB.Y1))
                        {
                            rectAHasNoIntersections = false;
                            overlapSquareInches += OverlapArea(RectA, RectB);
                            conflictingClaimIds.AddRange(new int[] { RectA.Id, RectB.Id });
                            //Console.WriteLine(overlapSquareInches);
                        }
                    }
                }
                if (rectAHasNoIntersections)
                    Console.WriteLine(RectA.Id);
            }

            DrawClaims(claims);
            usedPoints.Clear();
            foreach (var point in usedPointsHash)
            {
                usedPoints.Add(point.Value);
            }
            DrawUsedPoints(usedPoints);
            CombineGraphs();
            //Console.WriteLine(overlapSquareInches);
            Console.WriteLine("Elapsed Time");
            Console.WriteLine(endTime - startTime);
            Console.ReadKey();

        }
        /**
         * Calculate the overlapping area of two rectangles.
        */

        public static int OverlapArea(Claim claim1,
                                 Claim claim2)
        {
            /* Check if there is indeed an overlap.
             * e.g.  E >= C  i.e. the most left point of the rectangle (EFGH) is 
             *       on the right side of the most right point of the rectangle (ABCD),
             *       therefore there is no overlapping.
             */
            if ((claim2.X1 >= claim1.X2) || (claim2.Y1 >= claim1.Y2) || (claim1.X1 >= claim2.X2) || (claim1.Y1 >= claim2.Y2))
                return 0;
            
            

            /* bottom left point of the overlapping area. */
            int bl_x = Math.Max(claim1.X1, claim2.X1);
            int bl_y = Math.Max(claim1.Y1, claim2.Y1);

            /* top right point of the overlapping area. */
            int tr_x = Math.Min(claim1.X2, claim2.X2);
            int tr_y = Math.Min(claim1.Y2, claim2.Y2);

            var topRightPoint = new Point(tr_x, tr_y);
            var bottomLeftPoint = new Point(bl_x, bl_y);

            for (int y = bottomLeftPoint.Y; y < topRightPoint.Y; y++)
            {
                for (int x = bottomLeftPoint.X; x < topRightPoint.X; x++)
                {
                    var newUsedPoint = new Point(x, y);
                    usedPoints.Add(newUsedPoint);
                    var pointKey = string.Format("{0},{1}", x, y);
                    if (!usedPointsHash.ContainsKey(pointKey))
                        usedPointsHash.Add(pointKey, newUsedPoint);
                }
            }

            return (tr_x - bl_x) * (tr_y - bl_y);
        }

        /**
         * Calculate the area of a single rectangle.
         */
        public int ComputeArea(int A, int B, int C, int D)
        {
            return (C - A) * (D - B);
        }

        /**
         * Find the total area covered by two rectilinear rectangles in a 2D plane.
         * Each rectangle is defined by its bottom left corner and top right corner.
         */
        public int ComputeArea(Claim claim1,
                               Claim claim2)
        {
            // The addition of area of the two rectangles minus the overlapping area.
            return ComputeArea(claim1.X1, claim1.Y1, claim1.X2, claim1.Y2) + ComputeArea(claim2.X1, claim2.Y1, claim2.X2, claim2.Y2) -
                   OverlapArea(claim1, claim2);
        }

        public static void CombineGraphs()
        {
            Image playbutton;
            int width = 2000;
            int height = 1024;
            try
            {
                
                playbutton = Image.FromFile("usedPointsGraph.jpeg");
            }
            catch (Exception ex)
            {
                return;
            }

            Image frame;
            try
            {
                frame = Image.FromFile("claimsGraph.jpeg");
            }
            catch (Exception ex)
            {
                return;
            }

            using (frame)
            {
                using (var bitmap = new Bitmap(width, height))
                {
                    using (var canvas = Graphics.FromImage(bitmap))
                    {
                        canvas.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        canvas.DrawImage(frame,
                                         new Rectangle(0,
                                                       0,
                                                       width,
                                                       height),
                                         new Rectangle(0,
                                                       0,
                                                       frame.Width,
                                                       frame.Height),
                                         GraphicsUnit.Pixel);
                        canvas.DrawImage(playbutton,
                                         (bitmap.Width / 2) - (playbutton.Width / 2),
                                         (bitmap.Height / 2) - (playbutton.Height / 2));
                        canvas.Save();
                    }
                    try
                    {
                        bitmap.Save("combinedGraph.jpeg",
                                    System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                    catch (Exception ex) { }
                }
            }
        }

        public static void DrawUsedPoints(List<Point> usedPoints)
        {
            Image image = new Bitmap(2000, 1024);

            Graphics graph = Graphics.FromImage(image);

            graph.Clear(Color.Transparent);
            Brush brush = new  SolidBrush(Color.FromArgb(8, 0, 0, 255));
            Pen pen = new Pen(brush);

            //graph.DrawLines(pen, new Point[] { new Point(10, 10), new Point(800, 900) });

            foreach (var point in usedPoints)
            {
                //graph.DrawString("x",
                //new Font(new FontFamily("Arial"), 1, FontStyle.Bold),
                //Brushes.Blue, point);
                graph.DrawRectangle(pen, new Rectangle(point.X, point.Y, 1, 1));
            }



            image.Save("usedPointsGraph.jpeg", System.Drawing.Imaging.ImageFormat.Png);
        }

        public static void DrawClaims(List<Claim> claims)
        {
            int x = 100;

            Image image = new Bitmap(2000, 1024);

            Graphics graph = Graphics.FromImage(image);
            
            graph.Clear(Color.White);

            Pen pen = new Pen(Brushes.Black);

            //graph.DrawLines(pen, new Point[] { new Point(10, 10), new Point(800, 900) });

            foreach (var claim in claims)
            {
                graph.DrawRectangle(pen, new Rectangle() { Height = claim.HeightInInches, Width = claim.WidthInInches, Location = new Point() { X = claim.X1, Y = claim.Y1 } });

                graph.DrawString(string.Format("id:{0}", claim.Id),
                new Font(new FontFamily("Arial"), 2, FontStyle.Regular),
                Brushes.Red, new Point (claim.X1, claim.Y1));
                
                //Console.WriteLine(claim.Id);
            }

            

            image.Save("claimsGraph.jpeg", System.Drawing.Imaging.ImageFormat.Png);

        }

        public static int overlapArea(int A, int B, int C, int D,
                         int E, int F, int G, int H)
        {
            /* Check if there is indeed an overlap.
             * e.g.  E >= C  i.e. the most left point of the rectangle (EFGH) is 
             *       on the right side of the most right point of the rectangle (ABCD),
             *       therefore there is no overlapping.
             */
            if ((E >= C) || (F >= D) || (A >= G) || (B >= H))
                return 0;

            /* bottom left point of the overlapping area. */
            int bl_x = Math.Max(A, E);
            int bl_y = Math.Max(B, F);

            /* top right point of the overlapping area. */
            int tr_x = Math.Min(C, G);
            int tr_y = Math.Min(D, H);

            return (tr_x - bl_x) * (tr_y - bl_y);
        }

        /**
         * Calculate the area of a single rectangle.
         */
        public int computeArea(int A, int B, int C, int D)
        {
            return (C - A) * (D - B);
        }

        /**
         * Find the total area covered by two rectilinear rectangles in a 2D plane.
         * Each rectangle is defined by its bottom left corner and top right corner.
         */
        public int computeArea(int A, int B, int C, int D,
                               int E, int F, int G, int H)
        {
            // The addition of area of the two rectangles minus the overlapping area.
            return computeArea(A, B, C, D) + computeArea(E, F, G, H) -
                   overlapArea(A, B, C, D, E, F, G, H);
        }
    }
}
