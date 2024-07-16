namespace FallingSand.Components.Pages
{

    public partial class FallingSand
    {
        DisplayClass Display;
        protected override void OnInitialized()
        {
            Display = new DisplayClass();
        }

    }

    public class DisplayClass
    {
        private int displyX;
        private int displayY;  
        public Pixel[,] pixelData;

        public DisplayClass()
        {
            displyX = 60;
            displayY = 60;

            pixelData = new Pixel[displyX,displayY];
            InitialiseDisplay();
        }

        void InitialiseDisplay()
        {

            for ( int y = 0; y < displayY; y++ )
            { 
              for( int x = 0; x < displyX; x++ )
                {
                    pixelData[x, y] = new Pixel(x,y);

                    //Randomize Color 
                    Random ran = new Random();
                    pixelData[x, y].Color.Red = ran.Next(0, 250);
					pixelData[x, y].Color.Blue = ran.Next(0, 250);
					pixelData[x, y].Color.Green = ran.Next(0, 250);


				}
            }

        }

        public RGB GetCellColor(int x, int y)
        {
            return pixelData[x, y].Color;
        }

    }

    public class Pixel
    {

        public int X;
        public int Y;

        public RGB Color;

        public Pixel(int x, int y)
        {
            X = x;
            Y = y;
            Color = new RGB();
        }
    }
    public struct RGB
    {
        public int Red {  get; set; }
        public int Green { get; set; }
        public int Blue { get; set; }

        public RGB()
        { 
            Red = 0;
            Green = 0;
            Blue = 0;
        }
        public RGB(int red, int green, int blue)
        {

            Red = CheckedInputIsInRange(red);
            Green = CheckedInputIsInRange(green);
            Blue = CheckedInputIsInRange(blue);

        }
        private int CheckedInputIsInRange(int input)
        {
            if (input < 0) { return 0; }
            else if (input > 255) { return 255; }
            else { return input; }
        }


    }
    
}